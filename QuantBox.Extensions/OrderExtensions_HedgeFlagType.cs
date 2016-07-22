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
        public static int index = OrderTagType.Network;
        public static Order SetHedgeFlag(this Order order, HedgeFlagType hedgeFlag)
        {
            order.GetDictionary(index)[OrderTagType.HedgeFlag] = (byte)hedgeFlag;
            return order;
        }

        public static HedgeFlagType? GetHedgeFlag(this Order order)
        {
            object obj = (HedgeFlagType)order.GetDictionaryValue(OrderTagType.HedgeFlag, index);
            return obj as HedgeFlagType?;
        }
    }
}
