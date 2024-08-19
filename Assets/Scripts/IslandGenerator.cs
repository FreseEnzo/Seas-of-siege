using UnityEngine;
using System.Collections.Generic;

public class IslandGenerator : MonoBehaviour
{
    public GameObject islandPrefab;
    public int islandSize = 10;
    public float islandHeight = 0.2f; // Altura da ilha acima do nível do mar

    private List<Transform> generatedBlocks = new List<Transform>();

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
        Vector3 startPosition = new Vector3(15, islandHeight, 15); // Posição inicial da ilha com altura

        Queue<Vector3> blocksToCheck = new Queue<Vector3>();
        blocksToCheck.Enqueue(startPosition);
        islandBlocks.Add(startPosition);

        // Geração dos blocos da ilha
        while (blocksToCheck.Count > 0 && islandBlocks.Count < islandSize)
        {
            Vector3 currentBlock = blocksToCheck.Dequeue();
            foreach (Vector3 direction in new Vector3[] { Vector3.right, Vector3.left, Vector3.forward, Vector3.back })
            {
                Vector3 newBlock = currentBlock + direction;
                // Garante que o novo bloco não esteja já presente e que está dentro dos limites do mar
                if (!islandBlocks.Contains(newBlock) && Random.value > 0.5f && IsValidBlock(newBlock))
                {
                    islandBlocks.Add(newBlock);
                    blocksToCheck.Enqueue(newBlock);
                }
            }
        }

        // Instancia os blocos da ilha
        foreach (Vector3 block in islandBlocks)
        {
            GameObject islandObject = Instantiate(islandPrefab, new Vector3(block.x, islandHeight, block.z), Quaternion.identity);
            generatedBlocks.Add(islandObject.transform);
        }
    }

    bool IsValidBlock(Vector3 position)
    {
        // Verifica se o bloco está dentro dos limites do mar (opcional)
        return position.x >= 0 && position.x < 30 && position.z >= 0 && position.z < 30;
    }

    public Vector3 GetSpawnPosition()
    {
        // Retorna a posição do primeiro bloco gerado na ilha como ponto de spawn
        if (generatedBlocks.Count > 0)
        {
            return generatedBlocks[0].position;
        }
        else
        {
            // Se não houver blocos, retorna a posição padrão
            return new Vector3(15, islandHeight, 15);
        }
    }
}
