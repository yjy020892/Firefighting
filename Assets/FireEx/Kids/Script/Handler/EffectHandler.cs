using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    public float destroyTime = 0.5f;

    private void OnEnable()
    {
        StartCoroutine(DestroyTimer(destroyTime));
    }

    private IEnumerator DestroyTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        DestroyImmediate(this.gameObject);
    }
}
