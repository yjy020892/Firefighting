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
    public class EZServer : SocketServerHandler<EZMessageSocketData>
    {
        public string serverName = "sampleServer";
        public TerminatorType terminatorType;
        public string customTerminator = "$";
        private Dictionary<string, Socket> registeredClients = new Dictionary<string, Socket>();

        override protected void Awake()
        {
            base.Awake();

            string terminator = terminatorType == TerminatorType.CUSTOM
            ? customTerminator : EZMessageSocketData.TERMINATOR_STRINGS[(int)terminatorType];

            EZMessageSocketData.terminator = terminator;
        }

        public void SendTo(string target, string message, string[] options = null)
        {
            EZMessage msg = new EZMessage(serverName, target, message, options);
            SendTo(msg);
        }

        public void SendToAll(string message, string[] options = null)
        {
            foreach (var key in registeredClients.Keys)
                SendTo(key, message, options);
        }

        public void SendTo(EZMessage message)
        {
            string[] parameters = message.options;
            if (parameters == null) parameters = new string[] { };

            if (!Server.HasClients)
            {
                Debug.Log("client is not found");
                return;
            }

            string content = message.Serialize();

            if (registeredClients.ContainsKey(message.to))
            {
                var bytes = Encoding.UTF8.GetBytes(content);
                var target = registeredClients[message.to];

                Server.Send(target, bytes);
                Debug.LogFormat("Send content: {0}", content);
            }
        }

        override protected void OnClientDisconnectedHandler(EZMessageSocketData data)
        {
            if (registeredClients.ContainsValue(data.Socket))
            {
                foreach (string key in registeredClients.Keys)
                {
                    if (registeredClients[key] == data.Socket)
                    {
                        Debug.LogFormat("Remove Client {0}", key);
                        registeredClients.Remove(key);
                        break;
                        // disconnectQueue.Enqueue(string.Format("{0}: {1}", key, address));
                    }
                }
                Debug.Log("Clients count: " + registeredClients.Count);
            }

            base.OnClientConnectedHandler(data);
        }

        override protected void OnReceiveMessageHandler(EZMessageSocketData data)
        {
            if (data.Message.to != serverName || !data.Socket.IsConnected()) return;

            if (data.Message.message.ToLower() == "register")
            {
                string key = data.Message.from;
                Debug.LogFormat("Register client app name: {0}", key);
                if (registeredClients.ContainsKey(key))
                {
                    Debug.LogFormat("Client with name {0} is already exists. Overwritng it with new one.");

                    registeredClients[key].Close();
                    registeredClients.Remove(key);
                }
                registeredClients[key] = data.Socket;
            }

            base.OnReceiveMessageHandler(data);
        }
    }
}