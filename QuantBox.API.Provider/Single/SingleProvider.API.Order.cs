using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuantBox.Extensions;

using XAPI.Callback;
using XAPI;
using Newtonsoft.Json;

using SQ = SmartQuant;

namespace QuantBox.APIProvider.Single
{
    public partial class SingleProvider
    {
        #region 价格修正
        //public double FixPrice(double price, SmartQuant.OrderSide Side, double tickSize, double LowerLimitPrice, double UpperLimitPrice)
        //{
        //    //没有设置就直接用
        //    if (tickSize > 0)
        //    {
        //        decimal remainder = ((decimal)price % (decimal)tickSize);
        //        if (remainder != 0)
        //        {
        //            if (Side == SmartQuant.OrderSide.Buy)
        //            {
        //                price = Math.Round(Math.Ceiling(price / tickSize) * tickSize, 6);
        //            }
        //            else
        //            {
        //                price = Math.Round(Math.Floor(price / tickSize) * tickSize, 6);
        //            }
        //        }
        //        else
        //        {
        //            //正好能整除，不操作
        //        }
        //    }

        //    if (0 == UpperLimitPrice
        //        && 0 == LowerLimitPrice)
        //    {
        //        //涨跌停无效

        //    }
        //    else
        //    {
        //        //防止价格超过涨跌停
        //        if (price >= UpperLimitPrice)
        //            price = UpperLimitPrice;
        //        else if (price <= LowerLimitPrice)
        //            price = LowerLimitPrice;
        //    }
        //    return price;
        //}

        public double FixPrice(MarketDataRecord record, double price, SmartQuant.OrderSide Side, double tickSize)
        {
            double LowerLimitPrice = record.DepthMarket.LowerLimitPrice;
            double UpperLimitPrice = record.DepthMarket.UpperLimitPrice;

            //没有设置就直接用
            if (tickSize > 0)
            {
                decimal remainder = ((decimal)price % (decimal)tickSize);
                if (remainder != 0)
                {
                    if (Side == SmartQuant.OrderSide.Buy)
                    {
                        price = Math.Round(Math.Ceiling(price / tickSize) * tickSize, 6);
                    }
                    else
                    {
                        price = Math.Round(Math.Floor(price / tickSize) * tickSize, 6);
                    }
                }
                else
                {
                    //正好能整除，不操作
                }
            }

            if (0 == UpperLimitPrice
                && 0 == LowerLimitPrice)
            {
                //涨跌停无效
                _TdApi.GetLog().Warn("Symbol:{0},Symbol_Dot_Exchange:{1},LowerLimitPrice && UpperLimitPrice 为0,没有进行价格修正",
                    record.Symbol, record.Symbol_Dot_Exchange);
            }
            else
            {
                //防止价格超过涨跌停
                if (price >= UpperLimitPrice)
                    price = UpperLimitPrice;
                else if (price <= LowerLimitPrice)
                    price = LowerLimitPrice;
            }
            return price;
        }
        #endregion

        // 如果没有就取默认值
        public OpenCloseType GetOpenClose(Order order)
        {
            OpenCloseType? oc = order.GetOpenClose();
            if (oc.HasValue)
            {
                return oc.Value;
            }
            return DefaultOpenClose;
        }

        // 如果没有就取默认值
        public HedgeFlagType GetHedgeFlag(Order order)
        {
            HedgeFlagType? hf = order.GetHedgeFlag();
            if (hf.HasValue)
            {
                return hf.Value;
            }
            return DefaultHedgeFlag;
        }

        // 如果没有就取Order上可见的
        public XAPI.OrderSide GetSide(Order order)
        {
            XAPI.OrderSide? hf = order.GetSide();
            if (hf.HasValue)
            {
                return hf.Value;
            }
            return (XAPI.OrderSide)order.Side;
        }

        public XAPI.BusinessType GetBusiness(Order order)
        {
            XAPI.BusinessType? hf = order.GetBusinessType();
            if (hf.HasValue)
            {
                return hf.Value;
            }
            return DefaultBusiness;
        }

        public string GetPortfolioID1(Order order)
        {
            string str = order.GetPortfolioID1();
            if (!string.IsNullOrEmpty(str))
                return str;
            return DefaultPortfolioID1;
        }

        public string GetPortfolioID2(Order order)
        {
            string str = order.GetPortfolioID2();
            if (!string.IsNullOrEmpty(str))
                return str;
            return DefaultPortfolioID2;
        }

        public string GetPortfolioID3(Order order)
        {
            string str = order.GetPortfolioID3();
            if (!string.IsNullOrEmpty(str))
                return str;
            return DefaultPortfolioID3;
        }


        private void CmdCancelOrder(ExecutionCommand command)
        {
            orderMap.DoOrderCancel(command.Order);
        }

