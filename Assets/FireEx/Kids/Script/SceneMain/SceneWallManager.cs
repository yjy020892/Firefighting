using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneWallManager : MonoBehaviour
{

    public ClassManager classManager;


    private void Start()
    {
        classManager = GameObject.Find("ClassManager").GetComponent<ClassManager>();
        classManager.sceneWallManager = this;
        GameObject.Find("MainManager").GetComponent<MainManager>().contentType = MainManager.ContentType.Wall;
    }


    public void CallbackEventDelegate_TcpReceiveMessage(string msg)
    {
        Debug.Log("TcpReceiveMessage : " + msg);
        if (msg == string.Empty)
            return;

        string[] strs_semicolon = msg.Split(';');


        //잘못 들어온 메시지 걸러내기
        if (strs_semicolon.Length == 0)
            return;

        for (int i = 0; i < strs_semicolon.Length; i++)
        {
            string[] strs_rest = strs_semicolon[i].Split(',');

            // POS,x,y,w,h,x,y,w,h,x,y,w,h; 형식으로 들어오는 데이터를 가공해 raymanager.BoxCast로 전달
            if (strs_rest[0] == "POS" && strs_rest.Length == 5)
            {
                for (int j = 0; j < (strs_rest.Length - 1) / 4; j++)
                {
                    float x = float.Parse(strs_rest[(j * 4) + 1]);
                    float y = float.Parse(strs_rest[(j * 4) + 2]);
                    float w = float.Parse(strs_rest[(j * 4) + 3]);
                    float h = float.Parse(strs_rest[(j * 4) + 4]);
                    Debug.Log(x + " / " + y + " / " + w + " / " + h);
                    classManager.raymanager.BoxCast(new Vector2(x, y), new Vector2(w, h));
                }

            }
        }
    }

}
