public int numberOfMountains = 3;
public int mountainSizeRequirement = 2;
public int numberOfTrees = 10;
public int treeSizeRequirement = 1;
public GameObject MountainPrefap;
public GameObject TreePrefap;

void GenerateIsland()
{
    // Existing code for island generation...

    // Place mountains
    if (!PlaceAssets(islandBlocks, MountainPrefap, numberOfMountains, mountainSizeRequirement))
    {
        Debug.Log("Not enough space for all mountains. Placed as many as possible.");
    }

    // Place trees
    if (!PlaceAssets(islandBlocks, TreePrefap, numberOfTrees, treeSizeRequirement))
    {
        Debug.Log("Not enough space for all trees. Placed as many as possible.");
    }
}

bool PlaceAssets(HashSet<Vector3> islandBlocks, GameObject assetPrefab, int numberOfAssets, int sizeRequirement)
{
    List<Vector3> validPositions = new List<Vector3>();

    // Find all valid positions for asset placement
    foreach (Vector3 block in islandBlocks)
    {
        if (CheckFreeSpace(block, sizeRequirement, islandBlocks))
        {
            validPositions.Add(block);
        }
    }

    // Place as many assets as possible
    int assetsToPlace = Mathf.Min(numberOfAssets, validPositions.Count);
    for (int i = 0; i < assetsToPlace; i++)
    {
        Vector3 position = validPositions[i];
        Instantiate(assetPrefab, new Vector3(position.x, islandHeight, position.z), Quaternion.identity);
    }

    // Return true if all assets were placed, otherwise return false
    return assetsToPlace == numberOfAssets;
}

bool CheckFreeSpace(Vector3 startBlock, int size, HashSet<Vector3> islandBlocks)
{
    for (int x = 0; x < size; x++)
    {
        for (int z = 0; z < size; z++)
        {
            Vector3 checkBlock = new Vector3(startBlock.x + x, startBlock.y, startBlock.z + z);
            if (!islandBlocks.Contains(checkBlock)) return false;
        }
    }
    return true;
}
