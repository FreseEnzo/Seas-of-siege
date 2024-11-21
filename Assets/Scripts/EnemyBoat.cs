using UnityEngine;
using System.Collections;

public class EnemyBoat : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float stopDistance = 3f;
    public float breakTime = 5f;
    public float recoverySpeed = 0.5f;

    [Header("Health Settings")]
    public float health = 100f;

    private Transform[] islandBlocks;
    private Transform targetBlock;
    private float timeAtTarget = 0f;
    private Renderer targetBlockRenderer;
    private Color initialColor;
    private Color damageColor = Color.red;

    // Rotation speed multiplier
    public float rotationSpeed = 5f;

    public void SetIslandBlocks(Transform[] blocks)
    {
        islandBlocks = blocks;
    }

    public void Initialize()
    {
        FindAndMoveToTargetBlock();
    }

    void FindAndMoveToTargetBlock()
    {
        targetBlock = FindClosestIslandBlock();
        if (targetBlock != null)
        {
            StartCoroutine(MoveToTarget());
        }
        else
        {
            Debug.LogWarning("No island block found for the boat to move towards.");
            Destroy(gameObject); // Optionally destroy the boat if no targets are available
        }
    }

    Transform FindClosestIslandBlock()
    {
        if (islandBlocks == null || islandBlocks.Length == 0)
            return null;

        Transform closest = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Transform block in islandBlocks)
        {
            if (block == null)
                continue;

            float distance = Vector3.Distance(currentPosition, block.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = block;
            }
        }

        return closest;
    }

    IEnumerator MoveToTarget()
    {
        while (targetBlock != null)
        {
            // Calculate horizontal distance (ignore y)
            Vector2 enemyPos2D = new Vector2(transform.position.x, transform.position.z);
            Vector2 targetPos2D = new Vector2(targetBlock.position.x, targetBlock.position.z);
            float currentDistance = Vector2.Distance(enemyPos2D, targetPos2D);

            if (currentDistance > stopDistance)
            {
                // Move towards the target island block
                Vector3 direction = (targetBlock.position - transform.position);
                direction.y = 0f; // Ignore vertical difference
                direction.Normalize();
                transform.position += direction * moveSpeed * Time.deltaTime;

                // Rotate enemy to face the movement direction
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    // Rotate the boat by 180 degrees if necessary
                    targetRotation *= Quaternion.Euler(0, -180f, 0);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                }
            }
            else
            {
                // Enemy has reached the stop distance and starts attacking
                if (targetBlockRenderer == null)
                {
                    targetBlockRenderer = targetBlock.GetComponent<Renderer>();
                    if (targetBlockRenderer == null)
                    {
                        Debug.LogError("Island block does not have a Renderer.");
                        yield break;
                    }
                    initialColor = targetBlockRenderer.material.color;
                }

                timeAtTarget += Time.deltaTime;
                targetBlockRenderer.material.color = Color.Lerp(initialColor, damageColor, timeAtTarget / breakTime);

                if (timeAtTarget >= breakTime)
                {
                    // Destroy the island block after breakTime
                    BreakIsland(targetBlock);
                    timeAtTarget = 0f;
                    targetBlockRenderer = null;

                    // Find a new target block
                    targetBlock = FindClosestIslandBlock();
                    if (targetBlock != null)
                    {
                        StartCoroutine(MoveToTarget());
                    }
                    else
                    {
                        // No more island blocks, destroy the enemy
                        Destroy(gameObject);
                    }
                    yield break;
                }
            }

            yield return null;
        }

        // If targetBlock is null, attempt to find a new target
        if (targetBlock == null)
        {
            FindAndMoveToTargetBlock();
        }
    }

    void BreakIsland(Transform islandBlock)
    {
        if (islandBlock != null)
        {
            Destroy(islandBlock.gameObject);
            Debug.Log("Island block destroyed.");
        }
    }

    public void TakeDamage(float amount)
    {
        Debug.Log("Boat took damage.");
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boat destroyed!");
        Destroy(gameObject);
    }
}
