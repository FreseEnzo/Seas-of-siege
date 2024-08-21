using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform weaponTransform;            // Reference to the weapon's transform
    public Transform projectileExitPoint;        // The specific point where the projectile should exit
    public GameObject projectilePrefab;          // Reference to the projectile prefab (can be a cannonball, arrow, etc.)
    public float firingForce = 10f;              // Force applied to the projectile
    public float range = 50f;                    // Range within which to target enemies
    public float fireRate = 1f;                  // Rate of fire in seconds
    public float weaponHeight = 10f;             // Height of the weapon
    public float arcHeight = 2f;                 // Height of the arc for projectile trajectory

    private Transform targetEnemy;
    private float fireCooldown;

    void Start()
    {
        fireCooldown = 0f; // Initialize cooldown
    }

    void Update()
    {
        FindTarget();
        if (targetEnemy != null)
        {
            AimAtTarget();
            fireCooldown -= Time.deltaTime;

            // Automatic fire when cooldown is complete
            if (fireCooldown <= 0f)
            {
                ShootProjectile();
                fireCooldown = fireRate; // Reset the cooldown
            }
        }
    }

    void FindTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hitCollider.transform;
                }
            }
        }

        targetEnemy = closestEnemy;
    }

    void AimAtTarget()
    {
        if (targetEnemy != null)
        {
            // Calculate the direction from the weapon to the target
            Vector3 direction = (targetEnemy.position - weaponTransform.position).normalized;

            // Adjust direction based on weapon height
            direction.y += weaponHeight;

            // Calculate the target rotation based on the direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Ensure that the weapon's local up direction is aligned with the world up direction
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            weaponTransform.rotation = lookRotation;
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null && projectileExitPoint != null)
        {
            Debug.Log("Shooting projectile"); // Debug message to confirm shooting

            // Instantiate projectile at the specified exit point
            GameObject projectile = Instantiate(projectilePrefab, projectileExitPoint.position, projectileExitPoint.rotation);

            // Adjust the projectile orientation
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 shootDirection = (targetEnemy.position - projectileExitPoint.position).normalized;
                shootDirection.y += arcHeight; // Adjust the arc height for the trajectory

                // Set the velocity of the projectile's Rigidbody
                rb.velocity = shootDirection * firingForce;
            }
            else
            {
                Debug.LogError("Rigidbody component missing on projectile prefab");
            }

            Debug.Log("Projectile fired");
        }
        else
        {
            Debug.LogError("Projectile prefab or exit point is not assigned");
        }
    }
}
