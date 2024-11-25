using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpacShipManager : MonoBehaviour
{
    public ClassManager classManager;


    //spaceship
    public Coroutine updateCoroutine;
    public Transform spaceshipContainer;
    public List<GameObject> spaceshipPfList = new List<GameObject>();
    public List<GameObject> spaceshipList = new List<GameObject>();
    public float delay = 0.3f;


    //CreatePoint
    public GameObject pf_Point;
    public Transform pointContainer;
    public float freeSpace = 300;
    public int createPointCount = 10;
   
    public List<RectTransform> pointList_Up = new List<RectTransform>();
    public List<RectTransform> pointList_Down = new List<RectTransform>();
    public List<RectTransform> pointList_Right = new List<RectTransform>();
    public List<RectTransform> pointList_Left = new List<RectTransform>();


    public GameObject obj;

    private void Start()
    {
        classManager = GameObject.Find("ClassManager").GetComponent<ClassManager>();
        classManager.spacShipManager = this;
        GameObject.Find("MainManager").GetComponent<MainManager>().contentType = MainManager.ContentType.Floor;

        CreateTargets(pf_Point);
        updateCoroutine = StartCoroutine(UpdateCoroutine());
    }

    private void Update()
    {
    }

    public void CheckBytes(byte[] bytes)
    {
        Debug.Log(bytes[0] + " / " + bytes[393217]);
        for (int i = 0; i < spaceshipList.Count; i++)
            spaceshipList[i].GetComponent<MovingSpaceShipHandler>().CheckArea(bytes);
    }

    public IEnumerator UpdateCoroutine()
    {
        int count = 0;
        int prefabCount = 0;
        int startCount = 0;
        int targetCount = 0;

        while (true)
        {
            count = Random.Range(0,5);
            prefabCount = Random.Range(0, spaceshipPfList.Count);


            if (count == 0)
            {
                startCount = Random.Range(0, pointList_Up.Count);
                targetCount = Random.Range(0, pointList_Down.Count);

                CreateSpaceship(spaceshipPfList[prefabCount], pointList_Up[startCount], pointList_Down[targetCount]);

            }
            else if (count == 1)
            {
                startCount = Random.Range(0, pointList_Down.Count);
                targetCount = Random.Range(0, pointList_Up.Count);
                CreateSpaceship(spaceshipPfList[prefabCount], pointList_Down[startCount], pointList_Up[targetCount]);
            }
            else if (count == 2)
            {
                startCount = Random.Range(0, pointList_Right.Count);
                targetCount = Random.Range(0, pointList_Left.Count);
                CreateSpaceship(spaceshipPfList[prefabCount], pointList_Right[startCount], pointList_Left[targetCount]);
            }
            else if (count == 3)
            {
                startCount = Random.Range(0, pointList_Left.Count);
                targetCount = Random.Range(0, pointList_Right.Count);
                CreateSpaceship(spaceshipPfList[prefabCount], pointList_Left[startCount], pointList_Right[targetCount]);
            }
            

            yield return new WaitForSecondsRealtime(delay);
        }
    }

    //Create
    private void CreateTargets(GameObject pf_Point)
    {
        //Up Target
        float x = (Screen.width / -2);
        float y = (Screen.height / 2) + freeSpace;
        for (int i = 0; i < createPointCount; i++)
        {
            GameObject obj = Instantiate(pf_Point);
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.SetParent(pointContainer);
            rt.localScale = new Vector3(1, 1, 1);
            rt.eulerAngles = new Vector3(0, 0, 0);
            rt.anchoredPosition3D = new Vector3(x, y, 0);
            x += (Screen.width / (createPointCount - 1));

            pointList_Up.Add(obj.GetComponent<RectTransform>());
        }

        //Down
        x = (Screen.width / -2);
        y = (Screen.height / -2) - freeSpace;
        for (int i = 0; i < createPointCount; i++)
        {
            GameObject obj = Instantiate(pf_Point);
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.SetParent(pointContainer);
            rt.localScale = new Vector3(1, 1, 1);
            rt.eulerAngles = new Vector3(0, 0, 0);
            rt.anchoredPosition3D = new Vector3(x, y, 0);
            x += (Screen.width / (createPointCount - 1));

            pointList_Down.Add(obj.GetComponent<RectTransform>());
        }


        //Right
        x = (Screen.width / 2) + freeSpace;
        y = (Screen.height / 2);
        for (int i = 0; i < createPointCount; i++)
        {
            GameObject obj = Instantiate(pf_Point);
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.SetParent(pointContainer);
            rt.localScale = new Vector3(1, 1, 1);
            rt.eulerAngles = new Vector3(0, 0, 0);
            rt.anchoredPosition3D = new Vector3(x, y, 0);
            y -= (Screen.height / (createPointCount - 1));

            pointList_Right.Add(obj.GetComponent<RectTransform>());
        }

        //Left
        x = (Screen.width / -2) - freeSpace;
        y = (Screen.height / 2);
        for (int i = 0; i < createPointCount; i++)
        {
            GameObject obj = Instantiate(pf_Point);
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.SetParent(pointContainer);
            rt.localScale = new Vector3(1, 1, 1);
            rt.eulerAngles = new Vector3(0, 0, 0);
            rt.anchoredPosition3D = new Vector3(x, y, 0);
            y -= (Screen.height / (createPointCount - 1));

            pointList_Left.Add(obj.GetComponent<RectTransform>());
        }
    }





    public void CreateSpaceship(GameObject spaceship, RectTransform startPoint, RectTransform target)
    {
        GameObject obj = Instantiate(spaceship);
        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.SetParent(spaceshipContainer);
        rt.localScale = new Vector3(1, 1, 1);
        rt.eulerAngles = new Vector3(0, 0, 0);
        rt.anchoredPosition3D = startPoint.anchoredPosition3D;
        obj.GetComponent<MovingSpaceShipHandler>().spacShipManager = this;
        obj.GetComponent<MovingSpaceShipHandler>().target = target;
        if (spaceship == spaceshipPfList[0] || spaceship == spaceshipPfList[1] || spaceship == spaceshipPfList[2] || spaceship == spaceshipPfList[6])
            obj.GetComponent<MovingSpaceShipHandler>().isSpin = true;
        obj.GetComponent<MovingSpaceShipHandler>().isMoving = true;

        spaceshipList.Add(obj);
    }







    public void RemoveItem(GameObject item)
    {
       for(int i=0;i<spaceshipList.Count; i++)
        {
            if(item == spaceshipList[i])
            {
                DestroyImmediate(spaceshipList[i]);
                spaceshipList.RemoveAt(i);
                break;
            }
        }
    }














}
