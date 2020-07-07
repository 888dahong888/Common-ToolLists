using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncCallbackDelegateTest
{
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("我在Main方法中，这是方法的第一行！线程是：" + Thread.CurrentThread.ManagedThreadId);
            AddEventHandler add = new AddEventHandler(AddCalss.Add);
            //invoke的返回值就是自定义方法的返回值
            double result = add.Invoke(1, 1);
            Console.WriteLine("计算结束......");
            Console.WriteLine("返回的结果是" + result);
            Console.WriteLine("=========================================");
            Test tt = new Test();
            tt.Init1();
            Console.ReadKey();
        }
    }
}
