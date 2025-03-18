using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAvoidance : MonoBehaviour
{
    public NavMeshAgent agent; 
    public LayerMask obstacleLayer; 

    private void Start ()
    {
        agent = GetComponent<NavMeshAgent> ();
    }

    private void Update ()
    {
        RaycastHit hit;

      
        if (Physics.Raycast (transform.position, agent.velocity.normalized, out hit, agent.radius, obstacleLayer))
        {
            AvoidObstacle ();
        }
        else
        {
            MoveTowardsDestination ();
        }
    }
    void AvoidObstacle ()
    {
        Vector3 avoidanceDirection = Vector3.Cross (agent.velocity, Vector3.up).normalized;
        agent.SetDestination (transform.position + avoidanceDirection * 5f); 
    }

    void MoveTowardsDestination ()
    {
       
    }
}
