using System.Net.Sockets;
using System.Net;
using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Ezwith.Network
{
    public enum SocketEventType
    {
        Connected,
        Closed,
        Received,
        KeepAlive
    }

    public class SocketClient<TData> where TData : new()
    {
        public bool IsBound
        {
            get
            {
                return handler != null ? handler.IsBound : false;
            }
        }

        public bool IsConnected
        {
            get
            {
                return handler != null ? handler.IsConnected() : false;
            }
        }

        public delegate void SocketEvent(SocketEventType type, object data);

        public string remoteIpAddress { get; protected set; }
        public int remotePort { get; protected set; }

        protected Queue<TData> queue;
        private Socket handler;
        public int maxBufferSize = 1024;
        private byte[] recvBuffer = null;

        public SocketClient(Queue<TData> queue, string remoteIpAddress = "127.0.0.1", int remotePort = 3000)
        {
            Init(queue, remoteIpAddress, remotePort);
        }


        public void Connect(Queue<TData> queue, string remoteIpAddress, int remotePort)
        {
            if (IsConnected) throw new Exception("Socket is already connected. Disconnect first.");

            Init(queue, remoteIpAddress, remotePort);
            Connect();
        }

        public void Connect()
        {
            Debug.LogFormat("Client: trying to connect to {0}:{1}", remoteIpAddress, remotePort);
            try
            {
                if (handler != null && handler.IsConnected()) return;

                handler = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Debug.Log("trying to connect to server...");
                IPAddress IP = Dns.GetHostAddresses(remoteIpAddress).Where(x => x.AddressFamily == AddressFamily.InterNetwork).First();
                Debug.LogFormat("IPs {0}", IP);
                IPEndPoint endPoint = new IPEndPoint(IP, remotePort);
                handler.BeginConnect(endPoint, new AsyncCallback(HandleConnection), handler);
            }
            catch (Exception e)
            {
                Debug.Log("Exception: connectSocket: " + e.Message);
                Close();
            }
        }

        internal void Init(Queue<TData> queue, string remoteIpAddress = "127.0.0.1", int remotePort = 3000)
        {
            this.remoteIpAddress = remoteIpAddress;
            this.remotePort = remotePort;
            this.queue = queue;
        }

        private void HandleConnection(IAsyncResult ar)
        {
            TData item;
            try
            {
                handler.EndConnect(ar);
                if (!handler.IsConnected())
                {
                    throw new Exception("Cannot connect to " + remoteIpAddress + ":" + remotePort);
                }

                Debug.LogFormat("Connected to  {0}:{1}", remoteIpAddress, remotePort);

                var buff = Encoding.UTF8.GetBytes(remoteIpAddress + ":" + remotePort);
                item = (TData)Activator.CreateInstance(typeof(TData), SocketEventType.Connected, buff, buff.Length, (Socket)ar.AsyncState);
                queue.Enqueue(item);
                ReadyToReceiveMessage();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Close();
            }
        }

        //-------------------------------------------------------------------/
        // Socket : Receive
        //-------------------------------------------------------------------/
        private void ReadyToReceiveMessage()
        {
            recvBuffer = new byte[maxBufferSize];

            handler.BeginReceive(
                recvBuffer,
                0,
                recvBuffer.Length,
                SocketFlags.None,
                new AsyncCallback(ReceiveComplete),
                handler);
        }

        private void ReceiveComplete(IAsyncResult ar)
        {
            try
            {
                int len = handler.EndReceive(ar);
                if (len == 0)
                {
                    ReadyToReceiveMessage();
                    return;
                }

                TData item;
                SocketEventType eventType;
                if (len == 1)
                {
                    Debug.LogFormat("KeepAlive {0} bytes from {1}", len, handler.RemoteEndPoint.ToString());
                    eventType = SocketEventType.KeepAlive;
                }
                else
                {
                    Debug.LogFormat("Received {0} bytes from {1}", len, handler.RemoteEndPoint.ToString());
                    eventType = SocketEventType.Received;
                }
                item = (TData)Activator.CreateInstance(typeof(TData), eventType, recvBuffer, len, handler);
                queue.Enqueue(item);
                recvBuffer = new byte[maxBufferSize];

                ReadyToReceiveMessage();
            }
            catch (Exception e)
            {
                Debug.Log("Exception: ReceiveComplete: " + e.Message);
                Close();
            }
        }


        //-------------------------------------------------------------------/
        // Socket : Send
        //-------------------------------------------------------------------/
        public void Send(byte[] byteData)
        {
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Debug.LogFormat("Sent {0} bytes to {1}.", bytesSent, remoteIpAddress + ":" + remotePort);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        //-------------------------------------------------------------------/
        // Socket : Close
        //-------------------------------------------------------------------/
        public void Close()
        {
            if (handler == null) return;

            try
            {
                if (handler.IsConnected())
                {
                    handler.Shutdown(SocketShutdown.Both);
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }

            handler.Close();
            handler = null;
            Debug.Log("Close socket");

            var buff = Encoding.UTF8.GetBytes(remoteIpAddress + ":" + remotePort);
            TData item = (TData)Activator.CreateInstance(typeof(TData), SocketEventType.Closed, buff, buff.Length, null);
            queue.Enqueue(item);
        }
    }
}
