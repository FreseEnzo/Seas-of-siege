using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public float damageAmount = 10f;

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // Handle collision with enemies
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Find the EnemyBoat component on the enemy
            EnemyBoat enemyBoat = collision.gameObject.GetComponent<EnemyBoat>();
            if (enemyBoat != null)
            {
                // Apply damage to the enemy
                enemyBoat.TakeDamage(damageAmount);
                // Optionally, destroy the projectile after impact
                Destroy(gameObject);
            }
        }
        // Handle collision with water or other specific layers
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            Destroy(gameObject);
        }
    }
}
