using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QB = QuantBox;
using SQ = SmartQuant;

namespace QuantBox.APIProvider.Single
{
    class BaseMap
    {
        private Framework framework;
        protected SingleProvider provider;

        public BaseMap(Framework framework, SingleProvider provider)
        {
            this.framework = framework;
            this.provider = provider;
        }

        #region EmitExecutionReport
        public ExecutionReport CreateReport(
            OrderRecord record,
            SQ.ExecType? execType,
            SQ.OrderStatus? orderStatus)
        {
            ExecutionReport report = new ExecutionReport(record.Order);

            report.DateTime = framework.Clock.DateTime;

            //report.Order = record.Order;
            //report.Instrument = record.Order.Instrument;
            
            //report.Side = record.Order.Side;
            //report.OrdType = record.Order.Type;
            //report.TimeInForce = record.Order.TimeInForce;

            //report.OrdQty = record.Order.Qty;
            //report.Price = record.Order.Price;
            //report.StopPx = record.Order.StopPx;

            report.AvgPx = record.AvgPx;
            report.CumQty = record.CumQty;
            report.LeavesQty = record.LeavesQty;

            if (execType != null)
                report.ExecType = execType.Value;

            if (orderStatus != null)
                report.OrdStatus = orderStatus.Value;

            return report;
        }

        public void EmitExecutionReport(OrderRecord record, SQ.ExecType execType, SQ.OrderStatus orderStatus)
        {
            ExecutionReport report = CreateReport(record, execType, orderStatus);
            provider.EmitExecutionReport(report);
        }

        public void EmitExecutionReport(OrderRecord record, SQ.ExecType execType, SQ.OrderStatus orderStatus, string text)
        {
            ExecutionReport report = CreateReport(record, execType, orderStatus);
            report.Text = text;
            provider.EmitExecutionReport(report);
        }
        #endregion
    }
}
