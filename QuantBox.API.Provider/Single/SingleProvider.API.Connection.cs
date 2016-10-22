using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using XAPI.Callback;
using XAPI;
using NLog;
using QuantBox.Extensions;

namespace QuantBox.APIProvider.Single
{
    public partial class SingleProvider
    {
        // 实际的连接，由这个来向
        internal XApi _TdApi;
        internal XApi _MdApi;
        internal XApi _L2Api;
        internal XApi _QuoteRequestApi;
        internal XApi _HdApi;
        internal XApi _ItApi;
        internal XApi _QueryApi;

        private void _Connect(bool bFromUI)
        {
            lock(this)
            {
                if (IsConnected)
                    return;

                if (!IsConnected && !IsDisconnected)
                    _Disconnect(bFromUI);

                if (!bFromUI)
                    xlog.Info("插件尝试连接");

                bool bCheckOk = false;
                foreach(var item in ApiList)
                {
                    if(item.UseType>0)
                    {
                        bCheckOk = true;
                    }
                }

                if (false == bCheckOk)
                {
                    base.Status = ProviderStatus.Disconnected;
                    return;
                }

                // 连接前清理一下
                Clear();

                // 如果已经连接过一次，取消一个功能，再连，会再连上
                _TdApi = null;
                _MdApi = null;
                _L2Api = null;
                _QuoteRequestApi = null;
                _HdApi = null;
                _ItApi = null;
                _QueryApi = null;

                foreach (var item in ApiList)
                {
                    if (item.UseType > 0)
                    {
                        XApi api = ConnectToApi(item);
                        assign(item, api);
                    }
                    else
                    {
                        DisconnectToApi(item);
                    }
                }

                // 这个地方也会导致行情接收不到
                base.Status = ProviderStatus.Connecting;
            }
        }

        private void assign(ApiItem item,XApi api)
        {
            if ((item.UseType & ApiType.MarketData) == ApiType.MarketData)
            {
                _MdApi = api;
            }
            if ((item.UseType & ApiType.Trade) == ApiType.Trade)
            {
                _TdApi = api;
            }
            if ((item.UseType & ApiType.QuoteRequest) == ApiType.QuoteRequest)
            {
                _QuoteRequestApi = api;
            }
            if ((item.UseType & ApiType.Level2) == ApiType.Level2)
            {
                _L2Api = api;
            }
            if ((item.UseType & ApiType.HistoricalData) == ApiType.HistoricalData)
            {
                _HdApi = api;
            }
            if ((item.UseType & ApiType.Instrument) == ApiType.Instrument)
            {
                _ItApi = api;
            }
            if ((item.UseType & ApiType.Query) == ApiType.Query)
            {
                _QueryApi = api;
            }
        }

        public XApi GetXApi(ApiType type)
        {
            switch(type)
            {
                case ApiType.Trade:
                    return _TdApi;
                case ApiType.MarketData:
                    return _MdApi;
                case ApiType.Level2:
                    return _L2Api;
                case ApiType.HistoricalData:
                    return _HdApi;
                case ApiType.Instrument:
                    return _ItApi;
                case ApiType.Query:
                    return _QueryApi;
                default:
                    return null;
            }
        }

        void _Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _Timer.Enabled = false;

            do
            {
                // 列表为空，表示不处理。这时没有自动重连
                if (SessionTimeList == null || SessionTimeList.Count == 0)
                    break;

                bool bTryConnect = true;

                foreach (var st in SessionTimeList.ToList())
                {
                    // 如果当前时间在交易范围内，要开启重连
                    // 如果当前时间不在交易范围内，要主动断开
                    TimeSpan ts = e.SignalTime.TimeOfDay;
                    if (ts < st.SessionStart)
                    {
                        // 停
                        bTryConnect = false;
                    }
                    else if (ts <= st.SessionEnd)
                    {
                        // 启动
                        bTryConnect = true;
                        break;
                    }
                    else
                    {
                        // 停
                        bTryConnect = false;
                    }
                }

                if (bTryConnect)
                {
                    // 没有连接要连上，有连接要设置时间
                    if (!IsConnected || (!IsConnected && !IsDisconnected))
                    {
                        xlog.Info("当前[{0}]在交易时段，主动连接", e.SignalTime.TimeOfDay);
                        _Connect(false);
                    }

                    foreach (var item in ApiList)
                    {
                        if (item.UseType > 0 && item.Api != null)
                        {
                            item.Api.ReconnectInterval = _ReconnectInterval;
                        }
                    }
                }
                else
                {
                    foreach (var item in ApiList)
                    {
                        if (item.Api != null)
                        {
                            item.Api.ReconnectInterval = 0;
                        }
                    }

                    if (IsConnected || (!IsConnected && !IsDisconnected))
                    {
                        xlog.Info("当前[{0}]在非交易时段，主动断开连接", e.SignalTime.TimeOfDay);
                        // 要断开连接
                        _Disconnect(false);
                    }
                }
            }while(false);

