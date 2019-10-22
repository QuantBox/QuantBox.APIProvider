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
    class QuoteMap : BaseMap
    {
        private OrderMap orderMap;

        private ConcurrentDictionary<string, QuoteRecord> pendingOrders;    // 新单
        private Dictionary<string, QuoteRecord> workingOrders; // 挂单

        private Dictionary<int, string> orderIDs;   // 撤单时映射
        private ConcurrentDictionary<string, QuoteRecord> pendingCancels;   // 撤单拒绝时使用

        public QuoteMap(Framework framework, SingleProvider provider,OrderMap orderMap):base(framework,provider)
        {
            this.orderMap = orderMap;

            pendingOrders = new ConcurrentDictionary<string, QuoteRecord>();
            workingOrders = new Dictionary<string, QuoteRecord>();
            orderIDs = new Dictionary<int, string>();
            pendingCancels = new ConcurrentDictionary<string, QuoteRecord>();
        }

        public void Clear()
        {
            pendingOrders.Clear();
            workingOrders.Clear();
            orderIDs.Clear();
            pendingCancels.Clear();

            orderMap.Clear();
        }


        public void EmitExecutionReport(QuoteRecord record, SQ.ExecType execType, SQ.OrderStatus orderStatus)
        {
            EmitExecutionReport(new OrderRecord(record.AskOrder), execType, orderStatus);
            EmitExecutionReport(new OrderRecord(record.BidOrder), execType, orderStatus);
        }

        public void EmitExecutionReport(QuoteRecord record, SQ.ExecType execType, SQ.OrderStatus orderStatus, string text)
        {
            EmitExecutionReport(new OrderRecord(record.AskOrder), execType, orderStatus, text);
            EmitExecutionReport(new OrderRecord(record.BidOrder), execType, orderStatus, text);
        }

        public void DoQuoteSend(QuoteField quote, ExecutionCommand command,Order askOrder,Order bidOrder)
        {
            //string askRef;
            //string bidRef;
            //provider._TdApi.SendQuote(ref quote,out askRef,out bidRef);
            //if (askRef == null)
            //{
            //    // 直接将单子拒绝
            //    EmitExecutionReport(new QuoteRecord(askOrder, bidOrder), SQ.ExecType.ExecRejected, SQ.OrderStatus.Rejected, "ErrorCode:");
            //}
            //else
            //{
            //    this.pendingOrders.TryAdd(askRef, new QuoteRecord(askOrder, bidOrder));
            //}
        }

        public void DoQuoteCancel(ExecutionCommand command)
        {
            string quoteId;
            if (orderIDs.TryGetValue(command.Order.Id, out quoteId))
            {
                QuoteRecord record;
                if (this.workingOrders.TryGetValue(quoteId, out record))
                {
                    pendingCancels[quoteId] = record;
                }

                //string err;
                //provider._TdApi.CancelQuote(quoteId,out err);
                //if (!string.IsNullOrEmpty(err))
                //{
                //    EmitExecutionReport(record, SQ.ExecType.ExecCancelReject, record.Status, "ErrorCode:" + err);
                //}
            }
        }

        public void Process(ref QuoteField quote)
        {
            // 所有的成交信息都不处理，交给TradeField处理
            if (quote.ExecType == XAPI.ExecType.Trade)
                return;

            QuoteRecord record = null;

            switch (quote.ExecType)
            {
                case XAPI.ExecType.New:
                    if (this.pendingOrders.TryRemove(quote.ID, out record))
                    {
                        this.workingOrders.Add(quote.ID, record);
                        this.orderIDs.Add(record.AskOrder.Id, quote.ID);
                        this.orderIDs.Add(record.BidOrder.Id, quote.ID);

                        orderMap.ProcessNew(ref quote, record);

                        // 这个地方可以跳过
                        //EmitExecutionReport(record, quote.ExecType, quote.Status);
                    }
                    break;
                case XAPI.ExecType.Rejected:
                    if (this.pendingOrders.TryRemove(quote.ID, out record))
                    {
                        EmitExecutionReport(record, (SQ.ExecType)quote.ExecType, (SQ.OrderStatus)quote.Status, quote.Text());
                    }
                    else if (this.workingOrders.TryGetValue(quote.ID, out record))
                    {
                        // 比如说出现超出涨跌停时，先会到ProcessNew，所以得再多判断一次
                        workingOrders.Remove(quote.ID);
                        orderIDs.Remove(record.AskOrder.Id);
                        orderIDs.Remove(record.BidOrder.Id);
                        EmitExecutionReport(record, (SQ.ExecType)quote.ExecType, (SQ.OrderStatus)quote.Status, quote.Text());
                    }
                    break;
                case XAPI.ExecType.Cancelled:
                    if (this.workingOrders.TryGetValue(quote.ID, out record))
                    {
                        workingOrders.Remove(quote.ID);
                        orderIDs.Remove(record.AskOrder.Id);
                        orderIDs.Remove(record.BidOrder.Id);
                        EmitExecutionReport(record, SQ.ExecType.ExecCancelled, SQ.OrderStatus.Cancelled);
                    }
                    break;
                case XAPI.ExecType.PendingCancel:
                    if (this.workingOrders.TryGetValue(quote.ID, out record))
                    {
                        EmitExecutionReport(record, SQ.ExecType.ExecPendingCancel, SQ.OrderStatus.PendingCancel);
                    }
                    break;
                case XAPI.ExecType.CancelReject:
                    if (this.pendingCancels.TryRemove(quote.ID, out record))
                    {
                        EmitExecutionReport(record, SQ.ExecType.ExecCancelReject, (SQ.OrderStatus)quote.Status, quote.Text());
                    }
                    break;
            }
            if(record != null)
                record.Status = (SQ.OrderStatus)quote.Status;
        }
    }
}
