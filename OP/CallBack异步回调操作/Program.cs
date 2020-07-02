using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CallBack异步回调操作
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncCallback callback = new AsyncCallback(Test);

            for (int i = 0; i < 10; i++)
            {
                //Thread.Sleep(1000);
                Console.WriteLine($"主线程{i}");
            }
            Console.Read();
        }

        public static void Test(IAsyncResult result)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"委托线程{i}");
            }
        }
    }
}
