using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Location of player using transform properties
    public float detectionRange = 10f; // Range at which the enemy can detect the player

    private NavMeshAgent agent; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Get the NavMesh Agent component
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position); // Calculate the distance between the enemy and the player

        if (distanceToPlayer < detectionRange) // Check if the player is within the detection range
        {
            agent.SetDestination(player.position); // Move towards the player
        }
        else
        {
            agent.ResetPath(); // Stop moving if the player is out of range
        }
    }
}
