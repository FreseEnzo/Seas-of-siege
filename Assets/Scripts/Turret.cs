using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float range = 10f;              // Turret range
    public float damage = 10f;             // Damage per shot
    public float attackInterval = 1f;      // Time between shots
    public string enemyTag = "Enemy";      // Tag used to identify enemies

    private Transform target;              // The current target
    private float attackCooldown = 0f;     // Time until the next attack

    // Reference to the turret's rotating part (if applicable)
    public Transform rotatingPart;

    // Reference to the turret's firing point
    public Transform firePoint;

    void Update()
    {
        // Find the closest enemy within range
        FindTarget();

        // Rotate turret to face the target if one is found
        if (target != null)
        {
            RotateTowardsTarget();

            // Attack the target if cooldown allows
            if (attackCooldown <= 0f)
            {
                Attack();
                attackCooldown = attackInterval;
            }
        }

        // Reduce cooldown timer
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= range)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void RotateTowardsTarget()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(rotatingPart.rotation, lookRotation, Time.deltaTime * 5f).eulerAngles;
        rotatingPart.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Attack()
    {
        // Deal damage to the target (this could be replaced with projectile instantiation, etc.)
        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }
    }

    // Optional: Visualize the turret's range in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
