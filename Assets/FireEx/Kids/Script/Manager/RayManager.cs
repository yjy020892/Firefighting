using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayManager : MonoBehaviour
{

    public Camera camera;

    public Vector3 pos = new Vector3(0, 0, 0);
    public float x = 0;
    public float y = 0;


    private void Awake()
    {
        
    }

    private void Update()
    {
        if (camera == null)
            camera = GameObject.Find("MainCamera").GetComponent<Camera>();

        if (Input.GetMouseButtonUp(0))
        {
            BoxCast(Input.mousePosition, new Vector2(x, y));
        }
    }

    public void BoxCast(Vector2 pos, Vector2 size)
    {
        //전달 받은 데이터에 맞게 BoxCast를 쏴서 히트되는 오브젝트 검출
        float maxDistance = 100;
        RaycastHit hit;
        if (Physics.BoxCast(camera.ScreenPointToRay(pos).origin, new Vector3(size.x/10f, size.y/10f, 1), camera.ScreenPointToRay(pos).direction, out hit, transform.rotation, maxDistance))
        {
            if (hit.collider.gameObject.tag == "Animal")
                hit.collider.gameObject.GetComponent<MovingAnimalHandler>().HitEvent();
            else if (hit.collider.gameObject.tag == "SpaceShip")
                hit.collider.gameObject.GetComponent<MovingSpaceShipHandler>().HitEvent();
        }


    }


}
