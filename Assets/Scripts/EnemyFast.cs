using UnityEngine;

public class EnemyFast : Enemy
{
    [Header("Fast Enemy Settings")]
    [SerializeField] private float fastSpeed = 6.5f;
    [SerializeField] private float fastDetectionRange = 25f;

    protected override void Awake()
    {
        base.Awake();

        moveSpeed = fastSpeed;
        detectionRange = fastDetectionRange;
        alwaysChase = false;

        if (agent != null)
        {
            agent.speed = fastSpeed;
        }
    }
}