using UnityEngine;

public class WaterStream : MonoBehaviour
{
    public float streamLength = 5f;
    public float pushForce = 10f;
    public LayerMask pushableLayers;

    LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

void OnDrawGizmos()
{
    if (!Application.isPlaying) return; // Prevents errors in edit mode

    Vector2 startPos = transform.position;
    Vector2 direction = transform.right;

    RaycastHit2D hit = Physics2D.Raycast(startPos, direction, streamLength, pushableLayers);
    
    Gizmos.color = hit.collider != null ? Color.green : Color.red;
    Vector2 endPos = hit.collider != null ? hit.point : startPos + direction * streamLength;

    Gizmos.DrawLine(startPos, endPos);
}

 void Update()
{
    Vector2 startPos = transform.position;
    Vector2 direction = transform.right;

    RaycastHit2D hit = Physics2D.Raycast(startPos, direction, streamLength, pushableLayers);
    lineRenderer.SetPosition(0, startPos);

    if (hit.collider != null)
    {
        lineRenderer.SetPosition(1, hit.point);
        Rigidbody2D rb = hit.collider.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(direction * pushForce, ForceMode2D.Force);
        }

        // Draw a red ray to show the hit point
        // Debug.DrawRay(startPos, direction * hit.distance, Color.green);
    }
    else
    {
        lineRenderer.SetPosition(1, startPos + direction * streamLength);

        // Draw a green ray if nothing was hit
        // Debug.DrawRay(startPos, direction * streamLength, Color.red);
    }
}

}
