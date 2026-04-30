using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] protected float detectionRange = 15f;
    [SerializeField] protected bool alwaysChase = false;

    [Header("Movement")]
    [SerializeField] protected float moveSpeed = 3.5f;
    [SerializeField] protected float stoppingDistance = 1.2f;

    [Header("Attack")]
    [SerializeField] protected int attackDamage = 10;
    [SerializeField] protected float attackCooldown = 1.5f;

    [Header("References")]
    [SerializeField] protected Transform player;

    protected NavMeshAgent agent;
    protected Health playerHealth;

    private float nextAttackTime;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.speed = moveSpeed;
            agent.stoppingDistance = stoppingDistance;
        }
    }

    protected virtual void Start()
    {
        FindPlayer();
    }

    protected virtual void Update()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        UpdateBehaviour();
    }

    protected virtual void UpdateBehaviour()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (alwaysChase || distanceToPlayer <= detectionRange)
        {
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            StopMoving();
        }
    }

    protected void ChasePlayer(float distanceToPlayer)
    {
        if (agent == null) return;

        if (distanceToPlayer > stoppingDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
            agent.ResetPath();

            LookAtPlayer();
            TryAttack();
        }
    }

    protected void StopMoving()
    {
        if (agent == null) return;

        agent.isStopped = true;
        agent.ResetPath();
    }

    protected void TryAttack()
    {
        if (Time.time < nextAttackTime) return;

        if (playerHealth == null)
        {
            FindPlayer();
        }

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            nextAttackTime = Time.time + attackCooldown;

            Debug.Log(gameObject.name + " attacked the player. Damage: " + attackDamage);
        }
    }

    protected void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 8f
            );
        }
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponent<Health>();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }
}