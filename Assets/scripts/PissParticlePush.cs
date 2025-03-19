using UnityEngine;

public class PissParticlePush : MonoBehaviour
{
    [SerializeField] float pushForce;

    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Player1")
            return;
        // Revise this later so piss can't interact with parent at all.
        // else if (gameObject.transform.parent.gameObject != null)
        // {
        // if (other == gameObject.transform.parent.gameObject)
                // return;
        // }


        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Debug.Log("pissing collision check passed");

            Vector2 forceDirection = (other.transform.position - transform.position).normalized;
            
            rb.AddForce(forceDirection * pushForce, ForceMode2D.Force);
        }
    }
}
