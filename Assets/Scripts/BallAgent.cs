using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BallAgent : Agent
{
    public float ts;
    public GameObject parent;
    public Transform Target;
    public float forceMultiplier = 10;
    bool boom;
    Rigidbody rBody;
    public List<GameObject> planes;
    public List<GameObject> walls;
    public List<GameObject> spawns;
    void Start()
    {
        Time.timeScale = ts;
        rBody = GetComponent<Rigidbody>();
        SetGameObjects();
    }
    public override void OnEpisodeBegin()
    {
        ResetSpawn();

        ChangePlaneColour();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 targetDir = (Target.localPosition - transform.localPosition).normalized;
        sensor.AddObservation(targetDir.x); // 1
        sensor.AddObservation(targetDir.z); // 1
        sensor.AddObservation(Target.localPosition); // 3
        sensor.AddObservation(transform.localPosition); // 3
        foreach(GameObject t in walls) { // 4 walls
            sensor.AddObservation(t.transform.localPosition); // 3
            Vector3 wallDir = (t.transform.localPosition - transform.localPosition).normalized;
            sensor.AddObservation(wallDir.x); // 1
            sensor.AddObservation(wallDir.z); // 1
        }
        foreach(GameObject g in planes) { // 3
            sensor.AddObservation(g.transform.localPosition); // 3
            Vector3 planeDir = (g.transform.localPosition - transform.localPosition).normalized;
            sensor.AddObservation(planeDir.x); // 1
            sensor.AddObservation(planeDir.z); // 1
        }
        sensor.AddObservation(rBody.velocity.x); // 1
        sensor.AddObservation(rBody.velocity.z);// 1
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        //Vector3 controlSignal = Vector3.zero;
        //controlSignal.x = actionBuffers.ContinuousActions[0];
        //controlSignal.z = actionBuffers.ContinuousActions[1];
        //rBody.AddForce(controlSignal * forceMultiplier);

        float moveX = actionBuffers.ContinuousActions[0];
        float moveZ = actionBuffers.ContinuousActions[1];
        transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * forceMultiplier;

        // Fell off platform
        if(this.transform.localPosition.y < 0) {
            SetReward(-.1f);
            EndEpisode();
        }
        AddReward(-1f / MaxStep);
    }
    public override void Heuristic(in ActionBuffers actionsOut) {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("platforms") && other.gameObject.GetComponent<MeshRenderer>().material.color == Color.red) {
            SetReward(-1f);
            EndEpisode();
        }
        else if(other.gameObject.CompareTag("platforms") && other.gameObject.GetComponent<MeshRenderer>().material.color == Color.green) {
            SetReward(1f);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("target")) {
            SetReward(1f);
            EndEpisode();
        }

        if(other.gameObject.CompareTag("wall")) {
            EndEpisode();
        }
    }
    private void ResetSpawn()
    {
        // Target
        Target.localPosition = spawns[0].transform.localPosition;

        // Agent
        transform.localPosition = spawns[1].transform.localPosition;    

        //this.rBody.angularVelocity = Vector3.zero;
        //this.rBody.velocity = Vector3.zero;
        //int spawn = Random.Range(0, 11);
        //this.transform.localPosition = spawns[spawn].transform.localPosition;

        //// Move the target to a new spot
        //int spawn = Random.Range(1, 7);
        //spawn = Random.Range(1, 12);
        //for(int i = 1; i <= 12; i++) {
        //    if(spawn == i) {
        //        Target.localPosition = spawns[i - 1].transform.localPosition;
        //    }
        //}
    }
    private void SetGameObjects() {
        for(int i = 0; i < 3; i++) { // Initializing planes 
            planes.Add(parent.transform.Find($"Plane ({i})").gameObject);
        }
        for(int i = 0; i < 4; i++) { // initializing walls
            walls.Add(parent.transform.Find($"wall ({i})").gameObject);
        }
        for(int i = 0; i < 2; i++) { // initializing spawns
            spawns.Add(parent.transform.Find($"Spawn ({i})").gameObject);
        }
    }
    private void ChangePlaneColour() {
        int red = 0;
        foreach(GameObject plane in planes) {
            int colour = Random.Range(1, 3);
            if(colour == 1) {
                plane.GetComponent<MeshRenderer>().material.color = Color.green;
            }
            else {
                plane.GetComponent<MeshRenderer>().material.color = Color.red;
                red++;
            }
        }
        if(red >= 3) {
            planes[Random.Range(0,3)].GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }
}