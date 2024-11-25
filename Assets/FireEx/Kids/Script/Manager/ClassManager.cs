using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassManager : MonoBehaviour
{
    //Manager
    public MainManager mainManager;
    public SceneWallManager sceneWallManager;
    //public SceneManager sceneManager;
    public TCPManager tcpManager;
    public TCPServer tcpServer;
    public RayManager raymanager;
    public AnimalManager animalManager;
    public SpacShipManager spacShipManager;
    public BalloonManager balloonManager;
    public MMFManager mMFManager;

    //Controller
    public XmlController xmlController;


    //Handler
    public TCPServerHandler tcpServerhandler;

}
