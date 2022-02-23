using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateCells : MonoBehaviour
{
    public int NumberOfCells = 100;
    public int maxInitDist = 10;
    public GameObject cell;
    
    void Start()
    {
        InitializeRandom();
    }

    void InitializeRandom()
    {
        for (int i= 0; i < NumberOfCells; i++)
        {
            int randNum = Random.Range(-maxInitDist, maxInitDist+1);
            GameObject e = Instantiate(cell, Random.insideUnitSphere * randNum, Random.rotation) as GameObject;
            //e.transform.position = 
        }
    }

}
