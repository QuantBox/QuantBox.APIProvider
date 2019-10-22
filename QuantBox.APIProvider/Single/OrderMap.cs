using XAPI;
using SmartQuant;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQ = SmartQuant;

namespace QuantBox.APIProvider.Single
{
    class OrderMap : BaseMap
    {
        private ConcurrentDictionary<string, ExternalOrderRecord> externalOrders;    // 新单
        // LocalID到Order的映射
        private ConcurrentDictionary<string, OrderRecord> pendingOrders;    // 新单
        // ID到Order
        private Dictionary<string, OrderRecord> workingOrders; // 挂单

        // Order.ID与LocalID或ID的映射，没有收到回报时是LocalID，收到后要更新为ID
        private Dictionary<int, string> orderIDs;   // 撤单时映射
        private ConcurrentDictionary<string, OrderRecord> pendingCancels;   // 撤单拒绝时使用
        private ConcurrentDictionary<string, TradeField> tradesDict; // 防止重复收到成交回报
        private OrderRecord GetExternalOrder(ref TradeField field)
        {
            ExternalOrderRecord record;
            if (externalOrders.TryGetValue(field.InstrumentID, out record))
            {
                if (field.OpenClose == OpenCloseType.Open)
                {
                    return field.Side == XAPI.OrderSide.Buy ? record.BuyOpen : record.SellOpen;
                }
                return field.Side == XAPI.OrderSide.Buy ? record.BuyClose : record.SellClose;
            }
            return null;
        }
        private void SetExternalOrder(Order order, ref OrderField field)
        {
            var orderRecord = new OrderRecord(order);
            ExternalOrderRecord record = new ExternalOrderRecord();
            record = externalOrders.GetOrAdd(order.Instrument.Symbol, record);
            if (field.OpenClose == OpenCloseType.Open)
            {
                if (field.Side == XAPI.OrderSide.Buy)
                {
                    record.BuyOpen = orderRecord;
                }
                else
                {
                    record.SellOpen = orderRecord;
                }
            }
            else
            {
                if (field.Side == XAPI.OrderSide.Buy)
                {
                    record.BuyClose = orderRecord;
                }
                else
                {
                    record.SellClose = orderRecord;
                }
            }
        }
        public OrderMap(Framework framework, SingleProvider provider)
            : base(framework, provider)
        {
            externalOrders = new ConcurrentDictionary<string, ExternalOrderRecord>();
            pendingOrders = new ConcurrentDictionary<string, OrderRecord>();
            workingOrders = new Dictionary<string, OrderRecord>();
            orderIDs = new Dictionary<int, string>();
            pendingCancels = new ConcurrentDictionary<string, OrderRecord>();
            tradesDict = new ConcurrentDictionary<string, TradeField>();
        }

        public void Clear()
        {
            pendingOrders.Clear();
            workingOrders.Clear();
            orderIDs.Clear();
            pendingCancels.Clear();
            tradesDict.Clear();
        }

        public void DoOrderSend(ref OrderField[] ordersArray, Order order)
        {
            if (Convert.ToInt32(order.Qty) == int.MaxValue)
            {
                SetExternalOrder(order, ref ordersArray[0]);
            }
            else
            {
                DoOrderSend(ref ordersArray, new List<Order>() { order });
            }
        }

        public void DoOrderSend(ref OrderField[] ordersArray, List<Order> ordersList)
        {
            lock (this)
            {
                // 这里其实返回的是LocalID
                string outstr = provider._TdApi.SendOrder(ordersArray);
                string[] OrderIds = outstr.Split(';');

                int i = 0;
                foreach (var orderId in OrderIds)
                {
                    if (string.IsNullOrEmpty(orderId))
                    {
                        // 直接将单子拒绝
                        EmitExecutionReport(new OrderRecord(ordersList[i]), SQ.ExecType.ExecRejected, SQ.OrderStatus.Rejected, "Provider ErrorCode:" + orderId);
                    }
                    else
                    {
                        // 记下了本地ID,用于立即撤单时供API来定位
                        this.orderIDs[ordersList[i].Id] = orderId;
                        //Console.WriteLine(orderId);
                        this.pendingOrders[orderId] = new OrderRecord(ordersList[i]);

                        ordersList[i].Fields[9] = orderId;
                    }
                    ++i;
                }
            }
        }

