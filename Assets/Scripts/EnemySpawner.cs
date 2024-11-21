using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyBoatPrefab;
    public float spawnInterval = 5f;
    public float moveSpeed = 2f;
    public float stopDistance = 2.3f;
    public float breakTime = 5f;
    public float waterHeight = 0f;

    // Increased sea dimensions to ensure enemies spawn far from the island
    public float seaWidth = 100f;   // Increased from 30f
    public float seaLength = 100f;  // Increased from 30f
    public float seaHeight = 0f;
    public float recoverySpeed = 0.5f;

    private Transform[] islandBlocks;

    void Start()
    {
        StartCoroutine(InitializeIslandBlocksAndSpawn());
    }

    IEnumerator InitializeIslandBlocksAndSpawn()
    {
        yield return new WaitUntil(() =>
        {
            IslandGenerator islandGenerator = FindObjectOfType<IslandGenerator>();
            if (islandGenerator != null)
            {
                islandBlocks = islandGenerator.GetIslandBlocks();
                if (islandBlocks != null && islandBlocks.Length > 0)
                {
                    Debug.Log("Island blocks received by EnemySpawner.");
                    return true;
                }
                else
                {
                    Debug.LogWarning("Waiting for island blocks to be generated...");
                }
            }
            return false;
        });

        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            Vector3 spawnPosition = GetRandomEdgePosition();
            Transform targetBlock = GetClosestIslandBlock(spawnPosition);
            if (targetBlock != null)
            {
                GameObject enemy = Instantiate(enemyBoatPrefab, spawnPosition, Quaternion.identity);

                Rigidbody rb = enemy.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = false;
                    // Ensure collision detection is suitable for fast-moving objects
                    rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                }

                enemy.tag = "Enemy";

                EnemyBoat enemyBoat = enemy.GetComponent<EnemyBoat>();
                if (enemyBoat != null)
                {
                    enemyBoat.moveSpeed = moveSpeed;
                }

                StartCoroutine(MoveEnemyToTarget(enemy.transform, targetBlock));
            }
        }
    }

    Vector3 GetRandomEdgePosition()
    {
        float distanceFromIsland = 50f; // Desired minimum distance from the island

        // Generate a random direction on the horizontal plane
        Vector3 direction = Random.onUnitSphere;
        direction.y = 0; // Keep direction horizontal
        direction.Normalize();

        // Calculate island center to spawn enemies relative to it
        Vector3 islandCenter = GetIslandCenter();

        // Position enemies at a set distance from the island center
        Vector3 spawnPosition = islandCenter + direction * distanceFromIsland;
        spawnPosition.y = seaHeight; // Ensure enemies spawn at sea level

        return spawnPosition;
    }

    Vector3 GetIslandCenter()
    {
        // Calculate the average position of all island blocks to find the center
        Vector3 center = Vector3.zero;
        foreach (Transform block in islandBlocks)
        {
            center += block.position;
        }
        center /= islandBlocks.Length;
        return center;
    }

    Transform GetClosestIslandBlock(Vector3 position)
    {
        Transform closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform block in islandBlocks)
        {
            float distance = Vector3.Distance(position, block.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = block;
            }
        }

        return closest;
    }

    IEnumerator MoveEnemyToTarget(Transform enemy, Transform targetBlock)
    {
        Renderer blockRenderer = targetBlock.GetComponent<Renderer>();
        if (blockRenderer == null)
        {
            Debug.LogError("Island block does not have a Renderer.");
            yield break;
        }

        Color initialColor = blockRenderer.material.color;
        Color damageColor = Color.red;
        float timeAtTarget = 0f;

        while (enemy != null)
        {
            // Calculate horizontal distance (ignore y)
            float currentDistance = Vector2.Distance(
                new Vector2(enemy.position.x, enemy.position.z),
                new Vector2(targetBlock.position.x, targetBlock.position.z)
            );

            if (currentDistance > stopDistance)
            {
                // Move towards the target island block
                Vector3 direction = (targetBlock.position - enemy.position);
                direction.y = 0f; // Ignore vertical difference
                direction.Normalize();
                enemy.position += direction * moveSpeed * Time.deltaTime;

                // Rotate enemy to face the movement direction
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction); 
                    targetRotation *= Quaternion.Euler(0, -180f, 0);
                    enemy.rotation = Quaternion.Slerp(enemy.rotation, targetRotation, Time.deltaTime * moveSpeed);
                }
            }
            else
            {
                // Enemy has reached the island block and starts attacking
                Debug.Log("Enemy reached the target block, starting attack.");

                timeAtTarget += Time.deltaTime;
                Debug.Log($"Attacking block, timeAtTarget: {timeAtTarget}");

                // Visual feedback: change island block color over time
                blockRenderer.material.color = Color.Lerp(initialColor, damageColor, timeAtTarget / breakTime);

                if (timeAtTarget >= breakTime)
                {
                    // Destroy the island block after breakTime
                    BreakIsland(targetBlock);
                    Transform newTarget = GetClosestIslandBlock(enemy.position);
                    if (newTarget != null)
                    {
                        // Continue moving to the next block
                        StartCoroutine(MoveEnemyToTarget(enemy.transform, newTarget));
                    }
                    else
                    {
                        // No more island blocks, destroy the enemy
                        Destroy(enemy.gameObject);
                    }
                    yield break;
                }
            }

            yield return null;
        }

        if (enemy == null)
        {
            // Start recovery if enemy is destroyed
            StartCoroutine(RecoverIsland(blockRenderer, initialColor));
        }
    }

    void BreakIsland(Transform islandBlock)
    {
        if (System.Array.Exists(islandBlocks, block => block == islandBlock))
        {
            Destroy(islandBlock.gameObject);
            // Update the list of island blocks
            islandBlocks = System.Array.FindAll(islandBlocks, block => block != islandBlock);
            Debug.Log("Island block destroyed.");
        }
        else
        {
            Debug.LogWarning("Island block not found for destruction.");
        }
    }

    IEnumerator RecoverIsland(Renderer blockRenderer, Color initialColor)
    {
        // Gradually restore the island block's color
        while (blockRenderer != null && blockRenderer.material.color != initialColor)
        {
            blockRenderer.material.color = Color.Lerp(blockRenderer.material.color, initialColor, recoverySpeed * Time.deltaTime);
            yield return null;
        }

        if (blockRenderer != null)
        {
            Debug.Log("Island recovered its color.");
        }
    }
}
