using XAPI;
using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.Extensions
{
    public class TradeEx:Trade
    {
        public TradeEx()
            : base()
        {
        }

        public TradeEx(Trade trade)
            : base(trade)
        {
        }

        public TradeEx(DateTime dateTime, byte providerId, int instrumentId, double price, int size)
            : base(dateTime, providerId, instrumentId, price, size)
        {
        }

        public TradeEx(DateTime dateTime, DateTime exchangeDateTime, byte providerId, int instrumentId, double price, int size)
            : base(dateTime, exchangeDateTime, providerId, instrumentId, price, size)
        {
        }

        public DepthMarketDataNClass DepthMarketData;
    }
}