            // 查询持仓和资金
            if (IsApiConnected(_QueryApi))
            {
                _QueryAccountCount -= (int)_Timer.Interval / 1000;
                if(_QueryAccountCount <= 0)
                {
                    ReqQueryField query = default(ReqQueryField);

                    query.PortfolioID1 = DefaultPortfolioID1;
                    query.PortfolioID2 = DefaultPortfolioID2;
                    query.PortfolioID3 = DefaultPortfolioID3;
                    query.Business = DefaultBusiness;

                    _QueryApi.ReqQuery(QueryType.ReqQryTradingAccount, ref query);
                    _QueryAccountCount = _QueryAccountInterval;
                }

                _QueryPositionCount -= (int)_Timer.Interval / 1000;
                if (_QueryPositionCount <= 0)
                {
                    ReqQueryField query = default(ReqQueryField);

                    query.PortfolioID1 = DefaultPortfolioID1;
                    query.PortfolioID2 = DefaultPortfolioID2;
                    query.PortfolioID3 = DefaultPortfolioID3;
                    query.Business = DefaultBusiness;

                    _QueryApi.ReqQuery(QueryType.ReqQryInvestorPosition, ref query);
                    _QueryPositionCount = _QueryPositionInterval;
                }
            }
            
            _Timer.Enabled = true;
        }

        private XApi ConnectToApi(ApiItem item)
        {
            lock(this)
            {
                DisconnectToApi(item);
                
                XApi api = item.Api;

                if (api == null)
                {
                    api = new XApi(PathHelper.MakeAbsolutePath(item.DllPath));
                    item.Api = api;
                }

                api.Server = ServerList[item.Server].ToStruct();
                if(item.UserList.Count>0)
                {
                    foreach(var it in item.UserList)
                    {
                        api.UserList.Add(it.ToStruct());
                    }
                }
                else
                {
                    api.User = UserList[item.User].ToStruct();
                }

                // 更新Log名字，这样在日志中可以进行识别
                api.Log = LogManager.GetLogger(string.Format("{0}.{1}.{2}", Name, item.LogPrefix, api.User.UserID));

                if (api.IsConnected)
                    return api;

                api.OnConnectionStatus = OnConnectionStatus_callback;
                api.OnRtnError = OnRtnError_callback;
                api.OnLog = OnLog_callback;

                api.OnRtnDepthMarketData = OnRtnDepthMarketData_callback;

                api.OnRspQryInstrument = OnRspQryInstrument_callback;
                api.OnRspQryTradingAccount = OnRspQryTradingAccount_callback;
                api.OnRspQryInvestor = OnRspQryInvestor_callback;
                api.OnRspQryInvestorPosition = OnRspQryInvestorPosition_callback;

                api.OnRspQryOrder = OnRspQryOrder_callback;
                api.OnRspQryTrade = OnRspQryTrade_callback;
                api.OnRspQryQuote = OnRspQryQuote_callback;

                api.OnRtnOrder = OnRtnOrder_callback;
                api.OnRtnTrade = OnRtnTrade_callback;
                api.OnRtnQuote = OnRtnQuote_callback;

                api.OnRtnQuoteRequest = OnRtnQuoteRequest_callback;

                api.OnRspQryHistoricalTicks = OnRspQryHistoricalTicks_callback;
                api.OnRspQryHistoricalBars = OnRspQryHistoricalBars_callback;

                api.OnRspQrySettlementInfo = OnRspQrySettlementInfo;

                api.Connect();

                return api;
            }
        }


