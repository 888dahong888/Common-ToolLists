using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using Microsoft.Win32;
namespace 注册表加解密操作
{
    public class Time
    {
        /// <summary>
        /// 获取电脑CPU信息的方法
        /// </summary>
        /// <returns></returns>
        public static string GetCpuId()
        {
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();

            string strCpuID = null;
            foreach (ManagementObject mo in moc)
            {
                strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                break;
            }
            return strCpuID;
        }

        /// <summary>
        /// 获取系统当前时间的方法
        /// </summary>
        /// <returns></returns>
        public static string GetNowDate()
        {
            string NowDate = DateTime.Now.ToString("yyyyMMdd");
            return NowDate;
        }

        /// <summary>
        /// 生成序列号的方法
        /// </summary>
        /// <returns></returns>
        public static string CreatSerialNumber()
        {
            string SerialNumber = GetCpuId() + "-" + GetNowDate();
            return SerialNumber;
        }


        /// <summary>
        /// 写入注册表的方法
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Setting"></param>
        public static void WriteSetting(string Section, string Key, string Setting)  // name = key  value=setting  Section= path
        {
            string text1 = Section;
            RegistryKey key1 = Registry.CurrentUser.CreateSubKey("Software\\TestItem"); // .LocalMachine.CreateSubKey("Software\\mytest");
            if (key1 == null)
            {
                return;
            }
            try
            {
                key1.SetValue(Key, Setting);
            }
            catch (Exception exception)
            {
                return;
            }
            finally
            {
                key1.Close();
            }

        }

        /// <summary>
        /// 读取注册表的方法
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Default"></param>
        /// <returns></returns>
        public static string ReadSetting(string Section, string Key, string Default)
        {
            if (Default == null)
            {
                Default = "-1";
            }
            string text2 = Section;
            RegistryKey key1 = Registry.CurrentUser.OpenSubKey("Software\\TestItem");
            if (key1 != null)
            {
                object obj1 = key1.GetValue(Key, Default);
                key1.Close();
                if (obj1 != null)
                {
                    if (!(obj1 is string))
                    {
                        return "-1";
                    }
                    string obj2 = obj1.ToString();
                    obj2 = DES.DESDecrypt(obj2, "12345678", "87654321");
                    return obj2;
                }
                return "-1";
            }
            return Default;
        }
    }
}
