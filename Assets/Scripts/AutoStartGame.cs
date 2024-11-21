using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AutoStartGame : MonoBehaviour
{
    // Name of the scene to load after delay
    [SerializeField] private string gameSceneName = "SampleScene";

    // Delay duration in seconds
    [SerializeField] private float delayInSeconds = 4f;

    private void Start()
    {
        // Start the coroutine to load the scene after delay
        StartCoroutine(LoadGameAfterDelay());
    }

    private IEnumerator LoadGameAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delayInSeconds);

        // Load the game scene
        SceneManager.LoadScene(gameSceneName);
    }
}
