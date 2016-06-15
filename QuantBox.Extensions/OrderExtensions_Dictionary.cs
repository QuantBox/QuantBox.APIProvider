using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.Extensions
{
    public static class OrderExtensions_Dictionary
    {
        //public static Order SetDictionary(this Order order, string account)
        //{
        //    order[OrderTagType.Account] = account;
        //    return order;
        //}

        public static Dictionary<int,object> GetDictionary(this Order order)
        {
            object obj = order[OrderTagType.Zero];
            if (obj == null)
            {
                obj = new Dictionary<int, object>();
                order[OrderTagType.Zero] = obj;
            }
            return obj as Dictionary<int,object>;
        }

        public static object GetDictionaryValue(this Order order,int key)
        {
            object value = null;
            order.GetDictionary().TryGetValue(key, out value);
            return value;
        }

        public static double GetDictionaryDouble(this Order order, int key)
        {
            object value = null;
            order.GetDictionary().TryGetValue(key, out value);
            if (value == null)
                return 0;
            return (double)value;
        }

        public static int GetDictionaryInt(this Order order, int key)
        {
            object value = null;
            order.GetDictionary().TryGetValue(key, out value);
            if (value == null)
                return 0;
            return (int)value;
        }
    } 
}
