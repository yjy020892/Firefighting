using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class csFireSceneManager : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            if (csSerialPortManager.instance.b_ConnectDevice)
            {
                csSerialPortManager.instance.sendDATA('A', '0');
            }
            SceneManager.LoadScene(1);
        }
        else if(Input.GetKeyDown(KeyCode.F2))
        {
            if (csSerialPortManager.instance.b_ConnectDevice)
            {
                csSerialPortManager.instance.sendDATA('A', '0');
            }
            SceneManager.LoadScene(2);
        }
        else if(Input.GetKeyDown(KeyCode.F3))
        {
            if (csSerialPortManager.instance.b_ConnectDevice)
            {
                csSerialPortManager.instance.sendDATA('A', '0');
            }
            SceneManager.LoadScene(3);
        }

        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            if (csSerialPortManager.instance.b_ConnectDevice)
            {
                csSerialPortManager.instance.sendDATA('A', '0');
            }
            SceneManager.LoadScene(0);
        }
    }
}
