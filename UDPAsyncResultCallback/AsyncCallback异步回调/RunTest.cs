using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncCallback异步回调
{
    public class RunTest
    {
        public delegate void MyFunction(string msg);
        
        public void Init()
        {
            MyFunction fn = StartA;
            fn.BeginInvoke("[B]开始运行", AsyncCallback =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    Console.WriteLine("\t[B]运行了" + i + "%");
                };
            }, null);
            Console.WriteLine("[A]要开始运行了！");
            for (int i = 1; i <= 1000; i++)
            {
                Console.WriteLine("\t[A]运行了" + i + "%");
            };
        }
        public void StartA(string msg)
        {
            Console.WriteLine(msg);
        }
    }

    /// <summary>
    /// 构造异步回调对象
    /// AsyncCallback 异步回调对象名asyncCallback = new AsyncCallback(异步操作完成时调用的方法MyAsyncCallback);
    ///
    /// 定义委托，并进行异步调用，异步调用完成后自动触发
    /// 委托类型Action fn委托名 = Run委托定义;
    /// 委托名fn.BeginInvoke(异步回调对象名asyncCallback);    
    /// </summary>
    public class UseDelegateForAsyncCallback
    {
        public delegate string MethodDelegate(int iCallTime);
        /// <summary>
        /// 异步操作完成时调用的方法
        /// </summary>
        public void Init()
        {
            MethodDelegate dlgt = (m) =>
            {
                return "你输入的数字是" + m;
            };
            AsyncCallback asyncCallback = new AsyncCallback(MyAsyncCallback);
            Action fn = Run;
            fn.BeginInvoke(asyncCallback, null);

        }

        private void MyAsyncCallback(IAsyncResult ar)
        {
            Console.WriteLine("异步调用");
            Console.ReadLine();
        }


        public  void Run()
        {
            //
        }
    }
}
