using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class csFireManager : MonoBehaviour
{
    public static csFireManager instance;

    [HideInInspector] public List<GameObject> list_Fire = new List<GameObject>();

    public AudioSource mainSound;

    public Light mainLignt;

    public Slider finishTimerSlider;

    public GameObject successPanelObj;
    public GameObject failPanelObj;

    public GameObject explosionObj;
    public GameObject wildFireObj;
    public GameObject tinyFlame;

    public GameObject[] livingFireGroup;
    public GameObject[] kitchenFireGroup;
    public GameObject[] kindergartenFireGroup;

    public GameObject fireSmog;
    public GameObject electronicObj;

    public Image numberCnt;
    public Sprite[] numberCntImg;
    public GameObject startSprite;

    public int finalFireVal;
    public int firstFireVal;
    public int secondFireVal;
    public int thirdFireVal;

    public int fireCount = 0;

    private bool b_Burn = false;
    private bool b_Extinguish = false;
    private bool b_WildFire = false;
    public bool b_FirstFireDestroy = false;
    public bool b_Success = false;
    public bool b_Fail = false;

    public bool b_StartGame = false;
    

    public static bool b_SmogEnable = true;

    void Awake()
    {
        if (csFireManager.instance == null)
        {
            csFireManager.instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FirstExplosion());
    }

    // Update is called once per frame
    void Update()
    {
        if (b_Burn)
        {
            if(finishTimerSlider.maxValue > 30)
            {
                if ((finishTimerSlider.maxValue - 10 > finishTimerSlider.value) && !b_WildFire)
                {
                    b_WildFire = true;
                    wildFireObj.SetActive(true);
                }
            }
            else if(finishTimerSlider.maxValue <= 30)
            {
                if ((finishTimerSlider.maxValue - 5 > finishTimerSlider.value) && !b_WildFire)
                {
                    b_WildFire = true;
                    wildFireObj.SetActive(true);
                }
            }

            finishTimerSlider.value -= Time.deltaTime;

            if(finishTimerSlider.value <= 0 && !b_Extinguish)
            {
                csSerialPortManager.instance.sendDATA('A', '0');
                failPanelObj.SetActive(true);
                mainSound.Stop();
                csSoundManager.instance.PlayFailSound();
                b_Fail = true;
                b_Burn = false;

                StartCoroutine(FinishFire());
            }
            else if(finishTimerSlider.value > 0 && b_Extinguish)
            {
                csSerialPortManager.instance.sendDATA('A', '0');
                mainLignt.color = new Color32(255, 255, 255, 255);
                fireSmog.SetActive(false);
                wildFireObj.SetActive(false);
                successPanelObj.SetActive(true);
                mainSound.Stop();
                csSoundManager.instance.PlaySuccessSound();
                b_Success = true;
                b_Burn = false;

                StartCoroutine(FinishFire());

                fireCount = 0;
            }
        }

        if (csMainManager.instance.fireContents == FireContents.LIVINGROOM)
        {
            if (fireCount == 2)
            {
                b_FirstFireDestroy = true;
                //StartCoroutine(SecondExplosion());
            }
            else if(fireCount == 6)
            {
                //Debug.Log("b_Extinguish");
                b_Extinguish = true;
            }
        }
        else if(csMainManager.instance.fireContents == FireContents.KITCHEN)
        {
            if (fireCount == 2)
            {
                b_FirstFireDestroy = true;
                //StartCoroutine(SecondExplosion());
            }
            else if (fireCount == 6)
            {
                b_Extinguish = true;
            }
        }
        else if (csMainManager.instance.fireContents == FireContents.KINDERGARTEN)
        {
            if (fireCount == 2)
            {
                b_FirstFireDestroy = true;
                //StartCoroutine(SecondExplosion());
            }
            else if (fireCount == 6)
            {
                b_Extinguish = true;
            }
        }
    }

    public void CheckBytes(byte[] bytes)
    {
        for (int i = 0; i < list_Fire.Count; i++)
        {
            list_Fire[i].GetComponent<csFireHandler>().CheckArea(bytes);
        }
    }

    IEnumerator FirstExplosion()
    {
        switch (csMainManager.instance.fireContents)
        {
            case FireContents.LIVINGROOM:
                yield return new WaitForSeconds(4.0f);

                csSoundManager.instance.PlayExplosionSound();
                explosionObj.SetActive(true);
                
                yield return new WaitForSeconds(1.5f);
                
                explosionObj.SetActive(false);
                tinyFlame.SetActive(false);

                yield return new WaitForSeconds(0.8f);

                b_Burn = true;
                mainLignt.color = new Color32(255, 125, 125, 255);
                csSerialPortManager.instance.sendDATA('A', '1');
                livingFireGroup[0].SetActive(true);

                yield return new WaitForSeconds(2.5f);

                StartCoroutine(StartNumberCnt());
                livingFireGroup[1].SetActive(true);
                //livingFireGroup3.SetActive(true);

                yield return new WaitForSeconds(2.3f);
                
                livingFireGroup[3].SetActive(true);

                break;

            case FireContents.KITCHEN:
                yield return new WaitForSeconds(2.0f);

                csSoundManager.instance.PlayExplosionSound();
                explosionObj.SetActive(true);

                yield return new WaitForSeconds(1.5f);

                explosionObj.SetActive(false);

                yield return new WaitForSeconds(0.8f);
                
                b_Burn = true;
                mainLignt.color = new Color32(255, 125, 125, 255);
                csSerialPortManager.instance.sendDATA('A', '1');
                kitchenFireGroup[0].SetActive(true);

                yield return new WaitForSeconds(2.5f);

                StartCoroutine(StartNumberCnt());
                kitchenFireGroup[1].SetActive(true);
                kitchenFireGroup[2].SetActive(true);

                yield return new WaitForSeconds(2.3f);

                kitchenFireGroup[3].SetActive(true);
                break;

            case FireContents.KINDERGARTEN:
                yield return new WaitForSeconds(1.0f);

                //csSoundManager.instance.PlayExplosionSound();
                csSoundManager.instance.PlaySparkSound();
                electronicObj.SetActive(true);

                yield return new WaitForSeconds(3.5f);

                electronicObj.SetActive(false);

                yield return new WaitForSeconds(0.8f);

                b_Burn = true;
                mainLignt.color = new Color32(255, 125, 125, 255);
                csSerialPortManager.instance.sendDATA('A', '1');
                kindergartenFireGroup[0].SetActive(true);

                yield return new WaitForSeconds(2.5f);

                StartCoroutine(StartNumberCnt());
                kindergartenFireGroup[1].SetActive(true);

                yield return new WaitForSeconds(2.3f);

                kindergartenFireGroup[2].SetActive(true);
                
                break;
        }
    }

    IEnumerator FinishFire()
    {
        yield return new WaitForSeconds(5.0f);

        SceneManager.LoadScene(0);
    }

    IEnumerator StartNumberCnt()
    {
        numberCnt.enabled = true;
        numberCnt.sprite = numberCntImg[0];
        yield return new WaitForSeconds(1.5f);
        numberCnt.sprite = numberCntImg[1];
        yield return new WaitForSeconds(1.5f);
        numberCnt.sprite = numberCntImg[2];
        yield return new WaitForSeconds(1.5f);
        numberCnt.sprite = numberCntImg[3];
        yield return new WaitForSeconds(1.5f);
        numberCnt.sprite = numberCntImg[4];
        yield return new WaitForSeconds(1.5f);
        numberCnt.enabled = false;
        startSprite.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        startSprite.SetActive(false);

        b_StartGame = true;
    }
}
