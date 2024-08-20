using UnityEngine;
public class WeaponPlacement : MonoBehaviour
{
    public GameObject[] weaponPrefabs;    // Array of weapon prefabs to switch between
    public LayerMask islandLayer;         // Layer for detecting island blocks
    public LayerMask waterLayer;          // Layer for detecting water
    public LayerMask weaponLayer;         // Layer for detecting existing weapons
    public float placementOffsetY = 2f;   // Height adjustment for correctly positioning the weapon on the block
    public float placementRadius = 0.5f;  // Radius for checking overlapping objects
    public float pushForce = 5f;          // Force applied when the weapon is pushed

    private GameObject currentWeapon;     // Reference to the weapon being placed
    private int currentPrefabIndex = 0;   // Index of the currently selected weapon prefab
    private bool isPlacingWeapon = false; // Flag to determine if we are in placement mode

    void Update()
    {
        HandlePrefabSwitching();

        if (Input.GetKeyDown(KeyCode.B) && !isPlacingWeapon && currentWeapon == null)
        {
            StartPlacingWeapon();
        }

        if (isPlacingWeapon && currentWeapon != null)
        {
            MoveWeaponWithMouse();

            if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
            {
                PlaceWeapon();
            }
        }

        // Check if the weapon is in water and apply physics if necessary
        if (currentWeapon != null && IsWeaponInWater(currentWeapon.transform.position))
        {
            HandleWeaponInWater();
        }
    }

    void HandlePrefabSwitching()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            currentPrefabIndex--;
            if (currentPrefabIndex < 0) currentPrefabIndex = weaponPrefabs.Length - 1;
            Debug.Log("Switched to: " + weaponPrefabs[currentPrefabIndex].name);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            currentPrefabIndex++;
            if (currentPrefabIndex >= weaponPrefabs.Length) currentPrefabIndex = 0;
            Debug.Log("Switched to: " + weaponPrefabs[currentPrefabIndex].name);
        }

        // Destroy the current weapon if it exists, to replace it with the new prefab
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
            StartPlacingWeapon();
        }
    }

    void StartPlacingWeapon()
    {
        Vector3 spawnPosition = GetMousePositionOnIsland();
        if (spawnPosition != Vector3.zero && CanPlaceWeapon(spawnPosition))
        {
            currentWeapon = Instantiate(weaponPrefabs[currentPrefabIndex], spawnPosition, Quaternion.identity);
            isPlacingWeapon = true;
            Debug.Log(weaponPrefabs[currentPrefabIndex].name + " placed at: " + spawnPosition);

            VerifyWeaponPrefabAssignment();
        }
        else
        {
            Debug.LogWarning("Cannot place weapon here, another weapon is already in the way!");
        }
    }

    void MoveWeaponWithMouse()
    {
        Vector3 targetPosition = GetMousePositionOnIsland();
        if (targetPosition != Vector3.zero)
        {
            currentWeapon.transform.position = targetPosition;
        }
    }

    void PlaceWeapon()
    {
        if (IsWeaponInWater(currentWeapon.transform.position))
        {
            HandleWeaponInWater();
        }
        else
        {
            // Finalize weapon placement
            isPlacingWeapon = false;
            currentWeapon = null; // Reset reference to allow new placements
        }
    }

    Vector3 GetMousePositionOnIsland()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, islandLayer))
        {
            // Adjust position to the center of the block, considering the offset
            Vector3 blockCenter = hit.collider.bounds.center;
            blockCenter.y = hit.point.y + placementOffsetY;
            return blockCenter;
        }

        return Vector3.zero;
    }

    bool IsWeaponInWater(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.1f, waterLayer);
        return colliders.Length > 0;
    }

    bool CanPlaceWeapon(Vector3 position)
    {
        // Check if any object within the placementRadius and on the weaponLayer
        Collider[] colliders = Physics.OverlapSphere(position, placementRadius, weaponLayer);
        return colliders.Length == 0; // Can place if no other weapons are detected within the radius
    }

    void HandleWeaponInWater()
    {
        if (currentWeapon != null)
        {
            Rigidbody rb = currentWeapon.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = currentWeapon.AddComponent<Rigidbody>(); // Add a Rigidbody if not present
            }

            // Apply force to simulate sinking or movement in water
            rb.AddForce(Vector3.down * pushForce, ForceMode.Impulse);

            // Optionally destroy the weapon after a while in the water
            Destroy(currentWeapon, 5f); // Destroy the weapon after 5 seconds in the water
        }
    }

    void VerifyWeaponPrefabAssignment()
    {
        // Check if the prefab has a specific component (e.g., CannonController)
        CannonController cannonController = currentWeapon.GetComponent<CannonController>();
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
            Debug.LogError("CannonController script is missing from the weapon prefab.");
        }
    }
}
