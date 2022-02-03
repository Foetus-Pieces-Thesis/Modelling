using UnityEngine;

public class wander : MonoBehaviour{


    //Rigidbody rigidbody;

    [Range(0,100)]
    public float maxSpeed = 20;
    [Range(0,10)]
    public float steerStrength = 0.2f;
    [Range(0,10)]
    public float wanderStrength = 0.1f;
    [Range(1,10)]
    public float overlapRadius = 1.5f;
    public GameObject daughter;

    Vector3 position;
    Vector3 velocity;
    Vector3 desiredDirection;

   /* void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

    }*/

/*
    Collider[] cellsInNeighbourhood;
    private void FixedUpdate()
    {
        cellsInNeighbourhood = null;
        cellsInNeighbourhood = Physics.OverlapSphere(this.transform.position, overlapRadius); // all cells inside local agent's neighbourhood
        int noCellsAlive = 0;// cellsInNeighbourhood.Length;

        foreach (var cell in cellsInNeighbourhood) { noCellsAlive += cell.GetComponent<MeshRenderer>().enabled ? 1 : 0; }

        // Conway's Game of life Rules: 2333/5766
        if (this.GetComponent<MeshRenderer>().enabled)
        {
            if (noCellsAlive < 2)// underpopulation
            {
                this.GetComponent<MeshRenderer>().enabled = false;
                //Destroy(this);
            }
            else if (2 <= noCellsAlive || noCellsAlive <= 3)// lives on
            {
                //nothing
            }
            else if (noCellsAlive > 3) // overpopulation
            {
                this.GetComponent<MeshRenderer>().enabled = false;
                //Destroy(this);
            }

        }
        else if (noCellsAlive == 3)//reproductive zone
        {
            this.GetComponent<MeshRenderer>().enabled = true;
            GameObject e = Instantiate(daughter) as GameObject;
            e.transform.position = transform.position;
        }

    }
*/

   /* private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, overlapRadius);
    }
*/

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        desiredDirection = (desiredDirection + Random.insideUnitSphere * wanderStrength).normalized;// find distribution of random function

        Vector3 desiredVelocity = desiredDirection * maxSpeed;
        Vector3 desiredSteeringForce = (desiredVelocity - velocity) * steerStrength;
        Vector3 acceleration = Vector3.ClampMagnitude(desiredSteeringForce, steerStrength) / 1; //normalised mass = 1

        velocity = Vector3.ClampMagnitude(velocity + (acceleration * Time.deltaTime), maxSpeed);
        position += (velocity * Time.deltaTime);

        float theta = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        float phi = Mathf.Atan2(velocity.z, velocity.x) * Mathf.Rad2Deg;

        transform.SetPositionAndRotation(position, Quaternion.Euler(0, phi, theta));

    }



}


