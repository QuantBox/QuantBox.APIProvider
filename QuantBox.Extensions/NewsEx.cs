using XAPI;
using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.Extensions
{
    public class NewsEx:News
    {
        public NewsEx()
            : base()
        {
        }
        public NewsEx(DateTime dateTime, int providerId, int instrumentId, byte urgency, string url, string headline, string text)
            : base(dateTime, providerId, instrumentId, urgency, url, headline, text)
        {
        }

        public XAPI.ResponseType ResponseType;
        public object UserData;
    }
}
