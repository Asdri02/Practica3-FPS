using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Respawn")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float fallLimit = -10f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (transform.position.y < fallLimit)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        if (respawnPoint == null)
        {
            transform.position = new Vector3(0f, 1f, 0f);
        }
        else
        {
            transform.position = respawnPoint.position;
            transform.rotation = respawnPoint.rotation;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}