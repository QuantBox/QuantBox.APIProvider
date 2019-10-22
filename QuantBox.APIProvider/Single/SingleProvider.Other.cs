using XAPI;
using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.APIProvider.Single
{
    public partial class SingleProvider
    {
        // 得到API中的合约名与交易所
        private void GetApi_Symbol_Exchange_TickSize(Instrument instrument, byte id,
            out string altSymbol, out string altExchange,
            out string apiSymbol, out string apiExchange,
            out double apiTickSize)
        {
            // 取合约别名
            altSymbol = instrument.GetSymbol(id);
            altExchange = instrument.GetExchange(id);
            apiTickSize = instrument.TickSize;

            // 取合约在API中的名字
            apiSymbol = altSymbol;
            apiExchange = altExchange;

            // 对于UFX，没有实现查询合约的功能，所以这里其实使用的是AltID中的信息
            // 屏蔽这个功能，订阅的合约就根据设置来了
            if(true)
            {
                InstrumentField _Instrument;
                if (_dictInstruments.TryGetValue(altSymbol, out _Instrument))
                {
                    apiSymbol = _Instrument.InstrumentID;
                    apiExchange = _Instrument.ExchangeID;
                    apiTickSize = _Instrument.PriceTick;
                }
            }
        }
    }
}
