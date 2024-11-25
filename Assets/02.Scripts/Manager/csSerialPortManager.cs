using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;
using UnityEngine.SceneManagement;

public class csSerialPortManager : MonoBehaviour
{
    public static csSerialPortManager instance;

    public SerialPort mySerial;

    public string SerialPORT = "COM12";

    public bool b_ConnectDevice = false;

    public char[] serialSNDBUFF;
    public char[] serialRCVBUFF;

    public float SerialRateRCV = 0.0f;
    public float TimersRCV = 0.0f;

    void OnApplicationQuit()
    {
        if(mySerial != null)
        {
            b_ConnectDevice = false;
            sendDATA('A', '0');
            mySerial.Close();
        }
    }

    void Awake()
    {
        if (csSerialPortManager.instance == null)
        {
            csSerialPortManager.instance = this;
        }

        DontDestroyOnLoad(gameObject);

        readSETTING();
    }
    
    void Start()
    {
        serialRCVBUFF = new char[4];
        serialSNDBUFF = new char[4];

        SerialSetting();

        if(b_ConnectDevice)
        {
            sendDATA('A', '0');
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (csSerialPortManager.instance.b_ConnectDevice)
            {
                csSerialPortManager.instance.sendDATA('A', '0');
            }
            SceneManager.LoadScene(1);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            if (csSerialPortManager.instance.b_ConnectDevice)
            {
                csSerialPortManager.instance.sendDATA('A', '0');
            }
            SceneManager.LoadScene(2);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            if (csSerialPortManager.instance.b_ConnectDevice)
            {
                csSerialPortManager.instance.sendDATA('A', '0');
            }
            SceneManager.LoadScene(3);
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (csSerialPortManager.instance.b_ConnectDevice)
            {
                csSerialPortManager.instance.sendDATA('A', '0');
            }
            SceneManager.LoadScene(0);
        }

        MAINSERIAL();
    }

    void readSETTING()
    {
        string configPath = "";
        configPath = "./SERIALPORT.CFG";

        System.IO.StreamReader sr = new System.IO.StreamReader(configPath);

        if (sr == null)
        {
            SerialPORT = "COM9";
            return;
        }

        string line = "";
        line = sr.ReadLine();


        while (line != null)
        {
            string[] tokens = line.Split(',');
            if (tokens[0] == "COMPORT")
            {
                SerialPORT = tokens[1];
            }
            line = sr.ReadLine();
        }
        sr.Close();
    }

    public void SerialSetting()                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
    {
        if (mySerial != null)
        {
            if (mySerial.IsOpen)
            {
                mySerial.Close();
            }
        }
        
        string connectPORT = @"\\.\" + SerialPORT;
        mySerial = new SerialPort(connectPORT, 9600, Parity.None, 8, StopBits.One);
        
        mySerial.ReadTimeout = 100;

        try
        {
            mySerial.Open();

            sendDATA('A', '0');
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        if (mySerial.IsOpen)
        {
            //Debug.Log("COM Open " + SerialPORT);
            b_ConnectDevice = true;
            
        }
        else
        {
            //Debug.Log("COM Not Open " + SerialPORT);
            b_ConnectDevice = false;
        }
    }

    byte ReadData()
    {
        byte tmpByte;
        tmpByte = (byte)mySerial.ReadByte();
        return tmpByte;
    }

    public void sendDATA(char _id, char _command)
    {
        if (!b_ConnectDevice)
        {
            Debug.Log("return");
            return;
        }

        // STX
        serialSNDBUFF[0] = '#';

        // ID
        serialSNDBUFF[1] = _id;

        // COMMAND
        serialSNDBUFF[2] = _command;

        // ETX
        serialSNDBUFF[3] = '*';

        mySerial.Write(serialSNDBUFF, 0, 4);
    }

    void MAINSERIAL()
    {
        int length = 0;

        if (b_ConnectDevice)
        {
            if (TimersRCV >= SerialRateRCV)
            {
                TimersRCV = 0.0f;

                length = mySerial.Read(serialRCVBUFF, 0, 4);

                if (serialRCVBUFF[0] == '$' && serialRCVBUFF[1] == 'A' && serialRCVBUFF[2] == '1')
                {
                    // 거실

                    csSerialPortManager.instance.sendDATA('A', '0');

                    SceneManager.LoadScene(1);
                }
                else if(serialRCVBUFF[0] == '$' && serialRCVBUFF[1] == 'A' && serialRCVBUFF[2] == '2')
                {
                    // 주방
                    csSerialPortManager.instance.sendDATA('A', '0');
                    SceneManager.LoadScene(2);
                }
                else if (serialRCVBUFF[0] == '$' && serialRCVBUFF[1] == 'A' && serialRCVBUFF[2] == '3')
                {
                    // 유치원
                    csSerialPortManager.instance.sendDATA('A', '0');
                    SceneManager.LoadScene(3);
                }
                else if (serialRCVBUFF[0] == '$' && serialRCVBUFF[1] == 'A' && serialRCVBUFF[2] == '0')
                {
                    // 종료
                    csSerialPortManager.instance.sendDATA('A', '0');
                    //csMainManager.instance.Quit();
                    SceneManager.LoadScene(0);
                }

            }
            else
            {
                TimersRCV += Time.deltaTime;
            }
        }
        
            
        
    }
}
