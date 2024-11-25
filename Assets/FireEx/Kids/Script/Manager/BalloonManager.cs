using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalloonManager : MonoBehaviour
{
    public ClassManager classManager;

    public Camera mainCamera;
   
    public List<GameObject> balloonList = new List<GameObject>();
    public List<GameObject> balloonPFList = new List<GameObject>();
    public List<GameObject> pointList = new List<GameObject>();
    public GameObject balloonContainer;
    public int balloonMaxCount = 100;

    public Coroutine updateCoroutine = null;

    //Option
    public GameObject option;
    public MeshRenderer rightWall;
    public MeshRenderer leftWall;

    public GameObject cube;  //Test
    public Image image;  //test

    public GameObject obj;

    void Start ()
    {
        option.SetActive(false);
        rightWall.enabled = option.activeSelf;
        leftWall.enabled = option.activeSelf;

        classManager = GameObject.Find("ClassManager").GetComponent<ClassManager>();
        classManager.balloonManager = this;
        GameObject.Find("MainManager").GetComponent<MainManager>().contentType = MainManager.ContentType.Slider;
        updateCoroutine = StartCoroutine(UpdateCoroutine());
    }
	
	
	void Update ()
    {
        if(Input.GetKeyDown("o"))
        {
            option.SetActive(!option.activeSelf);
            rightWall.enabled = option.activeSelf;
            leftWall.enabled = option.activeSelf;
        }
    }


    private IEnumerator UpdateCoroutine()
    {
        while(true)
        {
            if (balloonList.Count < balloonMaxCount)
                CreateBalloon();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void CheckBytes(byte[] bytes)
    {
        for (int i = 0; i < balloonList.Count; i++)
            balloonList[i].GetComponent<BalloonHandler>().CheckArea(bytes);
    }


    private void CreateBalloon()
    {
        int pointValue = Random.RandomRange(0, pointList.Count);
        int balloonValue = Random.RandomRange(0, balloonPFList.Count);

        GameObject balloon = Instantiate(balloonPFList[balloonValue]);
        balloon.transform.SetParent(balloonContainer.transform);
        balloon.transform.position = pointList[pointValue].transform.position;
        balloon.GetComponent<BalloonHandler>().balloonManager = this;
        balloon.GetComponent<BalloonHandler>().mainCamera = mainCamera;

        balloonList.Add(balloon);
    }


    public void RemoveBalloon(GameObject balloon)
    {
        for(int i=0; i< balloonList.Count; i++)
        {
            if(balloonList[i] == balloon)
            {
                DestroyImmediate(balloonList[i]);
                balloonList.RemoveAt(i);
                break;
            }
        }
    }



}
