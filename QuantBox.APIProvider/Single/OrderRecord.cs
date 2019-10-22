using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.APIProvider.Single
{
    public class OrderRecord
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        public OrderRecord(Order order)
        {
            this.Order = order;

            this.LeavesQty = (int)order.Qty;
            this.CumQty = 0;
            this.AvgPx = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public Order Order { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int LeavesQty { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CumQty { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public double AvgPx { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastPx"></param>
        /// <param name="lastQty"></param>
        public void AddFill(double lastPx, int lastQty)
        {
            this.AvgPx = (this.AvgPx * this.CumQty + lastPx * lastQty) / (this.CumQty + lastQty);

            this.LeavesQty -= lastQty;

            this.CumQty += lastQty;
        }
    }
}
