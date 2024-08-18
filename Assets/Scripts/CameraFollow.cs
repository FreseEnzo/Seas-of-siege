using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // O alvo que a câmera irá seguir (personagem)
    public float distance = 10f;    // Distância da câmera para o personagem
    public float height = 2f;      // Altura da câmera em relação ao personagem
    public float smoothSpeed = 0.125f; // Suavidade no movimento da câmera
    public float fov = 60f;         // Campo de visão da câmera

    private Vector3 offset;

    void Start()
    {
        // Calcula o offset baseado na distância e altura
        offset = new Vector3(0f, height, -distance);

        // Configura o campo de visão da câmera
        Camera.main.fieldOfView = fov;
    }

    void LateUpdate()
    {
        // Posição desejada da câmera com base no alvo e no offset
        Vector3 desiredPosition = target.position + offset;

        // Suaviza o movimento da câmera
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Atualiza a posição da câmera
        transform.position = smoothedPosition;

        // Faz a câmera olhar para o personagem
        transform.LookAt(target);
    }
}
