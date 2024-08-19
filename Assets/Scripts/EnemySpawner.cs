using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyBoatPrefab;
    public float spawnInterval = 5f;
    public float moveSpeed = 2f;
    public float stopDistance = 2.3f;
    public float breakTime = 5f;
    public float waterHeight = 0f; // Altura da água

    public float seaWidth = 30f;
    public float seaLength = 30f;
    public float seaHeight = 0f;

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
                    Debug.Log("Blocos da ilha recebidos pelo EnemySpawner.");
                    return true;
                }
                else
                {
                    Debug.LogWarning("Aguardando blocos da ilha serem gerados...");
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
                    rb.useGravity = false; // Desativa a gravidade
                }
                StartCoroutine(MoveEnemyToTarget(enemy.transform, targetBlock));
            }
        }
    }

    Vector3 GetRandomEdgePosition()
    {
        float x = Random.Range(0, seaWidth);
        float z = Random.Range(0, seaLength);

        bool onXEdge = Random.value > 0.5f;
        if (onXEdge)
        {
            bool onLeftEdge = Random.value > 0.5f;
            x = onLeftEdge ? 0 : seaWidth;
            z = Random.Range(0, seaLength);
        }
        else
        {
            bool onBottomEdge = Random.value > 0.5f;
            z = onBottomEdge ? 0 : seaLength;
            x = Random.Range(0, seaWidth);
        }

        return new Vector3(x, seaHeight, z);
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
        Vector3 startPosition = enemy.position;
        Vector3 targetPosition = targetBlock.position;
        targetPosition.y = waterHeight; // Define a altura da água

        float timeAtTarget = 0f;

        while (Vector3.Distance(new Vector3(enemy.position.x, waterHeight, enemy.position.z), new Vector3(targetPosition.x, waterHeight, targetPosition.z)) > stopDistance)
        {
            Vector3 direction = (targetPosition - enemy.position).normalized;
            enemy.position += direction * moveSpeed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(-direction);
            enemy.rotation = Quaternion.Slerp(enemy.rotation, targetRotation, Time.deltaTime * moveSpeed);

            yield return null;
        }

        Vector3 directionAtStop = (targetPosition - enemy.position).normalized;
        enemy.position = new Vector3(targetPosition.x, waterHeight, targetPosition.z) - directionAtStop * stopDistance;

        while (Vector3.Distance(new Vector3(enemy.position.x, waterHeight, enemy.position.z), new Vector3(targetPosition.x, waterHeight, targetPosition.z)) <= stopDistance)
        {
            timeAtTarget += Time.deltaTime;

            if (timeAtTarget >= breakTime)
            {
                BreakIsland(targetBlock);

                Transform newTarget = GetClosestIslandBlock(enemy.position);
                if (newTarget != null)
                {
                    StartCoroutine(MoveEnemyToTarget(enemy, newTarget));
                }
                else
                {
                    enemy.gameObject.SetActive(false);
                }

                yield break;
            }

            yield return null;
        }

        enemy.gameObject.SetActive(false);
    }

    void BreakIsland(Transform islandBlock)
    {
        if (System.Array.Exists(islandBlocks, block => block == islandBlock))
        {
            Destroy(islandBlock.gameObject);
            islandBlocks = System.Array.FindAll(islandBlocks, block => block != islandBlock);
            Debug.Log("Bloco da ilha destruído.");
        }
        else
        {
            Debug.LogWarning("Bloco da ilha não encontrado para destruição.");
        }
    }
}
