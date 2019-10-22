using SmartQuant;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAPI;


namespace QuantBox.APIProvider.Single
{
    public partial class SingleProvider : IHistoricalDataProvider
    {
        private HistoricalDataRequestField ToStruct(HistoricalDataRequest request)
        {
            string altSymbol;
            string altExchange;
            string apiSymbol;
            string apiExchange;
            double apiTickSize;

            GetApi_Symbol_Exchange_TickSize(request.Instrument, this.id,
                out altSymbol, out altExchange,
                out apiSymbol, out apiExchange,
                out apiTickSize);

            HistoricalDataRequestField field = new HistoricalDataRequestField();
            field.Symbol = request.Instrument.Symbol;
            field.InstrumentID = apiSymbol;
            field.ExchangeID = apiExchange;
            field.Date1 = GetDate(request.DateTime1);
            field.Date2 = GetDate(request.DateTime2);
            field.Time1 = GetTime(request.DateTime1);
            field.Time2 = GetTime(request.DateTime2);
            field.DataType = (XAPI.DataObjetType)request.DataType;
            if (request.BarType.HasValue)
                field.BarType = (XAPI.BarType)request.BarType.Value;
            if (request.BarSize.HasValue)
                field.BarSize = (int)request.BarSize.Value;
            //field.RequestId;
            //field.Count;

            return field;
        }

        private Dictionary<int, HistoricalDataRecord> historicalDataRecords;
        private Dictionary<string, int> historicalDataIds;
        void IHistoricalDataProvider.Cancel(string requestId)
        {
            Console.WriteLine(requestId);
        }
        public override void Send(HistoricalDataRequest request)
        {
            if (!IsApiConnected(_HdApi))
            {
                EmitHistoricalDataEnd(request.RequestId, RequestResult.Error, "Provider is not connected.");
                xlog.Error("历史行情服务器没有连接");
                return;
            }

            int iRet = 1;
            switch (request.DataType)
            {
                case DataObjectType.Bid:
                case DataObjectType.Ask:
                case DataObjectType.Trade:
                case DataObjectType.Quote:
                    iRet = GetHistoricalTicks(ToStruct(request));
                    break;
                case DataObjectType.Bar:
                    iRet = GetHistoricalBars(ToStruct(request));
                    break;
            }
            historicalDataRecords.Add(iRet, new HistoricalDataRecord(request));
            historicalDataIds.Add(request.RequestId, iRet);
        }

        private int GetHistoricalTicks(HistoricalDataRequestField request)
        {
            return 1;
            //return _HdApi.ReqQryHistoricalTicks(ref request);
        }

        private int GetHistoricalBars(HistoricalDataRequestField request)
        {
            return 1;
            //return _HdApi.ReqQryHistoricalBars(ref request);
        }
    }
}
