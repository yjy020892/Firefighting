using System;
using System.Net.Sockets;
using System.Text;

namespace Ezwith.Network
{
    public class SocketData<TData> where TData : SocketData<TData>
    {
        public SocketEventType Type { get; private set; }
        public Socket Socket { get; private set; }
        public byte[] Buffer { get; private set; }
        public int ReceivedBytes { get; private set; }
        public SocketData()
        {
        }

        public SocketData(SocketEventType type, byte[] buffer, int receivedBytes, Socket socket)
        {
            Type = type;
            Socket = socket;
            Buffer = buffer;
            ReceivedBytes = receivedBytes;
        }

        public bool IsValidData()
        {
            return Buffer != null && Buffer.Length != 0 && ReceivedBytes != 0 && Buffer.Length >= ReceivedBytes;
        }
    }
}

