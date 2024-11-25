using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class TCPManager : MonoBehaviour
{
    public XmlController xmlController;
    public TCPServer tcpServer;
    public string ip = string.Empty;
    public int port = 0;


    void Start()
    {
        xmlController.loadCompleteXml += DelegateCallback_loadCompleteXml;
        xmlController.ReadXml("TCPData");
    }

    private void DelegateCallback_loadCompleteXml(XmlDocument xml)
    {
        xmlController.loadCompleteXml -= DelegateCallback_loadCompleteXml;

        XmlNodeList xmlNodeList_ip = xml.GetElementsByTagName("IP");
        ip = xmlNodeList_ip[0].InnerText;
        XmlNodeList xmlNodeList_port = xml.GetElementsByTagName("Port");
        port = int.Parse(xmlNodeList_port[0].InnerText);
        tcpServer.StartServer(port);
    }



}
