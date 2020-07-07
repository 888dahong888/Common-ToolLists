using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncCallback异步回调
{
    class Program
    {
        static void Main(string[] args)
        {
            RunTest run = new RunTest();
            run.Init(); //执行异步测试方法
            Console.Read();
        }
    }
}
