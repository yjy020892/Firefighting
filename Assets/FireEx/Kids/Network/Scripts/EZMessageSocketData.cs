using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Ezwith.Network
{
    public enum TerminatorType
    {
        NUL,
        LF,
        CRLF,
        CUSTOM,
    }
    public class EZMessageSocketData : SocketData<EZMessageSocketData>
    {
        public static readonly string[] TERMINATOR_STRINGS = new string[] { "\0", "\n", "\r\n" };
        public static string terminator = "\0";
        public EZMessage Message
        {
            get
            {
                if (!base.IsValidData()) return null;
                var text = Encoding.UTF8.GetString(Buffer);
                if (Type != SocketEventType.Received) return new EZMessage(null, null, text, null);

                try
                {
                    var chunk = text.Substring(0, ReceivedBytes).Split(terminator.ToCharArray());
                    return (EZMessage)chunk[0];
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
                return null;
            }
        }

        public EZMessageSocketData() : base() { }
        public EZMessageSocketData(SocketEventType type, byte[] buffer, int receivedBytes, Socket socket) : base(type, buffer, receivedBytes, socket)
        {
        }
    }
}