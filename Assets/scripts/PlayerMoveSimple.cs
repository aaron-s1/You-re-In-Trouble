using UnityEngine;

public class PlayerMoveSimple : MonoBehaviour
{
    [SerializeField] float inputSpeed = 10f; // Increased for smoother movement
    [SerializeField] float movementDamping = 5f; // Smooths movement over time

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontalMove, verticalMove).normalized * inputSpeed;

        // Apply movement as a force instead of directly setting velocity
        rigid.AddForce(movement - rigid.velocity * movementDamping);
    }
}
