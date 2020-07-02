using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeOperation
{
    ////设置了定位参数和命名参数
    /////该特性适用于所有的类，而且是非继承的。
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    class OldAttribute:Attribute
    {
        private string discretion;
        public string Discretion
        {
            get { return discretion; }
            set { discretion = value; }
        }
        public DateTime date;
        public OldAttribute(string discretion)
        {
            this.discretion = discretion;
            date = DateTime.Now;
        }
    }
    //定义两个类
    [Old("这个类将过期")]//使用定义的新特性
    class OldClass
    {
        public void OldTest()
        {
            Console.WriteLine("测试特性");
        }
    }
    class NewClass : OldClass
    {
        public void NewTest()
        {
            Console.WriteLine("测试特性的继承");
        }
    }
}
