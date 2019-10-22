using Newtonsoft.Json;
using OrderedPropertyGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.APIProvider.Single
{
    // 发现想让属性显示排序，但发现Json转换出问题了，所以还是决定不排序了
    [TypeConverter(typeof(PropertySorter))]
    [JsonConverter(typeof(NoTypeConverterJsonConverter<SessionTimeItem>))]
    public class SessionTimeItem
    {
        [PropertyOrder(1)]
        public TimeSpan SessionStart { get; set; }
        [PropertyOrder(2)]
        public TimeSpan SessionEnd { get; set; }
        [PropertyOrder(3)]
        public bool Enable { get; set; }

        public override string ToString()
        {
            //return string.Format("Start={0};End={1}", this.SessionStart, this.SessionEnd);
            if (Enable)
            {
                return string.Format("+|Start={0};End={1}", this.SessionStart, this.SessionEnd);
            }
            else
            {

                return string.Format("-|Start={0};End={1}", this.SessionStart, this.SessionEnd);
            }
        }
    }
}
