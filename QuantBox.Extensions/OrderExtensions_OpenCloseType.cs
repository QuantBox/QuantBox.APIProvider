using XAPI;
using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.Extensions
{
    public static class OrderExtensions_OpenCloseType
    {
        public static int index = OrderTagType.Network;

        public static Order Open(this Order order)
        {
            order.SetOpenClose(OpenCloseType.Open);
            return order;
        }

        public static Order Close(this Order order)
        {
            order.SetOpenClose(OpenCloseType.Close);
            return order;
        }

        public static Order CloseToday(this Order order)
        {
            order.SetOpenClose(OpenCloseType.CloseToday);
            return order;
        }

        public static Order SetOpenClose(this Order order, OpenCloseType OpenClose)
        {
            order.SubSide = order.OpenClose2SubSide(OpenClose);
            order.GetDictionary(index)[OrderTagType.PositionEffect] = (byte)OpenClose;
            return order;
        }

        public static OpenCloseType? GetOpenClose(this Order order)
        {
            object obj = order.GetDictionaryValue(OrderTagType.PositionEffect, index);
            if (obj == null)
            {
                return (OpenCloseType?)obj;
            }
            return (OpenCloseType?)(byte)obj;
        }

        public static SubSide OpenClose2SubSide(this Order order, OpenCloseType OpenClose)
        {
            //多头
            //Buy 就是开
            //Sell 就是平 SubSide 是 Undefined

            //空头
            //Sell 加 SubSide = SellShort 是开仓
            //Buy 加 SubSide = BuyCover 是平仓

            // 由于使用官方的办法无法指定平今与平昨，所以还是用以前的开平仓的写法
            // 区别只是官方维护了双向持仓
            if (order.Side == SmartQuant.OrderSide.Buy)
            {
                switch (OpenClose)
                {
                    case OpenCloseType.Open:
                        order.SubSide = SubSide.Undefined;
                        break;
                    case OpenCloseType.Close:
                    case OpenCloseType.CloseToday:
                        order.SubSide = SubSide.BuyCover;
                        break;
                }
            }
            else
            {
                switch (OpenClose)
                {
                    case OpenCloseType.Open:
                        order.SubSide = SubSide.SellShort;
                        break;
                    case OpenCloseType.Close:
                    case OpenCloseType.CloseToday:
                        order.SubSide = SubSide.Undefined;
                        break;
                }
            }

            return order.SubSide;
        }
    }
}
