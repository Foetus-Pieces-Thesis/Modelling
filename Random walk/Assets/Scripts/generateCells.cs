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


    void Start()
    {
        InitializeRandom();
    }

    void InitializeRandom()
    {
        //Vector3 offset = new Vector3(-10, 0, 10);
        for (int i = 0; i < NumberOfCells; i++)
        {
            int randNum = Random.Range(-maxInitDist, maxInitDist + 1);
            Vector3 InitPos = (Random.insideUnitSphere)* randNum;
            GameObject e = Instantiate(cell, InitPos, Random.rotation) as GameObject;
        }

        
      /*  for (int i = 0; i < NumberOfCells / 2; i++)
        {
            int randNum = Random.Range(-maxInitDist, maxInitDist + 1);
            Vector3 InitPos = (Random.insideUnitSphere) * randNum;
            GameObject e = Instantiate(cell, InitPos, Random.rotation) as GameObject;
        }
*/

    }

}

