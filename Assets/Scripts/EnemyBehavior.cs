using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3.5f;           // Movement speed
    public float damage = 5f;            // Damage dealt to the terrain
    public float attackInterval = 1f;    // Time between attacks

    private Transform target;            // The current target terrain
    private NavMeshAgent agent;          // Reference to the NavMeshAgent
    private float attackCooldown = 0f;   // Cooldown timer for attacks

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        SearchForNewTarget();
    }

    void Update()
    {
        if (target == null)
        {
            SearchForNewTarget();
        }
        else
        {
            agent.SetDestination(target.position);

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (attackCooldown <= 0f)
                {
                    AttackTarget();
                    attackCooldown = attackInterval;
                }
            }

            if (attackCooldown > 0f)
            {
                attackCooldown -= Time.deltaTime;
            }
        }
    }

    void SearchForNewTarget()
    {
        GameObject[] terrains = GameObject.FindGameObjectsWithTag("Terrain");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestTerrain = null;

        foreach (GameObject terrain in terrains)
        {
            float distanceToTerrain = Vector3.Distance(transform.position, terrain.transform.position);
            if (distanceToTerrain < shortestDistance)
            {
                shortestDistance = distanceToTerrain;
                nearestTerrain = terrain;
            }
        }

        if (nearestTerrain != null)
        {
            target = nearestTerrain.transform;
        }
        else
        {
            target = null; // No target found
        }
    }

    void AttackTarget()
    {
        TerrainHealth terrainHealth = target.GetComponent<TerrainHealth>();
        if (terrainHealth != null)
        {
            terrainHealth.TakeDamage(damage);

            if (terrainHealth.currentHealth <= 0f)
            {
                SearchForNewTarget(); // Search for a new target once the current one is destroyed
            }
        }
    }
}
