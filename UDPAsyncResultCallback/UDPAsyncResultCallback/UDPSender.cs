using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPAsyncResultCallback
{
    
    public class UDPSender
    {
        private int _port;
        public int Port { get => _port; set => _port = value; }
        private string _address;
        public string Address { get => _address; set => _address = value; }
        IPEndPoint RemoteIpEndPoint;
        Socket sock;

        public UDPSender(string address,int port)
        {
            _port = port;
            _address = address;
            sock=new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            var addresses= Dns.GetHostAddresses(address);
            if (addresses.Length == 0) throw new Exception("Unable to find IP address for " + address);
            RemoteIpEndPoint = new IPEndPoint(addresses[0], port);
        }
        public void Send(byte[] message)
        {
            sock.SendTo(message, RemoteIpEndPoint);
        }
        public void Close()
        {
            sock.Close();
        }
    }
}
