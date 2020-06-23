using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Timer定时器操作
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public System.Timers.Timer timer1 = new System.Timers.Timer(1000);
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnTestMethod1_Click(object sender, RoutedEventArgs e)
        {
            tbxResult.Clear();
            timer1.Elapsed += new ElapsedEventHandler(funcTest1);
            timer1.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            timer1.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
            timer1.Start();
        }

        private void funcTest1(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                tbxResult.Text += "测试时间:" + " " + "" + System.DateTime.Now.ToString("HH:mm:ss") + "" + "\r\n";
            });

            timer1.Stop();//停止
            timer1.Enabled = false;//关闭
        }

        private void BtnTestMethod2_Click(object sender, RoutedEventArgs e)
        {
            System.Timers.Timer timer2 = new System.Timers.Timer(1000);//实例化Timer类，设置间隔时间为3000毫秒；
            timer2.Elapsed += new System.Timers.ElapsedEventHandler(funcTest2);//到达时间的时候执行事件；
            timer2.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            timer2.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
            timer2.Start();//启动
        }

        private void funcTest2(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                tbxResult.Text = "测试时间:" + " " + "" + System.DateTime.Now.ToString("HH:mm:ss") + "";
            });
        }
    }
}