        public void DoOrderCancel(Order order)
        {
            DoOrderCancel(new List<Order>() { order });
        }

        public void DoOrderCancel(List<Order> ordersList)
        {
            lock (this)
            {
                OrderRecord[] recordList = new OrderRecord[ordersList.Count];
                string[] OrderIds = new string[ordersList.Count];

                for (int i = 0; i < ordersList.Count; ++i)
                {
                    // 如果需要下单的过程中撤单，这里有可能返回LocalID或ID
                    if (orderIDs.TryGetValue(ordersList[i].Id, out OrderIds[i]))
                    {
                        if (this.workingOrders.TryGetValue(OrderIds[i], out recordList[i]))
                        {
                            // 订单已经下到柜台上了
                            pendingCancels[OrderIds[i]] = recordList[i];
                        }
                        else if (this.pendingOrders.TryGetValue(OrderIds[i], out recordList[i]))
                        {
                            // 订单还没有下到柜台，需要撤单
                            pendingCancels[OrderIds[i]] = recordList[i];
                        }
                    }
                    else if (ordersList[i].Fields[9] != null)
                    {
                        OrderIds[i] = (string)ordersList[i].Fields[9];
                        recordList[i] = new OrderRecord(ordersList[i]);
                    }
                }

                string outstr = provider._TdApi.CancelOrder(OrderIds);
                string[] errs = outstr.Split(';');

                {
                    int i = 0;
                    foreach (var e in errs)
                    {
                        if (!string.IsNullOrEmpty(e) && e != "0")
                        {
                            if (recordList[i] != null)
                            {
                                EmitExecutionReport(recordList[i], SQ.ExecType.ExecCancelReject, recordList[i].Order.Status, "Provider ErrorCode:" + e);
                            }
                        }
                        ++i;
                    }
                }
            }
        }

        public void Process(ref OrderField order, NLog.Logger log)
        {
            // 所有的成交信息都不处理，交给TradeField处理
            if (order.ExecType == XAPI.ExecType.Trade)
                return;

            lock (this)
            {
                OrderRecord record;

                switch (order.ExecType)
                {
                    case XAPI.ExecType.New:
                        if (this.pendingOrders.TryRemove(order.LocalID, out record))
                        {
                            this.workingOrders[order.ID] = record;
                            // 将LocalID更新为ID
                            this.orderIDs[record.Order.Id] = order.ID;
                            EmitExecutionReport(record, (SQ.ExecType)order.ExecType, (SQ.OrderStatus)order.Status, order.Text());
                        }
                        else
                        {
                            //log.Warn("New,找不到订单，pendingOrders.Count={0}", pendingOrders.Count);
                        }
                        break;
                    case XAPI.ExecType.Rejected:
                        if (this.pendingOrders.TryRemove(order.LocalID, out record))
                        {
                            orderIDs.Remove(record.Order.Id);
                            record.LeavesQty = 0;
                            EmitExecutionReport(record, (SQ.ExecType)order.ExecType, (SQ.OrderStatus)order.Status, order.Text());
                        }
                        else if (this.workingOrders.TryGetValue(order.ID, out record))
                        {
                            // 比如说出现超出涨跌停时，先会到ProcessNew，所以得再多判断一次
                            workingOrders.Remove(order.ID);
                            orderIDs.Remove(record.Order.Id);
                            record.LeavesQty = 0;
                            EmitExecutionReport(record, (SQ.ExecType)order.ExecType, (SQ.OrderStatus)order.Status, order.Text());
                        }
                        break;
                    case XAPI.ExecType.Cancelled:
                        if (this.workingOrders.TryGetValue(order.ID, out record))
                        {
                            workingOrders.Remove(order.ID);
                            orderIDs.Remove(record.Order.Id);
                            record.LeavesQty = 0;
                            EmitExecutionReport(record, SQ.ExecType.ExecCancelled, SQ.OrderStatus.Cancelled);
                        }
                        else if (this.pendingOrders.TryRemove(order.LocalID, out record))
                        {
                            orderIDs.Remove(record.Order.Id);
                            record.LeavesQty = 0;
                            EmitExecutionReport(record, (SQ.ExecType)order.ExecType, (SQ.OrderStatus)order.Status, order.Text());
                        }
                        break;
                    case XAPI.ExecType.PendingCancel:
                        if (this.workingOrders.TryGetValue(order.ID, out record))
                        {
                            EmitExecutionReport(record, SQ.ExecType.ExecPendingCancel, SQ.OrderStatus.PendingCancel);
                        }
                        else if (this.pendingOrders.TryGetValue(order.LocalID, out record))
                        {
                            EmitExecutionReport(record, SQ.ExecType.ExecPendingCancel, SQ.OrderStatus.PendingCancel);
                        }
                        break;
                    case XAPI.ExecType.CancelReject:
                        if (this.pendingCancels.TryRemove(order.ID, out record))
                        {
                            // 已经收到第一回报的情况下
                            EmitExecutionReport(record, SQ.ExecType.ExecCancelReject, (SQ.OrderStatus)order.Status, order.Text());
                        }
                        else if (this.pendingCancels.TryRemove(order.LocalID, out record))
                        {
                            // 没有收到第一条回报的情况下
                            EmitExecutionReport(record, SQ.ExecType.ExecCancelReject, (SQ.OrderStatus)order.Status, order.Text());
                        }
                        //else if (this.workingOrders.TryGetValue(order.ID, out record))
                        //{
                        //    // 撤单回报延时的情况下
                        //    EmitExecutionReport(record, SQ.ExecType.ExecCancelReject, (SQ.OrderStatus)order.Status, order.Text());
                        //}
                        break;
                }
            }
        }

