using UnityEngine;
using System.Collections;

public class EnemyBoat : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float health = 100f;  // Boat's health
    private Transform targetBlock;
    private Coroutine moveCoroutine;

    void Start()
    {
        FindAndMoveToTargetBlock();
    }

    void Update()
    {
        if (targetBlock == null)
        {
            FindAndMoveToTargetBlock();
        }
    }

    void FindAndMoveToTargetBlock()
    {
        targetBlock = FindClosestIslandBlock();
        if (targetBlock != null)
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(MoveToTarget());
        }
        else
        {
            Debug.LogError("No island block found for the boat to move towards.");
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
        Destroy(gameObject);
        Debug.Log("Boat destroyed!");
    }

    Transform FindClosestIslandBlock()
    {
        GameObject[] islandBlocks = GameObject.FindGameObjectsWithTag("Island");

        Transform closestBlock = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject blockObj in islandBlocks)
        {
            Transform block = blockObj.transform;
            float distance = Vector3.Distance(currentPosition, block.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestBlock = block;
            }
        }

        return closestBlock;
    }

    IEnumerator MoveToTarget()
    {
        while (targetBlock != null && Vector3.Distance(transform.position, targetBlock.position) > 0.1f)
        {
            // Calculate direction to the target
            Vector3 direction = (targetBlock.position - transform.position).normalized;

            // Move towards the target
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Update rotation to face the direction of movement
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(-direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);
            }

            yield return null;
        }

        if (targetBlock == null)
        {
            // The target block was destroyed before we reached it
            Debug.Log("Target block destroyed before reaching it.");
        }
        else
        {
            // Reached the target block
            Debug.Log("Boat reached the island block!");
            StartCoroutine(AttackBlock());
        }
    }

    IEnumerator AttackBlock()
    {
        float attackDuration = 5f; // Time to destroy the block
        float elapsedTime = 0f;

        Renderer blockRenderer = targetBlock.GetComponent<Renderer>();
        if (blockRenderer == null)
        {
            Debug.LogError("Island block does not have a Renderer.");
            yield break;
        }

        Color initialColor = blockRenderer.material.color;
        Color damageColor = Color.red;

        while (elapsedTime < attackDuration && targetBlock != null)
        {
            elapsedTime += Time.deltaTime;
            // Visual feedback
            blockRenderer.material.color = Color.Lerp(initialColor, damageColor, elapsedTime / attackDuration);
            yield return null;
        }

        if (targetBlock != null)
        {
            // Destroy the block
            Destroy(targetBlock.gameObject);
            Debug.Log("Island block destroyed.");
            targetBlock = null;
        }

        // Decide what to do next: Find a new target or stay in place
        FindAndMoveToTargetBlock();
    }
}
