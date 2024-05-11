using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.IO.IsolatedStorage;

public class Moveto : Agent
{
    [SerializeField] private Transform targetTranform;
    //[SerializeField] private Transform targetTranform2;
    //[SerializeField] private Transform targetTranform3;

    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;


    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-2.5f, +2f), 0, Random.Range(-2.7f, +2.8f));
        targetTranform.localPosition = new Vector3(Random.Range(2.5f, +6.3f), 0, Random.Range(-2.7f, +2.8f));
        //targetTranform2.localPosition = new Vector3(Random.Range(2.5f, +6.3f), 0, Random.Range(-2.7f, +2.8f));
        //targetTranform3.localPosition = new Vector3(Random.Range(2.5f, +6.3f), 0, Random.Range(-2.7f, +2.8f));

    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTranform.localPosition);
        //sensor.AddObservation(targetTranform2.localPosition);
        //sensor.AddObservation(targetTranform3.localPosition);

    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];


        float moveSpeed = 3f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continousActions = actionsOut.ContinuousActions;
        continousActions[0] = Input.GetAxisRaw("Horizontal") * 4;
        continousActions[1] = Input.GetAxisRaw("Vertical") * 4;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Target>(out Target goal))
        {
            SetReward(+5f);
            floorMeshRenderer.material = winMaterial;
            EndEpisode();
        }
        if (other.TryGetComponent<Wall>(out Wall wall)){
            SetReward(-3f);
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();
        } 
    }


}
