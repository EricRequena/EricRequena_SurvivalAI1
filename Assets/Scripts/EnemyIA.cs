using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform [] patrolPoints;
    private int currentPoint = 0;
    public Transform player;

    public float maxHealth = 100f;
    public float currentHealth;
    public float detectionRange = 10f; // Rango de detección
    public float lostRange = 15f; // Si el jugador sale de esta distancia, vuelve a patrullar
    public float fleeDistance = 10f; // Distancia que huirá

    private enum State
    {
        Patrolling,
        Chasing,
        Fleeing
    }

    private State currentState;

    void Start ()
    {
        agent = GetComponent<NavMeshAgent> ();
        currentHealth = maxHealth;
        ChangeState (State.Patrolling);
    }

    void Update ()
    {
        float distanceToPlayer = Vector3.Distance (transform.position, player.position);

        if (currentHealth > maxHealth * 0.5f && distanceToPlayer <= detectionRange)
        {
            ChangeState (State.Chasing);
        }
        else if (currentHealth <= maxHealth * 0.5f)
        {
            ChangeState (State.Fleeing);
        }
        else if (distanceToPlayer > lostRange)
        {
            ChangeState (State.Patrolling);
        }

        switch (currentState)
        {
            case State.Patrolling: Patrol (); break;
            case State.Chasing: ChasePlayer (); break;
            case State.Fleeing: FleeFromPlayer (); break;
        }
    }

    void ChangeState (State newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }

    void Patrol ()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
            agent.destination = patrolPoints [currentPoint].position;
        }
    }

    void ChasePlayer ()
    {
        if (player != null)
        {
            agent.destination = player.position;
        }
    }

    void FleeFromPlayer ()
    {
        if (player != null)
        {
            // Dirección opuesta al jugador
            Vector3 fleeDirection = (transform.position - player.position).normalized;
            Vector3 fleeTarget = transform.position + fleeDirection * fleeDistance;

            agent.destination = fleeTarget;
        }
    }

    public void TakeDamage (float damage)
    {
        currentHealth -= damage;
    }
}
