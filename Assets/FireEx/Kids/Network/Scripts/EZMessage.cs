using UnityEngine;
using System.Collections;
using System;

namespace Ezwith.Network
{
    public class EZMessage
    {
        public string from;
        public string to;
        public string message;
        public string[] options;
        public EZMessage() {}

        public EZMessage(string from, string to, string message, string[] options)
        {
            this.from = from;
            this.to = to;
            this.message = message;
            this.options = options;
        }

        public override string ToString()
        {
            string optionStr = "";
            if (options != null && options.Length > 0)
                optionStr = string.Join(",", options);

            return "EZMESSAGE: from: " + from + " to: " + to + " msg: " + message + " params: " + optionStr;
        }

        public string Serialize(char delimeter = ';', char delimeterForParams =',', char terminator = '\0')
        {
            var parameters = options != null ? options : new string[] { };
            return string.Join(delimeter.ToString(), new string[] { to, from, message, string.Join(delimeterForParams.ToString(), parameters) }) + terminator;
        }

        
        static public EZMessage Parse(string content, char delimeter = ';', char delimeterForParams =',')
        {
            Debug.LogFormat("ParseMessage: {0}", content);
            string[] buffer = content.Trim().Split(delimeter);
            if (buffer.Length < 3) throw new Exception("Received buffer's length is lower than 3.");

            Queue queue = new Queue(buffer);
            string to = queue.Dequeue().ToString();
            string from = queue.Dequeue().ToString();
            string message = queue.Dequeue().ToString();

            string[] options = new string[] { };
            while (queue.Count > 0)
            {
                string chunk = queue.Dequeue().ToString();
                if (!string.IsNullOrEmpty(chunk))
                {
                    options = chunk.Split(delimeterForParams);
                }
            }
            return new EZMessage(from, to, message, options);
        }

        static public explicit operator EZMessage(string text)
        {
            return Parse(text);
        }
    }
}