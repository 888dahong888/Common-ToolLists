using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 注册表加解密操作
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 注册表中的注册日期
        /// </summary>
        public string triaDate;
        /// <summary>
        /// TXT中的注册日期
        /// </summary>
        public string triaDateInFile;
        /// <summary>
        /// 注册表秘钥
        /// </summary>
        public string registryKey;
        /// <summary>
        /// TXT秘钥
        /// </summary>
        public string textKey;
        /// <summary>
        /// 时间间隔
        /// </summary>
        public int timeGap = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 判断注册表项目是否存在的方法
        /// </summary>
        /// <returns></returns>
        private bool IsRegeditItemExist()
        {
            string[] subkeyNames;
            RegistryKey hkml = Registry.CurrentUser;
            RegistryKey software = hkml.OpenSubKey("SOFTWARE");
            //RegistryKey software = hkml.OpenSubKey("SOFTWARE",true);
            subkeyNames = software.GetSubKeyNames();
            //取得该项下所有子项的名称的序列，并传递给预定的数组中  
            foreach (string keyName in subkeyNames) //遍历整个数组
            {
                if (keyName == "TestItem") //判断子项的名称
                {
                    hkml.Close();
                    return true;
                }
            }
            hkml.Close();
            return false;
        }

        private void RegistryWrite()
        {
            bool isExit = IsRegeditItemExist();
            if (isExit!=true)
            {
                string sn, sm;
                sn=Time.
            }
        }
    }
}
