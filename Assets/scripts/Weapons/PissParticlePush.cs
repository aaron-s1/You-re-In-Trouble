using UnityEngine;

public class PissParticlePush : MonoBehaviour
{
    [SerializeField] float pushForce;

    void OnParticleCollision(GameObject other)
    {
        // if (other.transform.GetChild(0).TryGetComponent<ParticleSystem>(out _))
        // {
        //     Debug.Log($"{gameObject}'s particles hit {other}'s particles!");
        //     // Vector2 forceDirection = (other.transform.position - transform.position).normalized;
            
        //     // rb.AddForce(forceDirection * pushForce, ForceMode2D.Force);            
        //     return;
        // }

        // Debug.Log($"{gameObject} touched {other}");

        // if (other.tag == "Player1")
        // {
        //     Debug.Log("....");            
        //     Debug.Log($"{gameObject} touched {other}");
        //     return;
        // }

        // Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

        // if (rb != null)
        // {
        //     Debug.Log($"{gameObject}'s pissing particles hit {other}'s rigidbody");

        //     Vector2 forceDirection = (other.transform.position - transform.position).normalized;
            
        //     rb.AddForce(forceDirection * pushForce, ForceMode2D.Force);
        // }
    }
}