        private void CmdNewOrderSingle(ExecutionCommand command)
        {
            string altSymbol;
            string altExchange;
            string apiSymbol;
            string apiExchange;
            double apiTickSize;

            GetApi_Symbol_Exchange_TickSize(command.Instrument,
                out altSymbol, out altExchange,
                out apiSymbol, out apiExchange,
                out apiTickSize);

            OrderField[] fields = new OrderField[1];

            ToOrderStruct(ref fields[0], command.Order, apiSymbol, apiExchange);
            double price = fields[0].Price;

            //市价修正，如果不连接行情，此修正不执行，得策略层处理
            MarketDataRecord record;
            if (marketDataRecords.TryGetValue(command.Instrument.Symbol, out record))
            {
                switch (command.OrdType)
                {
                    case SQ.OrderType.Market:
                    case SQ.OrderType.MarketOnClose:
                    case SQ.OrderType.Stop:
                    case SQ.OrderType.TrailingStop:
                        {
                            if (command.Side == SQ.OrderSide.Buy)
                            {
                                price = record.DepthMarket.LastPrice + LastPricePlusNTicks * apiTickSize;
                            }
                            else
                            {
                                price = record.DepthMarket.LastPrice - LastPricePlusNTicks * apiTickSize;
                            }

                            // 市价单使用限价单模拟
                            if (SwitchMakertOrderToLimitOrder)
                            {
                                fields[0].Type = XAPI.OrderType.Limit;
                            }
                        }
                        break;
                }
                if (HasPriceLimit)
                {
                    //price = FixPrice(price, command.Side, apiTickSize, record.DepthMarket.LowerLimitPrice, record.DepthMarket.UpperLimitPrice);
                    price = FixPrice(record, price, command.Side, apiTickSize);
                }

                fields[0].Price = price;
            }

            orderMap.DoOrderSend(ref fields, command.Order);
        }

        private void CmdNewOrderList(ExecutionCommand command)
        {
            // 先查出所有的单子
            List<Order> orders = command.Order.GetSameTimeOrderList();

            OrderField[] fields = new OrderField[orders.Count];
            for (int i = 0; i < orders.Count; ++i)
            {
                string altSymbol;
                string altExchange;
                string apiSymbol;
                string apiExchange;
                double apiTickSize;

                GetApi_Symbol_Exchange_TickSize(orders[i].Instrument,
                    out altSymbol, out altExchange,
                    out apiSymbol, out apiExchange,
                    out apiTickSize);

                ToOrderStruct(ref fields[i], orders[i], apiSymbol, apiExchange);
            }

            orderMap.DoOrderSend(ref fields, orders);
        }

        //private void SubSide2OpenClose(ref OrderField field, Order order)
        //{
        //    // 由于无法指定平今与平昨，所以废弃
        //    if (framework.Configuration.UseSubPositions)
        //    {
        //        switch (order.SubSide)
        //        {
        //            case SubSide.Undefined:
        //                field.OpenClose = order.Side == SQ.OrderSide.Buy ? OpenCloseType.Open : OpenCloseType.Close;
        //                break;
        //            case SubSide.BuyCover:
        //                field.OpenClose = OpenCloseType.Close;
        //                break;
        //            case SubSide.SellShort:
        //                field.OpenClose = OpenCloseType.Open;
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        // 前面已经处理过了
        //        // field.OpenClose = GetOpenClose(order);
        //    }
        //}

        //private void OpenClose2SubSide(ref OrderField field, Order order)
        //{
        //    //多头
        //    //Buy 就是开
        //    //Sell 就是平 SubSide 是 Undefined

        //    //空头
        //    //Sell 加 SubSide = SellShort 是开仓
        //    //Buy 加 SubSide = BuyCover 是平仓

        //    // 由于使用官方的办法无法指定平今与平昨，所以还是用以前的开平仓的写法
        //    // 区别只是官方维护了双向持仓
        //    if (order.Side == SQ.OrderSide.Buy)
        //    {
        //        switch (field.OpenClose)
        //        {
        //            case OpenCloseType.Open:
        //                order.SubSide = SubSide.Undefined;
        //                break;
        //            case OpenCloseType.Close:
        //            case OpenCloseType.CloseToday:
        //                order.SubSide = SubSide.BuyCover;
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        switch (field.OpenClose)
        //        {
        //            case OpenCloseType.Open:
        //                order.SubSide = SubSide.SellShort;
        //                break;
        //            case OpenCloseType.Close:
        //            case OpenCloseType.CloseToday:
        //                order.SubSide = SubSide.Undefined;
        //                break;
        //        }
        //    }
        //}

        private void ToOrderStruct(ref OrderField field, Order order, string apiSymbol, string apiExchange)
        {
            field.InstrumentID = apiSymbol;
            field.ExchangeID = apiExchange;
            field.Price = order.Price;
            field.Qty = order.Qty;
            field.OpenClose = GetOpenClose(order);
            field.HedgeFlag = GetHedgeFlag(order);
            field.Side = GetSide(order);
            field.Type = (XAPI.OrderType)order.Type;
            field.StopPx = order.StopPx;
            field.TimeInForce = (XAPI.TimeInForce)order.TimeInForce;
            field.ClientID = order.ClientID;
            field.AccountID = order.Account;

            field.PortfolioID1 = GetPortfolioID1(order);
            field.PortfolioID2 = GetPortfolioID2(order);
            field.PortfolioID3 = GetPortfolioID3(order);
            field.Business = GetBusiness(order);

            //OpenClose2SubSide(ref field, order);
        }

        private void OnRtnOrder_callback(object sender, ref OrderField order)
        {
            (sender as XApi).GetLog().Debug("OnRtnOrder:" + order.ToFormattedString());
            try
            {
                orderMap.Process(ref order);
            }
            catch (Exception ex)
            {
                (sender as XApi).GetLog().Error(ex);
            }
        }

        private void OnRtnTrade_callback(object sender, ref TradeField trade)
        {
            (sender as XApi).GetLog().Debug("OnRtnTrade:" + trade.ToFormattedString());
            try
            {
                orderMap.Process(ref trade);
            }
            catch (Exception ex)
            {
                (sender as XApi).GetLog().Error(ex);
            }
        }
    }
}
