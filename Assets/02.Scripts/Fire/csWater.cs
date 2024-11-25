using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csWater : MonoBehaviour
{
    public delegate void WaterDestroyHandler();
    public static event WaterDestroyHandler AttackFire;

    Transform tr;

    public float waterSpeed;
    Vector3 saveFirePosition;

    float timer = 0.0f;

    void OnEnable()
    {
        saveFirePosition = csMouse.hitPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        tr = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = saveFirePosition - tr.position;

        Quaternion rot = Quaternion.LookRotation(direction);

        tr.rotation = Quaternion.Slerp(tr.rotation, rot, 10 * Time.deltaTime);

        tr.Translate(Vector3.forward * Time.deltaTime * waterSpeed);

        timer += Time.deltaTime;

        if(timer > 2.0f)
        {
            AttackFire();

            timer = 0.0f;
            tr.position = csMouse.spawnPosition;

            gameObject.SetActive(false);
        }
    }

    //void OnTriggerEnter(Collider col)
    //{
    //    if(col.gameObject.transform.tag == "Fire")
    //    {
    //        tr.position = csMouse.spawnPosition;

    //        gameObject.SetActive(false);
    //    }
    //}
}
