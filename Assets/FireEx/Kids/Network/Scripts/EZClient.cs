using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Ezwith.Network
{
    public class EZClient : SocketClientHandler<EZMessageSocketData>
    {
        public EZServerInfo ServerInfo
        {
            get
            {
                return new EZServerInfo(remoteIP, remotePort, serverName, autoConnecting);
            }
            set
            {
                remoteIP = value.ipaddress;
                remotePort = value.port;
                autoConnecting = value.autoConnect;
                serverName = value.name;

                Init();
            }
        }

        //-------------------------------------------------------------------/
        // Socket Settings & variables
        //-------------------------------------------------------------------/
        public string serverName;
        public string appName;
        public TerminatorType terminatorType;
        public string customTerminator = "$";
        public void Send(EZMessage message)
        {
            Debug.Log(message.ToString());
            var bytes = Encoding.UTF8.GetBytes(message.Serialize());
            Socket.Send(bytes);
        }

        override protected void Start()
        {
            base.Start();

            string terminator = terminatorType == TerminatorType.CUSTOM
            ? customTerminator : EZMessageSocketData.TERMINATOR_STRINGS[(int)terminatorType];

            EZMessageSocketData.terminator = terminator;
        }
    }
}