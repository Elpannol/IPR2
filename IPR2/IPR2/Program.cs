using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR2
{
    class Program
    {
        static void Main(string[] args)
        {
            var src = DateTime.Now;
            Console.WriteLine($"{src.Year}-{src.Month}-{src.Day} {src.Hour}:{src.Minute}:{src.Second}");
            Console.WriteLine($"{src.TimeOfDay}");
        }
    }
}
