//using QuantBox.Data.Serializer.V2;
using XAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtoBuf;
using SmartQuant;
using Ideafixxxer.Generics;

namespace QuantBox.APIProvider.Data
{
    //class OQTickWriter:TickWriter
    //{
    //    public OQTickWriter(string path):base(path)
    //    {

    //    }

    //    public void AddInstrument(ref InstrumentField pInstrument)
    //    {
    //        AddInstrument(pInstrument.Symbol, pInstrument.PriceTick, pInstrument.VolumeMultiple, 0);
    //        AddInstrument(pInstrument.InstrumentID + ".", pInstrument.PriceTick, pInstrument.VolumeMultiple, 0);
    //        AddInstrument(pInstrument.InstrumentID, pInstrument.PriceTick, pInstrument.VolumeMultiple, 0);
    //    }

    //    public void AddInstrument(Instrument pInstrument)
    //    {
    //        // 可能需要再补充
    //        AddInstrument(pInstrument.Symbol, pInstrument.TickSize, pInstrument.Factor, 0);
    //    }

    //    PbTick CreateTick(ref DepthMarketDataField pDepthMarketData, PbTickCodec codec)
    //    {
    //        var tick = new PbTick();
    //        tick.DepthList = new List<DepthItem>();
    //        tick.Config = codec.Config;

    //        tick.TradingDay = pDepthMarketData.TradingDay;
    //        tick.ActionDay = pDepthMarketData.ActionDay;
    //        tick.Time_HHmm = pDepthMarketData.UpdateTime / 100;
    //        tick.Time_____ssf__ = pDepthMarketData.UpdateTime % 100 * 10 + pDepthMarketData.UpdateMillisec / 100;
    //        tick.Time________ff = pDepthMarketData.UpdateMillisec % 100;

    //        codec.SetSymbol(tick, pDepthMarketData.Symbol);
    //        if(pDepthMarketData.Exchange != ExchangeType.Undefined)
    //            codec.SetExchange(tick, Enum<ExchangeType>.ToString(pDepthMarketData.Exchange));
    //        codec.SetLowerLimitPrice(tick, pDepthMarketData.LowerLimitPrice);
    //        codec.SetUpperLimitPrice(tick, pDepthMarketData.UpperLimitPrice);

    //        codec.SetOpen(tick, pDepthMarketData.OpenPrice);
    //        codec.SetHigh(tick, pDepthMarketData.HighestPrice);
    //        codec.SetLow(tick, pDepthMarketData.LowestPrice);
    //        codec.SetClose(tick, pDepthMarketData.ClosePrice);

    //        codec.SetVolume(tick, (long)pDepthMarketData.Volume);
    //        codec.SetOpenInterest(tick, (long)pDepthMarketData.OpenInterest);
    //        codec.SetTurnover(tick, pDepthMarketData.Turnover);//一定要设置合约乘数才能最优保存
    //        codec.SetAveragePrice(tick, pDepthMarketData.AveragePrice);
    //        codec.SetLastPrice(tick, pDepthMarketData.LastPrice);

    //        do
    //        {
    //            if (pDepthMarketData.BidVolume5 != 0)
    //            {
    //                tick.DepthList.Add(new DepthItem(codec.PriceToTick(pDepthMarketData.BidPrice5), pDepthMarketData.BidVolume5, 0));
    //            }

    //            if (pDepthMarketData.BidVolume4 != 0)
    //            {
    //                tick.DepthList.Add(new DepthItem(codec.PriceToTick(pDepthMarketData.BidPrice4), pDepthMarketData.BidVolume4, 0));
    //            }

    //            if (pDepthMarketData.BidVolume3 != 0)
    //            {
    //                tick.DepthList.Add(new DepthItem(codec.PriceToTick(pDepthMarketData.BidPrice3), pDepthMarketData.BidVolume3, 0));
    //            }

    //            if (pDepthMarketData.BidVolume2 != 0)
    //            {
    //                tick.DepthList.Add(new DepthItem(codec.PriceToTick(pDepthMarketData.BidPrice2), pDepthMarketData.BidVolume2, 0));
    //            }

    //            if (pDepthMarketData.BidVolume1 != 0)
    //            {
    //                tick.DepthList.Add(new DepthItem(codec.PriceToTick(pDepthMarketData.BidPrice1), pDepthMarketData.BidVolume1, 0));
                    
    //                // 先记录一个假的，防止只有买价没有卖价的情况
    //                codec.SetAskPrice1(tick, pDepthMarketData.BidPrice1);
    //                tick.AskPrice1 += 1;
    //            }

    //            if (pDepthMarketData.AskVolume1 == 0)
    //                break;
    //            tick.DepthList.Add(new DepthItem(codec.PriceToTick(pDepthMarketData.AskPrice1), pDepthMarketData.AskVolume1, 0));
    //            // 记录卖一价
    //            codec.SetAskPrice1(tick, pDepthMarketData.AskPrice1);

    //            if (pDepthMarketData.AskVolume2 == 0)
    //                break;
    //            tick.DepthList.Add(new DepthItem(codec.PriceToTick(pDepthMarketData.AskPrice2), pDepthMarketData.AskVolume2, 0));

    //            if (pDepthMarketData.AskVolume3 == 0)
    //                break;
    //            tick.DepthList.Add(new DepthItem(codec.PriceToTick(pDepthMarketData.AskPrice3), pDepthMarketData.AskVolume3, 0));

    //            if (pDepthMarketData.AskVolume4 == 0)
    //                break;
    //            tick.DepthList.Add(new DepthItem(codec.PriceToTick(pDepthMarketData.AskPrice4), pDepthMarketData.AskVolume4, 0));

    //            if (pDepthMarketData.AskVolume5 == 0)
    //                break;
    //            tick.DepthList.Add(new DepthItem(codec.PriceToTick(pDepthMarketData.AskPrice5), pDepthMarketData.AskVolume5, 0));

    //        } while (false);

    //        return tick;
    //    }

    //    public bool Write(ref DepthMarketDataField pDepthMarketData)
    //    {
    //        WriterDataItem item;
    //        if (Items.TryGetValue(pDepthMarketData.Symbol, out item))
    //        {
    //            item.Tick = CreateTick(ref pDepthMarketData, item.Serializer.Codec);
    //            Write(item, item.Tick);
    //            return true;
    //        }
    //        return false;
    //    }
    //}
}
