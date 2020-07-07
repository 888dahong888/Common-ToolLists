using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
 * 委托的BeginInvoke方法可以异步的执行该委托绑定的方法，
 * endinvoke用来接收委托返回的参数，
 * 并且一直堵塞直到方法执行完成才会继续执行后续代码
 * 
 * begininvoke传入一个asyncCallback的委托和一个object，当方法执行完毕，
 * 则会自动执行asyncCallback绑定的方法如果InitConfigTable需要传入参数的话，
 * 那么begininvoke前面的参数要与InitConfigTable保持一致
 * 
 * begininvoke传入的另一个参数object可以为空也可以传入上面定义的func委托，
 * 如果为空，则begininvoke方法内部的AsyncState需要改为AsyncDelegate，否则会出错
 */
namespace AsyncCallback异步回调
{
    public class DelegateInvoke
    {
        public void OneParams()
        {
            Func<string> func = InitConfigTable;
            
            func.BeginInvoke(asyncResult =>
            {
                Func<string> handler = (Func<string>)((AsyncResult)asyncResult).AsyncState;
                string msg = handler.EndInvoke(asyncResult);
                Console.WriteLine(msg);
            }, func);
            Console.WriteLine("OK");
        }

        public void TwoParams()
        {
            string s = "wow";
            Func<string, string> func = InitConfigTable;
            func.BeginInvoke(s, asyncResult =>
            {
                Func<string> handler = (Func<string>)((AsyncResult)asyncResult).AsyncState;
                string msg = handler.EndInvoke(asyncResult);
                Console.WriteLine(msg);
            }, func);
            Console.WriteLine("ok");
        }
        private string InitConfigTable()
        {
            Thread.Sleep(2000);
            string msg = "hafaf";
            return msg;
        }

        private string InitConfigTable(string s)
        {
            Thread.Sleep(2000);
            string msg = s;
            return msg;
        }
    }
}
