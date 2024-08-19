using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;
    public Animator animator;
    public GameObject sword; // Ensure this is assigned in the Inspector
    public Transform swordHand; // Ensure this is assigned in the Inspector

    private bool isAttacking = false;

    void Start()
    {
        // Parent the sword to the swordHand and adjust position and rotation
        if (sword != null && swordHand != null)
        {
            sword.transform.SetParent(swordHand);
            sword.transform.localPosition = new Vector3(0.1f, 0.2f, 0); // Adjust as needed
            sword.transform.localRotation = Quaternion.Euler(90, 0, 0); // Adjust as needed
            sword.SetActive(true); // Ensure sword is always visible
        }
        else
        {
            Debug.LogWarning("Sword or swordHand is not assigned.");
        }
    }

    void Update()
    {
        // Movement handling
        float move = Input.GetAxis("Vertical");
        float strafe = Input.GetAxis("Horizontal");

        Vector3 direction = new Vector3(strafe, 0, move).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);

            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            transform.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.World);

            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        // Sword attack handling
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        // Wait for attack animation to play
        yield return new WaitForSeconds(0.5f); // Adjust according to your attack animation

        // Add attack logic here (e.g., detect collisions, deal damage)

        yield return new WaitForSeconds(0.5f); // Adjust if necessary

        isAttacking = false;
    }
}
