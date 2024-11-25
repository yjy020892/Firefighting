using UnityEngine;
using System;
using System.Linq;
using System.Net;
using System.Xml;
using UnityEngine.Events;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Ezwith.Network
{
    public class SocketServerHandler<TData> : MonoBehaviour where TData: SocketData<TData>, new()
    {
        public int port = 3232;

        public float timeForCheckClientConnections = 2.0f;
        [SerializeField] public SocketServerEvent OnClientConnected;
        [SerializeField] public SocketServerEvent OnClientDisconnected;
        [SerializeField] public SocketServerEvent OnReceiveMessage;

        public SocketServer<TData> Server { get; private set; }
        public bool IsBound { get { return Server.isBound; } }
        public bool HasClients { get { return Server.HasClients; } }
        public int ClientCount { get { return Server.isBound ? Server.states.Count : 0; } }
        public string IPAddress { get { return Server.LocalEndPoint; } }
        public int Port { get { return Server != null ? Server.Port : 0; } }
        private float startTime;
        protected Queue<TData> queue = new Queue<TData>();


        protected Dictionary<string, Socket> clients = new Dictionary<string, Socket>();

        virtual protected void Awake()
        {
            OnClientConnected = new SocketServerEvent();
            OnClientDisconnected = new SocketServerEvent();
            OnReceiveMessage = new SocketServerEvent();

            Server = new SocketServer<TData>(queue);
        }

        virtual public bool Listen()
        {
            try
            {
                if (!IsBound)
                {
                    Server.Listen(port);

                    //Debug.Log("ServerHandler:: Listen");
                    return true;
                }
            }
            catch (Exception exc)
            {
                Debug.LogException(exc);
            }
            return false;
        }

        virtual protected void Update()
        {
            while(queue.Count > 0)
            {
                var data = queue.Dequeue();
                switch(data.Type)
                {
                    case SocketEventType.Connected: OnClientConnectedHandler(data); break;
                    case SocketEventType.Closed: OnClientDisconnectedHandler(data); break;
                    case SocketEventType.Received: OnReceiveMessageHandler(data); break;
                }
            }
        }

        /// <summary>
        /// Callback sent to all game objects before the application is quit.
        /// </summary>
        void OnApplicationQuit()
        {
            Close();
        }
        void OnDestroy()
        {
            Close();
        }

        virtual public void Close()
        {
            if (IsBound)
            {
                clients.Clear();
                Server.Close();
            }
        }

        virtual protected void OnClientConnectedHandler(TData data)
        {
            var address = GetClientAddress(data.Socket);
            Debug.LogFormat("OnClientConnectedHandler: {0}", address);
            // connectQueue.Enqueue(client);

            OnClientConnected.Invoke(data);
        }

        virtual protected void OnClientDisconnectedHandler(TData data)
        {
            //Debug.LogFormat("OnClientDisconnectedHandler: {0}", data);
            OnClientDisconnected.Invoke(data);
        }

        virtual protected void OnReceiveMessageHandler(TData data)
        {
            // messageQueue.Enqueue(message);
            var address = GetClientAddress(data.Socket);
            //Debug.LogFormat("OnReceiveMessageHandler: {0}, {1}", address, data);
            OnReceiveMessage.Invoke(data);
        }

        public string GetClientAddress(Socket client)
        {
            return client != null && client.IsBound && client.RemoteEndPoint != null ? client.RemoteEndPoint.ToString() : null;
        }

        public Socket GetClient(int i)
        {
            if (Server.states.Count > 0)
            {
                return Server.states[i].workSocket;
            }
            return null;
        }

        virtual public void SendToAll(byte[] buffer)
        {
            foreach (var client in Server.states)
                client.workSocket.Send(buffer);
        }

        public class SocketServerEvent : UnityEvent<TData> { };
    }
}
