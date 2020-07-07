using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UDPAsyncResultCallback
{
    public delegate void HandlerClassPacket(DataModel packet);
    public delegate void HandlerBytePacket(byte[] packet);
    public class UDPListener : IDisposable
    {
        public int Port { get; private set; }
        object callbackLock;
        UdpClient recevingUdpClient;
        IPEndPoint RemoteIpEndPoint;
        bool closing = false;
        HandlerClassPacket ClassPacketCallback = null;
        HandlerBytePacket BytePacketCallback = null;
        Queue<byte[]> queue;
        ManualResetEvent ClosingEvent;

        public UDPListener(int port)
        {
            Port = port;
            queue = new Queue<byte[]>();
            ClosingEvent = new ManualResetEvent(false);
            callbackLock = new object();

            // try to open the port 10 times, else fail
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    recevingUdpClient = new UdpClient(port);
                    break;
                }
                catch (Exception e)
                {
                    // Failed in ten tries, throw the exception and give up
                    if (i>=9)
                        throw;
                    Thread.Sleep(5);
                }
            }
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            //首次开启异步事件
            AsyncCallback callback = new AsyncCallback(ReceiveCallback);
            recevingUdpClient.BeginReceive(callback, null);
        }

        public UDPListener(int port, HandlerClassPacket callback) : this(port)
        {
            this.ClassPacketCallback=callback;
        }

        public UDPListener(int port,HandlerBytePacket callback) : this(port)
        {
            this.BytePacketCallback = callback;
        }
        public void ReceiveCallback(IAsyncResult result)
        {
            Monitor.Enter(callbackLock);
            Byte[] bytes = null;
            try
            {
                bytes = recevingUdpClient.EndReceive(result, ref RemoteIpEndPoint);
            }
            catch (ObjectDisposedException e)
            {
                // Ignore if disposed. This happens when closing the listener
                throw;
            }
            if (bytes != null&&bytes.Length>0)
            {
                if (BytePacketCallback!=null)
                {
                    BytePacketCallback(bytes);
                }

                else if (ClassPacketCallback != null)
                {
                    DataModel classdata = null;
                    try
                    {
                        classdata = DataModel.GetPacket(bytes);
                    }
                    catch (Exception e)
                    {
                        // If there is an error reading the packet, null is sent to the callback
                    }
                    ClassPacketCallback(classdata);
                }
                else
                {
                    lock(queue)
                    {
                        queue.Enqueue(bytes);
                    }
                }

            }

            if (closing)
            {
                ClosingEvent.Set();
            }
            else
            {
                //再次执行异步事件
                AsyncCallback callback = new AsyncCallback(ReceiveCallback);
                recevingUdpClient.BeginReceive(callback, null);
            }
            Monitor.Exit(callbackLock);
        }

        public void Close()
        {
            lock (callbackLock)
            {
                ClosingEvent.Reset();
                closing = true;
                recevingUdpClient.Close();
            }
            ClosingEvent.WaitOne();
        }
        public DataModel Receive()
        {
            if (closing)
            {
                throw new Exception("UDPListener has been closed.");
            }
            lock (queue)
            {
                if (queue.Count() > 0)
                {
                    byte[] bytes = queue.Dequeue();
                    var packet = DataModel.GetPacket(bytes);
                    return packet;
                }
                else
                    return null;
            }
        }

        public byte[] ReceiveBytes()
        {
            if (closing) 
            {
                throw new Exception("UDPListener has been closed.");
            }
            lock (queue)
            {
                if (queue.Count() > 0)
                {
                    byte[] bytes = queue.Dequeue();
                    return bytes;
                }
                else
                    return null;
            }
        }
        public void Dispose()
        {
            this.Close();
        }
    }
}
