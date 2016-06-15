using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.APIProvider.Single
{
    public class HistoricalDataRecord
    {
        public HistoricalDataRecord(HistoricalDataRequest request)
        {
            this.Request = request;
        }

        public HistoricalDataRequest Request { get; private set; }
    }
}
