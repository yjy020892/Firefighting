using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public List<GameObject> prefabList = new List<GameObject>();
    public List<GameObject> startPointList_Elephant = new List<GameObject>();
    public List<GameObject> startPointList_Rhino = new List<GameObject>();
    public Transform container;
    public Coroutine updateCoroutine;

    private void Start()
    {
        updateCoroutine = StartCoroutine(UpdateCoroutine());
    }



    public IEnumerator UpdateCoroutine()
    {
        while (true)
        {

            if (Random.Range(0, prefabList.Count) == 0)
                CreateItem(prefabList[0], startPointList_Elephant[Random.Range(0, startPointList_Elephant.Count)].transform.position);
            else
                CreateItem(prefabList[1], startPointList_Rhino[Random.Range(0, startPointList_Rhino.Count)].transform.position);

            yield return new WaitForSecondsRealtime(1);
        }
    }

    public void CreateItem(GameObject item, Vector3 startPoint)
    {
        GameObject obj = Instantiate(item);
        obj.transform.position = startPoint;
        obj.transform.SetParent(container);
    }
}
