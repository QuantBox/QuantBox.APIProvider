using Newtonsoft.Json;
using NLog;
using SmartQuant;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAPI;
using XAPI.Callback;

//using System.Threading.Tasks.Dataflow;


namespace QuantBox.APIProvider.Single
{
    public partial class SingleProvider:Provider
    {
        private Logger xlog;
        private Logger barLog;
        private Logger tickLog;

        // 行情
        private Dictionary<string, MarketDataRecord> marketDataRecords;

        // 普通交易
        OrderMap orderMap;
        QuoteMap quoteMap;

        Dictionary<string, PositionFieldEx> positions;

        public SingleProvider(Framework framework)
            : base(framework)
        {
        }

        ~SingleProvider()
        {
            Save();

            // 将行情记录器清理
            //if(_tickWriter != null)
            //{
            //    _tickWriter.Close();
            //    _tickWriter = null;
            //}
        }

        private static JsonSerializerSettings jSetting = new JsonSerializerSettings()
        {
            // json文件格式使用非紧凑模式
            //NullValueHandling = NullValueHandling.Ignore,
            //DefaultValueHandling = DefaultValueHandling.Ignore,
            Formatting = Formatting.Indented,
        };

        public void Init(byte id, string name)
        {
            base.id = id;
            base.name = name;
            base.description = "QuantBox API Provider";
            base.url = "www.quantbox.cn";

            // 使用TPL DataFlow来进行行情的保存
            //Input = new ActionBlock<DepthMarketDataField>((x) => OnInputMarketData(x));


            // 只是简单设置，等登录时将把账号设置上，X日志由于一些信息没法
            xlog = LogManager.GetLogger(Name + ".X");
            barLog = LogManager.GetLogger("Bar");
            tickLog = LogManager.GetLogger("Tick");

            // 以下初始化的值在，初始化后由软件读取文件中的参数据后又设置回来
            LastPricePlusNTicks = 10;
            EmitBidAsk = true;
            //UpdateInstrument = true;
            EnableEmitHistoricalData = true;
            FilterDateTime = true;
            EnableEmitData = true;
            HasPriceLimit = true;

            // 这两个地方的路径如果为空，设置一个默认的
            if (string.IsNullOrEmpty(ConfigPath))
            {
                ConfigPath = Path.Combine(Installation.ConfigDir.FullName, Name);
            }
            Directory.CreateDirectory(ConfigPath);
            //if (string.IsNullOrEmpty(Pd0Path))
            //{
            //    Pd0Path = Path.Combine(Installation.DataDir.FullName, "PD0", Name);
            //}
            //Directory.CreateDirectory(Pd0Path);

            marketDataRecords = new Dictionary<string, MarketDataRecord>();

            orderMap = new OrderMap(framework, this);
            quoteMap = new QuoteMap(framework, this, orderMap);

            positions = new Dictionary<string, PositionFieldEx>();

            historicalDataRecords = new Dictionary<int, HistoricalDataRecord>();
            historicalDataIds = new Dictionary<string,int>();

            // ConfigPath在做Setting时已经做了
            Load();
        }

        void SessionTimeList_ListChanged(object sender, ListChangedEventArgs e)
        {
            Save(ConfigPath, "SessionTimeList.json", SessionTimeList);
        }

        void UserList_ListChanged(object sender, ListChangedEventArgs e)
        {
            Save(ConfigPath, "UserList.json", UserList);
        }

        void ServerList_ListChanged(object sender, ListChangedEventArgs e)
        {
            Save(ConfigPath, "ServerList.json", ServerList);
        }

        void ApiList_ListChanged(object sender, ListChangedEventArgs e)
        {
            Save(ConfigPath, "ApiList.json", ApiList);
        }

        private object Load(string path, string file, object obj)
        {
            try
            {
                object ret;
                using (TextReader reader = new StreamReader(Path.Combine(path, file)))
                {
                    ret = JsonConvert.DeserializeObject(reader.ReadToEnd(), obj.GetType());
                    reader.Close();
                }
                return ret;
            }
            catch
            {
            }
            return obj;
        }

        private void Save(string path,string file,object obj)
        {
            using (TextWriter writer = new StreamWriter(Path.Combine(path, file)))
            {
                writer.Write("{0}", JsonConvert.SerializeObject(obj, obj.GetType(), jSetting));
                writer.Close();
            }
        }

        internal void Save()
        {
            Save(ConfigPath, "SessionTimeList.json", SessionTimeList);
            Save(ConfigPath, "ServerList.json", ServerList);
            Save(ConfigPath, "UserList.json", UserList);
            Save(ConfigPath, "ApiList.json", ApiList);
        }

        private void Load()
        {
            SessionTimeList = new BindingList<SessionTimeItem>();
            UserList = new BindingList<UserItem>();
            ServerList = new BindingList<ServerItem>();
            ApiList = new BindingList<ApiItem>();

            SessionTimeList = (BindingList<SessionTimeItem>)Load(ConfigPath, "SessionTimeList.json", SessionTimeList);
            UserList = (BindingList<UserItem>)Load(ConfigPath, "UserList.json", UserList);
            ServerList = (BindingList<ServerItem>)Load(ConfigPath, "ServerList.json", ServerList);
            ApiList = (BindingList<ApiItem>)Load(ConfigPath, "ApiList.json", ApiList);

            if (SessionTimeList == null)
                SessionTimeList = new BindingList<SessionTimeItem>();
            if (ServerList == null)
                ServerList = new BindingList<ServerItem>();
            if (UserList == null)
                UserList = new BindingList<UserItem>();
            if (ApiList == null)
                ApiList = new BindingList<ApiItem>();

            SessionTimeList.ListChanged += SessionTimeList_ListChanged;
            ServerList.ListChanged += ServerList_ListChanged;
            UserList.ListChanged += UserList_ListChanged;
            ApiList.ListChanged += ApiList_ListChanged;

            foreach(var a in ApiList)
            {
                a.LinkedUserList = UserList;
                a.LinkedServerList = ServerList;
            }
        }

        // 常用的定时器
        private System.Timers.Timer _Timer = new System.Timers.Timer();
        private int _ReconnectInterval = 60;
        private int _QueryAccountInterval = 90;
        private int _QueryPositionInterval = 180;
        private int _QueryAccountCount = 0;
        private int _QueryPositionCount = 0;

        public override void Connect()
        {
            //base.Status = ProviderStatus.Connected;
            //return;

            _QueryAccountCount = _QueryAccountInterval;
            _QueryPositionCount = _QueryPositionInterval;
            // 如果一开始没有连接，直接运行，会启动时调用一次，订阅时又调用一次，最后导致连接被断开
            // 如查开始就使用while sleep进行处理就没有这个问题

            // 启动重连定时器
            _Timer.Elapsed -= _Timer_Elapsed;
            _Timer.Interval = 15 * 1000;
            _Timer.Enabled = true;
            _Timer.Elapsed += _Timer_Elapsed;

            _Connect(true);
        }
        public override void Disconnect()
        {
            // 关闭重连定时器
            _Timer.Elapsed -= _Timer_Elapsed;
            _Timer.Enabled = false;

            base.Status = ProviderStatus.Disconnecting;

            _Disconnect(true);
        }

        private bool IsApiConnected(XApi api)
        {
            return (api != null && api.IsConnected);
        }

        public override void Clear()
        {
            base.Clear();

            marketDataRecords.Clear();

            orderMap.Clear();
            quoteMap.Clear();

            positions.Clear();

            historicalDataRecords.Clear();
            historicalDataIds.Clear();
        }
    }
}
