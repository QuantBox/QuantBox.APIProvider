using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.Extensions
{
    /// <summary>
    /// 有SameTimeOrder和NextTimeOrder两种区别
    /// 1.SameTimeOrde表示同一时刻发出来的订单
    /// 2.NextTimeOrder表示接下来要发的订单
    /// </summary>
    public static class OrderExtensions_Order
    {
        public static Order SetSameTimeOrder(this Order order, Order ord)
        {
            order.GetDictionary()[OrderTagType.SameTimeOrder] = ord;
            return order;
        }

        public static Order GetSameTimeOrder(this Order order)
        {
            object obj = order.GetDictionaryValue(OrderTagType.SameTimeOrder);
            return obj as Order;
        }

        public static List<Order> GetSameTimeOrderList(this Order order)
        {
            List<Order> orders = new List<Order>();

            orders.Add(order);

            Order ord = order;
            while ((ord = ord.GetSameTimeOrder()) != null)
            {
                orders.Add(ord);
            }

            return orders;
        }

        public static Order SetNextTimeOrder(this Order order, Order ord)
        {
            order.GetDictionary()[OrderTagType.NextTimeOrder] = ord;
            return order;
        }

        public static Order GetNextTimeOrder(this Order order)
        {
            object obj = order.GetDictionaryValue(OrderTagType.NextTimeOrder);
            return obj as Order;
        }

        public static List<Order> GetNextTimeOrderList(this Order order)
        {
            List<Order> orders = new List<Order>();

            orders.Add(order);

            Order ord = order;
            while ((ord = ord.GetNextTimeOrder()) != null)
            {
                orders.Add(ord);
            }

            return orders;
        }
    } 
}
