#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

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


    public void PlayBurstParticles()
    {
        if (particles != null)
            particles.Play();
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