using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuantBox.APIProvider.Single
{
    public partial class SingleProvider : IDataProvider
    {
        private void SubscribeForTest(Instrument instrument)
        {
            Thread.Sleep(1000);
            {
                Bid bid = new Bid(DateTime.Now, this.id, instrument.Id, 100, 5);
                EmitData(bid);

                Ask ask = new Ask(DateTime.Now, this.id, instrument.Id, 101, 5);
                EmitData(ask);
            }
            Thread.Sleep(1000);
            {
                Bid bid = new Bid(DateTime.Now, this.id, instrument.Id, 101, 5);
                EmitData(bid);

                Ask ask = new Ask(DateTime.Now, this.id, instrument.Id, 0, 0);
                EmitData(ask);
            }
        }

        public override void Subscribe(Instrument instrument)
        {
            //Task.Factory.StartNew(() => SubscribeForTest(instrument));
            //return;

            string altSymbol;
            string altExchange;
            string apiSymbol;
            string apiExchange;
            double apiTickSize;

            GetApi_Symbol_Exchange_TickSize(instrument, this.id,
                out altSymbol, out altExchange,
                out apiSymbol, out apiExchange,
                out apiTickSize);

            // 如果是对CTP接口，使用UFX的参数进行订阅是否有问题？IF1802.7,目前猜没有问题
            string Symbol_Dot_Exchange = string.Format("{0}.{1}", apiSymbol, apiExchange);
            string Symbol_Dot = string.Format("{0}.", apiSymbol);
            string Symbol = apiSymbol;

            MarketDataRecord record;

            if (!marketDataRecords.TryGetValue(Symbol_Dot_Exchange, out record))
            {
                record = new MarketDataRecord();

                record.TradeRequested = true;
                record.QuoteRequested = true;
                record.MarketDepthRequested = true;

                record.Symbol = apiSymbol;
                record.Exchange = apiExchange;
                record.Symbol_Dot = Symbol_Dot;
                record.Symbol_Dot_Exchange = Symbol_Dot_Exchange;

                // 可能没有交易所信息
                marketDataRecords[Symbol_Dot_Exchange] = record;
                marketDataRecords[Symbol_Dot] = record;
                marketDataRecords[Symbol] = record;

                record.Instrument = instrument.Symbol;
                record.Ids.Add(instrument.Id);
                Subscribe(record);
            }
            else
            {
                record.Ids.Add(instrument.Id);
            }
        }

        public override void Unsubscribe(Instrument instrument)
        {
            string altSymbol;
            string altExchange;
            string apiSymbol;
            string apiExchange;
            double apiTickSize;

            GetApi_Symbol_Exchange_TickSize(instrument, this.id,
                out altSymbol, out altExchange,
                out apiSymbol, out apiExchange,
                out apiTickSize);

            string Symbol_Dot_Exchange = string.Format("{0}.{1}", apiSymbol, apiExchange);
            string Symbol_Dot = string.Format("{0}.", apiSymbol);
            string Symbol = apiSymbol;

            // 订阅信息，使用的别名而不是API名，因为来股票行情时，得将API名与交易所名拼接成别名
            MarketDataRecord record;
            if (marketDataRecords.TryGetValue(Symbol_Dot_Exchange, out record))
            {
                record.Ids.Remove(instrument.Id);

                if (record.Ids.Count == 0)
                {
                    Unsubscribe(record);

                    // 多次订阅也无所谓
                    record.TradeRequested = false;
                    record.QuoteRequested = false;
                    record.MarketDepthRequested = false;

                    // 移除
                    marketDataRecords.Remove(Symbol_Dot_Exchange);
                    marketDataRecords.Remove(Symbol_Dot);
                    marketDataRecords.Remove(Symbol);
                }
            }
        }

        private void Subscribe(MarketDataRecord record)
        {
            if (IsApiConnected(_MdApi))
            {
                _MdApi.GetLog().Info("订阅合约:Symbol:{0};InstrumentID:{1};ExchangeID:{2}", record.Instrument, record.Symbol, record.Exchange);
                _MdApi.Subscribe(record.Symbol, record.Exchange);
            }
            else
            {
                EmitError("行情服务器没有连接");
                xlog.Error("行情服务器没有连接");
            }
            if (SubscribeQuote)
            {
                if (IsApiConnected(_QuoteRequestApi))
                    _QuoteRequestApi.SubscribeQuote(record.Symbol, record.Exchange);
            }
        }

        private void Unsubscribe(MarketDataRecord record)
        {
            if (IsApiConnected(_MdApi))
            {
                _MdApi.GetLog().Info("退订合约:Symbol:{0};InstrumentID:{1};ExchangeID:{2}", record.Instrument, record.Symbol, record.Exchange);

                _MdApi.Unsubscribe(record.Symbol, record.Exchange);
            }
            if (SubscribeQuote)
            {
                if (IsApiConnected(_QuoteRequestApi))
                    _QuoteRequestApi.UnsubscribeQuote(record.Symbol, record.Exchange);
            }
        }
    }
}
