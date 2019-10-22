using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XAPI.Callback;
using XAPI;
using System.Runtime.InteropServices;
using NLog;

namespace QuantBox.APIProvider.Single
{
    /*

<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
    xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
    autoReload="true">
    <variable name="logDirectory" value="${basedir}/logs"/>
    <variable name="csvDirectory" value="${basedir}/csv"/>
    <targets async="false">
        <target name="c" xsi:type="Console" />
        <target name="f" xsi:type="File" fileName="${logDirectory}/${shortdate}.log"/>
        <!-- "DateTime,Open,High,Low,Close,Volume,OpenInt" -->
        <target name="target_Bar" xsi:type="File" fileName="${logDirectory}/${event-context:item=Symbol}.${event-context:item=BarSize}.csv" layout="${message}"/>
        <!-- "DateTime,Price,Size,OpenInt,Bid,BidSize,Ask,AskSize" -->
        <target name="target_Tick" xsi:type="File" fileName="${logDirectory}/${event-context:item=Symbol}_${event-context:item=Date}.csv" layout="${message}"/>
    </targets>
    <rules>
        <logger name="*" minlevel="Debug" writeTo="c" />
        <logger name="*" minlevel="Debug" writeTo="f" />
        <logger name="Bar" writeTo="target_Bar" />
        <logger name="Tick" writeTo="target_Tick" />
  </rules>
</nlog>
     
     */
    public partial class SingleProvider
    {
        private const string Symbol = "Symbol";
        private const string Date = "Date";
        private const string BarSize = "BarSize";

