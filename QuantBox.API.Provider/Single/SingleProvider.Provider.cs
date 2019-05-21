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
            // OQ多开时，其中一个改了，另一个关闭会导致保存丢失
            // Save();
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

            // 只是简单设置，等登录时将把账号设置上，X日志由于一些信息没法
            xlog = LogManager.GetLogger(Name + ".X");
            barLog = LogManager.GetLogger("Bar");
            tickLog = LogManager.GetLogger("Tick");

            // 以下初始化的值在，初始化后由软件读取文件中的参数据后又设置回来
            LastPricePlusNTicks = 10;
            EmitBidAsk = true;
            EmitLevel2Snapshot = false;
            //UpdateInstrument = true;
            EnablEmitHistoricalData = true;
            FilterDateTime = true;
            EnableEmitData = true;
            HasPriceLimit = true;

            // 这两个地方的路径如果为空，设置一个默认的
            if (string.IsNullOrEmpty(ConfigPath))
            {
                ConfigPath = Path.Combine(Installation.ConfigDir.FullName, Name);
            }
            Directory.CreateDirectory(ConfigPath);

            marketDataRecords = new Dictionary<string, MarketDataRecord>();

            orderMap = new OrderMap(framework, this);
            quoteMap = new QuoteMap(framework, this, orderMap);

            positions = new Dictionary<string, PositionFieldEx>();

            historicalDataRecords = new Dictionary<int, HistoricalDataRecord>();
            historicalDataIds = new Dictionary<string,int>();

            // ConfigPath在做Setting时已经做了
            //Load();
        }

        void SessionTimeList_ListChanged(object sender, ListChangedEventArgs e)
        {
            //xlog.Info("交易会话时间更新");
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

        public void Save()
        {
            Save(ConfigPath, "SessionTimeList.json", SessionTimeList);
            Save(ConfigPath, "ServerList.json", ServerList);
            Save(ConfigPath, "UserList.json", UserList);
            Save(ConfigPath, "ApiList.json", ApiList);
        }

        public void Load()
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

        protected override void OnConnect()
        {
            Load();

            _QueryAccountCount = _QueryAccountInterval;
            _QueryPositionCount = _QueryPositionInterval;

            // 启动重连定时器
            _Timer.Elapsed -= _Timer_Elapsed;
            // 改小用来测试连接销毁，用完要改回去
            _Timer.Interval = 30 * 1000;
            _Timer.Enabled = true;
            _Timer.Elapsed += _Timer_Elapsed;

            xlog.Info("重连检测定时器开启，检测频率(毫秒):{0}", _Timer.Interval);
            _Connect(true);
        }
        protected override void OnDisconnect()
        {
            // 关闭重连定时器
            _Timer.Elapsed -= _Timer_Elapsed;
            _Timer.Enabled = false;

            xlog.Info("重连检测定时器关闭");

            _Disconnect(true);

            Save();
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
