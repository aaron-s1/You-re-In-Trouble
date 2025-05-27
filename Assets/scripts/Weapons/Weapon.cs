#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.HealthSystemCM;


public enum WeaponType { Streamer, Shotgun, Sniper }

// CONSIDER MOVING PlayerFire.cs WEAPON CHECK LOGIC TO HERE.
public class Weapon : MonoBehaviour
{

#region Variables.
    public WeaponType weaponType;

    [Space(10)]
    [SerializeField] float particleDamage = 0.05f;
    [SerializeField] ParticleSystem particles;
    [SerializeField] AudioSource soundClip;

    public float BurstForceMultiplier => burstSettings.burstForceMultiplier;
    public float BurstWeaponCooldown => burstSettings.burstWeaponCooldown;

    
    public WeaponType opposingPlayerWeaponType;
    public HealthSystemComponent opposingPlayerHealth;

    [Space(15)]
    [SerializeField] public bool isBurstWeapon;

    [Space(10)]
    [SerializeField] IsBurstWeapon burstSettings;

    // HealthSystem healthSystem;

    PlayerFire playerFire;

    GameObject thisPlayer;

#endregion


#region Listeners for Weapon changes.
    void OnEnable() {
        AddPlayerListener();
        thisPlayer = gameObject.transform.parent.parent.gameObject;
        particles = GetComponent<ParticleSystem>();
        // healthSystem = new HealthSystem(100);
    }

    void OnDisable() =>
        PlayerFire.OnPlayerWeaponChanged -= HandleWeaponChanged;


    void AddPlayerListener()
    {
        try
        {
            playerFire = transform.parent.parent.GetComponent<PlayerFire>();
            PlayerFire.OnPlayerWeaponChanged += HandleWeaponChanged;
        }
        catch (System.Exception)
        {
            Debug.Log("AddPlayerListener() failed!");
            throw;
        }
    }
 
    void HandleWeaponChanged(PlayerFire changingPlayer, Weapon newWeapon)
    {
        if (changingPlayer == this.playerFire)
            return;

        opposingPlayerWeaponType = newWeapon.weaponType;
        Debug.Log($"{changingPlayer.gameObject}'s HandleWeaponChanged() triggered. Weapon type is now: {newWeapon.weaponType}");
    }
#endregion


#region Particle playing.
public void PlayBurstParticles()
            {
            // ParticleSystem original = GetComponent<ParticleSystem>();

            // Clone just the ParticleSystem component, not the full GameObject
            // GameObject newGO = new GameObject("Duplicate Shotgun Particles");
            // ParticleSystem psCopy = Instantiate(original, newGO.transform);
            // psCopy.transform.position = transform.position;
            // psCopy.transform.rotation = transform.rotation;
            particles.Stop();
            particles.Play();            
            // StartCoroutine(DeleteBurstWeaponParticles(newGO, psCopy.main.duration));
            }

    // public void PlayBurstParticles()
    // {
    //     // return;
    //     // Debug.Log("should not be reached");        
    //     if (particles == null)
    //         Debug.Log($"{gameObject} found no particle system!!");
    //     // Debug.Break();
        
    //     ParticleSystem newParticleShot = GetComponent<ParticleSystem>();

    //     // GameObject weaponParticlesHolder = transform.GetChild(0).gameObject;
    //     GameObject newParticlesHolder = Instantiate (new GameObject(), transform.position, transform.rotation);
    //     Debug.Log("NAME::" + newParticlesHolder.name);
    //     newParticlesHolder.SetActive(true);
    //     newParticlesHolder.name = "Duplicate Shotgun Particles";

    //     ParticleSystem duplicateParticles = Instantiate(GetComponent<ParticleSystem>(), newParticlesHolder.transform.position, newParticlesHolder.transform.rotation);
    //     newParticlesHolder.GetComponent<ParticleSystem>().Play();

    //     // Debug.Break();

    //     StartCoroutine(DeleteBurstWeaponParticles(newParticlesHolder));
    // }
// IEnumerator DeleteBurstWeaponParticles(GameObject go, float delay)
// {
//     yield return new WaitForSeconds(delay);
//     Destroy(go);
// }
    IEnumerator DeleteBurstWeaponParticles(ParticleSystem particless)
    {
        yield return new WaitForSeconds(burstSettings.burstWeaponCooldown);
        particless.Stop();
        // Destroy(particlesHolder);
    }    
#endregion


#region Particle collisions.

    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    // Weapon's particle hit other Player DIRECTLY!!.
    void OnParticleCollision(GameObject otherPlayer)
    {
        if (otherPlayer == thisPlayer)
            return;

        if (otherPlayer.tag == "Player1" || otherPlayer.tag == "Player2")
            ParticlesApplyDamage(otherPlayer);
    }


    // Weapon hit OTHER player's Weapon!!
    // For determining what Weapon's particles do when hitting other player's Weapon's trigger (NOT its particles!!)
    void OnParticleTrigger()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];

            HandleParticlePhysics(ref p);
            enter[i] = p;
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    }

    void ParticlesApplyDamage(GameObject otherPlayer)
    {
        if (opposingPlayerHealth == null)
            try
            {
                opposingPlayerHealth = otherPlayer.GetComponent<HealthSystemComponent>();
            }
            catch (System.Exception)
            {
                Debug.Log($"{gameObject} found no enemy Health System!!!");
                throw;
            }

        // find particle's generic velocity to create another damage factor
        int numCollisionEvents = ParticlePhysicsExtensions.GetCollisionEvents(GetComponent<ParticleSystem>(), otherPlayer, collisionEvents);
        if (numCollisionEvents == 0)
            return;
        float velocityDamageMultiplier = collisionEvents[0].velocity.magnitude;


        float totalDamage = particleDamage * velocityDamageMultiplier;
        opposingPlayerHealth.Damage(totalDamage);
    }

    void HandleParticlePhysics(ref ParticleSystem.Particle p)
    {
        switch (this.weaponType)
        {
            case WeaponType.Streamer:
                Debug.Log($"{gameObject}'s TRIGGER struck something with its STREAMER particles!");
                Vector3 newDirection = Vector3.Reflect(p.velocity, Vector3.right);
                p.velocity = newDirection;
                break;

            case WeaponType.Shotgun:
                Debug.Log($"{gameObject} TRIGGER struck something with its SHOTGUN particles!");                
                break;

            case WeaponType.Sniper:
                Debug.Log($"{gameObject} TRIGGER struck something with its SNIPER particles!");
                break;
        }
    }
#endregion


#region Burst Weapon variables.
    [System.Serializable]
    internal class IsBurstWeapon
    {
        [SerializeField] internal float burstWeaponCooldown = 0;
        [SerializeField] internal float burstForceMultiplier = 1f;
    }
#endregion


#region Folds variables away if Weapon is not of type Streamer.
    [CustomEditor(typeof(Weapon))]
    public class WeaponEditor : Editor
    {
        SerializedProperty isBurstWeapon;
        SerializedProperty burstSettings;

        void OnEnable()
        {
            isBurstWeapon = serializedObject.FindProperty("isBurstWeapon");
            burstSettings = serializedObject.FindProperty("burstSettings");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "burstSettings");

            if (isBurstWeapon.boolValue)
                EditorGUILayout.PropertyField(burstSettings, true);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endregion
}