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
        GameObject waterPlane = Instantiate(waterPrefab, new Vector3(width / 2, waterHeight, height / 2), Quaternion.identity);
        waterPlane.transform.localScale = new Vector3(width, 1, height); // Ajusta o tamanho do plano
    }
}
