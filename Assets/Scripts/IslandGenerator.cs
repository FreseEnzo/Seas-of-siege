using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabChance
{
    public GameObject prefab; // Prefab to be instantiated
    public float chance; // Probability of placing this prefab
}

public class IslandGenerator : MonoBehaviour
{
    public GameObject islandPrefab; // Prefab that will contain the island
    public List<PrefabChance> vegetationPrefabs; // List of vegetation prefabs with their probabilities
    public int islandSize = 500; // Size of the island in blocks
    public float islandHeight = 0.2f; // Height of the island above sea level
    public float spawnAreaSize = 10f; // Size of the clear spawn area in blocks

    private List<Transform> generatedBlocks = new List<Transform>();
    private Dictionary<Vector3, List<Transform>> blockVegetationMap = new Dictionary<Vector3, List<Transform>>();

    void Start()
    {
        GenerateIsland();
    }

    public Transform[] GetIslandBlocks()
    {
        return generatedBlocks.ToArray();
    }

    void GenerateIsland()
    {
        HashSet<Vector3> islandBlocks = new HashSet<Vector3>();
        Vector3 startPosition = Vector3.zero; // Start position at the origin

        Queue<Vector3> blocksToCheck = new Queue<Vector3>();
        blocksToCheck.Enqueue(startPosition);
        islandBlocks.Add(startPosition);

        // Generate island blocks
        while (blocksToCheck.Count > 0 && islandBlocks.Count < islandSize)
        {
            Vector3 currentBlock = blocksToCheck.Dequeue();
            foreach (Vector3 direction in new Vector3[] { Vector3.right, Vector3.left, Vector3.forward, Vector3.back })
            {
                Vector3 newBlock = currentBlock + direction;
                if (!islandBlocks.Contains(newBlock) && Random.value > 0.5f && IsValidBlock(newBlock))
                {
                    islandBlocks.Add(newBlock);
                    blocksToCheck.Enqueue(newBlock);
                }
            }
        }

        // Instantiate island blocks
        foreach (Vector3 block in islandBlocks)
        {
            GameObject islandObject = Instantiate(islandPrefab, block, Quaternion.identity);
            islandObject.transform.position = new Vector3(block.x, islandHeight, block.z);
            generatedBlocks.Add(islandObject.transform);

            // Add vegetation
            if (!IsInSpawnArea(block))
            {
                AddVegetation(islandObject.transform, block);
            }
        }

        // Debug: Output the positions of all generated blocks
        foreach (var block in generatedBlocks)
        {
            Debug.Log("Generated Block at: " + block.position);
        }
    }

    void AddVegetation(Transform islandTransform, Vector3 blockPosition)
    {
        // Calculate dimensions of the island prefab
        Collider islandCollider = islandPrefab.GetComponent<Collider>();
        if (islandCollider == null)
        {
            Debug.LogError("Island prefab does not have a Collider component.");
            return;
        }
        Vector3 islandSize = islandCollider.bounds.size;

        // Initialize vegetation list for the block
        List<Transform> vegetationList = new List<Transform>();

        // Decide whether to place vegetation based on probabilities
        foreach (PrefabChance prefabChance in vegetationPrefabs)
        {
            if (Random.value < prefabChance.chance)
            {
                // Instantiate vegetation prefab
                GameObject vegetation = Instantiate(prefabChance.prefab, Vector3.zero, Quaternion.identity, islandTransform);
                // Adjust position to ensure it fits within the block
                AdjustPosition(vegetation.transform, islandSize);
                vegetationList.Add(vegetation.transform);
            }
        }

        // Add vegetation list to the map
        if (vegetationList.Count > 0)
        {
            blockVegetationMap[blockPosition] = vegetationList;
        }
    }

    void AdjustPosition(Transform objTransform, Vector3 islandSize)
    {
        // Adjust the position of the vegetation to fit within the island block
        float xOffset = Random.Range(-islandSize.x / 2 + 0.5f, islandSize.x / 2 - 0.5f);
        float zOffset = Random.Range(-islandSize.z / 2 + 0.5f, islandSize.z / 2 - 0.5f);
        objTransform.localPosition = new Vector3(xOffset, 0, zOffset);
    }

    bool IsValidBlock(Vector3 position)
    {
        // Ensure the block is within the sea limits
        float halfSize = islandSize / 2f;
        return Mathf.Abs(position.x) <= halfSize && Mathf.Abs(position.z) <= halfSize;
    }

    bool IsInSpawnArea(Vector3 position)
    {
        // Check if the position is within the spawn area
        Vector3 spawnCenter = Vector3.zero; // Center of the spawn area
        return Vector3.Distance(position, spawnCenter) <= spawnAreaSize / 2f;
    }

    public Vector3 GetSpawnPosition()
    {
        // Return the position of the first generated block as a spawn point
        if (generatedBlocks.Count > 0)
        {
            return generatedBlocks[0].position;
        }
        else
        {
            // Return a default position if no blocks were generated
            return Vector3.zero;
        }
    }

    public void RemoveBlock(Vector3 blockPosition)
    {
        // Remove the block and its vegetation if applicable
        if (blockVegetationMap.TryGetValue(blockPosition, out List<Transform> vegetationList))
        {
            foreach (Transform veg in vegetationList)
            {
                Destroy(veg.gameObject); // Destroy the vegetation
            }
            blockVegetationMap.Remove(blockPosition);
        }
    }
}
