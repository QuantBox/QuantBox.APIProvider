using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.Extensions
{
    public class OrderMsgType
    {
        public const char NewOrderSingle = 'D';
        public const char NewOrderList = 'E';
        //public const char NewOrderMultileg = '\0xAB';

        public const char QuoteRequest = 'R';
        public const char Quote = 'S';
        public const char QuoteCancel = 'Z';


        // 表示此Order完全忽视
        public const char Ignore = '\0';
    }
}
