using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Ezwith.Network;
using System.Net;
using System;

namespace Ezwith.Network.Example
{
    public class SampleServerSceneController : MonoBehaviour
    {

        public Text textServerInfo;
        public Text textClientList;
        public Text logText;
        public ScrollRect logTextRect;
        public int maxLogLength = 5000;
        public InputField portField;
        public InputField messageField;
        public Button toggleListenButton;

        private SimpleTextServer server;
        private int lastClientCount;
        private string logTextContent = "";

        public void Send()
        {
            string message = messageField.text;
            if (string.IsNullOrEmpty(message) || !server.IsBound) return;

            server.SendToAll(messageField.text);
            AppendLogText("Send To All: " + messageField.text);
        }

        void Start()
        {
            Debug.Log("SampleServerSceneController:: Start");
            server = (SimpleTextServer)GameObject.FindObjectOfType(typeof(SimpleTextServer));
            portField.text = server.port.ToString();

            IPAddress localAddress = Array.Find(Dns.GetHostEntry(Dns.GetHostName()).AddressList, item => item.AddressFamily == AddressFamily.InterNetwork);
            AppendLogText("local address: " + localAddress.ToString());
        }

        public void ToggleListen()
        {
            if(!server.IsBound)
            {
                Debug.Log("SampleServerScene:: Start Listening");

                int port = int.Parse(portField.text);
                server.port = port;

                if (server.Listen())
                {
                    server.OnClientConnected.AddListener(OnClientConnected);
                    server.OnClientDisconnected.AddListener(OnClientDisconnected);
                    server.OnReceiveMessage.AddListener(OnReceiveMessage);
                    portField.interactable = false;
                    AppendLogText(string.Format("Server is bound"));
                    Debug.LogFormat("Server is bound: {0}", server.Server.isBound);
                }
                else
                {
                    AppendLogText("Address already in use");
                }
            }
            else
            {
                server.OnClientConnected.RemoveListener(OnClientConnected);
                server.OnClientDisconnected.RemoveListener(OnClientDisconnected);
                server.OnReceiveMessage.RemoveListener(OnReceiveMessage);

                server.Close();
                portField.interactable = true;
                AppendLogText("Server Closed");
            }

            toggleListenButton.GetComponentInChildren<Text>().text = server.IsBound ? "Close": "Listen";
        }

        void OnReceiveMessage(TextSocketData data)
        {
            AppendLogText("Received: " + data.Message);
        }

        void OnClientConnected(TextSocketData data)
        {
            AppendLogText("Client connected: " + data.Message);
        }
        void OnClientDisconnected(TextSocketData data)
        {
            AppendLogText("Client disconnected from: " + data.Message);
        }

        void FixedUpdate()
        {
            if (server.HasClients)
            {
                if (lastClientCount != server.ClientCount)
                {
                    var sb = new StringBuilder();
                    // foreach (string key in server.Clients.Keys)
                    for(int i = 0; i < server.ClientCount; ++i)
                    {
                        var client = server.GetClient(i);
                        if (client != null)
                            sb.Append(string.Format("{0}:{1}\n", i, client.RemoteEndPoint.ToString()));
                    }

                    textClientList.text = sb.ToString();
                    lastClientCount = server.ClientCount;
                }
            }
            else if (lastClientCount > 0)
            {
                textClientList.text = "No Client yet..";
                lastClientCount = 0;
            }
        }
        private void AppendLogText(string text)
        {
            logTextContent += Environment.NewLine + text;

            if (logTextContent.Length > maxLogLength)
            {
                logTextContent = logTextContent.Substring(logTextContent.Length - maxLogLength);
            }

            logText.text = logTextContent;
            Canvas.ForceUpdateCanvases();
            logTextRect.verticalNormalizedPosition = 0;
        }
    }
}
