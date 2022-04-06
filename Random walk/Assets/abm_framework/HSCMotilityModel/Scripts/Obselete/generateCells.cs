using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateCells : MonoBehaviour
{
    [Range(1, 10000)]
    public int NumberOfCells = 100;
    [Range(1, 100)]
    public int maxInitDist = 10;
    public GameObject cell;
    public List<GameObject> cellList = new List<GameObject>();

    void Start()
    {
        InitializeRandom();
    }

    void Update()
    {
        Debug.Log("position: "+ cellList[0].transform.position);
    }


    void InitializeRandom()
    {
        Vector3 offset = new Vector3(2, 0, 0);
        for (int i = 0; i < NumberOfCells/2; i++)
        {
            //int randNum = Random.Range(-maxInitDist, maxInitDist + 1);
            Vector3 InitPos1 = (-offset+Random.insideUnitSphere)* maxInitDist;
            GameObject e1 = Instantiate(cell, InitPos1, Random.rotation) as GameObject;
            cellList.Add(e1);
        }

        for (int i = 0; i < NumberOfCells/2; i++)
        {
            //int randNum = Random.Range(-maxInitDist, maxInitDist + 1);
            Vector3 InitPos2 = (offset + Random.insideUnitSphere) * maxInitDist;
            GameObject e2 = Instantiate(cell, InitPos2, Random.rotation) as GameObject;
            cellList.Add(e2);
        }

    }

}

