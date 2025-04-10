#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float pushForce = 0;
    [SerializeField] float burstForceMultiplier = 5f;

    [Space(10)]
    [SerializeField] ParticleSystem particles;
    [SerializeField] AudioSource soundClip;


    public float BurstMultiplier => burstForceMultiplier;
    public float BurstWeaponCooldown => shotgunSettings.burstWeaponCooldown;
    

    [Space(15)]
    [SerializeField] public bool isBurstWeapon;


    [SerializeField] IsBurstWeapon shotgunSettings;


    public void PlayBurstParticles()
    {
        if (particles != null)
            particles.Play();
    }


    #region Folds variables away for non-Streamer Weapon.
    [System.Serializable]
    public class IsBurstWeapon
    {
        [SerializeField] public float burstWeaponCooldown = 0;
        [SerializeField] float burstForceMultiplier = 1f;
    }

    #endregion
}
#region GUI for non-Streamer Weapon folding.
[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{
    SerializedProperty isBurstWeapon;
    SerializedProperty shotgunSettings;

    void OnEnable()
    {
        isBurstWeapon = serializedObject.FindProperty("isBurstWeapon");
        shotgunSettings = serializedObject.FindProperty("shotgunSettings");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, "shotgunSettings");

        if (isBurstWeapon.boolValue)
            EditorGUILayout.PropertyField(shotgunSettings, true);

        serializedObject.ApplyModifiedProperties();
    }
}
#endregion
