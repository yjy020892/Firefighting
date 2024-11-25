using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpaceShipHandler : MonoBehaviour
{
    public SpacShipManager spacShipManager;

    public bool isMoving = false;
    public bool isSpin = false;

    private Vector3 dir;
    public float speed = 1;
    public float speedMin = 3;
    public float speedMax = 8;
    public RectTransform target;
    public GameObject effect;

    private void OnEnable()
    {
        speed = Random.Range(speedMin, speedMax);
        SetDirect();
    }
   

    void Update ()
    {
        if (!isMoving)
            return;

        if(target != null)
        {
            MovingPosition();
            if (isSpin)
                MovingAngle();
        }


        //Debug.Log(transform.position +" / "+ target.transform.position);
        if (transform.position == target.transform.position)
            HitEvent();
    }


    public void SetDirect()
    {
        GetComponent<RectTransform>().LookAt(target);
    }





    private void DirectTarget()
    {
        //transform.eulerAngles = new Vector3(0, 0, Random.Range(0,360));
    }

    private void MovingPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed);
    }

    private void MovingAngle()
    {
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z+3);
    }


    public void CheckArea(byte[] bytes)
    {
        RectTransform rt = GetComponent<RectTransform>();
        Vector2 pos = rt.anchoredPosition;
        bool isCheck = true;
        int pointX = 0;
        int pointY = 0;
        int width = 0;
        int height = 0;
        int value = 0;
       
        if (pos.x > ((Screen.width / -2) - (rt.sizeDelta.x / 2)) && pos.x < ((Screen.width / 2) + (rt.sizeDelta.x / 2)))
        {
            if (pos.y < ((Screen.height / 2) + (rt.sizeDelta.y / 2)) && pos.y > ((Screen.height / -2) - (rt.sizeDelta.x / 2)))
                isCheck = true;
        }


        if (isCheck)
        {
            pointX = ((int)pos.x + Screen.width / 2) - ((int)rt.sizeDelta.x / 2);
            pointY = (((int)pos.y - Screen.height / 2) + ((int)rt.sizeDelta.y / 2))*(-1);
            width = (int)rt.sizeDelta.x;
            height = (int)rt.sizeDelta.y;

            if(pointX < 0)
            {
                width = width + pointX;
                pointX = 0;
            }
            if(pointY < 0)
            {
                height = height + pointY;
                pointY = 0;
            }
            if((pointX + width) > 1024)
            {
                width = width - ((pointX + width) - 1024);
            }
            if ((pointY + height) > 768)
            {
                height = height - ((pointY + height) - 768);
            }

            for(int i=0; i< height; i++)
            {
                for(int j=0; j<width; j++)
                {
                    value = ((pointY * 1024) + (i * 1024)) + (pointX + j);
                    if (bytes[value] > 0)
                    {
                        if (isMoving)
                            HitEvent();
                        break;
                    }
                    
                }
            }
        }
    }


    public void HitEvent()
    {
        //Debug.Log("HitEvent");
        isMoving = false;
        GameObject obj = Instantiate(effect);
        obj.transform.position = gameObject.transform.position;
        spacShipManager.RemoveItem(this.gameObject);
    }
}
