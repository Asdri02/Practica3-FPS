using UnityEngine;

public class ShootRaycast : MonoBehaviour
{
    [Header("Shoot Settings")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float shootRange = 100f;
    [SerializeField] private float fireRate = 0.25f;
    [SerializeField] private int damage = 25;
    [SerializeField] private bool onlyShootWhileAiming = true;

    [Header("References")]
    [SerializeField] private GunAim gunAim;

    private float nextFireTime;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            TryShoot();
        }
    }

    private void TryShoot()
    {
        if (Time.time < nextFireTime) return;

        if (playerCamera == null)
        {
            Debug.LogWarning("ShootRaycast: Player Camera is not assigned.");
            return;
        }

        if (onlyShootWhileAiming)
        {
            if (gunAim == null)
            {
                Debug.LogWarning("ShootRaycast: GunAim is not assigned.");
                return;
            }

            if (!gunAim.IsAiming)
            {
                Debug.Log("You must aim before shooting.");
                return;
            }
        }

        Shoot();
        nextFireTime = Time.time + fireRate;
    }

    private void Shoot()
    {
        Vector3 origin = playerCamera.transform.position;
        Vector3 direction = playerCamera.transform.forward;

        Debug.Log("Shot fired");

        if (Physics.Raycast(origin, direction, out RaycastHit hit, shootRange))
        {
            Debug.DrawRay(origin, direction * hit.distance, Color.red, 1f);

            Debug.Log("Hit: " + hit.collider.name);

            Health health = hit.collider.GetComponentInParent<Health>();

            if (health != null)
            {
                health.TakeDamage(damage);
                Debug.Log("Damage applied to: " + health.gameObject.name);
            }
        }
        else
        {
            Debug.DrawRay(origin, direction * shootRange, Color.green, 1f);
            Debug.Log("Shot missed");
        }
    }
}