using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.Extensions
{
    public static class OrderExtensions_MsgType
    {
        public static Order SetMsgType(this Order order, char msgType)
        {
            order.GetDictionary()[OrderTagType.MsgType] = msgType;
            return order;
        }

        public static char? GetMsgType(this Order order)
        {
            object obj = order.GetDictionaryValue(OrderTagType.MsgType);
            return obj as char?;
        }
    } 
}
