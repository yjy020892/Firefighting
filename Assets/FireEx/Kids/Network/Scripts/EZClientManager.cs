using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

namespace Ezwith.Network
{
    public class EZClientManager : MonoBehaviour
    {
        public bool showLog = false;
        [SerializeField]
        public EZClient.SocketClientEvent OnAllClientConnected;
        [SerializeField]
        public EZClient.SocketClientEvent OnAllClientClosed;
        [SerializeField]
        public EZClient.SocketClientEvent OnMessageReceived;

        public string appName;
        [HideInInspector] public EZClient[] clients;

        private int connectedClientCount;
        public int ConnectedClientCount
        {
            get
            {
                return connectedClientCount;
            }
        }

        public string GetClientAddress(string clientName)
        {
            var client = Array.Find(clients, c => c.gameObject.name == clientName);
            if (client == null) return null;
            return client.remoteIP;
        }

        public int GetClientPort(string clientName)
        {
            var client = Array.Find(clients, c => c.gameObject.name == clientName);
            if (client == null) return 0;
            return client.remotePort;
        }

        public void Initialize(XmlDocument xml)
        {
            CloseAll();

            List<EZServerInfo> servers = new List<EZServerInfo>();
            var serversNode = xml.SelectNodes("config/server[@type='socket']");
            foreach (XmlNode s in serversNode)
            {
                var a = s.SelectSingleNode("ipaddress").InnerText;
                var p = int.Parse(s.SelectSingleNode("port").InnerText);
                var n = s.SelectSingleNode("name").InnerText;
                var c = s.SelectSingleNode("autoconnect").InnerText.ToLower() == "true";
                var server = new EZServerInfo(a, p, n, c);
                servers.Add(server);
            }

            Initialize(servers.ToArray());
        }

        public void Initialize(EZServerInfo[] servers)
        {
            Debug.Log("NetworkManager:: Initialize");
            CloseAll();
            connectedClientCount = 0;
            int len = servers.Length;
            clients = new EZClient[len];
            int i = 0;
            Array.ForEach(servers, (EZServerInfo info) =>
            {
                GameObject networkObj = new GameObject();
                networkObj.name = "AgentClient " + i;
                networkObj.transform.SetParent(transform);
                EZClient client = (EZClient)networkObj.AddComponent(typeof(EZClient));
                client.appName = appName;
                client.ServerInfo = info;
                client.autoConnecting = true;
                client.OnMessageReceived.AddListener(OnMessageReceivedHandler);
                client.OnConnected.AddListener(OnConnectedHandler);
                client.OnClosed.AddListener(OnClosedHandler);
                clients[i++] = client;
            });
        }

        public void SendTo(string serverName, string message, string[] options = null)
        {
            EZMessage msg = new EZMessage(appName, serverName, message, options);
            SendTo(msg);
        }

        public void SendTo(EZMessage message)
        {
            if (clients == null || clients.Length == 0) throw new Exception("Connected client not found");

            Array.ForEach(clients, c => c.Send(message));
        }

        public void SendToAll(string message, string[] options = null)
        {
            foreach (EZClient client in clients)
            {
                EZMessage msg = new EZMessage(client.name, client.serverName, message, options);
                client.Send(msg);
            }
        }

        public void CloseAll()
        {
            foreach (var c in clients)
            {
                c.Close();
                Destroy(c.gameObject, .5f);
            }
            clients = null;
        }

        private void OnConnectedHandler(object clientName)
        {
            connectedClientCount++;
            if (connectedClientCount == clients.Length)
            {
                OnAllClientConnected.Invoke(null);
            }
        }

        private void OnClosedHandler(object clientName)
        {
            connectedClientCount--;
            if (connectedClientCount == 0)
            {
                OnAllClientClosed.Invoke(null);
            }
        }

        private void OnMessageReceivedHandler(EZMessageSocketData data)
        {
            if (data == null) return;
            OnMessageReceived.Invoke(data);
        }
    }
}