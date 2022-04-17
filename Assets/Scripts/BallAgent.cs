using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BallAgent : Agent
{
    public GameObject parent;
    public Transform Target;
    public float forceMultiplier = 10;
    bool boom;
    Rigidbody rBody;
    public List<GameObject> planes;
    public Transform s1;
    public Transform s2;
    public Transform s3;
    public Transform s4;
    public Transform s5;
    public Transform s6;
    public Transform s7;
    public Transform s8;
    public Transform s9;
    public Transform s10;
    public Transform s11;
    public Transform s12;
    public Transform w1;
    public Transform w2;
    public Transform w3;
    public Transform w4;
    public Transform w5;
    public Transform w6;
    public Transform w7;
    public Transform w8;
    public Transform w9;
    public Transform w10;
    public Transform w11;
    public Transform w12;
    public Transform[] walls = new Transform[12];
    public Transform[] spawns = new Transform[12];
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        SetWallsAndSpawns();
        for(int i = 0; i < 7; i++) {
            planes.Add(parent.transform.Find($"Plane ({i})").gameObject);
        }
    }

    public override void OnEpisodeBegin()
    {
        int spawn = Random.Range(1, 7);
        ResetSpawn();

        foreach(GameObject plane in planes) {
            int colour = Random.Range(1, 3);
            if(colour == 1) {
                plane.GetComponent<MeshRenderer>().material.color = Color.green;
            }
            else {
                plane.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }

        // Move the target to a new spot
        spawn = Random.Range(1, 12);
        for(int i = 1; i <= 12; i++) {
            if(spawn == i) {
                Target.localPosition = spawns[i-1].localPosition;
            }
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Target.localPosition); // 3
        sensor.AddObservation(this.transform.localPosition); // 3
        foreach(Transform t in walls) { // 36
            sensor.AddObservation(t.localPosition);
        }
        foreach(GameObject g in planes) {
            sensor.AddObservation(g.transform.localPosition); // 21
        }
        // Agent velocity
        sensor.AddObservation(rBody.velocity.x); // 1
        sensor.AddObservation(rBody.velocity.z); // 1
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        AddReward(-1f / MaxStep);

        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        // Fell off platform
        if(this.transform.localPosition.y < 0) {
            SetReward(-.1f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("platforms") && other.gameObject.GetComponent<MeshRenderer>().material.color == Color.red) {
            SetReward(-.1f);
            EndEpisode();
        }
        //else if(other.gameObject.CompareTag("platforms") && other.gameObject.GetComponent<MeshRenderer>().material.color == Color.green) {
        //    SetReward(.1f);
        //}
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("target")) {
            SetReward(1f);
            EndEpisode();
        }
        //else if(other.gameObject.CompareTag("wall")) {
        //    //Debug.Log("Touched wall");
        //    //SetReward(-.1f);
        //    EndEpisode();
        //}
    }

    private void ResetSpawn()
    {
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        int spawn = Random.Range(0, 11);
        this.transform.localPosition = spawns[spawn].localPosition;
    }

    private void SetWallsAndSpawns() {
        walls[0] = w1;
        walls[1] = w2;
        walls[2] = w3;
        walls[3] = w4;
        walls[4] = w5;
        walls[5] = w6;
        walls[6] = w7;
        walls[7] = w8;
        walls[8] = w9;
        walls[9] = w10;
        walls[10] = w11;
        walls[11] = w12;
        spawns[0] = s1;
        spawns[1] = s2;
        spawns[2] = s3;
        spawns[3] = s4;
        spawns[4] = s5;
        spawns[5] = s6;
        spawns[6] = s7;
        spawns[7] = s8;
        spawns[8] = s9;
        spawns[9] = s10;
        spawns[10] = s11;
        spawns[11] = s12;
    }
}