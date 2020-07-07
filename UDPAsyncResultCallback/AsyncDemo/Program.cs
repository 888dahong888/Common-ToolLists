using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 此项目用于WPF中
/// </summary>
namespace AsyncDemo
{
    class Program
    {
        AsyncCallback callback;
        objstate obj;
        static void Main(string[] args)
        {
        }

        #region easy demo
        //声明委托
        public delegate string deltest();
        deltest begin;
        private void FirstMethod()
        {
            begin = new deltest(method);
            callback = new AsyncCallback(back);
            obj = new objstate();//实例化类，该对象可以传入回调函数中
            begin.BeginInvoke(back, obj);//异步执行method，界面不会假死，5秒后执行回调函数，弹出提示框
        }

        private string method()
        {
            Thread.Sleep(5000);
            return "welcome to this world";
        }

        private void back(IAsyncResult ar)
        {
            string res = begin.EndInvoke(ar);
            objstate obj = (objstate)(ar.AsyncState);//通过AsyncState获取传入的object
            //MessageBox.Show(res + "\n" + obj.fname + " " + obj.lname + " " + obj.birthday);
            Console.WriteLine(res + "\n" + obj.fname + " " + obj.lname + " " + obj.birthday);
        }
        #endregion

        #region complex demo
        //申明委托
        public delegate void deltest1();
        deltest1 begin1;
        public delegate string deltest2(List<string> list);
        deltest2 begin2;
        private void SecendMethod()
        {
            string id = Thread.CurrentThread.ManagedThreadId.ToString();
            Thread.CurrentThread.Name = "MainThread";
            //richTextBox1.Text = "主线程线程ID：" + id + "  主线程名：" + Thread.CurrentThread.Name + "\n";

            begin1 = new deltest1(method1);
            callback = new AsyncCallback(back1);

            List<objstate> list = new List<objstate>();
            for (int i = 0; i < 10; i++)
            {
                objstate obj = new objstate("James" + (i * i).ToString(), "Warke" + (i * 3).ToString());
                list.Add(obj);
            }
            //delegate.BeginInvoke(parameter[] para, AsyncCallback callback, object obj) 
            //para是method方法的参数; 
            //callback是method方法执行完后在同一子线程中立即执行的回调函数; 
            //obj是一个可以传入回调函数中的object ，在回调函数中通过 IAsyncResult.AsyncState获取
            begin1.BeginInvoke(back1, list);

            //this.Location = new System.Drawing.Point(0, 0);
            //MessageBox.Show("主线程没有阻塞");
            Console.WriteLine("主线程没有阻塞");
        }

        private void back1(IAsyncResult ar)
        {
            begin1.EndInvoke(ar);
            //回调函数中的线程ID与异步执行method时的线程ID相同，说明说明回调函数也是在子线程中执行
            string id = Thread.CurrentThread.ManagedThreadId.ToString();

            if (this.IsHandleCreated)
            {
                IAsyncResult iar = this.BeginInvoke(new Action(delegate ()
                {
                    //richTextBox1.Text += "正在调用back函数 " + "当前线程ID：" + id + "\n";
                }));
                this.EndInvoke(iar);
            }
            List<objstate> list = (List<objstate>)(ar.AsyncState);//通过AsyncState获取传入的object
            List<string> strList = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(3000);
                //winform中控件的属性设置或操作只能有主线程进行
                //因此在子线程中需要调用 Control.BeginInvoke(Delegate method)方法 在主线程上对控件进行操作
                //Control.BeginInvoke中传入的方法，应尽量只包含对控件操作的语句，这样主线程能够快速执行完对控件的操作，主界面不会假死
                //Action<T>()是无返回值的泛型委托   Func<T>()是带返回值的泛型委托
                //线程的执行有可能在窗口句柄创建完成前进行，此时会报错，因此 子线程中要异步操作主线程中的控件就需要在主线程的窗口句柄创建完成后进行
                //句柄的类型是 IntPtr ，相当于windows中的身份证， 是一个指向指针的指针， 在内存中有固定的地址
                //指针指向一块内存引用类或方法或程序集，不过内存中的地址会不断改变，系统需要通过一个指向指针的指针来记载数据地址的变更，便是句柄
                if (this.IsHandleCreated)
                {
                    IAsyncResult iar = this.BeginInvoke(new Action(delegate ()
                    {
                        //richTextBox1.Text += "正在调用back函数 " + "当前线程ID：" + id + "  正在给第" + (i + 1).ToString() + "个人赋值" + "\n";
                    }));
                    this.EndInvoke(iar);
                }
                DateTime now = DateTime.Now;
                list[i].birthday = now;
                string str = now.ToLongTimeString() + " " + list[i].fname + "." + list[i].lname;
                strList.Add(str);
            }

            begin2 = new deltest2(method2);
            begin2.BeginInvoke(strList, back2, null);
        }

        private string method2(List<string> list)
        {
            string id = Thread.CurrentThread.ManagedThreadId.ToString();
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(3000);
                if (this.IsHandleCreated)
                {
                    IAsyncResult iar = this.BeginInvoke(new Action(delegate ()
                    {
                        //richTextBox1.Text += "正在调用method2方法  " + "当前线程ID：" + id + "\n";
                        //richTextBox1.Text += list[i] + "\n";
                        //richTextBox1.Select(richTextBox1.TextLength, 0);
                    }));
                    this.EndInvoke(iar);
                }
            }
            return "finish";
        }

        private void back2(IAsyncResult ar)
        {
            //有返回值的方法，可以通过EndInvoke(IAsyncResult ar)方法获取返回值
            string res = begin2.EndInvoke(ar);
            //回调函数中的线程ID与异步执行method时的线程ID相同，说明说明回调函数也是在子线程中执行
            string id = Thread.CurrentThread.ManagedThreadId.ToString();

            if (this.IsHandleCreated)
            {
                IAsyncResult iar = this.BeginInvoke(new Action(delegate ()
                {
                    richTextBox1.Text += "正在调用back2函数 " + "当前线程ID：" + id + "\n";
                }));
                this.EndInvoke(iar);

            }
            //MessageBox.Show(res);
        }

        private void method1()
        {
            string id = Thread.CurrentThread.ManagedThreadId.ToString();
            for (int i = 5; i > 0; i--)
            {
                Thread.Sleep(1000);
                if (this.IsHandleCreated)
                {
                    IAsyncResult iar = this.BeginInvoke(new Action(delegate ()
                    {
                        //richTextBox1.Text += "正在调用method方法 " + "当前线程ID：" + id + "  " + i.ToString() + "秒后method方法,进入回调函数\n";
                    }));
                    this.EndInvoke(iar);
                }
            }

        }
    }
    /// <summary>
    /// 声明一个实体类
    /// </summary>
    public class objstate
    {
        public string fname;
        public string lname;
        public DateTime birthday;
        public objstate()
        {
            fname = "wang";
            lname = "nima";
            birthday = DateTime.Now;
        }
        public objstate(string fn, string ln)
        {
            fname = fn;
            lname = ln;
            birthday = DateTime.Now;
        }

    }
}
