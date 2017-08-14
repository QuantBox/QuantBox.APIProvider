using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.Extensions
{
    public static class OrderExtensions_CancelCount
    {
        public static int index = OrderTagType.Local;

        public static Order SetCancelCount(this Order order, int count)
        {
            order.GetDictionary(index)[OrderTagType.CancelCount] = count;
            return order;
        }

        public static int GetCancelCount(this Order order)
        {
            return order.GetDictionaryInt(OrderTagType.CancelCount, index);
        }
    } 
}
