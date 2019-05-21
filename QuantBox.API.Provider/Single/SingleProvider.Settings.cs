using NLog;
using QuantBox.APIProvider.UI;
using XAPI;
using SmartQuant;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.APIProvider.Single
{
    public partial class SingleProvider : Provider
    {
        private const string CATEGORY_SETTINGS = "Settings";
        private const string CATEGORY_COMMON = "Settings - Common";
        private const string CATEGORY_LEVEL2 = "Settings - Level2";
        private const string CATEGORY_MARKETDATA = "Settings - MarketData";
        private const string CATEGORY_TRADE = "Settings - Trade";
        private const string CATEGORY_QUOTE_REQUEST = "Settings - QuoteRequest";
        private const string CATEGORY_INSTRUMENT = "Settings - Instrument";
        private const string CATEGORY_HISTORICAL_DATA = "Settings - HistoricalData";
        private const string CATEGORY_LOG_INFO = "Settings - Log Info";

        private bool _enableEmitData;
        private bool _emitBidAsk;
        private bool _emitBidAskFirst;
        private bool _emitLevel2Snapshot;


        #region 行情配置
        [Category(CATEGORY_QUOTE_REQUEST)]
        [Description("【询价】订阅询价")]
        public bool SubscribeQuote { get; set; }
        [Category(CATEGORY_MARKETDATA)]
        [Description("【行情】是否触发EmitData事件(包括OnTrade/OnBid/OnAsk)")]
        [DisplayName("EmitData")]
        public bool EnableEmitData
        {
            get { return _enableEmitData; }
            set { _enableEmitData = value; }
        }
        [Category(CATEGORY_MARKETDATA)]
        [Description("【行情】触发OnBid/OnAsk")]
        public bool EmitBidAsk
        {
            get { return _emitBidAsk; }
            set { _emitBidAsk = value; }
        }
        [Category(CATEGORY_MARKETDATA)]
        [Description("【行情】将OnBid/OnAsk提前到OnTrade")]
        public bool EmitBidAskFirst
        {
            get { return _emitBidAskFirst; }
            set { _emitBidAskFirst = value; }
        }
        [Category(CATEGORY_MARKETDATA)]
        [Description("【行情】触发OnLevel2Snapshot事件")]
        public bool EmitLevel2Snapshot
        {
            get { return _emitLevel2Snapshot; }
            set { _emitLevel2Snapshot = value; }
        }

        #endregion

        #region 合约配置
        //[Category(CATEGORY_INSTRUMENT)]
        //[Description("【合约】对已经存在的合约，在Request时更新,包括AltId/Strike/Factor等")]
        //public bool UpdateInstrument { get; set; }
        #endregion

        #region 交易配置
        [Category(CATEGORY_TRADE)]
        [Description("【交易】默认开平设置")]
        public OpenCloseType DefaultOpenClose { get; set; }
        [Category(CATEGORY_TRADE)]
        [Description("【交易】默认投机保值设置")]
        public HedgeFlagType DefaultHedgeFlag { get; set; }
        [Category(CATEGORY_TRADE)]
        [Description("【交易】默认业务类型设置")]
        public BusinessType DefaultBusiness { get; set; }
        [Category(CATEGORY_TRADE)]
        [Description("【交易】默认PortfolioID1")]
        public string DefaultPortfolioID1 { get; set; }
        [Category(CATEGORY_TRADE)]
        [Description("【交易】默认PortfolioID2")]
        public string DefaultPortfolioID2 { get; set; }
        [Category(CATEGORY_TRADE)]
        [Description("【交易】默认PortfolioID3")]
        public string DefaultPortfolioID3 { get; set; }
        [Category(CATEGORY_TRADE)]
        [Description("【交易】模拟市价时在最新价的基础上加N跳")]
        public int LastPricePlusNTicks { get; set; }
        [Category(CATEGORY_TRADE)]
        [Description("【交易】市价单使用限价单来模拟")]
        public bool SwitchMakertOrderToLimitOrder { get; set; }
        [Category(CATEGORY_TRADE)]
        [Description("【交易】有价格限制，需要用涨跌停调整价格")]
        public bool HasPriceLimit { get; set; }
        [Category(CATEGORY_TRADE)]
        [Description("【交易】查询【资金】间隔")]
        public int QueryAccountInterval
        {
            get { return _QueryAccountInterval; }
            set
            {
                _QueryAccountInterval = value;
                if (_QueryAccountInterval < 1)
                    _QueryAccountInterval = 1;
                _QueryAccountCount = _QueryAccountInterval;
            }
        }
        [Category(CATEGORY_TRADE)]
        [Description("【交易】查询【持仓】间隔")]
        public int QueryPositionInterval
        {
            get { return _QueryPositionInterval; }
            set
            {
                _QueryPositionInterval = value;
                if (_QueryPositionInterval < 1)
                    _QueryPositionInterval = 1;
                _QueryPositionCount = _QueryPositionInterval;
            }
        }
        #endregion


        #region 通用
        [Category(CATEGORY_COMMON)]
        [Description("配置文件路径")]
        [Editor(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(UITypeEditor))]
        public string ConfigPath { get; set; }

        [Category(CATEGORY_COMMON)]
        [Description("交易时段列表，当前时间在这些列表中将启用重连机制，不在此列表中将主动断开，列表为空将不处理")]
        public BindingList<SessionTimeItem> SessionTimeList { get; set; }
        #endregion

        [Category(CATEGORY_SETTINGS), Editor(typeof(ApiManagerTypeEditor), typeof(UITypeEditor)),
        Description("综合设置")]
        public string AllConfig { get; set; }

        [Category(CATEGORY_SETTINGS), Editor(typeof(ApiControlTypeEditor), typeof(UITypeEditor)),
            Description("综合控制")]
        public string AllControl { get; set; }

        [Browsable(false)]
        [Category(CATEGORY_SETTINGS)]
        [Description("服务器列表")]
        public BindingList<ServerItem> ServerList { get; set; }
        [Browsable(false)]
        [Category(CATEGORY_SETTINGS)]
        [Description("用户列表")]
        public BindingList<UserItem> UserList { get; set; }
        [Browsable(false)]
        [Category(CATEGORY_SETTINGS)]
        [Description("API列表")]
        public BindingList<ApiItem> ApiList { get; set; }

        #region 历史数据
        [Category(CATEGORY_HISTORICAL_DATA)]
        [Description("【历史】同时将Tick数据中的Bid/Ask/Trade都保存下来")]
        public bool EmitAllTickType { get; set; }

        [Category(CATEGORY_HISTORICAL_DATA)]
        [Description("【历史】利用NLog将历史数据存文件")]
        public bool SaveToCsv { get; set; }

        [Category(CATEGORY_HISTORICAL_DATA)]
        [Description("【历史】是否触发EmitHistoricalData事件")]
        [DisplayName("EmitHistoricalData")]
        public bool EnablEmitHistoricalData { get; set; }

        [Category(CATEGORY_HISTORICAL_DATA)]
        [Description("【历史】是否过滤数据日期和时间")]
        public bool FilterDateTime { get; set; }
        #endregion

        [Category(CATEGORY_LOG_INFO)]
        [Description("【日志】是否显示OnRspQryInvestorPosition记录")]
        public bool IsLogOnRspQryInvestorPosition { get; set; }
        [Category(CATEGORY_LOG_INFO)]
        [Description("【日志】是否显示OnRspQryTradingAccount记录")]
        public bool IsLogOnRspQryTradingAccount { get; set; }
        [Category(CATEGORY_LOG_INFO)]
        [Description("【日志】是否显示OnRtnInstrumentStatus记录")]
        public bool IsLogOnRtnInstrumentStatus { get; set; }
    }
}
