using XAPI;
using SmartQuant;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuantBox.APIProvider.Single
{
    public partial class SingleProvider : IInstrumentProvider
    {
        public override void Send(InstrumentDefinitionRequest request)
        {
            // 改成异步，不然由于合约太多，可能界面没有回应
            Task.Factory.StartNew(() => ReturnInstrumentDefinition(request));
        }

        private void ReturnInstrumentDefinition(InstrumentDefinitionRequest request)
        {
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();

            List<Instrument> instruments = new List<Instrument>();

            // 不知道这样能不能解决死锁的问题
            List<InstrumentField> _instruments = new List<InstrumentField>(_dictInstruments.Values);

            foreach (InstrumentField contract in _instruments)
            {
                SmartQuant.InstrumentType instrumentType = (SmartQuant.InstrumentType)contract.Type;

                // filter by type
                if (request.FilterType != null
                    && request.FilterType != instrumentType)
                    continue;

                // filter by exchange
                if (request.FilterExchange != null && !contract.ExchangeID.StartsWith(request.FilterExchange, StringComparison.CurrentCultureIgnoreCase))
                    continue;

                // filter by symbol
                if (request.FilterSymbol != null
                    && !contract.Symbol.StartsWith(request.FilterSymbol, StringComparison.CurrentCultureIgnoreCase))
                    continue;

                Instrument instrument = null;

                //if (UpdateInstrument)
                //{
                //    instrument = framework.InstrumentManager.Get(contract.Symbol);
                //    //if (instrument != null)
                //    //{
                //    //    AltId ai = instrument.AltId.Get(this.id);
                //    //    if (ai != null)
                //    //    {
                //    //        instrument.AltId.Remove(ai);
                //    //    }
                //    //}
                //}

                if (instrument == null)
                {
                    instrument = new Instrument(instrumentType, contract.Symbol);
                }

                instrument.AltId.Add(new AltId(this.id, contract.InstrumentID, contract.ExchangeID));

                instrument.PutCall = (SmartQuant.PutCall)contract.OptionsType;
                instrument.Strike = contract.StrikePrice;

                instrument.Exchange = contract.ExchangeID;
                instrument.CurrencyId = CurrencyId.CNY;
                instrument.TickSize = contract.PriceTick;
                instrument.Factor = contract.VolumeMultiple;

                double x = instrument.TickSize;
                if (x > 0.000001)
                {
                    int i = 0;
                    for (; x - (int)x != 0; ++i)
                    {
                        x = x * 10;
                    }
                    instrument.PriceFormat = string.Format("F{0}", i);
                }

                var description = new InstrumentJson();
                description.InstrumentName = contract.InstrumentName();
                description.ProductID = contract.ProductID;
                description.Underlying = contract.UnderlyingInstrID;
                instrument.Description = JsonConvert.SerializeObject(description, description.GetType(), null);


                if (!string.IsNullOrWhiteSpace(contract.UnderlyingInstrID))
                {
                    // 要求先导入标的合约，再导入期权，如果在这里生成会丢失合约信息
                    Instrument UnderlyingInstrID = framework.InstrumentManager.Get(contract.UnderlyingInstrID);
                    if (UnderlyingInstrID == null)
                    {
                        //xlog.Warn("合约:{0},存在标的物字段，请先导入标的物合约{1},导入后放在Parent属性中", contract.Symbol, contract.UnderlyingInstrID);
                        List<string> list = null;
                        if (!dict.TryGetValue(contract.UnderlyingInstrID, out list))
                        {
                            list = new List<string>();
                            dict.Add(contract.UnderlyingInstrID, list);
                        }
                        list.Add(contract.Symbol);
                    }
                    else
                    {
                        instrument.Legs.Add(new Leg(UnderlyingInstrID));
                        //= framework.InstrumentManager.Get(contract.UnderlyingInstrID);
                    }
                }

                if (contract.ExpireDate > 0)
                {
                    int yyyy = contract.ExpireDate / 10000;
                    int MM = contract.ExpireDate % 10000 / 100;
                    int dd = contract.ExpireDate % 100;
                    // 居然有期权传回来的到期时间没有日
                    dd = Math.Max(dd, 1);
                    instrument.Maturity = new DateTime(yyyy, MM, dd);
                }

                //if (UpdateInstrument)
                //{
                //    framework.InstrumentManager.Save(instrument);
                //}

                instruments.Add(instrument);
            }

            if (dict.Count > 0)
            {
                xlog.Warn("标的物合约必须先导入,然后再Request,衍生品合约的Legs才会有一条记录指向标的物");
                foreach (var kv in dict)
                {
                    xlog.Warn("错过的标的物合约:{0},错过的衍生品合约数量{1}", kv.Key, kv.Value.Count);
                }
                xlog.Warn("请将以上合约先导入");
                xlog.Warn("如果在Request后列表中没有标的物合约，则需要手工添加");
            }


            instruments.Sort(SortInstrument);

            //
            InstrumentDefinition definition = new InstrumentDefinition();

            definition.RequestId = request.Id;
            definition.ProviderId = this.id;
            definition.Instruments = instruments.ToArray();
            definition.TotalNum = instruments.Count;

            EmitInstrumentDefinition(definition);

            //
            InstrumentDefinitionEnd end = new InstrumentDefinitionEnd();

            end.RequestId = request.Id;
            end.Result = RequestResult.Completed;
            end.Text = string.Format("{0:n0} instrument(s) found.", instruments.Count);

            EmitInstrumentDefinitionEnd(end);
        }

        private static int SortInstrument(Instrument a1, Instrument a2)
        {
            return a1.Symbol.CompareTo(a2.Symbol);
        }

        void IInstrumentProvider.Cancel(string requestId)
        {
            throw new NotImplementedException();
        }
    }
}
