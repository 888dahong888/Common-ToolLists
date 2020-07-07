using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncCallbackTest
{
    /// <summary>
    /// 模拟用户层，执行输入等操作
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            CalculayeClass cc = new CalculayeClass();
            FunctionClass fc = new FunctionClass();

            int result1 = cc.PrintAndCalculate(2, 3, fc.GetSum);
            Console.WriteLine($"调用了开发人员的加法函数，处理后返回结果:{result1}");

            int result2 = cc.PrintAndCalculate(2, 3, fc.GetMulti);
            Console.WriteLine($"调用了开发人员的加法函数，处理后返回结果:{result2}");
            Console.ReadKey();
        }
    }
}
