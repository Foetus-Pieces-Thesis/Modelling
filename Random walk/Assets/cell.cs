using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cell : MonoBehaviour
{
    public GameObject daughter;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit detected");
        GameObject e = Instantiate(daughter) as GameObject;
        e.transform.position = transform.position;
    }

    //rules of game of life - preliminary 
}
