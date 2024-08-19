using UnityEngine;

public class CannonPlacement : MonoBehaviour
{
    public GameObject cannonPrefab;       // Prefab do canhão a ser posicionado
    public LayerMask islandLayer;         // Layer para detectar os blocos da ilha
    public LayerMask waterLayer;          // Layer para detectar a água (mar)
    public float placementOffsetY = 0.5f; // Ajuste de altura para posicionar o canhão corretamente no bloco
    public float pushForce = 5f;          // Força aplicada ao empurrar o canhão

    private GameObject currentCannon;     // Referência ao canhão que será posicionado
    private bool isPlacingCannon = false; // Flag para determinar se estamos no modo de colocação

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && !isPlacingCannon && currentCannon == null)
        {
            StartPlacingCannon();
        }

        if (isPlacingCannon && currentCannon != null)
        {
            MoveCannonWithMouse();

            if (Input.GetMouseButtonDown(0)) // 0 é o botão esquerdo do mouse
            {
                PlaceCannon();
            }
        }

        // Verifica se o canhão está na água e aplica a física se necessário
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
        // Verifica se o canhão está na água e aplica a física se necessário
        if (IsCannonInWater(currentCannon.transform.position))
        {
            HandleCannonInWater();
        }
        else
        {
            // Coloca o canhão em sua posição final
            isPlacingCannon = false;
            currentCannon = null; // Reseta a referência para permitir novas colocações
        }
    }

    Vector3 GetMousePositionOnIsland()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, islandLayer | waterLayer))
        {
            // Ajusta a posição para o centro do bloco, considerando o offset
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
                rb = currentCannon.AddComponent<Rigidbody>(); // Adiciona um Rigidbody se não houver um
            }

            // Adiciona uma força para simular o afundamento ou movimento na água
            rb.AddForce(Vector3.down * pushForce, ForceMode.Impulse);

            // Opcionalmente, destrua o canhão se ele ficar na água por um tempo
            Destroy(currentCannon, 5f); // Destroi o canhão após 5 segundos na água
        }
    }
}
