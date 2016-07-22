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
        public static int index = OrderTagType.Network;
        public static Order SetPortfolioID1(this Order order, string portfolioID1)
        {
            order.GetDictionary(index)[OrderTagType.PortfolioID1] = portfolioID1;
            return order;
        }

        public static string GetPortfolioID1(this Order order)
        {
            return order.GetDictionaryString(OrderTagType.PortfolioID1, index);
        }

        public static Order SetPortfolioID2(this Order order, string portfolioID2)
        {
            order.GetDictionary(index)[OrderTagType.PortfolioID2] = portfolioID2;
            return order;
        }

        public static string GetPortfolioID2(this Order order)
        {
            return order.GetDictionaryString(OrderTagType.PortfolioID2, index);
        }

        public static Order SetPortfolioID3(this Order order, string portfolioID3)
        {
            order.GetDictionary(index)[OrderTagType.PortfolioID3] = portfolioID3;
            return order;
        }

        public static string GetPortfolioID3(this Order order)
        {
            return order.GetDictionaryString(OrderTagType.PortfolioID3, index);
        }
    } 
}
