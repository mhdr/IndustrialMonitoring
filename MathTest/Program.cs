using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace MathTest
{
    class Program
    {
        static void Main(string[] args)
        {
            List<double> data=new List<double>() {550,520,540};
            var result1 = Statistics.InterquartileRange(data);
            Console.WriteLine(result1);

            var result2 = Statistics.LowerQuartile(data);
            Console.WriteLine(result2);

            var result3 = Statistics.UpperQuartile(data);
            Console.WriteLine(result3);

            Console.ReadKey();
        }
    }
}
