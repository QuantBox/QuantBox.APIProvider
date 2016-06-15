using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.APIProvider.Single
{
    public class SessionTimeItem
    {
        public TimeSpan SessionStart { get; set; }
        public TimeSpan SessionEnd { get; set; }

        public override string ToString()
        {
            return string.Format("Start={0};End={1}", this.SessionStart, this.SessionEnd);
        }
    }
}
