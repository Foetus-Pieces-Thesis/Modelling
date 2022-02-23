using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class wander : MonoBehaviour{


    [Range(0,100)]
    public float maxSpeed = 20;
    [Range(0,100)]
    public float steerStrength = 0.2f;
    [Range(0,10)]
    public float wanderStrength = 0.1f;
    [Range(1,10)]
    public float viewRadius = 1.5f;
    [Range(0, 1)]
    public float cohesion = 1;
    public GameObject daughter;
    public LayerMask cellMask;
    public LayerMask obstacleMask;
    private Rigidbody rbody;

    Vector3 position;
    Vector3 velocity;
    Vector3 desiredDirection;

    void Start()
    {
        //StartCoroutine("DoLocalEvolutionWithDelay", 10f);
        rbody = this.GetComponent<Rigidbody>();
        rbody.position = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2));
    }


    IEnumerator DoLocalEvolutionWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            DoLocalEvolution();
        }
    }

    void DoLocalEvolution()
    {
        Collider[] cellsInNeighbourhood = Physics.OverlapSphere(this.transform.position, viewRadius, cellMask); // all cells inside local agent's neighbourhood
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
        else if (noCellsAlive >= 2)//reproductive zone
        {
            //this.GetComponent<MeshRenderer>().enabled = true;
            GameObject e = Instantiate(daughter) as GameObject;
            e.transform.position = transform.position + Random.onUnitSphere;
            //Destroy(this.gameObject);
        }
    }

    void FixedUpdate()// move towards direction of closest neighbour
    {
        if (this.GetComponent<MeshRenderer>().enabled)
        {
            float disToClosestNeighbour = 0;
            Vector3 dirToClosestNeighbour = new Vector3();

            Collider[] neighboursInView = Physics.OverlapSphere(transform.position, viewRadius, cellMask);

            foreach (var cell in neighboursInView)
            {
                if (Vector3.Distance(transform.position, cell.transform.position) < disToClosestNeighbour)
                {
                    disToClosestNeighbour = Vector3.Distance(transform.position, cell.transform.position);
                    dirToClosestNeighbour = (cell.transform.position - transform.position).normalized;
                }

            }

            // Dampen maxSpeed and steerStrength so motion towards neighbour doesn't halt general random motion
            Vector3 desiredVelocity = dirToClosestNeighbour * maxSpeed * cohesion;
            Vector3 desiredSteeringForce = (desiredVelocity - velocity) * steerStrength * cohesion;
            Vector3 acceleration = Vector3.ClampMagnitude(desiredSteeringForce, steerStrength) / 1; //normalised mass = 1

            velocity = Vector3.ClampMagnitude(velocity + (acceleration * Time.deltaTime), maxSpeed);

            rbody.position += (velocity * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, viewRadius);
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (this.GetComponent<MeshRenderer>().enabled) 
        {
            desiredDirection = (desiredDirection + Random.insideUnitSphere * wanderStrength).normalized;// find distribution of random function

            Vector3 desiredVelocity = desiredDirection * maxSpeed;
            Vector3 desiredSteeringForce = (desiredVelocity - velocity) * steerStrength;
            Vector3 acceleration = Vector3.ClampMagnitude(desiredSteeringForce, steerStrength) / 1; //normalised mass = 1

            velocity = Vector3.ClampMagnitude(velocity + (acceleration * Time.deltaTime), maxSpeed);
            position += (velocity * Time.deltaTime);

            float theta = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            float phi = Mathf.Atan2(velocity.z, velocity.x) * Mathf.Rad2Deg;

            //transform.SetPositionAndRotation(position, Quaternion.Euler(0, phi, theta));
            rbody.MovePosition(position);
            rbody.MoveRotation(Quaternion.Euler(0, phi, theta));
            
        }
        
    }



}


