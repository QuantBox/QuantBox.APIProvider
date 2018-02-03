using XAPI;
using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.APIProvider.Single
{
    public class MarketDataRecord
    {
        // 记录API中的合约名与交易所
        public string Symbol;
        public string Exchange;
        public string Symbol_Dot;
        public string Symbol_Dot_Exchange;

        public string Instrument;

        public bool TradeRequested;
        public bool QuoteRequested;
        public bool MarketDepthRequested;
        // 记录上次行情
        public DepthMarketDataNClass DepthMarket;

        public SortedSet<int> Ids;

        public MarketDataRecord()
        {
            DepthMarket = new DepthMarketDataNClass();
            Ids = new SortedSet<int>();
        }
    }
}
