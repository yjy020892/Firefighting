
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace Ezwith.Network
{
    public class ApiInfo
    {
        public string name;
        public string host;
        public Dictionary<string, Request> RequestList { get; private set; }

        public ApiInfo(string name, string host, Dictionary<string, Request> requestList)
        {
            this.name = name;
            this.host = host;
            RequestList = requestList;
        }

        public static ApiInfo Parse(XDocument xml)
        {
            var name = (string)xml.Root.Attribute("name");
            var host = xml.Root.Element("host").Value;
            var requestList = from api in xml.Root.Elements("api")
                              select new {
                                  key = (string)api.Attribute("name"),
                                  value = new Request { url = api.Element("url").Value, method = api.Element("method").Value }
                              };
            var dic = requestList.ToDictionary(e => e.key, e => e.value);

            var res = new ApiInfo(name, host, dic);
            return res;
        }
    }

    public class Request
    {
        public string url;
        public string method;
    }
}