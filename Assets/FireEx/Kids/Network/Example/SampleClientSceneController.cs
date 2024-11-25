using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

namespace Ezwith.Network.Example
{
    public class SampleClientSceneController : MonoBehaviour
    {
        public Text textClientInfo;
        public Text logText;
        public ScrollRect logTextRect;

        public InputField serverIPAddressField; 
        public InputField serverPortField;
        public InputField messageField;
        public Button connectButton;
        public Button sendButton;

        public int maxLogLength = 5000;
        private SimpleTextClient client;
        private bool isConnected;
        private string logTextContent = "";

        public void Send()
        {
            if (!isConnected) return;

            client.Send(messageField.text);

            AppendLogText("Send: " + messageField.text);
        }

        void Start()
        {
            PrintLocalInfo();
            client = FindObjectOfType<SimpleTextClient>();

            // 인스펙터에서 설정된 서버정보값을 GUI 에 적용한다.
            serverIPAddressField.text = client.remoteIP;
            serverPortField.text = client.remotePort.ToString();

            // 인스펙터에서 유니티 이벤트 리스너를 연결할 수 있고 하기와 같이 코드로 이벤트 리스너를 등록할 수 있다.
            client.OnConnected.AddListener(OnConnected);
            client.OnClosed.AddListener(OnClosed);
            client.OnMessageReceived.AddListener(OnReceived);

        }

        public void OnConnected(TextSocketData data)
        {
            AppendLogText(string.Format("connected to: {0}", data.Message));
            isConnected = true;
            connectButton.GetComponentInChildren<Text>().text = "Disconnect";
            connectButton.interactable = true;
        }

        public void OnClosed(TextSocketData data)
        {
            AppendLogText(string.Format("disconnected from: {0}", data.Message));
            isConnected = false;
            connectButton.interactable = true;
            connectButton.GetComponentInChildren<Text>().text = "Connect";
        }

        public void OnReceived(TextSocketData data)
        {
            AppendLogText(data.Message.ToString());
        }

        public void Connect()
        {
            if (isConnected)
            {
                client.Close();
                connectButton.interactable = false;
                return;
            }
            
            var remoteIp = serverIPAddressField.text;
            var remotePort = int.Parse(serverPortField.text);

            client.remoteIP = remoteIp;
            client.remotePort = remotePort;
            client.Connect();

            connectButton.interactable = false;
        }

        private void PrintLocalInfo()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress localAddress = Array.Find(ipHostInfo.AddressList, item => item.AddressFamily == AddressFamily.InterNetwork);

            textClientInfo.text = string.Format("ip address: {0}", localAddress.ToString());
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
