using UnityEngine;

public class TowerLevel : MonoBehaviour
{
    public Gradient colorGradient; // Assign this in the Inspector
    public int maxLevel = 100; // Define the maximum level for color changes
    private int currentLevel = 1;
    private Renderer towerRenderer;

    void Start()
    {
        // Get the Renderer component of the tower
        towerRenderer = GetComponent<Renderer>();
        
        // Initialize the tower color
        UpdateTowerColor();
    }

    public void LevelUp()
    {
        // Increment the level and clamp it within the range
        currentLevel = Mathf.Clamp(currentLevel + 1, 1, maxLevel);
        
        // Update the tower color based on the new level
        UpdateTowerColor();
    }

    private void UpdateTowerColor()
    {
        // Calculate the color based on the current level and max level
        float levelPercentage = (float)(currentLevel - 1) / (maxLevel - 1);
        Color newColor = colorGradient.Evaluate(levelPercentage);
        
        // Apply the new color to the tower's material
        towerRenderer.material.color = newColor;
    }

    // Optional: Method to manually set the tower level
    public void SetLevel(int level)
    {
        currentLevel = Mathf.Clamp(level, 1, maxLevel);
        UpdateTowerColor();
    }
}
