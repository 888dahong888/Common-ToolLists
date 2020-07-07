using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPAsyncResultCallback
{
    public abstract class DataModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public abstract byte[] GetBytes();
        public static DataModel GetPacket(byte[] Data)
        {
            DataModel model = null;
            if (Data.Length>=3)
            {
                
                model.X = Data[0];
                model.Y = Data[1];
                model.Z = Data[2];
                
            }
            else
            {
                model.X = 0;
                model.Y = 0;
                model.Z = 0;
            }
            return model;
        }
    }
}
