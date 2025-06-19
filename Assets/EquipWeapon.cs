using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipWeapon : MonoBehaviour
{
    [Tooltip("Set according to the corresponding Weapon array in Player")] 
    public int arrayOnPlayer;

    // public Image image;
    // public SpriteRenderer background;
    
    
    void OnTriggerEnter2D(Collider2D player)
    {
        if (player.tag == "Player1" || player.tag == "Player2")
        {
            // image.SetActive(false);
            // background.SetActive(false);            
            Weapon targetWeapon = player.transform.GetChild(0).transform.GetChild(arrayOnPlayer)
                                    .gameObject.GetComponent<Weapon>();

            Debug.Log($"UI weapon box thing should have made {player} equip {targetWeapon}");
            player.GetComponent<PlayerFire>().SetActiveWeapon(targetWeapon);
            gameObject.SetActive(false);
        }
    }
}
