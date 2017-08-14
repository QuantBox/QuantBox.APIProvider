using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.Extensions
{
    public static class OrderExtensions_SendCount
    {
        public static int index = OrderTagType.Local;

        public static Order SetSendCount(this Order order, int count)
        {
            order.GetDictionary(index)[OrderTagType.SendCount] = count;
            return order;
        }

        public static int GetSendCount(this Order order)
        {
            return order.GetDictionaryInt(OrderTagType.SendCount, index);
        }
    } 
}
