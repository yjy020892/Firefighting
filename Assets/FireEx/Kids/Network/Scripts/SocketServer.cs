using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

namespace Ezwith.Network
{

    public class StateObject
    {
        // client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
        public EndPoint remoteEndPoint;

        //public ISocketDataParser dataParser;

        public void Dispose()
        {
            if (workSocket != null)
            {
                try
                {
                    if (workSocket.IsConnected())
                        workSocket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }

                workSocket.Close();
                // dataParser.Init();
                // dataParser = null;
                workSocket = null;
                sb = null;
            }
        }
    }

    public class SocketServer<TData> : MonoBehaviour where TData : SocketData<TData>, new()
    {
        // Port Number
        public int Port { get; protected set; }
        public Socket listener;
        public List<StateObject> states = new List<StateObject>();
        protected Queue<TData> queue;
        public List<StateObject> States { get { return states; } }
        public bool IsConnected
        {
            get { return null != listener && listener.IsConnected(); }
        }

        public bool isBound
        {
            get { return null != listener && listener.IsBound; }
        }
        public bool HasClients
        {
            get { return states.Count > 0; }
        }

        public string LocalEndPoint
        {
            get { return listener != null ? listener.LocalEndPoint.ToString() : null; }
        }

        public SocketServer(Queue<TData> queue = null)
        {
            this.queue = queue;
        }

        public void Listen(int port = 3000)
        {
            Port = port;

            //Debug.Log("All IPAddresses available\n=======================");
            //Array.ForEach(Dns.GetHostEntry(Dns.GetHostName()).AddressList, x => Debug.Log("\t" + x));

            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, Port);
            //Debug.Log("ipaddress:" + localEndPoint.ToString() + ", port:" + Port.ToString());

            Listen(localEndPoint);
        }

        public void Listen(IPEndPoint localEndPoint)
        {

            if (listener != null && listener.IsBound)
            {
                Debug.Log("Already listening.");
                return;
            }

            // Create a TCP/IP socket.
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            listener.Bind(localEndPoint);
            listener.Listen(100);

            //Debug.Log("Waiting for a connection...");
            listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            try
            {
                // Create the state object.
                StateObject state = new StateObject { workSocket = handler, remoteEndPoint = handler.RemoteEndPoint };
                states.Add(state);

                var buff = Encoding.UTF8.GetBytes(state.remoteEndPoint.ToString());
                TData item = (TData)Activator.CreateInstance(typeof(TData), SocketEventType.Connected, buff, buff.Length, handler);
                queue.Enqueue(item);

                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                Debug.LogFormat("AcceptCallback:: client connected: {0}", state.workSocket.RemoteEndPoint.ToString());
            }
            catch (Exception exc)
            {
                Debug.LogException(exc);
            }

            listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
        }

        protected virtual void ReadCallback(IAsyncResult ar)
        {
            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            // begin receive again
            try
            {
                if (state.workSocket == null || !state.workSocket.IsConnected())
                {
                    KickOut(state);
                    return;
                }
                // Read data from the client socket. 
                int bytesRead = state.workSocket.EndReceive(ar);
                if (bytesRead == 0)
                {
                    KickOut(state);
                    return;
                }
                TData item = null;
                SocketEventType eventType;
                if (bytesRead == 1)
                {
                    //Debug.LogFormat("KeepAlive {0} bytes from {1}", bytesRead, state.remoteEndPoint.ToString());
                    eventType = SocketEventType.KeepAlive;
                }
                else eventType = SocketEventType.Received;

                item = (TData)Activator.CreateInstance(typeof(TData), eventType, state.buffer, bytesRead, state.workSocket);
                queue.Enqueue(item);
                state.buffer = new byte[StateObject.BufferSize];
                state.workSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                KickOut(state);
            }
        }

        public void KickOut(StateObject state)
        {
            Debug.LogFormat("KickOut: {0}", state.remoteEndPoint);

            var buff = Encoding.UTF8.GetBytes(state.remoteEndPoint.ToString());
            TData item = (TData)Activator.CreateInstance(typeof(TData), SocketEventType.Closed, buff, buff.Length, null);
            queue.Enqueue(item);

            states.Remove(state);
            state.Dispose();
        }

        //-------------------------------------------------------------------/
        // Socket : Send
        //-------------------------------------------------------------------/
        public void Send(Socket client, byte[] byteData)
        {
            client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), listener);
        }

        void SendCallback(IAsyncResult ar)
        {
            // Retrieve the socket from the state object.
            Socket handler = (Socket)ar.AsyncState;
            StateObject state = (StateObject)ar.AsyncState;

            try
            {
                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Debug.Log(string.Format("Sent {0} bytes to {1}.", bytesSent, handler.RemoteEndPoint.ToString()));

            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                KickOut(state);
            }
        }

        public void Close()
        {
            try
            {
                foreach (var state in states)
                    state.Dispose();

                states.Clear();
                listener.Close();
                listener = null;
                queue.Clear();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            Debug.Log("Close Server");
        }
    }
}