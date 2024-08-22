public int numberOfMountains = 3;
public int mountainSizeRequirement = 2;
public int numberOfTrees = 10;
public int treeSizeRequirement = 1;
public GameObject mountainPrefab;
public GameObject treePrefab;

void GenerateIsland()
{
    // Existing code for island generation...

    // Place mountains
    PlaceAssets(islandBlocks, mountainPrefab, numberOfMountains, mountainSizeRequirement);

    // Place trees
    PlaceAssets(islandBlocks, treePrefab, numberOfTrees, treeSizeRequirement);
}

void PlaceAssets(HashSet<Vector3> islandBlocks, GameObject assetPrefab, int numberOfAssets, int sizeRequirement)
{
    int placedAssets = 0;
    while (placedAssets < numberOfAssets)
    {
        foreach (Vector3 block in islandBlocks)
        {
            if (CheckFreeSpace(block, sizeRequirement, islandBlocks))
            {
                Instantiate(assetPrefab, new Vector3(block.x, islandHeight, block.z), Quaternion.identity);
                placedAssets++;
                if (placedAssets >= numberOfAssets) break;
            }
        }
    }
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
