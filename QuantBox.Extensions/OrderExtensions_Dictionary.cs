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
        public static ObjectTable GetDictionary(this Order order,int index = OrderTagType.Network)
        {
            var obj = order[index];
            if (obj == null)
            {
                obj = new ObjectTable();
                order[index] = obj;
            }
            return obj as ObjectTable;
        }

        public static object GetDictionaryValue(this Order order,int key, int index = OrderTagType.Network)
        {
            return order.GetDictionary(index)[key];
        }

        public static double GetDictionaryDouble(this Order order, int key, int index = OrderTagType.Network)
        {
            return order.GetDictionary(index).GetDouble(key);
        }

        public static int GetDictionaryInt(this Order order, int key, int index = OrderTagType.Network)
        {
            return order.GetDictionary(index).GetInt(key);
        }

        public static string GetDictionaryString(this Order order, int key, int index = OrderTagType.Network)
        {
            return order.GetDictionary(index).GetString(key);
        }
    } 
}
