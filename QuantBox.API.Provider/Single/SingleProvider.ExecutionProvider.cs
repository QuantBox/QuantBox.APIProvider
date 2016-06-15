using XAPI;
using SmartQuant;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QuantBox.Extensions;

namespace QuantBox.APIProvider.Single
{
    public partial class SingleProvider : IExecutionProvider
    {
        public override void Send(ExecutionCommand command)
        {
            if (!IsApiConnected(_TdApi))
            {
                EmitError(-1, -1, "交易服务器没有连接");
                xlog.Error("交易服务器没有连接");
                return;
            }

            // 取报单消息类型
            char? msgType = command.Order.GetMsgType();
            if (!msgType.HasValue)
                msgType = OrderMsgType.NewOrderSingle;

            switch (command.Type)
            {
                case ExecutionCommandType.Send:
                    switch (msgType)
                    {
                        case OrderMsgType.Ignore:// 一般是组合单，要求在下单时设置好
                            return;
                        case OrderMsgType.NewOrderSingle:
                            CmdNewOrderSingle(command);
                            break;
                        case OrderMsgType.NewOrderList:
                            CmdNewOrderList(command);
                            break;
                        case OrderMsgType.Quote:
                            CmdNewQuote(command);
                            break;
                    }
                    break;
                case ExecutionCommandType.Cancel:
                    switch (msgType)
                    {
                        case OrderMsgType.QuoteCancel:
                            CmdCancelQuote(command);
                            break;
                        default:
                            CmdCancelOrder(command);
                            break;
                    }
                    break;
            }
        }


        #region Reports

        public void EmitExecutionReport(ExecutionReport report)
        {
            base.EmitExecutionReport(report);
        }
        #endregion      
    }
}
