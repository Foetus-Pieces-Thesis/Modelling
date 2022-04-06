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
using ABMU;

public class HSCController : AbstractController
{
    [Header("Simulation Parameters")]
    public bool showNeighbourRelations = false;
    public bool applyBounds = false;
    public Bounds bounds;
    [Range(1,1000)]
    public int spawnCount = 100;
    public int frameCount = 0;
    public int renderFrameCount = 0;
    public float avgDistCovered = 0f;

    [Header("Agent Parameters")]
    public GameObject hscPrefab;
    [Range(0.001f, 10f)]
    public float mass = 1f;

    [Header("Random Walk")]
    [Range(0f, 100f)]
    public float maxSpeed = 15f;
    [Range(0, 10)]
    public float wanderStrength = 2f;
    [Range(0f, 100f)]
    public float steerStrength = 10f;

    [Header("Clustering")]
    [Range(0f, 50f)]
    public float clusterStiffness = 1.5f;
    [Range(0f, 1f)]
    public float separationFactor = 0.1f;
    

    [Header("View Shell(s)")]
    [Range(0.1f, 50.0f)] 
    public float seedViewRadius = 10f;
    public float[] shell; //= new float[5] { 1, 0, 0, 0, 0, 0 };
    [Range(0.0f,10f)]
    public float localBiasFactor = 1f;
    public LayerMask searchLayer;

    public class cell
    {

        public List<Vector3> position = new List<Vector3>();
        public List<Vector3> velocity = new List<Vector3>();

        public cell(Vector3 initPos, Vector3 initVelocity)
        {
            position.Add(initPos);
            velocity.Add(initVelocity);
        }

        public void addState(Vector3 newPos, Vector3 newVelocity)
        {
            position.Add(newPos);
            velocity.Add(newVelocity);
        }

    }

    public Dictionary<int, cell> StemCells = new Dictionary<int, cell>();


    // intialise cell positions
    public override void Init()
    {
        base.Init();

        frameCount = 0;

        for (var i = 0; i < spawnCount; i++) {
            Vector3 pos = Utilities.RandomPointInBounds(bounds);
            GameObject hsc = Spawn();
            hsc.GetComponent<HSCAgent>().Init(bounds, pos, Random.rotation,i);

            StemCells.Add(i, new cell(pos, Vector3.zero));
            
        }
    }

    public override void Step(){
        frameCount ++;
        renderFrameCount = Time.frameCount;
        avgDistCovered = 0f;

        base.Step();

        avgDistCovered /= agents.Count;
        Debug.Log("position0: " + StemCells[1].position.ToString());
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos() {
        DrawBoundsBox();    
    }

    void DrawBoundsBox(){
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(bounds.center, bounds.extents * 2f);
    }

    public GameObject Spawn()
    {
        return Instantiate(hscPrefab);
    }
    

    public void AgentReportDistCovered(float d){
        avgDistCovered += d;
    }

    public void AgentReportState(int id, Vector3 newPos, Vector3 newVelocity)
    {
        StemCells[id].addState(newPos, newVelocity);
    }
}
