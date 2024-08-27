using UnityEngine;

public class AssetTagCheck : MonoBehaviour
{
    public int sizeRequirement = 1;

    void Start()
    {
        ValidateAsset();
    }

    void ValidateAsset()
    {
        // Get the starting position of the asset
        Vector3 startPos = transform.position;
        bool isValid = true;

        // Loop through all the tiles under the asset based on sizeRequirement
        for (int x = 0; x < sizeRequirement; x++)
        {
            for (int z = 0; z < sizeRequirement; z++)
            {
                // Calculate the position to check
                Vector3 checkPos = new Vector3(startPos.x + x, startPos.y, startPos.z + z);
                // Use a raycast downwards to check the terrain tag
                if (!CheckTerrainTag(checkPos))
                {
                    isValid = false;
                    break;
                }
            }
            if (!isValid) break;
        }

        // Destroy the asset if any part of the terrain does not have the "Island" tag
        if (!isValid)
        {
            Destroy(gameObject);
        }
    }

    bool CheckTerrainTag(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up * 10f, Vector3.down, out hit, 20f))
        {
            if (hit.collider != null && hit.collider.CompareTag("Island"))
            {
                return true;
            }
        }
        return false;
    }
}
