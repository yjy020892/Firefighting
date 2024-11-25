using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class XmlController : MonoBehaviour
{
    public delegate void LoadCompleteXml(XmlDocument xmlDocument);
    public event LoadCompleteXml loadCompleteXml;
    public delegate void LoadFailXml();
    public event LoadFailXml loadFailXml;



   

    public void ReadXml(string fileName)
    {
        string xmlUrl = SetUrl(fileName);


        if (File.Exists(xmlUrl))
        {
            //Debug.Log("File.Exists Complete : "+ xmlUrl);
            StreamReader sr = new StreamReader(xmlUrl);
            string data = sr.ReadToEnd();
            if (data.Length > 0)
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(new StringReader(data));
                if (loadCompleteXml != null)
                    loadCompleteXml(xmlDocument);
            }
            else
            {
                //if (onLoadDataNull != null)
                //    onLoadDataNull();
            }
            sr.Close();
        }
        else
        {
            Debug.Log("File.Exists Fail : " + fileName);
            if (loadFailXml != null)
                loadFailXml();
        }
    }


    public void WriteXml(XmlDocument xmlDocument, string fileName)
    {
        string xmlUrl = SetUrl(fileName);
        File.WriteAllText(xmlUrl, xmlDocument.InnerXml);
        //if (onSaveXMLbyXML != null)
        //    onSaveXMLbyXML();
    }



    private string SetUrl(string fileName)
    {
        return Application.streamingAssetsPath + "/XMl/" + fileName + ".xml";
    }


}
