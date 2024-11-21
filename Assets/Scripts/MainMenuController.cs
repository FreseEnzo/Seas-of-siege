using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Function to start the game
    public void StartGame()
    {
        // Replace "GameScene" with the name of your game scene
        SceneManager.LoadScene("SampleScene");
    }

    // Function to quit the game
    public void QuitGame()
    {
        // If running in the editor
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
