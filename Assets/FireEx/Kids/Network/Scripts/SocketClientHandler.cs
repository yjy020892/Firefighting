using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using System.Text;
using System.Net.Sockets;

namespace Ezwith.Network
{
    public class SocketClientHandler<TData> : MonoBehaviour where TData : SocketData<TData>, new()
    {

        public SocketClientEvent OnConnected = new SocketClientEvent();
        public SocketClientEvent OnMessageReceived = new SocketClientEvent();
        public SocketClientEvent OnClosed = new SocketClientEvent();

        public string remoteIP = "127.0.0.1";
        public int remotePort = 3000;
        public bool autoConnecting = true;
        public bool IsConnected
        {
            get
            {
                return Socket != null && Socket.IsBound;
            }
        }

        protected SocketClient<TData> Socket { get; set; }

        private float timerForReconnect;
        private float timerForKeepingAlive;
        public float rateToReconnect = 5f;
        public float rateToKeepAlive = 5f;
        private Queue<TData> queue = new Queue<TData>();

        virtual protected void Start()
        {
            if (autoConnecting)
                Connect();
        }

        virtual protected void Init()
        {
            Debug.Log("Initialize");

            if (Socket == null)
            {
                Socket = new SocketClient<TData>(queue, remoteIP, remotePort);
            }
            else
            {
                if (Socket.IsBound && (Socket.remoteIpAddress != remoteIP || Socket.remotePort != remotePort))
                {
                    Socket.Close();
                }
            }
        }

        public void Connect()
        {
            if (Socket == null) Init();
            Socket.Connect(queue, remoteIP, remotePort);
        }

        public void Connect(string remoteIP, int remotePort)
        {
            this.remoteIP = remoteIP;
            this.remotePort = remotePort;

            Connect();
        }

        public void Close()
        {
            if (Socket == null) return;

            Socket.Close();
        }

        virtual protected void OnDestroy()
        {
            if (Socket != null)
            {
                Socket.Close();
            }
        }

        virtual protected void Update()
        {
            while (queue.Count > 0)
            {
                var data = queue.Dequeue();
                DispatchEvent(data);
            }
        }

        virtual protected void FixedUpdate()
        {
            if (Socket == null) return;
            if (autoConnecting)
            {
                timerForReconnect += Time.deltaTime;
                if (timerForReconnect > rateToReconnect && !Socket.IsConnected)
                {
                    Socket.Connect(queue, remoteIP, remotePort);
                    timerForReconnect = 0.0f;
                }
            }

            timerForKeepingAlive += Time.deltaTime;
            if (timerForKeepingAlive > rateToKeepAlive)
            {
                timerForKeepingAlive = 0.0f;
                if (!Socket.IsBound) return;
                Socket.Send(new byte[] { 1 });
            }
        }

        virtual protected void DispatchEvent(TData data)
        {
            switch (data.Type)
            {
                case SocketEventType.Connected:
                    OnConnected.Invoke(data);
                    break;
                case SocketEventType.Closed:
                    OnClosed.Invoke(data);
                    break;
                case SocketEventType.Received:
                    OnMessageReceived.Invoke(data);
                    break;
            }
        }

        virtual public void Send(byte[] bytes)
        {
            Socket.Send(bytes);
        }

        [Serializable]
        public class SocketClientEvent : UnityEvent<TData> { }
    }
}