        private string TickFieldToString(TickField field, DateTime dt, double volume)
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                dt.ToString("yyyy-MM-dd HH:mm:ss.fff"), field.LastPrice, volume, field.OpenInterest,
                field.BidPrice1,field.BidSize1,field.AskPrice1,field.AskSize1);
        }
        private string BarFieldToString(BarField field, DateTime dt)
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6}",
                dt.ToString("yyyy-MM-dd HH:mm:ss.fff"), field.Open, field.High, field.Low, field.Close, field.Volume, field.OpenInterest);
        }

        private bool FilterDateTime_(HistoricalDataRequest request,DateTime dt)
        {
            if(dt>request.DateTime2 || dt<request.DateTime1)
                return false;
            return true;
        }

        private void OnRspQryHistoricalTicks_callback(object sender, IntPtr pTicks, int size1, ref HistoricalDataRequestField request, int size2, bool bIsLast)
        {
            int size = Marshal.SizeOf(typeof(TickField));

            (sender as XApi).GetLog().Info("<--OnRspQryHistoricalTicks:{0},{1},{2},{3}条", request.CurrentDate, request.InstrumentID, request.ExchangeID, size1 / size);
            HistoricalDataRecord record;
            if(!historicalDataRecords.TryGetValue(request.RequestId,out record))
                return;

            int day = -1;
            double volume = 0;
            DateTime datetime = DateTime.MinValue;
            DateTime updatetime = DateTime.MinValue;

            List<DataObject> list = new List<DataObject>();
            
            for (int i = 0; i < size1 / size; ++i)
            {
                IntPtr ptr = (IntPtr)(pTicks + size * i);
                TickField obj = Marshal.PtrToStructure<TickField>(ptr);

                DateTime dt = GetDateTime(obj.Date,obj.Time,obj.Millisecond);
                if (datetime == dt)
                {
                    updatetime = updatetime.AddMilliseconds(100);
                }
                else
                {
                    updatetime = dt;
                }
                if (day != updatetime.Day)
                {
                    volume = 0;
                }
                day = updatetime.Day;
                volume = obj.Volume - volume;


                // 这地方应当加历史数据另存的地方才好
                if (SaveToCsv)
                {
                    LogEventInfo logEvent = new LogEventInfo(NLog.LogLevel.Trace, tickLog.Name, TickFieldToString(obj, updatetime, volume));

                    // 用户可能需要按收到数据的合约与时间分目录或文件
                    logEvent.Properties[Symbol] = record.Request.Instrument.Symbol;
                    logEvent.Properties[Date] = request.CurrentDate;

                    tickLog.Log(logEvent);
                }


                if(FilterDateTime)
                {
                    if(FilterDateTime_(record.Request, updatetime))
                    {
                        DataObject o = null;

                        if (EmitAllTickType)
                        {
                            // 全面保存数据
                            o = new Trade(updatetime, this.id, record.Request.Instrument.Id, obj.LastPrice, (int)volume);
                            list.Add(o);
                            o = new Quote(updatetime, this.id, record.Request.Instrument.Id, obj.BidPrice1, obj.BidSize1, obj.AskPrice1, obj.AskSize1);
                            list.Add(o);
                        }
                        else
                        {
                            // 分别保存
                            switch (record.Request.DataType)
                            {
                                case DataObjectType.Tick:
                                case DataObjectType.Bid:
                                    o = new Bid(updatetime, this.id, record.Request.Instrument.Id, obj.BidPrice1, obj.BidSize1);
                                    break;
                                case DataObjectType.Ask:
                                    o = new Ask(updatetime, this.id, record.Request.Instrument.Id, obj.AskPrice1, obj.AskSize1);
                                    break;
                                case DataObjectType.Trade:
                                    o = new Trade(updatetime, this.id, record.Request.Instrument.Id, obj.LastPrice, (int)volume);
                                    break;
                                case DataObjectType.Quote:
                                    o = new Quote(updatetime, this.id, record.Request.Instrument.Id, obj.BidPrice1, obj.BidSize1, obj.AskPrice1, obj.AskSize1);
                                    break;
                            }

                            list.Add(o);
                        }
                    }
                }

                datetime = dt;
                volume = obj.Volume;
            }

            if(EnablEmitHistoricalData)
            {
                HistoricalData data = new HistoricalData
                {
                    RequestId = record.Request.RequestId,
                    Objects = list.ToArray(),
                    TotalNum = list.Count,
                };

                //Console.WriteLine("============");
                //Console.WriteLine(list.Count);

                base.EmitHistoricalData(data);
            }
            
            if(bIsLast)
                EmitHistoricalDataEnd(record.Request.RequestId, RequestResult.Completed, "");
        }
        
        private void OnRspQryHistoricalBars_callback(object sender, IntPtr pBars, int size1, ref HistoricalDataRequestField request, int size2, bool bIsLast)
        {
            int size = Marshal.SizeOf(typeof(BarField));

            (sender as XApi).GetLog().Info("<--OnRspQryHistoricalBars:{0},{1},{2},{3}条", request.CurrentDate, request.InstrumentID, request.ExchangeID, size1 / size);
            HistoricalDataRecord record;
            if (!historicalDataRecords.TryGetValue(request.RequestId, out record))
                return;

            List<Bar> list = new List<Bar>();

            for (int i = 0; i < size1 / size; ++i)
            {
                IntPtr ptr = (IntPtr)(pBars + size * i);
                BarField obj = PInvokeUtility.GetObjectFromIntPtr<BarField>(ptr);

                int Millisecond = 0;
                DateTime dt = GetDateTime(obj.Date, obj.Time, Millisecond);

                // 这地方应当加历史数据另存的地方才好
                if (SaveToCsv)
                {
                    LogEventInfo logEvent = new LogEventInfo(NLog.LogLevel.Trace, tickLog.Name, BarFieldToString(obj, dt));

                    // 用户可能需要按收到数据的合约与时间分目录或文件
                    logEvent.Properties[Symbol] = record.Request.Instrument.Symbol;
                    logEvent.Properties[Date] = request.CurrentDate.ToString();
                    logEvent.Properties[BarSize] = request.BarSize.ToString();

                    barLog.Log(logEvent);
                }

                if (FilterDateTime)
                {
                    if (FilterDateTime_(record.Request, dt))
                    {
                        Bar b = new Bar(dt, dt.AddSeconds(record.Request.BarSize.Value), record.Request.Instrument.Id, record.Request.BarType.Value, record.Request.BarSize.Value,
                            obj.Open, obj.High, obj.Low, obj.Close, (long)obj.Volume, (long)obj.OpenInterest);
                        list.Add(b);
                    }
                }
            }

            if(EnablEmitHistoricalData)
            {
                HistoricalData data = new HistoricalData
                {
                    RequestId = record.Request.RequestId,
                    Objects = list.ToArray(),
                    TotalNum = list.Count,
                };

                base.EmitHistoricalData(data);
            }
            
            if (bIsLast)
                EmitHistoricalDataEnd(record.Request.RequestId, RequestResult.Completed, "");
        }
    }
}
