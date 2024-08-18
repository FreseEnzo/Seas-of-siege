using UnityEngine;
using System.Collections;

public class EnemyBoat : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Transform targetBlock;

    void Start()
    {
        // Encontra o bloco alvo mais próximo (o bloco da ilha mais próximo)
        targetBlock = FindClosestIslandBlock();
        if (targetBlock != null)
        {
            StartCoroutine(MoveToTarget());
        }
        else
        {
            Debug.LogError("Nenhum bloco da ilha encontrado para o barco se mover em direção.");
        }
    }

    Transform FindClosestIslandBlock()
    {
        // Substitua isso pelo código que retorna os blocos da ilha
        Transform[] islandBlocks = GameObject.FindObjectsOfType<Transform>(); // Exemplo simplificado

        Transform closestBlock = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Transform block in islandBlocks)
        {
            if (block.CompareTag("IslandBlock")) // Certifique-se de que o bloco da ilha tem a tag "IslandBlock"
            {
                float distance = Vector3.Distance(currentPosition, block.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestBlock = block;
                }
            }
        }

        return closestBlock;
    }

    IEnumerator MoveToTarget()
    {
        while (Vector3.Distance(transform.position, targetBlock.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetBlock.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Opcional: Adicionar lógica quando o barco chega ao bloco da ilha, como atacar
        Debug.Log("Barco chegou ao bloco da ilha!");
    }
}
