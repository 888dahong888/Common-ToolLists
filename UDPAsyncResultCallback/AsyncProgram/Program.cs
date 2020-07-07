using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            DemoSyncCall(); //示例 1
            DemoEndInvoke();//示例 2
            DemoWaitHandle();//示例 3
            DemoPolling();//示例 4
            DemoCallback();//示例 5
            Console.Read();
        }
        /// <summary>
        /// 实现委托的方法
        /// </summary>
        /// <param name="iCallTime"></param>
        /// <param name="iExecThread"></param>
        /// <returns></returns>
        public static string LongRunningMethod(int iCallTime, out int iExecThread)
        {
            Thread.Sleep(iCallTime);
            iExecThread = AppDomain.GetCurrentThreadId();
            return "MyCallTime was:" + iCallTime.ToString();
        }

        delegate string MethodDelegate(int iCallTime, out int iExecThread);

        #region 示例 1： 同步调用方法
        /// <summary>
        /// 同步调用方法
        /// </summary>
        public static void DemoSyncCall()
        {
            string str;
            int iExecThread;
            MethodDelegate md = new MethodDelegate(LongRunningMethod);
            str = md(3000, out iExecThread);
            Console.WriteLine("the Delegate call returned string:{0},and the thread ID is:{1}", str, iExecThread.ToString());
            //sara Bareilles
        }
        #endregion

        #region 示例 2： 通过EndInvoke()调用模式异步调用模式
        /*
         * 使用调用模式是要调用 BeginInvoke ， 做某些处理主线程, 并调用 EndInvoke() 。 
         * 注意不 EndInvoke() 不返回直到异步调用已完成。 
         * 此调用模式是有用当要有调用线程正在执行异步调用， 同时工作。 
         * 有同时发生工作可改善许多应用程序的性能。 
         * 常见任务以异步运行以此方式是文件或网络操作。 
         * */
        /// <summary>
        /// 通过EndInvoke()调用模式异步调用模式
        /// </summary>
        public static void DemoEndInvoke()
        {
            string str;
            int iExecTread;
            MethodDelegate dlgt = new MethodDelegate(LongRunningMethod);
            IAsyncResult iar = dlgt.BeginInvoke(5000, out iExecTread, null, null);
            str = dlgt.EndInvoke(out iExecTread, iar);
            Console.WriteLine("the Delegate call returned string:{0},and the number is:{1}", str, iExecTread.ToString());
        }
        #endregion
        #region  示例 3：异步调用方法并使用 A WaitHandle 来等待调用完成
        /*
         * 由 BeginInvoke() 返回 IAsyncResult 具有一个 AsyncWaitHandle 属性。 
         * 该属性返回 WaitHandle 异步调用完成后， 通知是。 等待 WaitHandle 是常见线程同步技术。 
         * 通过是 WaitHandle WaitOne() 方法调用线程等待 WaitHandle 上。 
         * 直到是通知 WaitHandle WaitOne() 块。 当 WaitOne() 返回, 您在调用 EndInvoke() 之前进行一些额外工作。 
         * 对于执行文件或网络操作， 否则会阻塞调用主线程存为, 以前示例中此技术很有用。
         * */
        /// <summary>
        /// 异步调用方法并使用 A WaitHandle 来等待调用完成
        /// </summary>
        public static void DemoWaitHandle()
        {
            string str;
            int iExecTread;
            MethodDelegate dlgt = new MethodDelegate(LongRunningMethod);
            IAsyncResult iar = dlgt.BeginInvoke(3000, out iExecTread, null, null);
            iar.AsyncWaitHandle.WaitOne();
            str = dlgt.EndInvoke(out iExecTread, iar);
            Console.WriteLine("the Delegate call returned string:{0},and the number is:{1}", str, iExecTread.ToString());
        }
        #endregion
        #region 示例 4： 异步调用方法通过轮询调用模式
        /*
         * 由 BeginInvoke() 返回 IAsyncResult 对象有个 IsCompleted 属性异步调用完成后返回 True 。 
         * 然后可调用 EndInvoke() 。 如果您应用程序不断工作对不做要长期函数调用已被此调用模式很有用。 
         * MicrosoftWindows 应用程序是这样的示例。 
         * 主线程的 Windows 应用程序可以继续以执行异步调用时处理用户输入。 
         * 它可定期检查 IsCompleted 到调用是否完成。 它调用 EndInvoke 当 IsCompleted 返回 True 。 
         * 直到它知道操作已完成因为 EndInvoke() 阻止直到异步操作为完整, 应用程序不调用它。 
         * */
        /// <summary>
        ///  异步调用方法通过轮询调用模式
        /// </summary>
        public static void DemoPolling()
        {
            string str;
            int iExecThread;
            MethodDelegate dlgt = new MethodDelegate(LongRunningMethod);
            IAsyncResult iar = dlgt.BeginInvoke(3000, out iExecThread, null, null);
            while (iar.IsCompleted == false)
            {
                Thread.Sleep(10);
            }
            str = dlgt.EndInvoke(out iExecThread, iar);
            Console.WriteLine("the Delegate call returned string:{0},and the number is:{1}", str, iExecThread.ToString());
        }
        #endregion

        #region 示例 5： 异步方法完成后执行回调
        /*
         * 本节, 中示例提供对 BeginInvoke() 函数， 异步调用完成后系统执行回调委托。 
         * 回调调用 EndInvoke() 并处理异步调用的结果。 
         * 如果启动异步调用线程不需要处理结果是调用此调用模式很有用。 
         * 异步调用完成后系统调用线程以外启动线程上调。 
         * 若使用此调用模式, 作为第二到最后 - BeginInvoke() 函数的参数必须传递 AsyncCallback 类型的委托。 
         * BeginInvoke() 还有最后参数键入 对象 到您可以将任何对象。 当它调用该对象可用于您回调函数。 
         * 为此参数一个重要用途是以传递用于初始化调用该委托。 
         * 回调函数然后使用与该委托 EndInvoke() 函数来完成调用。 此调用模式是所示。
         * */
        /// <summary>
        /// 异步方法完成后执行回调
        /// </summary>
        public static void DemoCallback()
        {
            MethodDelegate dlgt = new MethodDelegate(LongRunningMethod);
            int iExecThread;
            AsyncCallback asyncCallback = new AsyncCallback(MyAsyncCallback);
            IAsyncResult iar = dlgt.BeginInvoke(5000, out iExecThread, asyncCallback, dlgt);
        }

        /// <summary>
        ///  异步方法完成后执行回调
        /// </summary>
        /// <param name="iar"></param>
        public static void MyAsyncCallback(IAsyncResult iar)
        {
            string str;
            int iExecThread;
            MethodDelegate dlgt = (MethodDelegate)iar.AsyncState;
            str = dlgt.EndInvoke(out iExecThread, iar);
            Console.WriteLine("the Delegate call returned string:{0},and the number is:{1}", str, iExecThread.ToString());
        }
        #endregion
    }
}
