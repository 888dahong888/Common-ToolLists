using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncCallbackTest
{
    /// <summary>
    /// 开发层处理，编写的具体的计算方法
    /// </summary>
    public class FunctionClass
    {
        public int GetSum(int a,int b)
        {
            return (a + b);
        }
        public int GetMulti(int a,int b)
        {
            return (a * b);
        }
    }

    /// <summary>
    /// 实际开发中，下面的类会封装起来，只提供函数接口，相当于系统底层
    /// </summary>
    public class CalculayeClass
    {
        public delegate int SomeCalculateWay(int num1, int num2);

        /// <summary>
        /// 将传入的参数在系统底层进行某种处理，
        /// 具体计算方法有开发者开发，
        /// 函数仅提供执行计算方法后的返回值
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <param name="cal"></param>
        /// <returns></returns>
        public int PrintAndCalculate(int num1,int num2,SomeCalculateWay cal)
        {
            Console.WriteLine($"系统底层接收:{num1}");
            Console.WriteLine($"系统底层接收:{num2}");
            return cal(num1, num2);//调用出入函数的一个引用
        }


        //可以继续封装更多的业务逻辑方法

    }
}
