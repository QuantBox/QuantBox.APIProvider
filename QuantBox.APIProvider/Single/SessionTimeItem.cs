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
        public const string CATEGORY_DAY_OF_WEEK = "DayOfWeek";

        private static string[] DayOfWeekChinese = new string[] { "日", "一", "二", "三", "四", "五", "六" };

        private List<DayOfWeek> DayOfWeekList = null;

        [PropertyOrder(1)]
        public TimeSpan SessionStart { get; set; }
        [PropertyOrder(2)]
        public TimeSpan SessionEnd { get; set; }

        [Category(CATEGORY_DAY_OF_WEEK)]
        [PropertyOrder(1)]
        public bool Sunday { get; set; }
        [Category(CATEGORY_DAY_OF_WEEK)]
        [PropertyOrder(2)]
        public bool Monday { get; set; }
        [Category(CATEGORY_DAY_OF_WEEK)]
        [PropertyOrder(3)]
        public bool Tuesday { get; set; }
        [Category(CATEGORY_DAY_OF_WEEK)]
        [PropertyOrder(4)]
        public bool Wednesday { get; set; }
        [Category(CATEGORY_DAY_OF_WEEK)]
        [PropertyOrder(5)]
        public bool Thursday { get; set; }
        [Category(CATEGORY_DAY_OF_WEEK)]
        [PropertyOrder(6)]
        public bool Friday { get; set; }
        [Category(CATEGORY_DAY_OF_WEEK)]
        [PropertyOrder(7)]
        public bool Saturday { get; set; }

        public bool Contains(DayOfWeek dayOfWeek)
        {
            if (DayOfWeekList == null)
                DayOfWeekList = GetDayOfWeekList();

            return DayOfWeekList.Contains(dayOfWeek);
        }

        public List<DayOfWeek> GetDayOfWeekList()
        {
            var list = new List<DayOfWeek>();
            if (Sunday) list.Add(DayOfWeek.Sunday);
            if (Monday) list.Add(DayOfWeek.Monday);
            if (Tuesday) list.Add(DayOfWeek.Tuesday);
            if (Wednesday) list.Add(DayOfWeek.Wednesday);
            if (Thursday) list.Add(DayOfWeek.Thursday);
            if (Friday) list.Add(DayOfWeek.Friday);
            if (Saturday) list.Add(DayOfWeek.Saturday);

            DayOfWeekList = list;

            return list;
        }

        public string GetDayOfWeekString()
        {
            var strs = new List<string>();
            var list = GetDayOfWeekList();
            foreach (var l in list)
            {
                strs.Add(DayOfWeekChinese[Convert.ToInt16(l)]);
            }
            return string.Join("", strs);
        }


        public override string ToString()
        {
            var list = new List<TimeSpan>() { SessionStart, SessionEnd };
            var strs = new List<string>();
            foreach (var ts in list)
            {
                if (ts.TotalDays >= 1.0)
                {
                    strs.Add(ts.ToString(@"d\:mm"));
                }
                else
                {
                    strs.Add(ts.ToString(@"hh\:mm"));
                }

            }
            return $"[{string.Join(",", strs)}] {{{GetDayOfWeekString()}}}";
        }
    }
}
