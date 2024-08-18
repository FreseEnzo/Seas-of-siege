using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public GameObject waterPrefab;
    public GameObject islandBlockPrefab;
    public GameObject terraPrefab; // Prefab do bloco de terra
    public GameObject weaponPrefab; // Prefab da arma
    public GameObject placementIndicatorPrefab; // Prefab do indicador de colocação
    public Color availableColor = Color.green; // Cor para áreas disponíveis
    public Color occupiedColor = Color.red; // Cor para áreas ocupadas

    private int gridSize = 25;
    private GameObject[,] grid;
    private GameObject[,] indicators; // Matriz para armazenar os indicadores de colocação

    private void Start()
    {
        InitializeGrid();
        CreateRandomizedIsland(10);
    }

    private void InitializeGrid()
    {
        grid = new GameObject[gridSize, gridSize];
        indicators = new GameObject[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                GameObject tile = Instantiate(waterPrefab, new Vector3(x, 0, y), Quaternion.identity);
                tile.transform.parent = this.transform;
                grid[x, y] = tile;

                // Criar e posicionar indicadores
                GameObject indicator = Instantiate(placementIndicatorPrefab, new Vector3(x, 0, y), Quaternion.identity);
                indicator.transform.parent = this.transform;
                indicator.SetActive(false); // Inicialmente desativado
                indicators[x, y] = indicator;
            }
        }
    }

    private void CreateRandomizedIsland(int numBlocks)
    {
        if (numBlocks <= 0 || numBlocks > gridSize * gridSize)
        {
            Debug.LogError("Número inválido de blocos para a ilha.");
            return;
        }

        List<Vector2Int> islandBlocks = new List<Vector2Int>();
        Vector2Int start = GetRandomStartPosition();
        islandBlocks.Add(start);

        Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(start);

        while (queue.Count > 0 && islandBlocks.Count < numBlocks)
        {
            Vector2Int current = queue.Dequeue();
            List<Vector2Int> shuffledDirections = new List<Vector2Int>(directions);
            Shuffle(shuffledDirections);

            foreach (Vector2Int direction in shuffledDirections)
            {
                Vector2Int neighbor = current + direction;

                if (IsInBounds(neighbor) && !islandBlocks.Contains(neighbor) && 
                    IsConnected(islandBlocks, neighbor))
                {
                    islandBlocks.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        foreach (Vector2Int block in islandBlocks)
        {
            Destroy(grid[block.x, block.y]);
            GameObject islandBlock = Instantiate(islandBlockPrefab, new Vector3(block.x, 0, block.y), Quaternion.identity);
            islandBlock.transform.parent = this.transform;
            islandBlock.tag = "Island";
            grid[block.x, block.y] = islandBlock;
        }
    }

    public void ShowPlacementIndicators(Vector3 position, bool isPlacingBlock)
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                GameObject indicator = indicators[x, y];
                if (indicator != null)
                {
                    Vector3 indicatorPosition = new Vector3(x, 0, y);
                    bool isAvailable = GetGridCell(indicatorPosition) == null;
                    indicator.GetComponent<SpriteRenderer>().color = isAvailable ? availableColor : occupiedColor;
                    indicator.SetActive(isAvailable || !isPlacingBlock);
                }
            }
        }
    }

    public void AddBlock(Vector3 position)
    {
        if (GetGridCell(position) == null)
        {
            Instantiate(terraPrefab, position, Quaternion.identity);
            grid[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z)] = terraPrefab;
        }
    }

    public void PlaceWeapon(Vector3 position)
    {
        if (GetGridCell(position) != null)
        {
            Instantiate(weaponPrefab, position, Quaternion.identity);
        }
    }

    private GameObject GetGridCell(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.z);
        if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
        {
            return grid[x, y];
        }
        return null;
    }

    private Vector2Int GetRandomStartPosition()
    {
        int centerX = gridSize / 2;
        int centerY = gridSize / 2;
        int offsetX = Random.Range(-4, 5);
        int offsetY = Random.Range(-4, 5);
        return new Vector2Int(centerX + offsetX, centerY + offsetY);
    }

    private bool IsInBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridSize && pos.y >= 0 && pos.y < gridSize;
    }

    private bool IsConnected(List<Vector2Int> islandBlocks, Vector2Int pos)
    {
        Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighbor = pos + direction;
            if (islandBlocks.Contains(neighbor))
            {
                return true;
            }
        }
        return false;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}
