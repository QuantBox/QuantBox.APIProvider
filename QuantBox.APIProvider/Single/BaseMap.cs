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
        protected Framework framework;
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

            report.AvgPx = record.AvgPx;
            report.CumQty = record.CumQty;
            report.LeavesQty = record.LeavesQty;

            if (execType != null)
                report.ExecType = execType.Value;

            if (orderStatus != null)
                report.OrdStatus = orderStatus.Value;

            return report;
        }

        public ExecutionReport CreateReport(
            Order order,
            SQ.ExecType? execType,
            SQ.OrderStatus? orderStatus)
        {
            ExecutionReport report = new ExecutionReport(order);

            report.DateTime = framework.Clock.DateTime;

            report.AvgPx = order.AvgPx;
            report.CumQty = order.CumQty;
            report.LeavesQty = order.LeavesQty;

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
