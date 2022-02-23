using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class wander : MonoBehaviour{

<<<<<<< HEAD
    [Range(0,100)]
    public float maxSpeed = 5f;
    [Range(0,100)]
    public float steerStrength = 0.2f;
    [Range(0,10)]
    public float wanderStrength = 1.0f;
    [Range(1,10)]
    public float viewRadius = 1.5f;
    [Range(0,1)]
    public float cohesion = 0f;
    
    public GameObject daughter;
    public LayerMask cellMask;
    public LayerMask obstacleMask;

    private Rigidbody rbody;
    private float pathLength;
    
    private Vector3 InitPos;
    private Vector3 nextPosition;
    private Vector3 velocity;
    private Vector3 desiredDirection;
=======

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
>>>>>>> 9a661de443f1963005796c3788be5320a0ba10bf

    void Start()
    {
        //StartCoroutine("DoLocalEvolutionWithDelay", 10f);
        rbody = this.GetComponent<Rigidbody>();
<<<<<<< HEAD
        pathLength = 0f;
        InitPos = rbody.position;
        nextPosition = InitPos;
        desiredDirection = Vector3.zero;
        velocity = Vector3.zero;

        //Debug.Log(InitPos);
    }

    /*
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
    */


/*  private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, viewRadius*21f);
    }*/

    private void Update()
    {
        // 0-Shell
        Collider[] cellsInView = Physics.OverlapSphere(transform.position, viewRadius, cellMask);
        float cellCount = cellsInView.Length;
        /*Debug.Log("shell0: " + cellCount);*/

        Vector3 resultantForce = Vector3.zero;
/*
        if (cellCount != 0)
        {
            foreach (var cell in cellsInView)
            {

                desiredDirection = (cell.transform.position - rbody.position).normalized;//direction towards cell
                Debug.Log("cell.transform.position0: " + cell.transform.position);
                Debug.Log("rbody.position0: " + rbody.position);
                Debug.Log("desiredDirection0: " + desiredDirection);

                Vector3 desiredVelocity = desiredDirection * (maxSpeed);//RandomGaussian(cohesion - 0.5f, cohesion + 0.5f);             
                Debug.Log("desiredVelocity0: " + desiredVelocity);

                float distanceToCell = Vector3.Distance(rbody.position, cell.transform.position);
                Debug.Log("distanceToCell0: " + distanceToCell);
                Debug.Log("cohesion: " + cohesion);
                Vector3 desiredSteeringForce = (distanceToCell < 1f) ? (-0.05f * (desiredVelocity - velocity) * cohesion) : ((desiredVelocity - velocity) * steerStrength * cohesion / (distanceToCell * distanceToCell));
                Debug.Log("desiredSteeringForce0: " + desiredSteeringForce);

                resultantForce += desiredSteeringForce;

            }

            Vector3 acceleration = resultantForce;// Vector3.ClampMagnitude(resultantForce, steerStrength); //normalised mass = 1
            Debug.Log("resultantForce: " + resultantForce);

            velocity = Vector3.ClampMagnitude(velocity + (acceleration * Time.deltaTime), maxSpeed);
            Debug.Log("velocity0: " + velocity);

            nextPosition += (velocity * Time.deltaTime);// + RandomStep();
            Debug.Log("desiredPosition0: " + nextPosition);
        }

*/

        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        // +n- Fibonacci Shell
        for (int n = 1; n < 5; n++)
        {
            float f = Mathf.Round(Mathf.Pow(goldenRatio, n + 1) / Mathf.Sqrt(5));//fibonacci number
            cellsInView = Physics.OverlapSphere(rbody.position, viewRadius * f, cellMask);
            cellCount = cellsInView.Length;
            Vector3 toCenterOfCrowd = new Vector3();
            float crowdSpread = 0f;


            Debug.Log("shell" + n + ": " + cellCount);
            if (cellCount >= 2)
            {
                foreach (var cell in cellsInView)
                {
                    toCenterOfCrowd += (cell.transform.position - rbody.position) / cellCount;// equivalent to a sample mean  
                }
                Debug.Log("toCenterofCrowd" + n + ": " + toCenterOfCrowd);

                foreach (var cell in cellsInView)
                {
                    crowdSpread += ((cell.transform.position - rbody.position) - toCenterOfCrowd).sqrMagnitude / (cellCount - 1);// equivalent to a sample variance
                }
                Debug.Log("variance" + n + ": " + crowdSpread);

                float distanceToCenter = toCenterOfCrowd.magnitude;
                float virtualAuthority = (cellCount * cellCount) / (crowdSpread + cellCount);

                Vector3 desiredVelocity = toCenterOfCrowd * (maxSpeed *RandomGaussian());
                Vector3 desiredSteeringForce = (desiredVelocity - velocity) * steerStrength * cohesion * virtualAuthority / (distanceToCenter * distanceToCenter);

                resultantForce += desiredSteeringForce;
                Debug.Log("desiredSteeringForce" + n + ": " + desiredSteeringForce);
                Debug.Log("resultantForce" + n + ": " + resultantForce);
                Vector3 acceleration = Vector3.ClampMagnitude(resultantForce, steerStrength) / 1; //normalised mass = 1
                velocity = Vector3.ClampMagnitude(velocity + (acceleration * Time.deltaTime), maxSpeed);
                Debug.Log("velocity" + n + ": " + velocity);

                nextPosition += (velocity * Time.deltaTime);
            }
        }
        //HandleDirectionalMovement();

        Debug.Log("Update time: " + Time.deltaTime);
    }

    void FixedUpdate()
    {
        HandleDirectionalMovement();   
        Debug.Log("FixedUpdate time: " + Time.deltaTime);
    }

    // Returns the Linear Progression (displacement/total path length)
    public float getLinearProgression()
    {
        return Vector3.Distance(rbody.position, InitPos) / pathLength;
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


    // Handles 3D pseudo random walk
    // (Rule 0 - Baseline motion)
    private Vector3 RandomStep()
    {
        
        desiredDirection = (desiredDirection + Random.insideUnitSphere*wanderStrength).normalized;//pseudo-random nudge in direction (Order 1 Markov process)

        Vector3 desiredVelocity = desiredDirection * (maxSpeed * RandomGaussian());//speed is normally distribution
        Vector3 desiredSteeringForce = (desiredVelocity - velocity) * steerStrength;
        Vector3 acceleration = Vector3.ClampMagnitude(desiredSteeringForce, steerStrength); //normalised mass = 1

        velocity = Vector3.ClampMagnitude(velocity + (acceleration * Time.deltaTime), maxSpeed);//v = u + at

        Vector3 randomStep = (velocity * Time.deltaTime);
        //nextPosition += (velocity * Time.deltaTime);//s = vt

        //rbody.MovePosition(nextPosition);

        /*Debug.Log("nextPosition-random: " + nextPosition);*/

            // increment total path length

            //pathLength += randomStep.magnitude;

            return randomStep;
=======
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
        
>>>>>>> 9a661de443f1963005796c3788be5320a0ba10bf
    }



<<<<<<< HEAD

    // Handles directional movement 
    // (Rule 1 - Force applied by neighbouring cells = k/D^2, where D is the distance to neighbour)
    private void HandleDirectionalMovement()
    {  
        rbody.MovePosition(nextPosition + RandomStep());
        // increment total path length
        pathLength += (velocity * Time.deltaTime).magnitude;

/*
        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        // +n- Fibonacci Shell
        for (int n = 1; n < 7; n++)
        {
            float f = Mathf.Round(Mathf.Pow(goldenRatio, n + 1) / Mathf.Sqrt(5));//fibonacci number
            cellsInView = Physics.OverlapSphere(rbody.position, viewRadius * f, cellMask);
            cellCount = cellsInView.Length;
            Vector3 toCenterOfCrowd = new Vector3();
            float crowdSpread = 0f;


            Debug.Log("shell" + n + ": " + cellCount);
            if (cellCount >= 2)
            {
                foreach (var cell in cellsInView)
                {
                    toCenterOfCrowd += (cell.transform.position - rbody.position) / cellCount;// equivalent to a sample mean  
                }
                Debug.Log("toCenterofCrowd" + n + ": " + toCenterOfCrowd);

                foreach (var cell in cellsInView)
                {
                    crowdSpread += ((cell.transform.position - rbody.position) - toCenterOfCrowd).sqrMagnitude / (cellCount - 1);// equivalent to a sample variance
                }
                Debug.Log("variance" + n + ": " + crowdSpread);

                float distanceToCenter = toCenterOfCrowd.magnitude;
                float virtualAuthority = (cellCount * cellCount) / (crowdSpread + cellCount);

                Vector3 desiredVelocity = toCenterOfCrowd * (maxSpeed);
                Vector3 desiredSteeringForce = (desiredVelocity - velocity) * steerStrength * cohesion * virtualAuthority / (distanceToCenter * distanceToCenter);

                resultantForce += desiredSteeringForce;
                Debug.Log("desiredSteeringForce" + n + ": " + desiredSteeringForce);
                Debug.Log("resultantForce" + n + ": " + resultantForce);
                Vector3 acceleration = Vector3.ClampMagnitude(resultantForce, steerStrength) / 1; //normalised mass = 1
                velocity = Vector3.ClampMagnitude(velocity + (acceleration * Time.deltaTime), maxSpeed);
                Debug.Log("velocity" + n + ": " + velocity);

                nextPosition += (velocity * Time.deltaTime);// + RandomStep();

                //rbody.position += (velocity * Time.deltaTime);
                rbody.MovePosition(nextPosition);
               

                // increment total path length
                pathLength += (velocity * Time.deltaTime).magnitude;

            }

        }*/

    }

=======
>>>>>>> 9a661de443f1963005796c3788be5320a0ba10bf
}


