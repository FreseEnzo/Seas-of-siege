using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotationSpeed = 700f;

    void Update()
    {
        // Input de movimentação
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Direção de movimento
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Ângulo de rotação
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);

            // Rotação do personagem
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Movimento
            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
