using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csFire : MonoBehaviour
{
    public int fireHPCnt;

    //public ParticleSystem particleFire;
    //public ParticleSystem.MainModule main;
    public GameObject[] fireObj;
    public Transform[] tr;
    //public ParticleSystem.MinMaxCurve minMaxCurve;

    //[HideInInspector] private float fireStartSizeOffset;
    //[HideInInspector] public float fireStartSizeVar;
    
    bool b_MouseOn;

    void Start()
    {
        b_MouseOn = false;

        for (int i = 0; i < fireObj.Length; i++)
        {
            tr[i] = fireObj[i].transform;
        }
        csFireManager.instance.list_Fire.Add(this.gameObject);

        //main = particleFire.main;
        //fireStartSizeOffset = main.startSizeMultiplier;
        //fireStartSizeVar = main.startSizeXMultiplier / 4;
        //minMaxCurve = main.startLifetime;
    }

    // Update is called once per frame
    void Update()
    {
        if (fireHPCnt >= csFireManager.instance.finalFireVal)
        {
            this.gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        for (int i = 0; i < fireObj.Length; i++)
        {
            tr[i].localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        //main.startLifetimeMultiplier = 4;
        //main.startSizeMultiplier = fireStartSizeOffset;

        csFireManager.instance.fireCount += 1;
        
        //Debug.Log("firecnt : " + csFireManager.instance.fireCount);
    }

    void OnMouseOver()
    {
        if(!csMainManager.instance.b_Mouse)
        {
            return;
        }

        if(!csFireManager.instance.b_StartGame)
        {
            return;
        }

        if(b_MouseOn)
        {
            return;
        }

        //if(!csFireManager.instance.b_FirstFireDestroy && gameObject.tag.Equals("Fire"))
        //{
        //    return;
        //}
        //else if(csFireManager.instance.b_FirstFireDestroy && gameObject.tag.Equals("Fire2"))
        //{
        //    return;
        //}

        if(csFireManager.instance.b_Fail || csFireManager.instance.b_Success)
        {
            return;
        }

        //Debug.Log("in");

        b_MouseOn = true;

        StartCoroutine(ResetMouseTime());
    }

    IEnumerator ResetMouseTime()
    {
        GameObject obj = csPooledObject.instance.GetPooledObject_Water(gameObject.transform);
        obj.SetActive(true);

        csSoundManager.instance.PlayfireExtinguisherSound();

        if(gameObject.CompareTag("CameraFire"))
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

                //Debug.Log(fireObj.name + " , " + fireHPCnt);

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

        b_MouseOn = false;
    }
}
