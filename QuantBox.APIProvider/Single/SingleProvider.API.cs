using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XAPI.Callback;
using XAPI;
using NLog;
using System.Reflection;
using QuantBox.Extensions;
using System.IO;

namespace QuantBox.APIProvider.Single
{
    public partial class SingleProvider
    {
        static SingleProvider()
        {
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(Path.Combine(PathHelper.RootPath.LocalPath, "NLog.config"), true);
        }
        public DelegateOnRspQryInvestorPosition OnRspQryInvestorPosition { get; set; }
        public DelegateOnRspQryTradingAccount OnRspQryTradingAccount { get; set; }
        public DelegateOnRtnInstrumentStatus OnRtnInstrumentStatus { get; set; }
        public DelegateOnRtnOrder OnRtnOrder { get; set; }
        public DelegateOnRtnTrade OnRtnTrade { get; set; }

        public DelegateOnRspQryOrder OnRspQryOrder { get; set; }
        public DelegateOnRspQryTrade OnRspQryTrade { get; set; }

        //记录合约列表,从实盘合约名到对象的映射
        private readonly Dictionary<string, InstrumentField> _dictInstruments = new Dictionary<string, InstrumentField>();
        private readonly Dictionary<string, InstrumentStatusField> _dictInstrumentsStatus = new Dictionary<string, InstrumentStatusField>();

        public static int GetDate(DateTime dt)
        {
            return dt.Year * 10000 + dt.Month * 100 + dt.Day;
        }

        public static int GetTime(DateTime dt)
        {
            return dt.Hour * 10000 + dt.Minute * 100 + dt.Second;
        }

        public static DateTime GetDateTime(int yyyyMMdd, int hhmmss, int Millisecond)
        {
            int yyyy = yyyyMMdd / 10000;
            int MM = yyyyMMdd % 10000 / 100;
            int dd = yyyyMMdd % 100;
            int hh = hhmmss / 10000;
            int mm = hhmmss % 10000 / 100;
            int ss = hhmmss % 100;
            DateTime dt = new DateTime(yyyy, MM, dd, hh, mm, ss, Millisecond);
            return dt;
        }

        private void OnRspQryInstrument_callback(object sender, ref InstrumentField instrument, int size1, bool bIsLast)
        {
            if (size1 <= 0)
            {
                (sender as XApi).GetLog().Info("OnRspQryInstrument");
                return;
            }

            _dictInstruments[instrument.Symbol] = instrument;

            if (bIsLast)
            {
                (sender as XApi).GetLog().Info("合约列表已经接收完成,共 {0} 条", _dictInstruments.Count);
            }
        }

        private void OnRspQryTradingAccount_callback(object sender, ref AccountField account, int size1, bool bIsLast)
        {
            if (size1 <= 0)
            {
                (sender as XApi).GetLog().Info("OnRspQryTradingAccount");
            }
            else
            {
                if (IsLogOnRspQryTradingAccount)
                    (sender as XApi).GetLog().Info("OnRspQryTradingAccount:" + account.ToFormattedString());
            }

            // 由策略来收回报
            if (OnRspQryTradingAccount != null)
                OnRspQryTradingAccount(sender, ref account, size1, bIsLast);

            if (size1 <= 0)
                return;

            if (!IsConnected)
                return;

            string currency = "CNY";

            AccountData ad = new AccountData(DateTime.Now, AccountDataType.AccountValue,
                account.AccountID, this.id, this.id);

            Type type = typeof(AccountField);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                ad.Fields.Add(field.Name, currency, field.GetValue(account));
            }
            // 将对像完全设置进去，等着取出
            ad.Fields.Add(AccountDataFieldEx.USER_DATA, currency, account);
            ad.Fields.Add(AccountDataFieldEx.DATE, currency, GetDate(DateTime.Today));


            try
            {
                EmitAccountData(ad);
            }
            catch (Exception ex)
            {
                (sender as XApi).GetLog().Error(ex);
            }
        }

        private void OnRspQryInvestor_callback(object sender, ref InvestorField investor, int size1, bool bIsLast)
        {
            if (size1 <= 0)
            {
                (sender as XApi).GetLog().Info("OnRspQryInvestor");
                return;
            }

            (sender as XApi).GetLog().Info("OnRspQryInvestor:{0}", investor.ToFormattedString());
        }

