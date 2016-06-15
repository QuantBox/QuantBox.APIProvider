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
        public static Order SetCancelCount(this Order order, int count)
        {
            order.GetDictionary()[OrderTagType.CancelCount] = count;
            return order;
        }

        public static int GetCancelCount(this Order order)
        {
            return order.GetDictionaryInt(OrderTagType.CancelCount);
        }
    } 
}
