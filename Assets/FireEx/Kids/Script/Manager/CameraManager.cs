using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera camera;

	void Start ()
    {
        SetOthographicSize();

    }


    private void SetOthographicSize()
    {
        camera.orthographicSize = (Screen.height / 2f) / 10f;
    }


}
