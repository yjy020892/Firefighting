using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum FireContents
{
    NONE,
    INTRO,
    LIVINGROOM,
    KITCHEN,
    KINDERGARTEN,
}

public class csMainManager : MonoBehaviour
{
    public static csMainManager instance;

    public FireContents fireContents = FireContents.NONE;

    private float sceneTimer = 0.0f;
    public float sceneTimerVal = 60.0f;

    public bool b_Mouse;

    #region Halloween Data

    #endregion

    void Awake()
    {
        if (csMainManager.instance == null)
        {
            csMainManager.instance = this;
        }

        b_Mouse = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        CheckSceneName();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(fireContents != FireContents.INTRO)
        {
            ReadSETTING();

            csFireManager.instance.finishTimerSlider.maxValue = sceneTimerVal;
            csFireManager.instance.finishTimerSlider.value = csMainManager.instance.sceneTimerVal;
        }

        //Screen.SetResolution(1024, 768, false);
    }

    void CheckSceneName()
    {
        string sceneNameStr = SceneManager.GetActiveScene().name;

        switch (sceneNameStr)
        {
            case "Intro":
                fireContents = FireContents.INTRO;
                break;

            case "LivingRoom":
                fireContents = FireContents.LIVINGROOM;
                break;

            case "Kitchen":
                fireContents = FireContents.KITCHEN;
                break;

            case "Kindergarten":
                fireContents = FireContents.KINDERGARTEN;
                break;

                //default:
                //    wallContents = WallContents.NONE;
                //    break;
        }
    }

    void Update()
    {
        SceneRoutine();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }

        if(Input.GetKeyDown(KeyCode.F10))
        {
            if(Cursor.visible)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                b_Mouse = false;
            }
            else if(!Cursor.visible)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                b_Mouse = true;
            }
        }
    }

    void SceneRoutine()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            int sceneNum = SceneManager.GetActiveScene().buildIndex;
            
            if (sceneNum != 4)
            {
                SceneManager.LoadScene(sceneNum + 1);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int sceneNum = SceneManager.GetActiveScene().buildIndex;
            
            if (sceneNum != 0)
            {
                SceneManager.LoadScene(sceneNum - 1);
            }
            else
            {
                SceneManager.LoadScene(4);
            }
        }

        sceneTimer += Time.deltaTime;

        if (sceneTimer > sceneTimerVal)
        {
            switch (fireContents)
            {
                case FireContents.LIVINGROOM:
                    SceneManager.LoadScene(1);
                    break;

                case FireContents.KITCHEN:
                    SceneManager.LoadScene(3);
                    break;

                case FireContents.KINDERGARTEN:
                    SceneManager.LoadScene(4);
                    break;
            }
        }
    }

    void ReadSETTING()
    {
        string configPath = "";
        configPath = "./DATA.CFG";

        System.IO.StreamReader sr = new System.IO.StreamReader(configPath);

        if (sr == null)
        {
            sceneTimerVal = 60.0f;
            //SerialPORT = "COM9";
            return;
        }

        string line = "";
        line = sr.ReadLine();


        while (line != null)
        {
            string[] tokens = line.Split(',');
            if (tokens[0] == "SCENE_TIMER")
            {
                sceneTimerVal = float.Parse(tokens[1]);
                //SerialPORT = tokens[1];
            }
            else if(tokens[0] == "FINAL_FIRE_VALUE")
            {
                csFireManager.instance.finalFireVal = int.Parse(tokens[1]);
            }
            else if (tokens[0] == "FIRST_FIRE_VALUE")
            {
                csFireManager.instance.firstFireVal = int.Parse(tokens[1]);
            }
            else if (tokens[0] == "SECOND_FIRE_VALUE")
            {
                csFireManager.instance.secondFireVal = int.Parse(tokens[1]);
            }
            else if (tokens[0] == "THIRD_FIRE_VALUE")
            {
                csFireManager.instance.thirdFireVal = int.Parse(tokens[1]);
            }

            line = sr.ReadLine();
        }
        sr.Close();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
