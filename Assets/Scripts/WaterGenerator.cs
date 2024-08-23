using UnityEngine;

public class WaterGenerator : MonoBehaviour
{
    public GameObject waterPrefab;
    public int width = 30;
    public int height = 30;
    public float waterHeight = 0f; // Altura da água

    void Start()
    {
        GenerateWater();
    }

    void GenerateWater()
    {
        // Instancia o plano de água
        GameObject waterPlane = Instantiate(waterPrefab, Vector3.zero, Quaternion.identity);

        // Ajusta o tamanho do plano de água
        waterPlane.transform.localScale = new Vector3(width, 1, height);

        // Posiciona o plano de água ao redor do centro da cena
        waterPlane.transform.position = new Vector3(0, waterHeight, 0);
    }
}
