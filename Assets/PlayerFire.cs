using System.Collections;
using UnityEngine;

// Also (currently?) handles recoil movement from Firing.
public class PlayerFire : MonoBehaviour
{
    [SerializeField] GameObject streamerWeaponObject; // (perhaps make not actually a weapon later)
    [SerializeField] GameObject shotgunWeaponObject;
    [SerializeField] GameObject sniperWeaponObject;

    [SerializeField] Texture2D sniperCursor;

    Weapon activeWeapon;
    Weapon streamerWeapon;
    Weapon shotgunWeapon;
    Weapon sniperWeapon;
    
    [Space(10)]
    [SerializeField] float pushForce = 10f;
    [SerializeField] float streamerPushForceCoefficient = 0.01f;


    Rigidbody2D rb;
    bool canFireBurstWeapon = true;

    public bool sniperWeaponTest;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        streamerWeapon = streamerWeaponObject.GetComponent<Weapon>();
        shotgunWeapon = shotgunWeaponObject.GetComponent<Weapon>();
        // sniperWeapon = sniperWeapon.GetComponent<Weapon>();

        // Add a default weapon.
        activeWeapon = streamerWeapon; 
        streamerWeaponObject.SetActive(true);
    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPosition = rb.position;

        if (Input.GetMouseButton(0) && canFireBurstWeapon)
        {
            if (activeWeapon != shotgunWeapon)
            {
                activeWeapon.gameObject.SetActive(false);
                activeWeapon = shotgunWeapon;
                activeWeapon.gameObject.SetActive(true);
            }

            PerformWeaponBurst(mousePosition, playerPosition);
        }

        else if (Input.GetMouseButton(1))
            ActivateStreamer(true, playerPosition, mousePosition);

        if (Input.GetMouseButtonUp(1))
            ActivateStreamer(false, playerPosition, mousePosition);

        if (sniperWeaponTest)
        {
            // activeWeapon = sniperWeapon;
            sniperWeaponTest = false;
            gameObject.GetComponent<CursorSetter>().hasSniperWeapon = true;
        }

        // ActivateSniperCursor();
    }


    void ActivateStreamer(bool activate, Vector2 playerPos, Vector2 mousePos)
    {
        if (activate)
        {
            if (activeWeapon != streamerWeapon)
            {
                if (activeWeapon != null)
                    activeWeapon.gameObject.SetActive(false);
                                
                activeWeapon = streamerWeapon;
                rb.velocity = Vector2.zero;
            }

            if (!activeWeapon.gameObject.activeInHierarchy)
                activeWeapon.gameObject.SetActive(true);
            

            Vector2 pushDirection = (playerPos - mousePos).normalized;
            rb.AddForce(pushDirection * pushForce * streamerPushForceCoefficient, ForceMode2D.Impulse);

            // Alternate movement.
            // rb.velocity = Vector2.zero;
            // Vector2 pushDirection = (playerPosition - mousePosition).normalized;
            // rb.velocity = pushDirection * pushForce * .1f;                       
        }
        
        else
        {
            activeWeapon.GetComponent<ParticleSystem>().Stop();
            activeWeapon.gameObject.SetActive(false);            
        }
    }

#region BURST WEAPON.
    void PerformWeaponBurst(Vector2 mousePosition, Vector2 playerPosition)
    {
        if (!activeWeapon.isBurstWeapon)
            return;

        activeWeapon.PlayBurstParticles();  // SpawnBurstParticles();
        ApplyBurstMovement(mousePosition, playerPosition);

        canFireBurstWeapon = false;
        
        StartCoroutine(ResetBurstWeaponCooldown());
    }

    void ApplyBurstMovement(Vector2 mousePos, Vector2 playerPos)
    {
        Vector2 pushDirection = (playerPos - mousePos).normalized;
        rb.velocity = Vector2.zero;
        rb.AddForce(pushDirection * pushForce * activeWeapon.BurstForceMultiplier, ForceMode2D.Impulse);

        // Alternate movement.
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
    }


    IEnumerator ResetBurstWeaponCooldown()
    {
        yield return new WaitForSeconds(activeWeapon.BurstWeaponCooldown);
        canFireBurstWeapon = true;
    }
#endregion



    // void OnMouseEnter()
    // {
    //     Cursor.SetCursor(sniperCursor, hotSpot, CursorMode.Auto);
    // }

    // void OnMouseExit()
    // {
    //     // Pass 'null' to the texture parameter to use the default system cursor.
    //     Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    // }

    // void ActivateSniperCursor()
    // {

    // }

    

    IEnumerator DeleteObj(GameObject objToDelete, float timer = 0)
    {
        yield return new WaitForSeconds(timer);        
        Destroy(objToDelete);
    }
}