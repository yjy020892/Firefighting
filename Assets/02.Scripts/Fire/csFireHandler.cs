using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csFireHandler : MonoBehaviour
{
    public GameObject[] fireObj;
    public Transform[] tr;

    public int fireHPCnt;

    public bool b_Check = false;

    void OnEnable()
    {
        b_Check = false;
    }

    void OnDisable()
    {
        for (int i = 0; i < fireObj.Length; i++)
        {
            tr[i].localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

    void Update()
    {
        if (fireHPCnt >= csFireManager.instance.finalFireVal)
        {
            this.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        for (int i = 0; i < fireObj.Length; i++)
        {
            tr[i] = fireObj[i].transform;
        }
    }

    public void CheckArea(byte[] bytes)
    {
        if (b_Check)
        {
            return;
        }

        //오브젝트 포지션을 화면상의 포지션으로 변경.
        Vector2 pos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        bool isCheck = true;
        int pointX = 0;  //byte상의 포지션x
        int pointY = 0;  //byte상의 포지션y
        int width = 100; //체크할 오브젝트 넓이.
        int height = 100; //체크할 오브젝트 높이.
        int value = 0; //검사할 배열번호

        //오브젝트 피벗에서 UI상의 피벗으로 치환.
        pos = new Vector2(pos.x - (Screen.width / 2) - (width / 2), pos.y - (Screen.height / 2) - (height / 2));

        //오브젝트가 화면 밖을 완전 벗어나는지 체크.
        if (pos.x > ((Screen.width / -2) - (width / 2)) && pos.x < ((Screen.width / 2) + (width / 2)))
        {
            if (pos.y < ((Screen.height / 2) + (height / 2)) && pos.y > ((Screen.height / -2) - (height / 2)))
                isCheck = true;
        }

        if (isCheck)
        {
            pointX = ((int)pos.x + Screen.width / 2) - (width / 2);
            pointY = (((int)pos.y - Screen.height / 2) + (height / 2)) * (-1);

            if (pointX < 0)
            {
                //오브젝트가 0보다 작아질때 예외처리
                width = width + pointX;
                pointX = 0;
            }
            if (pointY < 0)
            {
                //오브젝트가 0보다 작아질때 예외처리
                height = height + pointY;
                pointY = 0;
            }
            if ((pointX + width) > 1024)
            {
                //오브젝트가 화면을 벗어나는 부분에 대한 With축소.
                width = width - ((pointX + width) - 1024);
            }
            if ((pointY + height) > 768)
            {
                //오브젝트가 화면을 벗어나는 부분에 대한 height축소.
                height = height - ((pointY + height) - 768);
            }

            //오브젝트 넓이를 바이트 배열의 넘버로 변환해 바이트 배열의 해당 값을 체크해 이벤트 발생.
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    value = ((pointY * 1024) + (i * 1024)) + (pointX + j);
                    if (bytes[value] > 0)
                    {
                        if (!b_Check)
                        {
                            //if (!csFireManager.instance.b_FirstFireDestroy && gameObject.tag.Equals("Fire"))
                            //{
                            //    return;
                            //}
                            //else if (csFireManager.instance.b_FirstFireDestroy && gameObject.tag.Equals("Fire2"))
                            //{
                            //    return;
                            //}

                            if (!csFireManager.instance.b_StartGame)
                            {
                                return;
                            }

                            if (csFireManager.instance.b_Fail || csFireManager.instance.b_Success)
                            {
                                return;
                            }

                            Check();
                        }
                        //if (isPlay)
                        //    HitEvent();
                        break;
                    }
                }
            }
        }
    }

    private void Check()
    {
        b_Check = true;

        StartCoroutine(ResetFire());
    }

    IEnumerator ResetFire()
    {
        GameObject obj = csPooledObject.instance.GetPooledObject_Water(gameObject.transform);
        obj.SetActive(true);

        csSoundManager.instance.PlayfireExtinguisherSound();

        if (gameObject.CompareTag("CameraFire"))
        {
            if (fireHPCnt <= csFireManager.instance.finalFireVal)
            {
                fireHPCnt += 1;
            }
        }
        else
        {
            if (fireHPCnt <= csFireManager.instance.finalFireVal)
            {
                fireHPCnt += 1;

                if (fireHPCnt == csFireManager.instance.firstFireVal)
                {
                    for (int i = 0; i < fireObj.Length; i++)
                    {
                        tr[i].localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    }
                }
                else if (fireHPCnt == csFireManager.instance.secondFireVal)
                {
                    for (int i = 0; i < fireObj.Length; i++)
                    {
                        tr[i].localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    }
                }
                else if (fireHPCnt == csFireManager.instance.thirdFireVal)
                {
                    for (int i = 0; i < fireObj.Length; i++)
                    {
                        tr[i].localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    }
                }
            }
        }
        
        yield return new WaitForSeconds(1.0f);


        b_Check = false;
    }
}
