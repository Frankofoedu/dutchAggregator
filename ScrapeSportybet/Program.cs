using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapeSportybet
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing scrapper");
            var action = new Action();
            var rtn = action.Scrape();

            Console.WriteLine("Testing scrapper again");

            foreach (var item in rtn)
            {
                Console.WriteLine(item.League);
                foreach (var i in item.Matches)
                {
                    Console.WriteLine("-----------" + i.TeamNames);
                }
            }

            Console.Read();
        }
    }
}
