using XAPI;
using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.Extensions
{
    public static class OrderExtensions_BusinessType
    {
        public static int index = OrderTagType.Network;

        public static Order SetBusinessType(this Order order, BusinessType business)
        {
            order.GetDictionary(index)[OrderTagType.Business] = (byte)business;
            return order;
        }

        public static BusinessType? GetBusinessType(this Order order)
        {
            object obj = order.GetDictionaryValue(OrderTagType.Business, index);
            return obj as BusinessType?;
        }
    }
}
