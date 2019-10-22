using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.APIProvider.Single
{
    class QuoteRecord
    {
        public QuoteRecord(Order askOrder, Order bidOrder)
        {
            this.AskOrder = askOrder;
            this.BidOrder = bidOrder;
        }

        public Order AskOrder { get; private set; }
        public Order BidOrder { get; private set; }

        public SmartQuant.OrderStatus Status { get; set; }
    }
}
