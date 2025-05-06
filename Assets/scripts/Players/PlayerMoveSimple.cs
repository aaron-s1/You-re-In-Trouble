using UnityEngine;

public class PlayerMoveSimple : MonoBehaviour
{
    [SerializeField] float inputSpeed = 10f;
    [SerializeField] float movementDamping = 5f;

    // [SerializeField] bool ifBurstWeapon;

    Rigidbody2D rigid;

    void Awake() => 
        rigid = GetComponent<Rigidbody2D>();

    void FixedUpdate()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontalMove, verticalMove).normalized * inputSpeed;

        rigid.AddForce(movement - rigid.velocity * movementDamping);
    }
}


// using UnityEngine;

// public class PlayerMoveSimple : MonoBehaviour
// {
//     [SerializeField] float inputSpeed = 10f; // Speed for normal movement
//     [SerializeField] float movementDamping = 5f; // Smooths movement over time
//     [SerializeField] bool ifShotgun; // Enables shotgun burst movement
//     [SerializeField] float shotgunForce = 20f; // Strength of shotgun burst
//     [SerializeField] float shotgunCooldown = 0.5f; // Cooldown time between bursts

//     private Rigidbody2D rigid;
//     private float lastShotgunTime; // Tracks last shotgun burst

//     void Awake()
//     {
//         rigid = GetComponent<Rigidbody2D>();
//     }

//     void FixedUpdate()
//     {
//         float horizontalMove = Input.GetAxis("Horizontal");
//         float verticalMove = Input.GetAxis("Vertical");

//         Vector2 movement = new Vector2(horizontalMove, verticalMove).normalized;

//         if (ifShotgun && Time.time > lastShotgunTime + shotgunCooldown && movement != Vector2.zero)
//         {
//             // Apply a burst of force in the movement direction
//             rigid.AddForce(movement * shotgunForce, ForceMode2D.Impulse);
//             lastShotgunTime = Time.time; // Update cooldown timer
//         }
//         else
//         {
//             // Apply normal movement with damping
//             rigid.AddForce(movement * inputSpeed - rigid.velocity * movementDamping);
//         }

//         // Rotate player to face movement direction if moving
//         if (movement != Vector2.zero)
//         {
//             float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
//             rigid.rotation = angle;
//         }
//     }
// }