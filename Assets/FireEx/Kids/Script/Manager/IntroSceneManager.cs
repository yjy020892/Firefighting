using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSceneManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            Application.LoadLevel(1);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Application.LoadLevel(2);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Application.LoadLevel(3);
        }
    }

}
