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
        public static Order SetBusinessType(this Order order, BusinessType business)
        {
            order.GetDictionary()[OrderTagType.Business] = business;
            return order;
        }

        public static BusinessType? GetBusinessType(this Order order)
        {
            object obj = order.GetDictionaryValue(OrderTagType.Business);
            return obj as BusinessType?;
        }
    }
}
