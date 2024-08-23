using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // O alvo que a câmera irá seguir (personagem)
    public float distance = 10f;    // Distância da câmera para o personagem
    public float height = 2f;       // Altura da câmera em relação ao personagem
    public float smoothSpeed = 0.125f; // Suavidade no movimento da câmera
    public float fov = 60f;         // Campo de visão da câmera
    public float rotationAngle = 90f; // Ângulo de rotação em graus

    private float currentRotationAngle = 0f;

    void Start()
    {
        // Configura o campo de visão da câmera
        Camera.main.fieldOfView = fov;
    }

    void Update()
    {
        // Rotaciona a câmera com base na entrada do usuário
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentRotationAngle -= rotationAngle;
            if (currentRotationAngle < -360f)
            {
                currentRotationAngle += 360f;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentRotationAngle += rotationAngle;
            if (currentRotationAngle > 360f)
            {
                currentRotationAngle -= 360f;
            }
        }
    }

    void LateUpdate()
    {
        // Calcula a rotação desejada com base no ângulo atual
        Quaternion rotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Calcula a nova posição da câmera com base na rotação
        Vector3 offset = new Vector3(0f, height, -distance);
        Vector3 rotatedOffset = rotation * offset;
        Vector3 desiredPosition = target.position + rotatedOffset;

        // Suaviza o movimento da câmera
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Atualiza a posição da câmera
        transform.position = smoothedPosition;

        // Faz a câmera olhar para o personagem
        transform.LookAt(target);
    }
}
