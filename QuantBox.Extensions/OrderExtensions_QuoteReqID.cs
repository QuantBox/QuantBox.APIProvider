using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.Extensions
{
    public static class OrderExtensions_QuoteReqID
    {
        public static int index = OrderTagType.Network;

        public static Order SetQuoteReqID(this Order order, string quoteReqID)
        {
            order.GetDictionary(index)[OrderTagType.QuoteReqID] = quoteReqID;
            return order;
        }

        public static string GetQuoteReqID(this Order order)
        {
            return order.GetDictionaryString(OrderTagType.QuoteReqID, index);
        }
    } 
}
