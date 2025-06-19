using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public enum WeaponType {Streamer, Sniper, Shotgun}

public class WeaponSpawner : MonoBehaviour
{
    public List<GameObject> weaponEquipUIs; // fill out manually

    void OnEnable()
    {
        Invoke("Spawn", 1f);
    }


    void Spawn() // change spawnCount to be an optional parameter later
    {
        int spawnCount = 2;
        int firstWeaponIndex = Random.Range(0, weaponEquipUIs.Count);
 
        GameObject firstWeapon = weaponEquipUIs[firstWeaponIndex];
        Instantiate(firstWeapon);

        if (spawnCount > 1)
        {
            List<GameObject> remainingWeapons = new List<GameObject>();
            remainingWeapons = weaponEquipUIs;
            remainingWeapons.RemoveAt(firstWeaponIndex);

            int secondWeaponIndex = Random.Range(0, remainingWeapons.Count);
            GameObject secondWeapon = remainingWeapons[secondWeaponIndex];
            Instantiate(secondWeapon);
        }
    }
}
