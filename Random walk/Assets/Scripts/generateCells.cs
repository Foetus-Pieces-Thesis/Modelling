using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateCells : MonoBehaviour
{
<<<<<<< HEAD
    [Range(1, 10000)]
    public int NumberOfCells = 100;
    [Range(1, 50)]
    public int maxInitDist = 10;
    public GameObject cell;
    
    
=======
    public int NumberOfCells = 100;
    public int maxInitDist = 10;
    public GameObject cell;
    
>>>>>>> 9a661de443f1963005796c3788be5320a0ba10bf
    void Start()
    {
        InitializeRandom();
    }

    void InitializeRandom()
    {
        for (int i= 0; i < NumberOfCells; i++)
        {
            int randNum = Random.Range(-maxInitDist, maxInitDist+1);
<<<<<<< HEAD
            Vector3 InitPos = Random.insideUnitSphere * randNum;
            GameObject e = Instantiate(cell, InitPos, Random.rotation) as GameObject;
=======
            GameObject e = Instantiate(cell, Random.insideUnitSphere * randNum, Random.rotation) as GameObject;
            //e.transform.position = 
>>>>>>> 9a661de443f1963005796c3788be5320a0ba10bf
        }
    }

}
