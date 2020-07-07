using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncCallback异步回调
{
    public class AsyncDelegateCal
    {
        public int num1 { get; set; }
        public int num2 { get; set; }
        /// <summary>
        /// 定义委托方法
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public delegate void AsyncComputeCaller(int x, int y);

        public void Cal_num(int x,int y)
        {
            //运算过程
        }

        public void Init()
        {
            AsyncComputeCaller caller = new AsyncComputeCaller(this.Cal_num);
            AsyncCallback callback = new AsyncCallback(ComputerCallback);

            //BeginInvoke不会阻塞，实现异步调用 执行传入的ComputerCallback的回调方法
            caller.BeginInvoke(num1, num2, callback, caller);
        }

        /// <summary>
        /// 执行后续处理，可以在UI线程中直接更新界面不会有阻塞，假死
        /// </summary>
        /// <param name="result"></param>
        private void ComputerCallback(IAsyncResult result)
        {
            AsyncComputeCaller caller = (AsyncComputeCaller)result.AsyncState;
            caller.EndInvoke(result);
        }
    }
}
