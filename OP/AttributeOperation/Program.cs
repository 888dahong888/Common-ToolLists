using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
DLLImport
Conditional
Obsolete

预定义特性
	AttributeUsage
		[AttributeUsage(AttributeTargets.Class |AttributeTargets.Constructor |AttributeTargets.Field |AttributeTargets.Method |AttributeTargets.Property, AllowMultiple = true)]
	Conditional
		[Conditional(conditionalSymbol)]
	Obsolete
		[Obsolete(message)]
		[Obsolete(message,iserror)]  
*/
namespace AttributeOperation
{
    class Program
    {
        static void Main(string[] args)
        {
            //OldClass old = new OldClass();
            //old.OldMethod();

            GetAttributeInfo(typeof(OldClass));
            Console.WriteLine("==================");
            GetAttributeInfo(typeof(NewClass));

            Console.ReadKey();
        }
        private static void GetAttributeInfo(Type type)
        {
            OldAttribute myattribute = (OldAttribute)Attribute.GetCustomAttribute(type, typeof(OldAttribute));
            if (myattribute == null)
            {
                Console.WriteLine(type.ToString() + "类中自定义特性不存在！");
            }
            else
            {
                Console.WriteLine("特性描述:{0}\n加入事件{1}", myattribute.Discretion, myattribute.date);
            }
        }

    }

    //[Obsolete("该类已经过时",false)]//使用默认的特性目标，直接作用于紧随其后的Class OldClass,第二个参数我这里设置为true将使用已过时的元素视为错误
    //class OldClass
    //{
    //    [method:Obsolete("该方法已经过时")]
    //    public void OldMethod()
    //    {
    //        Console.WriteLine("过时的方法");
    //    }
    //}

    //[System.AttributeUsage(System.AttributeTargets.Class|System.AttributeTargets.Struct,AllowMultiple =true)]
    //public class Author:System.Attribute
    //{
    //    private string name;
    //    public double version;
    //    public Author(string name)
    //    {
    //        this.name = name;
    //        version = 1.0;
    //    }
    //}

    //[Author("P.Ackerman",version=1.1)]
    //[Author("P.Backman",version=1.2)]
    //class SampleClass
    //{
    //    Author a = new Author("wewe");
    //}

}
