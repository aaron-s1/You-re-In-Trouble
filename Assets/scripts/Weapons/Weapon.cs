#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// using CodeMonkey.HealthSystemCM;


public enum WeaponType { Streamer, Shotgun, Sniper }

// CONSIDER MOVING PlayerFire.cs WEAPON CHECK LOGIC TO HERE.
public class Weapon : MonoBehaviour
{

#region Variables.
    public WeaponType weaponType;

    [Space(10)]
    [SerializeField] ParticleSystem particles;
    [SerializeField] AudioSource soundClip;

    public float BurstForceMultiplier => burstSettings.burstForceMultiplier;
    public float BurstWeaponCooldown => burstSettings.burstWeaponCooldown;

    
    public WeaponType opposingPlayerWeaponType;

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
        if (particles == null)
            Debug.Log(gameObject.name + " found no particle system!!");
        
        // Review this all later and see if having Shotgun's particles on a child is even necessary
        GameObject weaponParticlesHolder = transform.GetChild(0).gameObject;

        GameObject newParticlesHolder = Instantiate(weaponParticlesHolder, weaponParticlesHolder.transform.position, weaponParticlesHolder.transform.rotation);
            newParticlesHolder.transform.parent = gameObject.transform;
        newParticlesHolder.GetComponent<ParticleSystem>().Play();
            newParticlesHolder.transform.parent = null;

        StartCoroutine(DeleteParticles(newParticlesHolder));
    }

    IEnumerator DeleteParticles(GameObject spawnedParticles)
    {
        yield return new WaitForSeconds(burstSettings.burstWeaponCooldown);
        Destroy(spawnedParticles);
    }    
#endregion


#region Particle collisions.

    // Weapon hit other Player DIRECTLY.
    void OnParticleCollision(GameObject otherPlayer)
    {
        if (otherPlayer == thisPlayer)
            return;

        // Handle damage here.
        else if (otherPlayer.tag == "Player1" || otherPlayer.tag == "Player2")
        {
            Debug.Log($"{gameObject.transform.parent.parent.gameObject}'s REGULAR collider hit {otherPlayer}");
            otherPlayer.GetComponent<PlayerFire>().Damage(0.05f);
            // otherPlayer.transform.GetChild(0).transform.GetChild(0).gameObject.Damage(1);
        }
    }


    // Weapon hit other player's Weapon.
    // Determine how particles interact with other player's Weapon here.
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

    // public void Damage(float damage) =>
    //     healthSystem.Damage(damage);

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