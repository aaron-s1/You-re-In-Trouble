using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockHealthbarPosition : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;


    void FixedUpdate()
    {
        Vector3 playerPos = player.transform.position;
        transform.position = playerPos + offset;
    }
}
