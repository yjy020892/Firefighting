using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Ezwith.Network;
using System.Net;
using System;

public class TCPServer : MonoBehaviour
{
    public delegate void ReceiveMessage(string message);
    public event ReceiveMessage receiveMessage;
    
    public TCPServerHandler server;
    private int lastClientCount;
    private string logTextContent = "";

    public void Send(string message)
    {
        if (string.IsNullOrEmpty(message) || !server.IsBound) return;

        server.SendToAll(message);
        Debug.Log("Send Message : " + message);
    }


    public void StartServer(int port)
    {
        server.port = port;

        if (!server.IsBound)
        {
            Debug.Log("Start TCP server");

            server.port = port;

            if (server.Listen())
            {
                server.OnClientConnected.AddListener(OnClientConnected);
                server.OnClientDisconnected.AddListener(OnClientDisconnected);
                server.OnReceiveMessage.AddListener(OnReceiveMessage);
                //Debug.Log(string.Format("Server is bound"));
                //Debug.LogFormat("Server is bound: {0}", server.Server.isBound);
            }
            else
            {
                Debug.Log("Address already in use");
            }
        }
        else
        {
            server.OnClientConnected.RemoveListener(OnClientConnected);
            server.OnClientDisconnected.RemoveListener(OnClientDisconnected);
            server.OnReceiveMessage.RemoveListener(OnReceiveMessage);

            server.Close();
            Debug.Log("Server Closed");
        }
    }

    void OnReceiveMessage(TextSocketData data)
    {
        //Debug.Log("Received: " + data.Message);
        if (receiveMessage != null)
            receiveMessage(data.Message);
    }

    void OnClientConnected(TextSocketData data)
    {
        Debug.Log("Client connected: " + data.Message);
    }
    void OnClientDisconnected(TextSocketData data)
    {
        Debug.Log("Client disconnected from: " + data.Message);
    }

    void FixedUpdate()
    {
        if (server.HasClients)
        {
            if (lastClientCount != server.ClientCount)
            {
                var sb = new StringBuilder();
                // foreach (string key in server.Clients.Keys)
                for (int i = 0; i < server.ClientCount; ++i)
                {
                    var client = server.GetClient(i);
                    if (client != null)
                        sb.Append(string.Format("{0}:{1}\n", i, client.RemoteEndPoint.ToString()));
                }

                //textClientList.text = sb.ToString();
                lastClientCount = server.ClientCount;
            }
        }
        else if (lastClientCount > 0)
        {
            //textClientList.text = "No Client yet..";
            lastClientCount = 0;
        }
    }

    /*
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
    */


}