        private void DisconnectToApi(ApiItem item)
        {
            lock(this)
            {
                if (item.Api != null)
                {
                    item.Api.Dispose();
                    item.Api = null;
                }
            }
        }
        
        private void _Disconnect(bool bFromUI)
        {
            lock (this)
            {
                if (IsConnected || (!IsConnected && !IsDisconnected))
                {
                    if(!bFromUI)
                        xlog.Info("插件尝试断开");

                    foreach (var item in ApiList)
                    {
                        if (item.UseType > 0)
                        {
                            DisconnectToApi(item);
                        }
                    }

                    base.Status = ProviderStatus.Disconnected;
                }
            }
        }

        private void OnConnectionStatus_callback(object sender, ConnectionStatus status, ref RspUserLoginField userLogin, int size1)
        {
            if(size1>0)
            {
                if(userLogin.RawErrorID != 0 )
                {
                    (sender as XApi).GetLog().Info("{0}:{1}", status, userLogin.ToFormattedStringShort());
                }
                else
                {
                    (sender as XApi).GetLog().Info("{0}:{1}", status, userLogin.ToFormattedStringLong());
                }
            }
            else
            {
                (sender as XApi).GetLog().Info("{0}", status);
            }

            switch (status)
            {
                case ConnectionStatus.Done:
                    OnConnectionStatus_Done(sender, status);
                    break;
                case ConnectionStatus.Disconnected:
                    OnConnectionStatus_Disconnected(sender, status, ref userLogin);
                    break;
            }
            // 断线重连的功能，可能正好与连接上的时间在同一时点，所以想法重新计时
            _Timer.Enabled = false;
            _Timer.Enabled = true;
        }

        private void OnConnectionStatus_Done(object sender, ConnectionStatus status)
        {
            bool bCheckOk = true;

            foreach (var item in ApiList)
            {
                if (item.UseType > 0)
                {
                    if(!IsApiConnected(item.Api))
                    {
                        bCheckOk = false;
                        break;
                    }
                }
            }

            if(bCheckOk)
            {
                base.Status = ProviderStatus.Connected;
                
                ReqQueryField query = default(ReqQueryField);
                query.PortfolioID1 = DefaultPortfolioID1;
                query.PortfolioID2 = DefaultPortfolioID2;
                query.PortfolioID3 = DefaultPortfolioID3;
                query.Business = DefaultBusiness;

                // 查持仓，查资金
                if (_QueryApi != null)
                {
                    _QueryApi.ReqQuery(QueryType.ReqQryTradingAccount, ref query);
                    _QueryApi.ReqQuery(QueryType.ReqQryInvestorPosition, ref query);
                }

                // 查合约
                if (_ItApi != null)
                {
                    _ItApi.ReqQuery(QueryType.ReqQryInstrument, ref query);
                }
            }
        }

        private void OnConnectionStatus_Disconnected(object sender, ConnectionStatus status, ref RspUserLoginField userLogin)
        {
            if (IsConnected)
            {
                //以前连接过，现在断了次线，要等重连
                base.Status = ProviderStatus.Connecting;
            }
            else
            {
                //从来没有连接成功过，可能是密码错误，直接退出

                //不能在线程中停止线程，这样会导致软件关闭进程不退出
                //_Disconnect();
                base.Status = ProviderStatus.Disconnected;
            }
        }

        private void OnRtnError_callback(object sender, ref ErrorField error)
        {
            (sender as XApi).GetLog().Error("OnRtnError:" + error.ToFormattedString());
        }

        private void OnLog_callback(object sender, ref LogField log)
        {
            (sender as XApi).GetLog().Info("OnLog:" + log.ToFormattedString());
        }

        private void OnRtnQuoteRequest_callback(object sender, ref QuoteRequestField quoteRequest)
        {
            (sender as XApi).GetLog().Info("OnRtnQuoteRequest:" + quoteRequest.ToFormattedString());

            MarketDataRecord record;
            if (!marketDataRecords.TryGetValue(quoteRequest.Symbol, out record))
            {
                return;
            }

            NewsEx news = new NewsEx(DateTime.Now, this.id, record.Instrument.Id, NewsUrgency.Flash, "", "", quoteRequest.ToFormattedString());
            news.ResponseType = XAPI.ResponseType.OnRtnQuoteRequest;
            news.UserData = quoteRequest;
            EmitData(news);
        }
    }
}
