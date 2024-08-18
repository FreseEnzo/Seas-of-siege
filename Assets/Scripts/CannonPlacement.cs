using UnityEngine;

public class CannonPlacement : MonoBehaviour
{
    public GameObject cannonPrefab;
    public Transform playerTransform;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Pressione 'P' para colocar um canhão
        {
            PlaceCannon();
        }
    }

    void PlaceCannon()
    {
        // Suponha que o jogador está no bloco mais próximo para colocar o canhão
        Vector3 placementPosition = playerTransform.position;
        placementPosition.y = 1; // Altura do bloco
        Instantiate(cannonPrefab, placementPosition, Quaternion.identity);
    }
}
