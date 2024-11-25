using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Xml;
using UnityEngine.Networking;

namespace Ezwith.Network
{
    public class Http
    {
        static public IEnumerator Request(string url, string method, WWWForm data, Action<UnityWebRequest> callback)
        {
            UnityWebRequest request = null;
            switch (method.ToUpper())
            {
                case "PUT":
                    string _data = data.ToString();
                    request = UnityWebRequest.Put(url, _data);
                    break;
                case "POST":
                    request = UnityWebRequest.Post(url, data);
                    break;
                case "DELETE":
                    request = UnityWebRequest.Delete(url);
                    break;
                default:
                    request = UnityWebRequest.Get(url);
                    break;
            }
            yield return request.Send();
            callback(request);
        }

        static public IEnumerator Post(string url, WWWForm data, Action<UnityWebRequest> callback)
        {
            return Request(url, "POST", data, callback);
        }

        static public IEnumerator Update(string url, WWWForm data, Action<UnityWebRequest> callback)
        {
            return Request(url, "UPDATE", data, callback);
        }

        static public IEnumerator Delete(string url, Action<UnityWebRequest> callback)
        {
            return Request(url, "DELETE", null, callback);
        }

        static public IEnumerator Get(string url, Action<UnityWebRequest> callback)
        {
            return Request(url, "GET", null, callback);
        }

        static public IEnumerator Get(string url, string[] urlParams, Action<UnityWebRequest> callback)
        {
            if (urlParams != null) return Request(url + "/" + string.Join("/", urlParams), "GET", null, callback);

            return Request(url, "GET", null, callback);
        }

        static public IEnumerator WaitForWWW(WWW www, Action<bool> callback)
        {
            yield return www;
            if (www.error != null)
            {
                Debug.LogErrorFormat("WaitForWWW: Fail! {0} -> {1}", www.url, www.error);
                callback(false);
            }
            else
            {
                Debug.LogFormat("WaitForWWW: Success! {0} -> {1}", www.url, www.text);
                callback(true);
            }
        }
    }
}