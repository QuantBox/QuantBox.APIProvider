using XAPI;
using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.Extensions
{
    public static class OrderExtensions_HedgeFlagType
    {
        public static Order SetHedgeFlag(this Order order, HedgeFlagType hedgeFlag)
        {
            order.GetDictionary()[OrderTagType.HedgeFlag] = hedgeFlag;
            return order;
        }

        public static HedgeFlagType? GetHedgeFlag(this Order order)
        {
            object obj = order.GetDictionaryValue(OrderTagType.HedgeFlag);
            return obj as HedgeFlagType?;
        }
    }
}
