using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XAPI.Callback;
using XAPI;
//using System.Threading.Tasks.Dataflow;
using QuantBox.Extensions;

namespace QuantBox.APIProvider.Single
{
    public partial class SingleProvider
    {
        private DateTime _dateTime = DateTime.Now;
        private DateTime _exchangeDateTime = DateTime.Now;

        private void OnRtnDepthMarketData_callback(object sender, ref DepthMarketDataNClass pDepthMarketData)
        {
            if (!_enableEmitData)
                return;

            try
            {
                // 传过来的Symbol，有可能是不带点，有可能是带点的
                // 要同时存成完整版，因为下单时
                MarketDataRecord record;
                if (!marketDataRecords.TryGetValue(pDepthMarketData.Symbol, out record))
                {
                    return;
                }

                // 取出上次的行情记录
                DepthMarketDataNClass depthMarket = record.DepthMarket;

                //将更新字典的功能提前，因为如果一开始就OnTrade中下单，涨跌停没有更新
                record.DepthMarket = pDepthMarketData;

                _dateTime = DateTime.Now;
                try
                {
                    _exchangeDateTime = pDepthMarketData.ExchangeDateTime();
                }
                catch
                {
                    _exchangeDateTime = _dateTime;
                    (sender as XApi).Log.Error("{0} ExchangeDateTime有误，现使用LocalDateTime代替，请找API开发人员处理API中的时间兼容问题。", pDepthMarketData.ToFormattedStringExchangeDateTime());
                }


                if (_emitBidAskFirst)
                {
                    if (_emitBidAsk)
                    {
                        FireBid(record.Instrument.Id, _dateTime, _exchangeDateTime, pDepthMarketData, depthMarket);
                        FireAsk(record.Instrument.Id, _dateTime, _exchangeDateTime, pDepthMarketData, depthMarket);
                    }
                    FireTrade(record.Instrument.Id, _dateTime, _exchangeDateTime, pDepthMarketData, depthMarket);
                }
                else
                {
                    FireTrade(record.Instrument.Id, _dateTime, _exchangeDateTime, pDepthMarketData, depthMarket);
                    if (_emitBidAsk)
                    {
                        FireBid(record.Instrument.Id, _dateTime, _exchangeDateTime, pDepthMarketData, depthMarket);
                        FireAsk(record.Instrument.Id, _dateTime, _exchangeDateTime, pDepthMarketData, depthMarket);
                    }
                }
            }
            catch (Exception ex)
            {
                (sender as XApi).Log.Error(ex);
            }
        }

        private void FireTrade(int InstrumentId,DateTime _dateTime, DateTime _exchangeDateTime, DepthMarketDataNClass pDepthMarketData,DepthMarketDataNClass DepthMarket)
        {
            //行情过来时是今天累计成交量，得转换成每个tick中成交量之差
            double volume = pDepthMarketData.Volume - DepthMarket.Volume;
            // 以前第一条会导致集合竞价后的第一条没有成交量，这种方法就明确了上一笔是空数据
            if (0 == DepthMarket.TradingDay && 0 == DepthMarket.ActionDay)
            {
                //没有接收到最开始的一条，所以这计算每个Bar的数据时肯定超大，强行设置为0
                volume = 0;
            }
            else if (volume < 0)
            {
                //如果隔夜运行，会出现今早成交量0-昨收盘成交量，出现负数，所以当发现为负时要修改
                volume = pDepthMarketData.Volume;
            }

            // 使用新的类,保存更多信息
            var trade = new TradeEx(
                _dateTime,
                _exchangeDateTime,
                this.id,
                InstrumentId,
                pDepthMarketData.LastPrice,
                (int) volume) {DepthMarketData = pDepthMarketData};

            // 启用底层数据上传

            EmitData(trade);
        }

        private void FireBid(int InstrumentId, DateTime _dateTime, DateTime _exchangeDateTime, DepthMarketDataNClass pDepthMarketData, DepthMarketDataNClass DepthMarket)
        {
            do
            {
                if (pDepthMarketData.Bids == null || pDepthMarketData.Bids.Length == 0)
                    break;

                if (DepthMarket.Bids != null && DepthMarket.Bids.Length > 0)
                {
                    if (DepthMarket.Bids[0].Size == pDepthMarketData.Bids[0].Size
                    && DepthMarket.Bids[0].Price == pDepthMarketData.Bids[0].Price)
                    {
                        // 由于与上次一样，不能动
                        break;
                    }
                }

                Bid bid = new Bid(
                        _dateTime,
                        _exchangeDateTime,
                        this.id,
                        InstrumentId,
                        pDepthMarketData.Bids[0].Price,
                        pDepthMarketData.Bids[0].Size);

                EmitData(bid);

            } while (false);
        }

        private void FireAsk(int InstrumentId, DateTime _dateTime, DateTime _exchangeDateTime, DepthMarketDataNClass pDepthMarketData, DepthMarketDataNClass DepthMarket)
        {
            do
            {
                if (pDepthMarketData.Asks == null || pDepthMarketData.Asks.Length == 0)
                    break;

                if (DepthMarket.Asks != null && DepthMarket.Asks.Length > 0)
                {
                    if (DepthMarket.Asks[0].Size == pDepthMarketData.Asks[0].Size
                    && DepthMarket.Asks[0].Price == pDepthMarketData.Asks[0].Price)
                    {
                        // 由于与上次一样，不能动
                        break;
                    }

                }

                Ask ask = new Ask(
                        _dateTime,
                        _exchangeDateTime,
                        this.id,
                        InstrumentId,
                        pDepthMarketData.Asks[0].Price,
                        pDepthMarketData.Asks[0].Size);

                EmitData(ask);

            } while (false);
        }
    }
}
