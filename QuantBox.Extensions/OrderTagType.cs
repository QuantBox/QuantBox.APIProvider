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
        // 数据类型0位置都放需要在网络中传的参数，争取让ObjectTable小于10
        public const int Network = 0;
        // 在本地就可以处理好的数据类型，没有必要在网络中传输，并且有可能无法序列化
        public const int Local = 1;
        // 留给用户自定义的数据类型，让用户自己发挥的类型，可做临时变量等等
        public static int Index_MAX = 2;


        // ===== 需要跨网络传输的Tag
        public const int MsgType = 0;
        public const int Side = 1;
        public const int PositionEffect = 2;
        public const int HedgeFlag = 3; // 投机与保值，这个不知道对应的是哪一个，先这样用着

        public const int PortfolioID1 = 4;
        public const int PortfolioID2 = 5;
        public const int PortfolioID3 = 6;
        public const int Business = 7;
        public const int QuoteReqID = 8;  // 这个名字可能有错，需要确认
        public static int Network_MAX = 9;


        // ===== 不需要网络传输的类型，并且有可能无法序列化
        public const int NextTimeOrder = 0; // 用于记录下一笔Order,如平仓后开仓 
        public const int SameTimeOrder = 1; // 特殊的用于记录关联的Order，但没有先后循序，如Quote报单
        public const int CancelCount = 2; // 定时撤单次数，用来区分是否跟单功能撤单
        public const int SendCount = 3;//记录跟单时重发次数
        public static int Local_MAX = 4;
    }
}

// 使用static 定义，主要是想在不同的模块下，ID不冲突
// 目前这个写法还没有在不同模块下测试过

//public class MyOrderTagType
//{
//    // TODO: 这种方法是否有坑
//    public static int TargetAmount = OrderTagType.Local_MAX + 1;
//    public static int MaxQtyPerLot = TargetAmount + 1;
//    public static int TimesTicks = MaxQtyPerLot + 1;
//    public static int MaxCancelCnt = TimesTicks + 1;

//    static MyOrderTagType()
//    {
//        OrderTagType.Local_MAX = MaxCancelCnt + 1;
//    }
//}
