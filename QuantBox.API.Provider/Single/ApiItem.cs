using Newtonsoft.Json;
using QuantBox.APIProvider.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XAPI;

namespace QuantBox.APIProvider.Single
{
    /// <summary>
    /// 由用户选择Dll,然后加载，得到
    /// 
    /// </summary>
    public class ApiItem : ICloneable
    {
        public const string CATEGORY_INFO = "Information";
        public const string CATEGORY_TYPE = "Type";

        internal BindingList<UserItem> LinkedUserList;
        internal BindingList<ServerItem> LinkedServerList;

        private string _dllPath;
        private string _typeName;

        private IXApi CheckApi(string typeName,string dllPath)
        {
            if(string.IsNullOrEmpty(typeName))
            {
                TypeName = "XAPI.Callback.XApi, XAPI_CSharp";
                return null;
            }
            var api = XApiHelper.CreateInstance(typeName, dllPath);
            try
            {
                Type = api.GetApiTypes;
                Name = api.GetApiName;
                Version = api.GetApiVersion;

                // 取公共部分
                UseType = UseType & Type;
            }
            catch (Exception ex)
            {
                api = null;
                Type = ApiType.None;
                Name = ex.Message;
                Version = "请使用depends检查一下是否缺少依赖";
                UseType = ApiType.None;
            }
            return api;
        }

        [Editor(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string DllPath
        {
            get
            {
                return _dllPath;
            }
            set
            {
                _dllPath = value;
                Api = CheckApi(_typeName, _dllPath);
            }
        }

        public string TypeName
        {
            get
            {
                return _typeName;
            }
            set {
                _typeName = value;
                Api = CheckApi(_typeName, _dllPath);
            }
        }

        [Browsable(false)]
        internal IXApi Api { get; set; }

        [Category(CATEGORY_INFO)]
        [ReadOnly(true)]
        public string Name { get; set; }

        [Category(CATEGORY_INFO)]
        [ReadOnly(true)]
        public string Version { get; set; }

        [TypeConverter(typeof(UserItemConverter))]
        public int User { get; set; }
        [TypeConverter(typeof(ServerItemConverter))]
        public int Server { get; set; }

        public string LogPrefix { get; set; }


        [Category(CATEGORY_TYPE)]
        [ReadOnly(true)]
        public ApiType Type { get; set; }
        [Category(CATEGORY_TYPE)]
        [Editor(typeof(ApiTypeSelectorEditor), typeof(UITypeEditor))]
        public ApiType UseType { get; set; }


        public override string ToString()
        {
            return string.Format("LogPrefix={0};UseType={1};Name={2}", LogPrefix, UseType, Name);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
