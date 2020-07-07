using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPAsyncResultCallback
{
    class Program
    {
        static void Main(string[] args)
        {

            HandlerClassPacket packet = delegate (DataModel dm)
              {
                  var msg = dm.X;
                  Console.WriteLine(msg);
              };
            var l1 = new UDPListener(10001, packet);
            Console.ReadLine();
        }
    }
}
