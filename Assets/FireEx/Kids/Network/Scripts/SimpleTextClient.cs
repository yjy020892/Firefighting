using System;
using System.Net.Sockets;
using System.Text;
using Ezwith.Network;

namespace Ezwith.Network
{
    public class SimpleTextClient : SocketClientHandler<TextSocketData>
    {
        public void Send(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            Send(bytes);
        }
    }

    public class TextSocketData : SocketData<TextSocketData>
    {
        public string Message
        {
            get
            {
                try
                {
                    if (!base.IsValidData()) return null;
                    return Encoding.UTF8.GetString(Buffer, 0, ReceivedBytes);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogException(ex);
                }
                return null;
            }
        }
        public TextSocketData()
        {
        }

        public TextSocketData(SocketEventType type, byte[] buffer, int receivedBytes, Socket socket) : base(type, buffer, receivedBytes, socket)
        {
        }
    }
}