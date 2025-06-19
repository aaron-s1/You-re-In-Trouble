using System.Collections;
using UnityEngine;
using CodeMonkey.HealthSystemCM;

// Also (currently?) handles recoil physics from Firing.
public class PlayerFire : MonoBehaviour
{
#region Events.

    public delegate void WeaponChangedHandler(PlayerFire player, Weapon newWeapon);
    public static event WeaponChangedHandler OnPlayerWeaponChanged;
  
#endregion

#region Variables.

    [SerializeField] GameObject streamerWeaponObject; // (perhaps make not actually a weapon (that damages) later)
    [SerializeField] GameObject shotgunWeaponObject;
    [SerializeField] GameObject sniperWeaponObject;

    public Weapon activeWeapon; // temp  public.
    Weapon streamerWeapon;
    Weapon shotgunWeapon;
    Weapon sniperWeapon;
    
    [Space(10)]
    [SerializeField] float pushForce = 10f;
    [SerializeField] float streamerPushForceCoefficient = 0.01f;

    Rigidbody2D rb;
    bool canFireBurstWeapon = true;

    public bool sniperWeaponTest;

    CursorSetter cursorSetter;

#endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        streamerWeapon = streamerWeaponObject.GetComponent<Weapon>();
        shotgunWeapon = shotgunWeaponObject.GetComponent<Weapon>();
        cursorSetter = GetComponent<CursorSetter>();

        // Add a default weapon.
        SetActiveWeapon(streamerWeapon);
    }

    void Update()
    {
        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPosition = rb.position;

        if (Input.GetMouseButton(0) && canFireBurstWeapon)
        {
            SetActiveWeapon(shotgunWeapon);
            PerformWeaponBurst(mousePosition, playerPosition);
        }

        else if (Input.GetMouseButton(1))
            ActivateStreamer(true, playerPosition, mousePosition);

        if (Input.GetMouseButtonUp(1))
            ActivateStreamer(false, playerPosition, mousePosition);

        cursorSetter.SetCursor(sniperWeaponTest);
    }


#region WEAPONS.
    public void OutsideWeaponEquip(int arrayIndex)
    {
        
    }

    public void SetActiveWeapon(Weapon newWeapon)
    {
        if (activeWeapon == newWeapon)
        {
            if (!activeWeapon.gameObject.activeSelf)
                activeWeapon.gameObject.SetActive(true);
            else
                return;
        }

        else if (activeWeapon != null)
            activeWeapon.gameObject.SetActive(false);
        
        activeWeapon = newWeapon;
        activeWeapon.gameObject.SetActive(true);
        // activeWeapon.gameObject.SetActive(true);

        OnPlayerWeaponChanged?.Invoke(this, newWeapon);
    }


    void ActivateStreamer(bool activate, Vector2 playerPos, Vector2 mousePos)
    {
        if (activate)
        {
            SetActiveWeapon(streamerWeapon);

            rb.velocity = Vector2.zero;
            MovePlayer(playerPos, mousePos);
            // AlternateMove(playerPos, mousePos);
        }
        
        else
        {
            activeWeapon.GetComponent<ParticleSystem>().Stop();
            activeWeapon.gameObject.SetActive(false);
        }
    }


    void PerformWeaponBurst(Vector2 mousePosition, Vector2 playerPosition)
    {
        if (!activeWeapon.isBurstWeapon)
            return;

        // activeWeapon.GetCompomnent<Weapon>().StartCoroutine(PlayBurstParticles());
        StartCoroutine(activeWeapon.PlayBurstParticles());

        // Debug.Break();
        ApplyBurstMovement(mousePosition, playerPosition);

        canFireBurstWeapon = false;
        
        StartCoroutine(ResetBurstWeaponCooldown());
    }

    
    IEnumerator ResetBurstWeaponCooldown()
    {
        yield return new WaitForSeconds(activeWeapon.BurstWeaponCooldown);
        canFireBurstWeapon = true;
    }

#endregion

#region PLAYER MOVEMENT.
    void ApplyBurstMovement(Vector2 mousePos, Vector2 playerPos)
    {
        Vector2 pushDirection = (playerPos - mousePos).normalized;
        rb.velocity = Vector2.zero;
        rb.AddForce(pushDirection * pushForce * activeWeapon.BurstForceMultiplier, ForceMode2D.Impulse);

        // Alternate movement:
            // Vector2 pushDirection = (playerPos - mousePos).normalized;
            // float alignment = Vector2.Dot(rb.velocity.normalized, pushDirection);
            // float forceMultiplier = alignment > 0.5f ? 1.5f : 1f;
            // rb.AddForce(pushDirection * pushForce * burstForceMultiplier * forceMultiplier, ForceMode2D.Impulse);
    }

    void MovePlayer(Vector2 playerPos, Vector2 mousePos)
    {
        Vector2 pushDirection = (playerPos - mousePos).normalized;
        rb.AddForce(pushDirection * pushForce * streamerPushForceCoefficient, ForceMode2D.Impulse);
    }

    // void AlternateMove(Vector2 playerPos, Vector2 mousePos)
    // {
    //     rb.velocity = Vector2.zero;
    //     Vector2 pushDirection = (playerPos - mousePos).normalized;
    //     rb.velocity = pushDirection * pushForce * .1f;        
    // }            

#endregion



#region SNIPER WEAPON TESTING.
    // void OnMouseEnter()
    // {
    //     Cursor.SetCursor(sniperCursor, hotSpot, CursorMode.Auto);
    // }

    // void OnMouseExit()
    // {
    //     // Pass 'null' to the texture parameter to use the default system cursor.
    //     Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    // }
    
#endregion

    IEnumerator DeleteObjAfter(GameObject objToDelete, float timer = 0)
    {
        yield return new WaitForSeconds(timer);        
        Destroy(objToDelete);
    }
}