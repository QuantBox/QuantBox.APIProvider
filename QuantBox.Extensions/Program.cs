using SmartQuant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantBox.Extensions
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                var es = Enum.GetValues(typeof(OrderSide));
                foreach (var e in es)
                    Console.WriteLine("{0}:{1}", e, (int)(OrderSide)e);
                Console.WriteLine("=====================");
            }

            {
                var es = Enum.GetValues(typeof(PutCall));
                foreach (var e in es)
                    Console.WriteLine("{0}:{1}", e, (int)(PutCall)e);
                Console.WriteLine("=====================");
            }

            {
                var es = Enum.GetValues(typeof(OrderStatus));
                foreach (var e in es)
                    Console.WriteLine("{0}:{1}", e, (int)(OrderStatus)e);
                Console.WriteLine("=====================");
            }
            {
                var es = Enum.GetValues(typeof(OrderType));
                foreach (var e in es)
                    Console.WriteLine("{0}:{1}", e, (int)(OrderType)e);
                Console.WriteLine("=====================");
            }
            {
                var es = Enum.GetValues(typeof(TimeInForce));
                foreach (var e in es)
                    Console.WriteLine("{0}:{1}", e, (int)(TimeInForce)e);
                Console.WriteLine("=====================");
            }
            {
                var es = Enum.GetValues(typeof(PositionSide));
                foreach (var e in es)
                    Console.WriteLine("{0}:{1}", e, (int)(PositionSide)e);
                Console.WriteLine("=====================");
            }
            {
                var es = Enum.GetValues(typeof(ExecType));
                foreach (var e in es)
                    Console.WriteLine("{0}:{1}", e, (int)(ExecType)e);
                Console.WriteLine("=====================");
            }
            {
                var es = Enum.GetValues(typeof(InstrumentType));
                foreach (var e in es)
                    Console.WriteLine("{0}:{1}", e, (int)(InstrumentType)e);
                Console.WriteLine("=====================");
            }
            {
                var es = Enum.GetValues(typeof(OrderType));
                foreach (var e in es)
                    Console.WriteLine("{0}:{1}", e, (int)(OrderType)e);
                Console.WriteLine("=====================");
            }
        }
    }
}
