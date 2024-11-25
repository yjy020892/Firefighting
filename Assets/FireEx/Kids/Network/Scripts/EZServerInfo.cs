namespace Ezwith.Network
{
    public class EZServerInfo
    {
        public string ipaddress;
        public string name;
        public int port;
        public bool autoConnect;
        public EZServerInfo(string ipaddress, int port, string name, bool autoConnect)
        {
            this.ipaddress = ipaddress;
            this.port = port;
            this.name = name;
            this.autoConnect = autoConnect;
        }

    }
}