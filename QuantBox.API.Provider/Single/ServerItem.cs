using XAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.APIProvider.Single

{
    [DefaultProperty("Label")]
    public class ServerItem : ICloneable
    {
        private const string OPEN_QUANT = "OpenQuant";
        [Category("标签")]
        public string Label
        {
            get;
            set;
        }
        /// <summary>
        /// 订阅主题
        /// </summary>
        [Category("行情 - Femas")]
        [Description("Femas")]
        public int TopicId { get; set; }
        
        /// <summary>
        /// 流恢复
        /// </summary>
        [Category("流重传方式")]
        public ResumeType MarketDataTopicResumeType { get; set; }
        [Category("流重传方式")]
        public ResumeType PrivateTopicResumeType { get; set; }
        [Category("流重传方式")]
        public ResumeType PublicTopicResumeType { get; set; }
        [Category("流重传方式")]
        public ResumeType UserTopicResumeType { get; set; }
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [Category("服务端信息")]
        public string BrokerID { get; set; }
        /// <summary>
        /// 用户端产品信息
        /// </summary>
        [Category("客户端认证")]
        public string UserProductInfo { get; set; }
        /// <summary>
        /// 认证码
        /// </summary>
        [Category("客户端认证")]
        public string AuthCode { get; set; }
        /// <summary>
        /// App认证码
        /// </summary>
        [Category("客户端认证")]
        public string AppID { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [Category("服务端信息")]
        public string Address { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [Category("扩展信息")]
        public string ExtInfoChar128 { get; set; }
        [Category("扩展信息")]
        public string ConfigPath { get; set; }
        /// <summary>
        /// 端口号
        /// </summary>
        [Category("服务端信息")]
        public int Port { get; set; }
        /// <summary>
        /// UDP行情
        /// </summary>
        [Category("行情")]
        [Description("CTP/DFITC_Level2")]
        public bool IsUsingUdp { get; set; }
        /// <summary>
        /// 多播行情
        /// </summary>
        [Category("行情")]
        [Description("CTP")]
        public bool IsMulticast { get; set; }

        public ServerItem()
        {
            UserProductInfo = OPEN_QUANT;
        }

        public ServerInfoField ToStruct()
        {
            ServerInfoField field = new ServerInfoField();
            field.IsUsingUdp = this.IsUsingUdp;
            field.IsMulticast = this.IsMulticast;
            field.TopicId = this.TopicId;
            field.Port = this.Port;
            field.MarketDataTopicResumeType = this.MarketDataTopicResumeType;
            field.PrivateTopicResumeType = this.PrivateTopicResumeType;
            field.PublicTopicResumeType = this.PublicTopicResumeType;
            field.UserTopicResumeType = this.UserTopicResumeType;
            field.BrokerID = this.BrokerID;
            field.UserProductInfo = this.UserProductInfo;
            field.AuthCode = this.AuthCode;
            field.AppID = this.AppID;
            field.Address = this.Address;
            field.ConfigPath = this.ConfigPath;
            field.ExtInfoChar128 = this.ExtInfoChar128;

            return field;
        }

        public override string ToString()
        {
            return string.Format("Label={0};BrokerID={1};Address={2}", this.Label, this.BrokerID, this.Address);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
