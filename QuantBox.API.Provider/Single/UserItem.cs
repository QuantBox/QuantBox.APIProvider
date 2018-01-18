using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAPI;

namespace QuantBox.APIProvider.Single
{
    [DefaultProperty("Label")]
    public class UserItem:ICloneable
    {
        [Category("标签")]
        public string Label
        {
            get;
            set;
        }
        /// <summary>
        /// 用户代码
        /// </summary>
        [Category("账号")]
        public string UserID { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Category("账号")]
        public string Password { get; set; }
        /// <summary>
        /// 扩展信息
        /// </summary>
        [Category("账号")]
        public string ExtInfoChar64 { get; set; }
        [Category("账号")]
        public int ExtInfoInt32 { get; set; }

        public UserInfoField ToStruct()
        {
            UserInfoField field = new UserInfoField();

            field.UserID = this.UserID;
            field.Password = this.Password;
            field.ExtInfoChar64 = this.ExtInfoChar64;
            field.ExtInfoInt32 = this.ExtInfoInt32;

            return field;
        }

        public override string ToString()
        {
            return string.Format("Label={0};UserID={1}", this.Label,this.UserID);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
