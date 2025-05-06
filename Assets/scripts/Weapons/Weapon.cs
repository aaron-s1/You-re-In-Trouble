#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum WeaponType { Streamer, Shotgun, Sniper }

// CONSIDER MOVING PlayerFire.cs WEAPON CHECK LOGIC TO HERE.
public class Weapon : MonoBehaviour
{
    public WeaponType weaponType;
    // [SerializeField] float pushForce = 0;
    [SerializeField] Transform subEmitterSpawnPoint;
    public Vector3 subEmitterSpawnPos;

    [Space(10)]
    [SerializeField] ParticleSystem particles;
    [SerializeField] AudioSource soundClip;


    public float BurstForceMultiplier => burstSettings.burstForceMultiplier;
    public float BurstWeaponCooldown => burstSettings.burstWeaponCooldown;

    
    public WeaponType opposingPlayerWeaponType;

    [Space(15)]
    [SerializeField] public bool isBurstWeapon;


    [SerializeField] IsBurstWeapon burstSettings;

    void HandleWeaponChanged(Weapon newWeapon)
    {
        // if (newWeapon != this)
            // return;

        opposingPlayerWeaponType = newWeapon.weaponType;
        Debug.Log("Weapon.cs saw opposing player weapon's type as: " + newWeapon.weaponType);
    }

    PlayerFire playerFire;
    // For testing, multiple fake Players are in the scene, but only one has a PlayerFire that it can be controlled with.
    // Adding a temporary dirty check to account for this.
    void AddPlayerListener()
    {
        try
        {
            playerFire = transform.parent.parent.GetComponent<PlayerFire>();
            playerFire.OnWeaponChanged += HandleWeaponChanged;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    void Start() 
    {
        AddPlayerListener();
        opposingPlayerWeaponType = WeaponType.Streamer;
    }

    // void OnEnable() =>
        // AddPlayerListener();

    void OnDisable() 
    {        
        playerFire.OnWeaponChanged -= HandleWeaponChanged;
    }



    public void PlayBurstParticles()
    {
        if (particles == null)
            Debug.Log(gameObject.name + " found no particle system");
            
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


    #region Folds variables away for non-Streamer Weapon.
    [System.Serializable]
    internal class IsBurstWeapon
    {
        [SerializeField] internal float burstWeaponCooldown = 0;
        [SerializeField] internal float burstForceMultiplier = 1f;
    }

    #endregion

        void OnParticleTrigger()
        {
            // Player was hit by a particle.            
            ParticleSystem ps = GetComponent<ParticleSystem>();
            List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
            int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

            for (int i = 0; i < numEnter; i++)
            {
                ParticleSystem.Particle p = enter[i];

                HandleParticlePhysics(p);
                enter[i] = p;
            }

            // Actually update the particle changes.
            ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        }

        // Needs testing. This should work just fine, but current setup makes testing too tricky.
        // Revisit once this is actually needed.
        void HandleParticlePhysics(ParticleSystem.Particle p)
        {
            switch (opposingPlayerWeaponType)
            {
                case WeaponType.Streamer:
                    Debug.Log($"{gameObject} was hit with STREAMER particles");
                    Vector3 newDirection = Vector3.Reflect(p.velocity, Vector3.right);
                    p.velocity = newDirection;
                    break;
                case WeaponType.Shotgun:
                    Debug.Log($"{gameObject} was hit with SHOTGUN particles");
                    // Add physics to opposing particles.
                    break;
                case WeaponType.Sniper:
                    Debug.Log($"{gameObject} was hit with SNIPER particles");
                    // Add physics to opposing particles.
                    break;
            }
        }
}



#region GUI for non-Streamer Weapon folding.
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