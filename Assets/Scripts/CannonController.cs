using UnityEngine;

public class CannonController : MonoBehaviour
{
    public Transform cannonTransform;         // Reference to the cannon's transform
    public Transform cannonballExitPoint;     // The specific point where the cannonball should exit
    public GameObject cannonballPrefab;       // Reference to the cannonball prefab
    public float firingForce = 10f;           // Force applied to the cannonball
    public float range = 10f;                 // Range within which to target enemies
    public float fireRate = 1f;               // Rate of fire in seconds

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
                ShootCannonball();
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
            // Calculate the direction from the cannon to the target
            Vector3 direction = (targetEnemy.position - cannonTransform.position).normalized;
            direction.y = 0;

            // Calculate the target rotation based on the direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);


            // Smoothly rotate the cannon towards the corrected target rotation
            cannonTransform.rotation = Quaternion.Slerp(cannonTransform.rotation, targetRotation, Time.deltaTime * 2f);
        }
    }

    void ShootCannonball()
    {
        if (cannonballPrefab != null && cannonballExitPoint != null)
        {
            Debug.Log("Shooting cannonball"); // Debug message to confirm shooting

            // Instantiate cannonball at the specified exit point
            GameObject cannonball = Instantiate(cannonballPrefab, cannonballExitPoint.position, cannonballExitPoint.rotation);
            Rigidbody rb = cannonball.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(-cannonballExitPoint.forward * firingForce, ForceMode.Impulse);
                Debug.Log("Cannonball fired");
            }
            else
            {
                Debug.LogError("Rigidbody component missing on cannonball prefab");
            }
        }
        else
        {
            Debug.LogError("Cannonball prefab or exit point is not assigned");
        }
    }
}
