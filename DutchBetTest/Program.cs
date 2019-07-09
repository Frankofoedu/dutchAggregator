using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchBetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            double odd1 = Convert.ToDouble(Console.ReadLine());
            double odd2 = Convert.ToDouble(Console.ReadLine());


            var x = 1.0 / (odd1 + odd2) * odd2;
            var y = 1.0 / (odd1 + odd2) * odd1;
            double x2 = x * 100;
            double y2 = y * 100;
            double rtn = x2 * odd1;

            Console.WriteLine(rtn + " - " + x2 + " - " + y2);
            var v = Console.ReadLine();
        }
    }
}
