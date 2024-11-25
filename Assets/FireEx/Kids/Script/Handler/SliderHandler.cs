using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderHandler : MonoBehaviour
{
    public GameObject slider;
    public GameObject right;
    public GameObject left;
    public GameObject flor;
    public GameObject[] upPoints = new GameObject[2];
    public GameObject[] downPoints = new GameObject[2];


	void Start ()
    {
      
    }

    //Callback Event
    public void SliderEvent_LeftAngle(Slider slider)
    {
        left.transform.localEulerAngles = new Vector3(0, slider.value, -90);
        SetSize();
    }

    public void Sliderevent_LeftMoving(Slider slider)
    {
        left.transform.localPosition = new Vector3(slider.value, 2.5f, 0);
        SetSize();
    }

    public void SliderEvent_RightAngle(Slider slider)
    {
        right.transform.localEulerAngles = new Vector3(0, slider.value, 90);
        SetSize();
    }

    public void Sliderevent_RightMoving(Slider slider)
    {
        right.transform.localPosition = new Vector3(slider.value, 2.5f, 0);
        SetSize();
    }




    private void SetSize()
    {
        float up_distance = Vector3.Distance(upPoints[0].transform.position, upPoints[1].transform.position);
        float down_distance = Vector3.Distance(downPoints[0].transform.position, downPoints[1].transform.position);
        float distance = 0;
        float posX = 0;

        Debug.Log(up_distance +" / "+ down_distance);
        if (up_distance > down_distance)
        {
            Debug.Log("up_distance");
            distance = up_distance;
            posX = (upPoints[0].transform.position.x + upPoints[1].transform.position.x) / 2;
        }
        else
        {
            Debug.Log("down_distance");
            distance = down_distance;
            posX = (downPoints[0].transform.position.x + downPoints[1].transform.position.x) / 2;
        }
            



        //Debug.Log(distance);
       
        slider.transform.localScale = new Vector3(distance/10f, 1, 1);
        flor.transform.localScale = new Vector3(0.1f, 1, distance/10f);
        slider.transform.localPosition = new Vector3(posX, 0, 0);
        flor.transform.localPosition = new Vector3(posX, 0.5f, -5f);

    }
}
