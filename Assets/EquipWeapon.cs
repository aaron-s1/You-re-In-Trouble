using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipWeapon : MonoBehaviour
{
    [Tooltip("Set according to the corresponding Weapon array in Player")] 
    public int arrayOnPlayer;

    
    void OnCollisionEnter2D(Collision2D playerCol)
    // void OnTriggerEnter2D(Collider2D player)
    {
        if (playerCol.gameObject.tag == "Player1" || playerCol.gameObject.tag == "Player2")
        {
            GameObject player = playerCol.gameObject;
            PlayerFire playerFire = player.GetComponent<PlayerFire>();

            GameObject targetWeaponObj = player.transform.GetChild(0).transform.GetChild(arrayOnPlayer).gameObject; // skip over a child due to weapons being children of "Weapons" empty obj
            Weapon targetWeaponScript = targetWeaponObj.GetComponent<Weapon>();

            Debug.Log($"UI weapon box thing should have made {player} equip {targetWeaponScript}");
            
            playerFire.SetActiveWeapon(targetWeaponScript);
            gameObject.SetActive(false);
        }
    }
}
