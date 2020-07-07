using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/* 
 * 解释两个对象：AsyncCallback，IAsyncResultAsyncCallback委托，
 * 概括来说它的作用是在一个单独的线程中处理异步操作的结果。
 * AsyncCallback就是一个异步的操作完成后要执行的回调函数，
 * 该函数把IAsyncResult作为它的参数，由这个参数得到相应的异步操作的结果IAsyncResult接口，
 * 就像是一个信使，作为一个参数，把有用的信息带给异步的线程看一下下面的代码
 */
namespace AsyncCallbackDelegateTest
{
    //定义一个委托，因为这是基于委托的异步调用
    public delegate double AddEventHandler(int a, int b);
    public class AddCalss
    {
        //稍后该方法会用作对一个委托的注册
        public static double Add(int a, int b)
        {
            Console.WriteLine("我在AddClass中的Add方法下，线程是：" + Thread.CurrentThread.ManagedThreadId);
            return a + b;
        }

    }
    public class Test
    {
        static AsyncCallback MyCall;
        public void Init1()
        {
            //委托的注册
            MyCall = Method1;
            Console.WriteLine("我在Main方法中，这是方法的第一行！，线程号是：" + Thread.CurrentThread.ManagedThreadId);
            //注册自己声明的委托
            AddEventHandler add = new AddEventHandler(AddCalss.Add);
            //用BeginInvoke开始异步操作，一步的执行AddClass.Add方法，前面两个是要传入该方法的参数，
            //第三个是AsyncCallback委托，第四个是传一个附加对象，会由AsyncState得到。
            IAsyncResult result = add.BeginInvoke(1, 2, MyCall, "这是传过来的一句话");
            Console.WriteLine("计算完毕.......");
        }

        /// <summary>
        /// 实现AsyncCallback委托的方法，当然该类型跟该委托完全一样，会用此方法注册AsyncCallback委托
        ///  上面的Add方法是一个异步的操作，而此操作的结果都返回到下面这个方法中，在里面再进行你想要的操作
        /// <param name="ar"></param>
        private void Method1(IAsyncResult ar)
        {
            //该类实现了IAsyncResult接口，丰富了许多的方法。
            AsyncResult aResult = (AsyncResult)ar;
            //在新线程中得到原委托的对象。
            AddEventHandler handler = (AddEventHandler)aResult.AsyncDelegate;
            //EndInvoke返回的就是Add方法执行后的结果。
            Console.WriteLine(handler.EndInvoke(ar));
            Console.WriteLine(ar.AsyncState);
            Console.WriteLine("我在Method1方法中，线程号：" + Thread.CurrentThread.ManagedThreadId);
        }
    }
}
