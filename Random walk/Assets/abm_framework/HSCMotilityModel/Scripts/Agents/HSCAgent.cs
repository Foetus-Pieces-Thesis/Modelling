/*
Boids - Flocking behavior simulation.

Copyright (C) 2014 Keijiro Takahashi

Edited by Kostas Cheliotis (2019)

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using ABMU.Core;

public class HSCAgent : AbstractAgent
{

    public HSCController hsc_controller;

    Bounds bounds;
    List<Collider> cellsInView;
    Vector3 desiredDirection = Vector3.zero;
    Vector3 velocity = Vector3.zero;

    private Rigidbody rbody;
    public float distCovered = 0f;
    public int id;
    float norm = 0.01f;

    public void Init(Bounds _bounds, Vector3 _pos, Quaternion _rot, int _id)
    {
        base.Init();
        hsc_controller = GameObject.FindObjectOfType<HSCController>();
        rbody = this.GetComponent<Rigidbody>();

        bounds = _bounds;
        id = _id;

        this.transform.position = _pos;
        this.transform.rotation = _rot;

        //CreateStepper(HSCBehaviourFindNeighbours, 1, Stepper.StepperQueueOrder.EARLY);
        //CreateStepper(HSCBehaviourRandomWalk, 1, Stepper.StepperQueueOrder.NORMAL);
        CreateStepper(HSCBehaviourCluster, 1, Stepper.StepperQueueOrder.NORMAL);
        CreateStepper(CheckOutOfBounds, 1, Stepper.StepperQueueOrder.LATE);
    }

    List<Collider> HSCBehaviourFindNeighbours(int n)
    {
        // Finds HSCs in a neighbourhood with an upper limit of viewRadius(n) and a lower limit of viewRadius(n-1)

   
        Collider[] temp_shell = Physics.OverlapSphere(this.transform.position, viewRadius(n), hsc_controller.searchLayer); 
        
        List<Collider> shell_n = new List<Collider>();
        /* foreach (var cell in temp_shell)
         {
             shell_n.Add(cell);
         }*/


        if (n != 0)
        {
            int j = 0;
            foreach (var cell in temp_shell)
            {
                float dist2cell = Vector3.Distance(cell.transform.position, this.transform.position);
                if (dist2cell > viewRadius(n - 1))
                {
                    shell_n.Add(cell);
                    j++;
                }
            }
        }
        else
        {
            foreach (var cell in temp_shell)
            {
                shell_n.Add(cell);
            }

        }


        //Display lines showing network relationship between cells
        if (shell_n.Count != 0 && hsc_controller.showNeighbourRelations)
        {

            Color[] colors = { Color.cyan, Color.blue, Color.green, Color.yellow, Color.red, Color.white, Color.black };
            
            //LineDrawer line = new LineDrawer()
            foreach(var cell in shell_n)
            {
                Vector3 p = cell.transform.position;
                
                //line.DrawLineInGameView(this.transform.position, p, colors[n]);
                Debug.DrawLine(this.transform.position, p, colors[n]);
                
            } 

        }

        return shell_n;
        
    }
        

    Vector3 RandomStep()
    {

        desiredDirection = (desiredDirection + Random.insideUnitSphere * hsc_controller.wanderStrength).normalized;//pseudo-random nudge of direction (Order 1 Markov process)

        Vector3 desiredVelocity = desiredDirection * RandomGaussian(0f, hsc_controller.maxSpeed);//speed is normally distribution

        Vector3 desiredSteeringForce = (desiredVelocity - velocity) * hsc_controller.steerStrength;

        Vector3 acceleration = desiredSteeringForce / hsc_controller.mass;
        acceleration = Vector3.ClampMagnitude(acceleration, hsc_controller.steerStrength);

        velocity = Vector3.ClampMagnitude(velocity + (acceleration * norm), hsc_controller.maxSpeed);//v = u + at

        Vector3 randomStep = (velocity * norm);

        return randomStep;

    }

    void MoveFromForce(Vector3 Force)
    {
        Vector3 acceleration = Force / hsc_controller.mass;
        velocity = Vector3.ClampMagnitude(velocity + (acceleration * norm), hsc_controller.maxSpeed);

        Vector3 prevPos = this.transform.position;
        transform.position = prevPos + (velocity * norm) + RandomStep();

        distCovered += Vector3.Distance(transform.position, prevPos);

        hsc_controller.AgentReportDistCovered(distCovered);
        hsc_controller.AgentReportState(id, transform.position, velocity);
    }


    // considers attractive force to a cell that is distance d away, within a neighbourhood of radius r - defined by n.
    float forceFunction(float d, int n=0)
    {
        float s = viewRadius(n) * hsc_controller.separationFactor;// s = equilibrium distance
        float K = hsc_controller.clusterStiffness;// * s * s;// The stiffness or the energy of cluster system

        if (n == 0) 
        {

            if (hsc_controller.laplacianSpringSystem)
            {
                //return K * ((d * d) / (Mathf.Pow(s, 4)) - Mathf.Pow(s, -2)) * Mathf.Exp(-d * d / (2 * s * s)); //K * (d - s) * (r - d);
                return K * ((d * d) / (Mathf.Pow(s, 4)) - Mathf.Pow(s,-2)) * Mathf.Exp(-d * d / (2 * s * s)); //NLoG
            }
            else
            {
                float r = viewRadius(n);
                K *= 4f / ((r - s) * (r - s)); // K = max attractive force
                return K * (d - s) * (r - d);
            }

        }
        else
        {
            K *= Mathf.Exp(-n * hsc_controller.localBiasFactor);

            float center = (viewRadius(n) + viewRadius(n - 1)) / 2;
            float y = (d - center);

            return K* y/(s*s) * Mathf.Exp(-y*y/(2*s*s));
        }
    }


    /*float _3DGaussianMask(Vector3 globalCoords, float r,  float sigma)
    {
        Vector3 localCoords = (globalCoords - this.transform.position) / r;

        //float Gnorm = Mathf.Pow( Mathf.PI * Mathf.Sqrt(2f)/Mathf.Pow(sigma, 2), 1.5f);
        
        float Gauss =  Mathf.Exp(-1f * (localCoords.sqrMagnitude) / 2 * Mathf.Pow(sigma,2));

        return Gauss;
    }*/


    void HSCBehaviourCluster()
    {
        Vector3 resultantForce = Vector3.zero;
        Vector3 F_drag = hsc_controller.viscousity * velocity;

        for (int n = 0; n < hsc_controller.shell.Length;n++)
        {
            List<Collider> cellsInView = HSCBehaviourFindNeighbours(n);

            int cellCount = cellsInView.Count;
            float r = viewRadius(n);

            foreach (var cell in cellsInView)
            {

                float dist2cell = Vector3.Distance(cell.transform.position, this.transform.position);
                Vector3 dir2cell = (cell.transform.position - this.transform.position).normalized;


                
                resultantForce += dir2cell * forceFunction(dist2cell, n);// * Mathf.Log(cellCount + 1);

            }

        }

        MoveFromForce(resultantForce-F_drag);
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


    // Returns view radius of a given shell n (informed by seedViewRadius)
    float viewRadius(int n)
    {
        if (n >= 0)
        {
            return hsc_controller.seedViewRadius * hsc_controller.shell[n];
        }
        else
        {
            return 0;
        }
    }


    void CheckOutOfBounds()
    {
        if (hsc_controller.applyBounds)
        {
            Vector3 pos = this.transform.position;
            if (!bounds.Contains(this.transform.position))
            {
                pos.x = Mathf.Clamp(pos.x, bounds.center.x - bounds.extents.x, bounds.center.x + bounds.extents.x);
                pos.y = Mathf.Clamp(pos.y, bounds.center.y - bounds.extents.y, bounds.center.y + bounds.extents.y);
                pos.z = Mathf.Clamp(pos.z, bounds.center.z - bounds.extents.z, bounds.center.z + bounds.extents.z);
                transform.position = pos;

                /*Vector3 fromCenterToHere = this.transform.position - bounds.center;
                this.transform.position = bounds.center - fromCenterToHere;
                // transform.position = this.transform.position + transform.forward * (velocity * Time.deltaTime);
                transform.position = this.transform.position + (velocity * norm);*/
            }

        }
        
    }
}
