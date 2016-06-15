using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.Extensions
{
    public static class OrderExtensions_ID
    {
        public static Order SetPortfolioID1(this Order order, string portfolioID1)
        {
            order.GetDictionary()[OrderTagType.PortfolioID1] = portfolioID1;
            return order;
        }

        public static string GetPortfolioID1(this Order order)
        {
            object obj = order.GetDictionaryValue(OrderTagType.PortfolioID1);
            return obj as string;
        }

        public static Order SetPortfolioID2(this Order order, string portfolioID2)
        {
            order.GetDictionary()[OrderTagType.PortfolioID2] = portfolioID2;
            return order;
        }

        public static string GetPortfolioID2(this Order order)
        {
            object obj = order.GetDictionaryValue(OrderTagType.PortfolioID2);
            return obj as string;
        }

        public static Order SetPortfolioID3(this Order order, string portfolioID3)
        {
            order.GetDictionary()[OrderTagType.PortfolioID3] = portfolioID3;
            return order;
        }

        public static string GetPortfolioID3(this Order order)
        {
            object obj = order.GetDictionaryValue(OrderTagType.PortfolioID3);
            return obj as string;
        }
    } 
}
