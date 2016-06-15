using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.Extensions
{
    /// <summary>
    /// 默认使用FIX 5.0中的协议字段来填充
    /// </summary>
    public class OrderTagType
    {
        public const int Zero = 0;

        //public const int Account = 1;已经支持了，所以这不需要了
        public const int MsgType = 35;
        public const int Side = 54;
        public const int PositionEffect = 77;
        public const int QuoteReqID = 131;  // 这个名字可能有错，需要确认

        public const int HedgeFlag = 5000; // 投机与保值，这个不知道对应的是哪一个，先这样用着

        public const int CancelCount = 9996; // 定时撤单次数，用来区分是否跟单功能撤单
        public const int SendCount = 9997;//记录跟单时重发次数
        public const int NextTimeOrder = 9998; // 用于记录下一笔Order,如平仓后开仓 
        public const int SameTimeOrder = 9999; // 特殊的用于记录关联的Order，但没有先后循序，如Quote报单

        public const int PortfolioID1 = 60001;
        public const int PortfolioID2 = 60002;
        public const int PortfolioID3 = 60003;
        public const int Business = 60003;
    }
}
