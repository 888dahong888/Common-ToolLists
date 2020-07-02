using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Thread线程操作
{
    class Program
    {
        static void Main(string[] args)
        {
            new Thread(Go).Start();
            Go();

            Thread s = new Thread(WriteY);
            s.Start();
            while (true)
            {
                Console.Write("x");
            }
            Console.Read();
        }
        private static void WriteY()
        {
            while (true)
            {
                Console.Write("y");
            }
        }

        private static void Go()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(i);
            }
        }
    }
}