        public void Process(ref TradeField trade, NLog.Logger log)
        {
            lock (this)
            {
                OrderRecord record;
                if (!workingOrders.TryGetValue(trade.ID, out record))
                {
                    record = GetExternalOrder(ref trade);
                }
                if (record != null)
                {
                    // CTP出现过重复发送委托与成交的情况，需要过滤。并提示
                    // 同一交易ID同一方向不会同时出现，出现表示系统重复提示
                    // 不向方向表示自成交了
                    TradeField field = null;
                    if (tradesDict.TryGetValue(trade.TradeID, out field))
                    {
                        // 有取出，可能是自成交，也可能是重复发单
                        if (trade.Side == field.Side)
                        {
                            log.Error("重复收到成交回报！");
                            // 后面不再处理
                            return;
                        }
                        else
                        {
                            log.Warn("出现自成交！");
                            // 自成交也得处理
                        }
                    }
                    else
                    {
                        tradesDict[trade.TradeID] = trade;
                    }

                    record.AddFill(trade.Price, (int)trade.Qty);
                    SQ.ExecType execType = SQ.ExecType.ExecTrade;
                    SQ.OrderStatus orderStatus = (record.LeavesQty > 0) ? SQ.OrderStatus.PartiallyFilled : SQ.OrderStatus.Filled;
                    ExecutionReport report = CreateReport(record, execType, orderStatus);
                    report.LastPx = trade.Price;
                    report.LastQty = trade.Qty;
                    provider.EmitExecutionReport(report);
                }
                else
                {
                    // log.Warn("Trade,找不到订单，workingOrders.Count={0}", workingOrders.Count);
                }
            }
        }

        public void ProcessNew(ref QuoteField quote, QuoteRecord record)
        {
            OrderRecord askRecord = new OrderRecord(record.AskOrder);
            this.workingOrders[quote.AskID] = askRecord;
            this.orderIDs[askRecord.Order.Id] = quote.AskID;
            EmitExecutionReport(askRecord, (SQ.ExecType)quote.ExecType, (SQ.OrderStatus)quote.Status);

            OrderRecord bidRecord = new OrderRecord(record.BidOrder);
            this.workingOrders[quote.BidID] = bidRecord;
            this.orderIDs[bidRecord.Order.Id] = quote.BidID;
            EmitExecutionReport(bidRecord, (SQ.ExecType)quote.ExecType, (SQ.OrderStatus)quote.Status);
        }
    }
}
