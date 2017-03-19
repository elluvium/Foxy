using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Data.Matrixes;
using Data.BusinessStructures;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var bs = new BusinessSystem();
            bs.Title = "title";
            bs.Mission = "mission";
            bs.KeyAreas = new List<string>() { "sd", "ds" };
            bs.GlobalGoal = "sd";
            bs.Vision = " vision";
            Data.DataHelper.SerializeBS(bs, "this");
            var newbs = DataHelper.DeserializeBS("this");
            Console.WriteLine(newbs.Title);
            Console.WriteLine(newbs.Vision);
            Console.ReadLine();
        }
    }
}
