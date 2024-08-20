using UnityEngine;

public class CannonPlacement : MonoBehaviour
{
    public GameObject cannonPrefab;       // Prefab of the cannon to be placed
    public LayerMask islandLayer;         // Layer for detecting island blocks
    public LayerMask waterLayer;          // Layer for detecting water
    public float placementOffsetY = 0.5f; // Height adjustment for correctly positioning the cannon on the block
    public float pushForce = 5f;          // Force applied when the cannon is pushed

    private GameObject currentCannon;     // Reference to the cannon being placed
    private bool isPlacingCannon = false; // Flag to determine if we are in placement mode

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && !isPlacingCannon && currentCannon == null)
        {
            StartPlacingCannon();
        }

        if (isPlacingCannon && currentCannon != null)
        {
            MoveCannonWithMouse();

            if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
            {
                PlaceCannon();
            }
        }

        // Check if the cannon is in water and apply physics if necessary
        if (currentCannon != null && IsCannonInWater(currentCannon.transform.position))
        {
            HandleCannonInWater();
        }
    }

    void StartPlacingCannon()
    {
        Vector3 spawnPosition = GetMousePositionOnIsland();
        if (spawnPosition != Vector3.zero)
        {
            currentCannon = Instantiate(cannonPrefab, spawnPosition, Quaternion.identity);
            isPlacingCannon = true;
            Debug.Log("Cannon placed at: " + spawnPosition);

            // Verify cannonball prefab assignment
            CannonController cannonController = currentCannon.GetComponent<CannonController>();
            if (cannonController != null)
            {
                if (cannonController.cannonballPrefab != null)
                {
                    Debug.Log("Cannonball prefab is assigned.");
                }
                else
                {
                    Debug.LogError("Cannonball prefab is not assigned in the CannonController.");
                }
            }
            else
            {
                Debug.LogError("CannonController script is missing from the cannon prefab.");
            }
        }
    }

    void MoveCannonWithMouse()
    {
        Vector3 targetPosition = GetMousePositionOnIsland();
        if (targetPosition != Vector3.zero)
        {
            currentCannon.transform.position = targetPosition;
        }
    }

    void PlaceCannon()
    {
        // Check if the cannon is in water and apply physics if necessary
        if (IsCannonInWater(currentCannon.transform.position))
        {
            HandleCannonInWater();
        }
        else
        {
            // Finalize cannon placement
            isPlacingCannon = false;
            currentCannon = null; // Reset reference to allow new placements
        }
    }

    Vector3 GetMousePositionOnIsland()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, islandLayer | waterLayer))
        {
            // Adjust position to the center of the block, considering the offset
            Vector3 blockCenter = hit.collider.bounds.center;
            blockCenter.y = hit.point.y + placementOffsetY;
            return blockCenter;
        }

        return Vector3.zero;
    }

    bool IsCannonInWater(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.1f, waterLayer);
        return colliders.Length > 0;
    }

    void HandleCannonInWater()
    {
        if (currentCannon != null)
        {
            Rigidbody rb = currentCannon.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = currentCannon.AddComponent<Rigidbody>(); // Add a Rigidbody if not present
            }

            // Apply force to simulate sinking or movement in water
            rb.AddForce(Vector3.down * pushForce, ForceMode.Impulse);

            // Optionally destroy the cannon after a while in the water
            Destroy(currentCannon, 5f); // Destroy the cannon after 5 seconds in the water
        }
    }
}
