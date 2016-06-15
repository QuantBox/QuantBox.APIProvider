using XAPI;
using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.Extensions
{
    public static class OrderExtensions_Side
    {
        public static Order LOFCreation(this Order order)
        {
            if (order.Side != SmartQuant.OrderSide.Buy)
                throw new InvalidOperationException("只能使用Buy操作");
            order.SetSide(XAPI.OrderSide.LOFCreation);
            return order;
        }

        public static Order LOFRedemption(this Order order)
        {
            if (order.Side != SmartQuant.OrderSide.Sell)
                throw new InvalidOperationException("只能使用Sell操作");
            order.SetSide(XAPI.OrderSide.LOFRedemption);
            return order;
        }

        public static Order ETFCreation(this Order order)
        {
            if (order.Side != SmartQuant.OrderSide.Buy)
                throw new InvalidOperationException("只能使用Buy操作");
            order.SetSide(XAPI.OrderSide.ETFCreation);
            return order;
        }

        public static Order ETFRedemption(this Order order)
        {
            if (order.Side != SmartQuant.OrderSide.Sell)
                throw new InvalidOperationException("只能使用Sell操作");
            order.SetSide(XAPI.OrderSide.ETFRedemption);
            return order;
        }


        public static Order Merge(this Order order)
        {
            if (order.Side != SmartQuant.OrderSide.Buy)
                throw new InvalidOperationException("只能使用Buy操作");
            order.SetSide(XAPI.OrderSide.Merge);
            return order;
        }

        public static Order Split(this Order order)
        {
            if (order.Side != SmartQuant.OrderSide.Sell)
                throw new InvalidOperationException("只能使用Sell操作");
            order.SetSide(XAPI.OrderSide.Split);
            return order;
        }

        public static Order CBConvert(this Order order)
        {
            if (order.Side != SmartQuant.OrderSide.Buy)
                throw new InvalidOperationException("只能使用Buy操作");
            order.SetSide(XAPI.OrderSide.CBConvert);
            return order;
        }

        public static Order CBRedemption(this Order order)
        {
            if (order.Side != SmartQuant.OrderSide.Sell)
                throw new InvalidOperationException("只能使用Sell操作");
            order.SetSide(XAPI.OrderSide.CBRedemption);
            return order;
        }

        public static Order SetSide(this Order order, XAPI.OrderSide side)
        {
            order.GetDictionary()[OrderTagType.Side] = side;
            return order;
        }

        public static XAPI.OrderSide? GetSide(this Order order)
        {
            object obj = order.GetDictionaryValue(OrderTagType.Side);
            return obj as XAPI.OrderSide?;
        }
    } 
}
