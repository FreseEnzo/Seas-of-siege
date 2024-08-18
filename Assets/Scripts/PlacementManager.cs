using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public GridManager gridManager;
    public GameManager gameManager;

    private bool isPlacingBlock = true; // Switch between placing blocks and weapons

    private void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            if (isPlacingBlock)
            {
                gameManager.AddBlock(mousePosition);
            }
            else
            {
                gameManager.PlaceWeapon(mousePosition);
            }
        }

        // Toggle between placing blocks and weapons
        if (Input.GetKeyDown(KeyCode.B))
        {
            isPlacingBlock = true;
            UpdatePlacementIndicators();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            isPlacingBlock = false;
            UpdatePlacementIndicators();
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return new Vector3(Mathf.Round(hit.point.x), 0, Mathf.Round(hit.point.z));
        }
        return Vector3.zero;
    }

    private void UpdatePlacementIndicators()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        gridManager.ShowPlacementIndicators(mousePosition, isPlacingBlock);
    }
}
