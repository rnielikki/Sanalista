using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanalista
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("program started");
            Node root = Nodes.Maker();//Täytyy olla ennen Quizia.
            char[,] Quiz = Generate.MakeQuiz();
            Generate.PrintQuiz(Quiz);
            Finder.StartAsync(root,Quiz).Wait();
        }
    }
}
