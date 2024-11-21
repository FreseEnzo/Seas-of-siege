using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Function to start the game
    public void StartGame()
    {
        // Replace "GameScene" with the exact name of your game scene
        SceneManager.LoadScene("SampleScene");
    }

    // Optional: Function to quit the game
    public void QuitGame()
    {
        // If running in the Unity Editor
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
