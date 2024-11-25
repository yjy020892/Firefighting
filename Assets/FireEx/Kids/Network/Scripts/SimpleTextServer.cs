using System;
using System.Text;

namespace Ezwith.Network.Example
{
    public class SimpleTextServer : SocketServerHandler<TextSocketData>
    {
        public SimpleTextServer(): base()
        { 
        }

        public void SendToAll(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            SendToAll(bytes);
        }

    }
}