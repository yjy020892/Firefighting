using Ezwith.Network;
using System;
using System.Text;


public class TCPServerHandler : SocketServerHandler<TextSocketData>
{
    public TCPServerHandler() : base()
    {
    }


    public void SendToAll(string message)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(message);
        SendToAll(bytes);
    }

}