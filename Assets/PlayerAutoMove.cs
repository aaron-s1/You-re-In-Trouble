using System.Collections;
using UnityEngine;

public class PlayerAutoMove : MonoBehaviour
{
    [SerializeField] GameObject streamerWeapon; // (perhaps make not actually a weapon later)
    [SerializeField] GameObject shotgunWeapon;
    
    [SerializeField] float deadZoneTurnRadius = 1f;

    [Space(10)]
    [SerializeField] float pushForce = 10f;

    [Space(10)]
    [SerializeField] AudioSource soundClip;

    [Space(15)]
    public GameObject activeWeapon;
    [SerializeField] float burstForceMultiplier = 5f;
    [SerializeField] float burstWeaponCooldown = 1f;


    Rigidbody2D rb;
    bool canShootShotgun = true;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        activeWeapon = streamerWeapon;
        streamerWeapon.SetActive(true);
    }


    void FixedUpdate()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPosition = rb.position;

        // Player targets mouse.
        if (Vector2.Distance(playerPosition, mousePosition) > deadZoneTurnRadius)
        {
            float angle = Mathf.Atan2(mousePosition.y - playerPosition.y, mousePosition.x - playerPosition.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }

        if (Input.GetMouseButton(0) && canShootShotgun)
        {
            if (activeWeapon != shotgunWeapon)
            {
                activeWeapon.SetActive(false);
                activeWeapon = shotgunWeapon;
                activeWeapon.SetActive(true);
            }

            PerformWeaponBurst(mousePosition, playerPosition);
            // ^ also applies movement.
        }

        else if (Input.GetMouseButton(1))
        {
            if (activeWeapon != streamerWeapon)
            {
                activeWeapon.SetActive(false);
                activeWeapon = streamerWeapon;
                activeWeapon.SetActive(true);
                rb.velocity = Vector2.zero;
            }

            Debug.Log("right click being held down");

            Vector2 pushDirection = (playerPosition - mousePosition).normalized;
            // rb.velocity = Vector2.zero;
            rb.AddForce(pushDirection * pushForce * 0.01f, ForceMode2D.Impulse);

            // Vector2 pushDirection = (playerPosition - mousePosition).normalized;
            // rb.velocity = pushDirection * pushForce * .1f;
        }        
    }

#region BURST WEAPON.
    void PerformWeaponBurst(Vector2 mousePosition, Vector2 playerPosition)
    {
        SpawnBurstParticles();        
        ApplyBurstMovement(mousePosition, playerPosition);

        canShootShotgun = false;

        StartCoroutine(ResetBurstWeaponCooldown());
    }

    void ApplyBurstMovement(Vector2 mousePos, Vector2 playerPos)
    {
        Vector2 pushDirection = (playerPos - mousePos).normalized;
        rb.velocity = Vector2.zero;
        rb.AddForce(pushDirection * pushForce * burstForceMultiplier, ForceMode2D.Impulse);

        // Vector2 pushDirection = (playerPos - mousePos).normalized;
        // float alignment = Vector2.Dot(rb.velocity.normalized, pushDirection);
        // float forceMultiplier = alignment > 0.5f ? 1.5f : 1f;
        // rb.AddForce(pushDirection * pushForce * burstForceMultiplier * forceMultiplier, ForceMode2D.Impulse);
    }

    
    void SpawnBurstParticles()
    {
        GameObject weaponParticlesHolder = shotgunWeapon.transform.GetChild(0).gameObject;

        GameObject newParticlesHolder = Instantiate(weaponParticlesHolder, weaponParticlesHolder.transform.position, weaponParticlesHolder.transform.rotation);
        newParticlesHolder.transform.parent = gameObject.transform;
        newParticlesHolder.GetComponent<ParticleSystem>().Play();
        newParticlesHolder.transform.parent = null;

        StartCoroutine(DeleteObj(newParticlesHolder, burstWeaponCooldown));
    }


    IEnumerator ResetBurstWeaponCooldown()
    {
        yield return new WaitForSeconds(burstWeaponCooldown);
        canShootShotgun = true;
    }
#endregion

    

    IEnumerator DeleteObj(GameObject objToDelete, float timer = 0)
    {
        yield return new WaitForSeconds(timer);        
        Destroy(objToDelete);
    }
}