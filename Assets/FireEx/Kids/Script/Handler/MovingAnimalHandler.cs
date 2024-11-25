using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAnimalHandler : MonoBehaviour
{
    public bool isMoving = false;
    public float speedRangeMin = 1;
    public float speedRangeMax = 10;
    public float speed = 1; //1~10

    public float deletePointX = 0;
    public float deletePointY = 0;

    public Vector3 startVecter = new Vector3();
    public GameObject effect;
    public Transform effectContainer;  


    private void OnEnable()
    {
        isMoving = true;
        speed = Random.Range(speedRangeMin, speedRangeMax);
    }


    // Update is called once per frame
    void Update()
    {
        if (isMoving)
            transform.position += (Vector3.left *  (speed/10f));

        if (transform.position.x < -100)
            HitEvent();
    }








    public void HitEvent()
    {
        isMoving = false;
        GameObject obj = Instantiate(effect);
        obj.transform.SetParent(effectContainer);
        obj.transform.position = gameObject.transform.position;

        DestroyImmediate(gameObject);
    }
}
