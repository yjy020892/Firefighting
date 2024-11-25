using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csFireWaterEffect : MonoBehaviour
{
    private float timer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 2.0f)
        {
            timer = 0.0f;

            csPooledObject.instance.poolObjs_Water.Remove(this.gameObject);
            csPooledObject.instance.poolObjs_Water.Add(this.gameObject);

            gameObject.transform.SetAsLastSibling();
            gameObject.SetActive(false);
        }

    }
}
