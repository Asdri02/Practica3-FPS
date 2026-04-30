using UnityEngine;

public class EnemyPatrol : Enemy
{
    [Header("Patrol")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolPointDistance = 1f;

    private int currentPointIndex;

    protected override void Awake()
    {
        base.Awake();
        alwaysChase = false;
    }

    protected override void UpdateBehaviour()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (agent == null) return;
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPointIndex];

        agent.isStopped = false;
        agent.SetDestination(targetPoint.position);

        float distanceToPoint = Vector3.Distance(transform.position, targetPoint.position);

        if (distanceToPoint <= patrolPointDistance)
        {
            currentPointIndex++;
            currentPointIndex %= patrolPoints.Length;
        }
    }
}