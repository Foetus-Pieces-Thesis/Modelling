using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cluster : MonoBehaviour
{
    //TODO:
    //analyse the clustering algorithm - do the higher order shells make sense?
    //study the c# job system - is multi-threading useful for our purposes?
    //find out what is neccessary to implement system wide stats-analysis of a cell's motility

    [Range(1, 100)]
    public float viewRadius = 5f;// the radius of the primary sphere of interaction
    [Range(0, 1)]
    public float separationFactor = 0.5f;// Normalised separation between neighbouring cells
    [Range(0,10)]
    public float clusterStiffness = 1f;// the constant k in spring equation F = kx (strength of neighbour attraction)

    public int[] resonantShells = { 1 };

    [Range(0, 10)]
    public float wanderStrength = 2f;

    [Range(0,10)]
    public float randStrength = 5f;

    //public GameObject daughter;
    public LayerMask cellMask;
    public LayerMask obstacleMask;

    private Rigidbody rbody;
    //private float pathLength;

    private Vector3 InitPos;
    private Vector3 nextPosition;
    private Vector3 velocity;
    private Vector3 desiredDirection;
    private Vector3 randomForce;

    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody>();
        //pathLength = 0f;
        InitPos = rbody.position;
        nextPosition = InitPos;
        desiredDirection = Vector3.zero;
        randomForce = Vector3.zero;
        velocity = Vector3.zero;
    }

    public void cohere(int n)//spring ball model - where F = kx (consider other models such as F=sinx or F=arctanx: F=force,x=extension)  
    {
        Collider[] cellsInView;
        Vector3 resultantForce = Vector3.zero;
        float separation;
        float x;

        switch (n)
        {
            case 1:
                // code block
                cellsInView = Physics.OverlapSphere(transform.position, viewRadius * 1f, cellMask);

                separation = (viewRadius * 1f) * separationFactor;// equilibrium distance between neighbouring cells

                foreach (var cell in cellsInView)
                {
                    x = Vector3.Distance(cell.transform.position, this.transform.position) - separation;//deviation from equilibrium distance;
                    Vector3 dir2cell = (cell.transform.position - this.transform.position).normalized;
                    resultantForce += dir2cell * clusterStiffness * x;
                }

                rbody.AddForce(resultantForce + RandomForce(), ForceMode.Force);
                Debug.Log("resultantForce: " + resultantForce);

                break;

            default:
                // code block
                cellsInView = Physics.OverlapSphere(transform.position, viewRadius * n, cellMask);
                int cellCount = cellsInView.Length;

                separation = (viewRadius * n) * separationFactor;// equilibrium distance between neighbouring cells
                Vector3 clusterCenter = Vector3.zero;

                foreach (var cell in cellsInView)
                {
                    clusterCenter += cell.transform.position / cellCount;
                }

                x = Vector3.Distance(clusterCenter, this.transform.position) - separation;//deviation from equilibrium distance;
                Vector3 dir2cluster = (clusterCenter - this.transform.position).normalized;
                resultantForce = dir2cluster * clusterStiffness*Mathf.Exp(-n)*cellCount*x;

                rbody.AddForce(resultantForce + RandomForce(), ForceMode.Force);
                Debug.Log("resultantForce: " + resultantForce);

                break;
        }

    }

    private Vector3 RandomForce()
    {
        Vector3 RandomInsideUnitCube = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));//new Vector3(RandomGaussian(-1f, 1f), RandomGaussian(-1f, 1f), RandomGaussian(-1f, 1f));
        randomForce = (randomForce + (RandomInsideUnitCube) * wanderStrength).normalized;
        randomForce *= randStrength * RandomGaussian();

        return randomForce;
    }


    // Returns a random Gaussian number in range [minValue maxValue] (default = [0,1])
    public static float RandomGaussian(float minValue = 0.0f, float maxValue = 1.0f)
    {
        float u, v, S;

        do
        {
            u = 2.0f * Random.value - 1.0f;
            v = 2.0f * Random.value - 1.0f;
            S = u * u + v * v;
        }
        while (S >= 1.0f);

        // Standard Normal Distribution
        float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);

        // Normal Distribution centered between the min and max value
        // and clamped following the "three-sigma rule"
        float mean = (minValue + maxValue) / 2.0f;
        float sigma = (maxValue - mean) / 3.0f;
        return Mathf.Clamp(std * sigma + mean, minValue, maxValue);
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i <resonantShells.Length; i++)//TODO: find a method to run cohere shells in parallel 
        {
            cohere(resonantShells[i]);
        }
        
    }
/*
    private void OnDrawGizmos()
    {
        for(int i = 0; i < resonantShells.Length; i++)
        {
            Gizmos.DrawWireSphere(this.transform.position, viewRadius*resonantShells[i]);
        }
    }
*/


}
