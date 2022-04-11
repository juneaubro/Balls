using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BallAgent : Agent {
    public Transform Target;
    public float forceMultiplier = 10;
    bool boom;
    Rigidbody rBody;
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
    void Start() {
        rBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin() {
        int spawn = Random.Range(1, 7);
        // If the Agent fell, zero its momentum
        if(this.transform.localPosition.y < 0) {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            if(spawn == 1)
                this.transform.localPosition = s5.localPosition;
            else if(spawn == 2)
                this.transform.localPosition = s1.localPosition;
            else if(spawn == 3)
                this.transform.localPosition = s2.localPosition;
            else
                this.transform.localPosition = new Vector3(0, 0.5f, 0);
        }

        // Move the target to a new spot
        spawn = Random.Range(1, 12);
        if(spawn == 1)
            Target.localPosition = s1.localPosition;
        if(spawn == 2)
            Target.localPosition = s2.localPosition;
        if(spawn == 3)
            Target.localPosition = s3.localPosition;
        if(spawn == 4)
            Target.localPosition = s4.localPosition;
        if(spawn == 5)
            Target.localPosition = s5.localPosition;
        if(spawn == 6)
            Target.localPosition = s6.localPosition;
        if(spawn == 7)
            Target.localPosition = s7.localPosition;
        if(spawn == 8)
            Target.localPosition = s8.localPosition;
        if(spawn == 9)
            Target.localPosition = s9.localPosition;
        if(spawn == 10)
            Target.localPosition = s10.localPosition;
        if(spawn == 11)
            Target.localPosition = s11.localPosition;
        if(spawn == 12)
            Target.localPosition = s12.localPosition;
    }

    public override void CollectObservations(VectorSensor sensor) {
        // Target and Agent positions
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(w1.localPosition);
        sensor.AddObservation(w2.localPosition);
        sensor.AddObservation(w3.localPosition);
        sensor.AddObservation(w4.localPosition);
        sensor.AddObservation(w5.localPosition);
        sensor.AddObservation(w6.localPosition);
        sensor.AddObservation(w7.localPosition);
        sensor.AddObservation(w8.localPosition);
        sensor.AddObservation(w9.localPosition);
        sensor.AddObservation(w10.localPosition);
        sensor.AddObservation(w11.localPosition);
        sensor.AddObservation(w12.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers) {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);
        //float dtw1 = Vector3.Distance(this.transform.localPosition, w1.localPosition);
        //float dtw2 = Vector3.Distance(this.transform.localPosition, w1.localPosition);
        //float dtw3 = Vector3.Distance(this.transform.localPosition, w1.localPosition);
        //float dtw4 = Vector3.Distance(this.transform.localPosition, w1.localPosition);
        //float dtw5 = Vector3.Distance(this.transform.localPosition, w1.localPosition);
        //float dtw6 = Vector3.Distance(this.transform.localPosition, w1.localPosition);
        //float dtw7 = Vector3.Distance(this.transform.localPosition, w1.localPosition);
        //float dtw8 = Vector3.Distance(this.transform.localPosition, w1.localPosition);
        //float dtw9 = Vector3.Distance(this.transform.localPosition, w1.localPosition);
        //float dtw10 = Vector3.Distance(this.transform.localPosition, w1.localPosition);
        //float dtw11 = Vector3.Distance(this.transform.localPosition, w1.localPosition);
        //float dtw12 = Vector3.Distance(this.transform.localPosition, w1.localPosition);

        // Reached target
        if(distanceToTarget < 1.42f) {
            SetReward(1.0f);
            EndEpisode();
        }

        // Fell off platform
        else if(this.transform.localPosition.y < 0) {
            EndEpisode();
        }
    }

    //public override void Heuristic(in ActionBuffers actionsOut) {
    //    var continuousActionsOut = actionsOut.ContinuousActions;
    //    continuousActionsOut[0] = Input.GetAxis("Horizontal");
    //    continuousActionsOut[1] = Input.GetAxis("Vertical");
    //}
}