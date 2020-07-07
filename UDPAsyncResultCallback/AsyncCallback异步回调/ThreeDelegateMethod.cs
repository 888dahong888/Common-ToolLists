using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
/*
 * （1）普通方法调用（直接调用）与Invoke（）方法调用方法 使用的线程Id是一样的 即属于同步。
 * （2）BeginInvoke(<输入和输出变量>，AsyncCallback callback,object asyncState)方法调用方法 则是启用了新的线程Id，属于异步
 *  可通过 Thread.CurrentThread.ManagedThreadId    获取当前线程的Id
 *  委托类型的BeginInvoke(<输入和输出变量>，AsyncCallback callback,object asyncState)方法 
 *
 *   异步调用的核心， BeginInvoke（多线程能执行的原因）。
 *   输入和输出变量：表式委托对应的实参。
 *   第二个参数（AsyncCallback callback）：回调函数，表示异步调用后自动调用的函数 ,共用一个线程id.  AsyncCallback是一个委托   有一个参数（asyncState ）
 *   第三个参数（object asyncState）：用于向回调函数提供参数信息。返回值：IasyncResult:异步操作状态接口，封装了异步执行中的参数。
 *   EndInvoke监视BeginInvoke。委托类型的EndInvoke（）方法：借助IasyncResult接口对象，不断查询异步调用是否结束。该方法知道异步调用的方法所有参数，所以，异步调用完毕后，取出异步调用的结果作为返回值。

 * 对于同步和异步的总结：异步三大特点
 *（1）同步方法会出现“假死现象”即卡住界面，异步则不会 原因：异步启动了子线程执行任务，主线程得到释放
 *（2）同步速度慢，异步速度快  原因：异步启动了多个线程执行任务，占用更多的资源（异步时cpu瞬间上升）
 *（3）异步是无序的 原因：线程的启动和执行是由操作系统决定的，是无序的。可能每个子线程里面耗时不一样 
 * */


namespace AsyncCallback异步回调
{
    public delegate int AddHandler(int a, int b);
    public class ThreeDelegateMethod
    {
        public int Add(int a,int b)
        {
            Console.WriteLine("开始计算：" + a + "+" + b);
            Thread.Sleep(1000);
            Console.WriteLine("计算完成！");
            return a + b;
        }

        /// <summary>
        /// 同步方法
        /// </summary>
        public void Synchronized_Method()
        {
            Console.WriteLine("===== 同步调用 SyncInvokeTest =====");
            AddHandler handler = new AddHandler(Add);

            //invoke后阻塞主进程，直到handler执行完毕
            int result = handler.Invoke(1, 2);
            Console.WriteLine("继续做别的事情。。。");
            Console.WriteLine(result);
        }

        /// <summary>
        /// 异步方法
        /// 主线程并没有等待，而是直接向下运行了。但是问题依然存在，
        /// 当主线程运行到EndInvoke时，如果这时调用没有结束（这种情况很可能出现），
        /// 这时为了等待调用结果，线程依旧会被阻塞。
        /// </summary>
        public void Asynchronous_Method()
        {
            Console.WriteLine("===== 异步调用 AsyncInvokeTest =====");
            AddHandler handler = new AddHandler(Add);
            //IAsyncResult: 异步操作接口(interface)
            //BeginInvoke: 委托(delegate)的一个异步方法的开始
            IAsyncResult result = handler.BeginInvoke(1, 2, null, null);
            Console.WriteLine("继续做别的事情。。。");
            //异步操作返回
            Console.WriteLine(handler.EndInvoke(result));
        }

        /// <summary>
        /// 异步回调方法
        /// 用回调函数，当调用结束时会自动调用回调函数，
        /// 解决了为等待调用结果，而让线程依旧被阻塞的局面。
        /// </summary>
        public void Asynchronous_Callback_Method()
        {
            Console.WriteLine("===== 异步回调 AsyncInvokeTest =====");
            AddHandler handler = new AddHandler(Add);             //异步操作接口(注意BeginInvoke方法的不同！)            
            IAsyncResult result = handler.BeginInvoke(1, 2, new AsyncCallback(CallBackMethod), "AsycState:OK");
            Console.WriteLine("继续做别的事情。。。");
        }

        private void CallBackMethod(IAsyncResult ar)
        {
            //result 是“加法类.Add()方法”的返回值            
            //AsyncResult 是IAsyncResult接口的一个实现类，空间：System.Runtime.Remoting.Messaging            
            //AsyncDelegate 属性可以强制转换为用户定义的委托的实际类。  
            AddHandler handler = (AddHandler)((AsyncResult)ar).AsyncDelegate;
            Console.WriteLine(handler.EndInvoke(ar));
            Console.WriteLine(ar.AsyncState);
        }
    }
}
