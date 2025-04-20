#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    [SerializeField] float pushForce = 0;
    // [SerializeField] float burstForceMultiplier = 5f;

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
        Debug.Log(GetComponentInChildren<ParticleSystem>());
        try
        {           
            particles = GetComponentInChildren<ParticleSystem>();
        }
        catch (System.Exception)
        {
            throw;
        }
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