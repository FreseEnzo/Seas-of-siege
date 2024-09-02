using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // Prefab do jogador
    private IslandGenerator islandGenerator;

    void Start()
    {
        islandGenerator = FindObjectOfType<IslandGenerator>();
        if (islandGenerator != null)
        {
            // Obtém a posição de spawn da ilha
            Vector3 spawnPosition = islandGenerator.GetSpawnPosition();
            spawnPosition.y += 2;

            // Encontra o jogador na cena
            GameObject existingPlayer = GameObject.FindWithTag("Player");

            if (existingPlayer != null)
            {
                // Move o jogador existente para a nova posição
                existingPlayer.transform.position = spawnPosition;
                existingPlayer.transform.rotation = Quaternion.identity; // Opcional: Reseta a rotação do jogador
            }
            else
            {
                // Se o jogador não existir, instancia um novo
                Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogError("IslandGenerator não encontrado!");
        }
    }
}
