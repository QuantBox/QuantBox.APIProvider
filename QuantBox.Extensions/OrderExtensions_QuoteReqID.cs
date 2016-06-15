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
        public static Order SetQuoteReqID(this Order order, string quoteReqID)
        {
            order.GetDictionary()[OrderTagType.QuoteReqID] = quoteReqID;
            return order;
        }

        public static string GetQuoteReqID(this Order order)
        {
            object obj = order.GetDictionaryValue(OrderTagType.QuoteReqID);
            return obj as string;
        }
    } 
}
