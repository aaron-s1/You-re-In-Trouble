using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoMove : MonoBehaviour
{

    [SerializeField] float pushForce = 10f;
    [SerializeField] float deadZoneTurnRadius = 1f; // Distance where rotation won't update

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("KillGravity", 10f);
    }

    void FixedUpdate()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPosition = rb.position;
        Vector2 pushDirection = (playerPosition - mousePosition).normalized; // Opposite direction

        // Instantly set velocity instead of applying force to prevent acceleration buildup
        rb.velocity = pushDirection * pushForce;

        // Rotate player to face the mouse if outside the dead zone
        if (Vector2.Distance(playerPosition, mousePosition) > deadZoneTurnRadius)
        {
            float angle = Mathf.Atan2(mousePosition.y - playerPosition.y, mousePosition.x - playerPosition.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }
    }

    void KillGravity()
    {
        rb.gravityScale = 1f;
        pushForce = 0;
    }
}
