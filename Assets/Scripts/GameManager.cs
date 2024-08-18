using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject grayScreenPanel;  // Referência ao painel cinza
    public GridManager gridManager;     // Referência ao gerenciador do grid
    public float waveDuration = 5f;    // Duração de cada horda em segundos
    private float timeRemaining;        // Tempo restante na horda atual

    private void Start()
    {
        grayScreenPanel.SetActive(false); // Inicia com a tela cinza desativada
        StartCoroutine(WaveRoutine());    // Inicia a primeira horda
    }

    private IEnumerator WaveRoutine()
    {
        while (true)
        {
            StartWave();
            yield return new WaitForSeconds(waveDuration);  // Espera o tempo da horda
            EndWave();
            yield return new WaitForSeconds(1f);  // Pequena pausa antes de reiniciar
        }
    }

    private void StartWave()
    {
        Time.timeScale = 1f;  // Certifique-se de que o jogo está em execução
    }

    private void EndWave()
    {
        grayScreenPanel.SetActive(true); // Ativa a tela cinza
        Time.timeScale = 0f; // Pausa o jogo
    }

    public void AddBlock(Vector3 position)
    {
        gridManager.AddBlock(position);
        ResumeGame();
    }

    public void PlaceWeapon(Vector3 position)
    {
        gridManager.PlaceWeapon(position);
        ResumeGame();
    }

    private void ResumeGame()
    {
        grayScreenPanel.SetActive(false);
        Time.timeScale = 1f; // Retoma o jogo
        StartCoroutine(WaveRoutine()); // Inicia a próxima horda
    }
}
