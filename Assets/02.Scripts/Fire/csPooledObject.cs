using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csPooledObject : MonoBehaviour
{
    public static csPooledObject instance;

    public GameObject poolObj_Water;
    public GameObject group_Water;
    public int poolAmount_Water;
    [HideInInspector] public List<GameObject> poolObjs_Water = new List<GameObject>();

    public Transform spawnWaterPoint;

    void Awake()
    {
        if(csPooledObject.instance == null)
        {
            csPooledObject.instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < poolAmount_Water; i++)
        {
            GameObject obj_Water = (GameObject)Instantiate(poolObj_Water, spawnWaterPoint.position, Quaternion.identity);

            obj_Water.name = "Water";
            obj_Water.transform.parent = group_Water.transform;

            obj_Water.SetActive(false);
            poolObjs_Water.Add(obj_Water);
        }
    }

    public GameObject GetPooledObject_Water(Transform posi)
    {
        for (int i = 0; i < poolObjs_Water.Count; i++)
        {
            if (!poolObjs_Water[i].activeInHierarchy)
            {
                poolObjs_Water[i].transform.SetPositionAndRotation(posi.position, Quaternion.identity);

                return poolObjs_Water[i];
            }
        }

        return null;
    }

    //public GameObject GetPooledObject_Water()
    //{
    //    for (int i = 0; i < poolObjs_Water.Count; i++)
    //    {
    //        if (!poolObjs_Water[i].activeInHierarchy)
    //        {
    //            poolObjs_Water[i].transform.SetPositionAndRotation(csMouse.spawnPosition, csMouse.tr.rotation);

    //            return poolObjs_Water[i];
    //        }
    //        else
    //        {
    //            for(int j = 0; j < 10; j++)
    //            {
    //                GameObject obj_Water = (GameObject)Instantiate(poolObj_Water, csMouse.spawnPosition, csMouse.tr.rotation);
    //                obj_Water.name = "Water";

    //                obj_Water.transform.parent = group_Water.transform;

    //                obj_Water.SetActive(false);
    //                poolObjs_Water.Add(obj_Water);
    //            }
    //        }
    //    }

    //    return null;
    //}
}