        private void OnRspQryInvestorPosition_callback(object sender, ref PositionField position, int size1, bool bIsLast)
        {
            if (size1 <= 0)
            {
                (sender as XApi).GetLog().Info("OnRspQryInvestorPosition");
            }
            else if (position.Position != 0)
            {
                // UFX中已经过期的持仓也会推送，所以这里过滤一下不显示
                if (IsLogOnRspQryInvestorPosition)
                    (sender as XApi).GetLog().Info("OnRspQryInvestorPosition:" + position.ToFormattedString());
            }

            // 由策略来收回报
            if (OnRspQryInvestorPosition != null)
                OnRspQryInvestorPosition(sender, ref position, size1, bIsLast);

            if (size1 <= 0)
                return;

            if (!IsConnected)
                return;

            PositionFieldEx item;
            if (!positions.TryGetValue(position.Symbol, out item))
            {
                item = new PositionFieldEx();
                positions[position.Symbol] = item;
            }
            item.AddPosition(position);

            AccountData ad = new AccountData(DateTime.Now, AccountDataType.Position,
                position.AccountID, this.id, this.id);

            ad.Fields.Add(AccountDataField.SYMBOL, item.Symbol);
            ad.Fields.Add(AccountDataField.EXCHANGE, item.Exchange);
            ad.Fields.Add(AccountDataField.QTY, item.Qty);
            ad.Fields.Add(AccountDataField.LONG_QTY, item.LongQty);
            ad.Fields.Add(AccountDataField.SHORT_QTY, item.ShortQty);

            ad.Fields.Add(AccountDataFieldEx.USER_DATA, item);
            ad.Fields.Add(AccountDataFieldEx.DATE, GetDate(DateTime.Today));

            try
            {
                EmitAccountData(ad);
            }
            catch (Exception ex)
            {
                (sender as XApi).GetLog().Error(ex);
            }
        }

        private void OnRspQrySettlementInfo_callback(object sender, ref SettlementInfoClass settlementInfo, int size1, bool bIsLast)
        {
            if (size1 <= 0)
            {
                (sender as XApi).GetLog().Info("OnRspQrySettlementInfo");
                return;
            }

            if (bIsLast)
            {
                (sender as XApi).GetLog().Info("OnRspQrySettlementInfo:" + Environment.NewLine + settlementInfo.Content);
            }
        }


        private void OnRspQryQuote_callback(object sender, ref QuoteField quote, int size1, bool bIsLast)
        {
            if (size1 <= 0)
            {
                (sender as XApi).GetLog().Info("OnRspQryQuote");
                return;
            }

            (sender as XApi).GetLog().Info("OnRspQryQuote:" + quote.ToFormattedString());
        }

        private void OnRspQryTrade_callback(object sender, ref TradeField trade, int size1, bool bIsLast)
        {
            if (size1 <= 0)
            {
                (sender as XApi).GetLog().Info("OnRspQryTrade");

            }
            else
            {
                (sender as XApi).GetLog().Info("OnRspQryTrade:" + trade.ToFormattedString());
            }
            if (OnRspQryTrade != null)
                OnRspQryTrade(this, ref trade, size1, bIsLast);
        }

        private void OnRspQryOrder_callback(object sender, ref OrderField order, int size1, bool bIsLast)
        {
            if (size1 <= 0)
            {
                (sender as XApi).GetLog().Info("OnRspQryOrder");
            }
            else
            {
                (sender as XApi).GetLog().Info("OnRspQryOrder:" + order.ToFormattedString());
            }
            if (OnRspQryOrder != null)
                OnRspQryOrder(this, ref order, size1, bIsLast);
        }

        private void OnRtnInstrumentStatus_callback(object sender, ref InstrumentStatusField instrumentStatus)
        {
            if (OnRtnInstrumentStatus != null)
                OnRtnInstrumentStatus(sender, ref instrumentStatus);

            // 记录下来，后期可能要用到
            _dictInstrumentsStatus[instrumentStatus.Symbol] = instrumentStatus;

            // 合约状态信息太多了，也不关心，这里屏蔽显示
            if (IsLogOnRtnInstrumentStatus)
                (sender as XApi).GetLog().Info("OnRtnInstrumentStatus:" + instrumentStatus.ToFormattedString());
        }

        public InstrumentStatusField GetInstrumentStatus(string symbol)
        {
            InstrumentStatusField instrumentStatus;
            if (_dictInstrumentsStatus.TryGetValue(symbol, out instrumentStatus))
            {
                return instrumentStatus;
            }
            return instrumentStatus;
        }
    }
}
