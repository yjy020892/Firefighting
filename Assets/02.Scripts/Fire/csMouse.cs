using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csMouse : MonoBehaviour
{
    private GameObject target;

    Animator Anim;

    public static Transform tr;
    public static Vector3 hitPosition;
    public static Vector3 spawnPosition;
    public GameObject csFireOBJ;
    csFire csFireScript;

    float animTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        csWater.AttackFire += MinusFireHP;
        Anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        tr = gameObject.transform;
        spawnPosition = tr.position;

        if (Input.GetMouseButtonDown(0))
        {
            GetClickedObject();
        }
        
        if(!csFireManager.b_SmogEnable)
        {
            if (animTimer < 2.0f)
            {
                animTimer += Time.deltaTime;
            }

            if (animTimer >= 2.0f)
            {
                Anim.SetBool("Move", true);
            }
        }
    }

    private void OnMouseOver()
    {
        
    }

    private void GetClickedObject()
    {
        RaycastHit hit;
        //GameObject target = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            if (hit.transform.gameObject.tag == "Fire")
            {
                //Debug.Log("hit");
                hitPosition = hit.transform.position;
                
                //GameObject obj = csPooledObject.instance.GetPooledObject_Water();
                //obj.SetActive(true);

                csSoundManager.instance.PlayfireExtinguisherSound();

                csFireScript = hit.transform.gameObject.GetComponent<csFire>();
                //if (csFireScript.fireHP > 0)
                //{
                //    csFireScript.fireHP -= 1;

                //    csFireScript.main.startLifetimeMultiplier = csFireScript.fireHP;
                //    //csFireScript.main.startSizeXMultiplier = csFireScript.fireStartSizeVar - 1;
                //}
                //if (csFireScript.fireHP > 0.0f)
                //{
                //    csFireScript.fireHP -= 0.2f;

                //    if(csFireScript.fireHP == 0.6f)
                //    {
                //        csFireScript.fireTransForm.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                //        //csFireScript.main.startLifetimeMultiplier = 3;
                //        //csFireScript.main.startSizeXMultiplier = csFireScript.main.startSizeXMultiplier - csFireScript.fireStartSizeVar;
                //    }
                //    else if(csFireScript.fireHP == 0.4f)
                //    {
                //        csFireScript.fireTransForm.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                //        //csFireScript.main.startLifetimeMultiplier = 2;
                //        //csFireScript.main.startSizeXMultiplier = csFireScript.main.startSizeXMultiplier - csFireScript.fireStartSizeVar;
                //    }
                //    else if (csFireScript.fireHP == 0.2f)
                //    {
                //        csFireScript.fireTransForm.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                //        //csFireScript.main.startLifetimeMultiplier = 1;
                //        //csFireScript.main.startSizeXMultiplier = csFireScript.main.startSizeXMultiplier - csFireScript.fireStartSizeVar;
                //    }

                    

                //    //csFireScript.main.startLifetimeMultiplier = csFireScript.fireHP;
                //    //csFireScript.main.startSizeXMultiplier = csFireScript.fireStartSizeVar - 1;
                //}
            }
        }

        //return target;
    }

    void MinusFireHP()
    {

    }
}
