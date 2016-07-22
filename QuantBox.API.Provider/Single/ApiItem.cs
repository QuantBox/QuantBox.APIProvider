using Newtonsoft.Json;
using NLog;
using QuantBox.APIProvider.UI;
using XAPI.Callback;
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
        public const string CATEGORY_Type = "Type";

        internal BindingList<UserItem> LinkedUserList;
        internal BindingList<ServerItem> LinkedServerList;

        private string _dllPath;

        [Editor(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string DllPath
        {
            get {
                if (ProviderHost.autoMakeRelativePath)
                {
                    return PathHelper.MakeRelativePath(_dllPath);
                }
                else
                {
                    return PathHelper.MakeAbsolutePath(_dllPath);
                }
            }
            set
            {
                string tmp_dllPath;
                if (ProviderHost.autoMakeRelativePath)
                {
                    tmp_dllPath = PathHelper.MakeRelativePath(value);
                }
                else
                {
                    tmp_dllPath = PathHelper.MakeAbsolutePath(value);
                }

                // 不一样
                bool diff = _dllPath != tmp_dllPath;
                _dllPath = tmp_dllPath;


                // 这个地方导致Json无法还原，所以先判断是否需要进行调用
                if (LinkedUserList != null) {
                    if (Api == null || diff)
                    {
                        Api = new XApi(_dllPath);
                    }
                    if (Api != null) {
                        try
                        {
                            Type = Api.GetApiTypes;
                            Name = Api.GetApiName;
                            Version = Api.GetApiVersion;

                            // 取公共部分
                            UseType = UseType & Type;
                        }
                        catch
                        {
                            Api = null;
                            Type = ApiType.Nono;
                            Name =  null;
                            Version = null;
                            UseType = ApiType.Nono;
                        }
                    }
                }
            }
        }

        [Browsable(false)]
        internal XApi Api { get; set; }

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

        private BindingList<UserItem> userList = new BindingList<UserItem>();
        public BindingList<UserItem> UserList
        {
            get { return userList; }
            set { userList = value; }
        }

        public string LogPrefix { get; set; }


        [Category(CATEGORY_Type)]
        [ReadOnly(true)]
        public ApiType Type { get; set; }
        [Category(CATEGORY_Type)]
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
