#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    // [SerializeField] float pushForce = 0;
    [SerializeField] Transform subEmitterSpawnPoint;
    public Vector3 subEmitterSpawnPos;

    [Space(10)]
    [SerializeField] ParticleSystem particles;
    [SerializeField] AudioSource soundClip;


    public float BurstForceMultiplier => burstSettings.burstForceMultiplier;
    public float BurstWeaponCooldown => burstSettings.burstWeaponCooldown;
    

    [Space(15)]
    [SerializeField] public bool isBurstWeapon;


    [SerializeField] IsBurstWeapon burstSettings;

    void Start()
    {
        if (subEmitterSpawnPoint != null)
            subEmitterSpawnPos = subEmitterSpawnPoint.position;
        // Debug.Log(GetComponentInChildren<ParticleSystem>());
        // try
        // {           
            particles = GetComponentInChildren<ParticleSystem>();
        // }
        // catch (System.Exception)
        // {
        //     throw;
        // }
        // Debug.Log("weapon says its particle holder is: " + transform.GetChild(0).gameObject);
    }



    public void PlayBurstParticles()
    {
        if (particles == null)
            Debug.Log(gameObject.name + "found no particle system");
            
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
            // received a particle.
            ParticleSystem ps = GetComponent<ParticleSystem>();
            List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
            int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

            Debug.Log($"{ps.transform.parent?.name} was hit with a particle");

            for (int i = 0; i < numEnter; i++)
            {
                ParticleSystem.Particle p = enter[i];

                // Change direction
                Debug.Log("should change direction");
                Vector3 newDirection = Vector3.Reflect(p.velocity, Vector3.right); // Or any direction

                p.velocity = newDirection;

                enter[i] = p; // Make sure to assign it back
            }

            // Apply changes
            ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        }


        // void OnParticleTrigger()
        // {
        //     Debug.Log($"{gameObject}'s particles touched something");
        //     ParticleSystem ps = GetComponent<ParticleSystem>();
        //     List<ParticleSystem.Particle> triggered = new List<ParticleSystem.Particle>();

        //     int insideCount = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, triggered);

        //     for (int i = 0; i < insideCount; i++)
        //     {
        //         Debug.Log("I'm inside");
        //         ParticleSystem.Particle p = triggered[i];

        //         // Redirect particle (e.g., reflect or randomly deflect)
        //         Vector3 newDirection = Vector3.Reflect(p.velocity, Vector3.right * 100f); // Example
        //         p.velocity = newDirection * p.velocity.magnitude * 100f;

        //         // Optional: change color, size, lifetime to simulate reaction
        //         // p.startColor = Color.cyan;
        //         // p.remainingLifetime *= 0.8f;

        //         triggered[i] = p;
        //     }

        //     ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, triggered);
        // }
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