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
    /// 1.SameTimeOrde表示同一时刻发出来的订单,特殊的用于记录关联的Order，但没有先后循序，如Quote报单
    /// 2.NextTimeOrder表示接下来要发的订单，用于记录下一笔Order,如平仓后开仓 
    /// </summary>
    public static class OrderExtensions_Order
    {
        public static int index = OrderTagType.Local;

        public static Order SetSameTimeOrder(this Order order, Order ord)
        {
            order.GetDictionary(index)[OrderTagType.SameTimeOrder] = ord;
            return order;
        }

        public static Order GetSameTimeOrder(this Order order)
        {
            object obj = order.GetDictionaryValue(OrderTagType.SameTimeOrder, index);
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
            order.GetDictionary(index)[OrderTagType.NextTimeOrder] = ord;
            return order;
        }

        public static Order GetNextTimeOrder(this Order order)
        {
            object obj = order.GetDictionaryValue(OrderTagType.NextTimeOrder, index);
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
