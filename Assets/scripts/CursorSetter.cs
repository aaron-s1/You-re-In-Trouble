using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSetter : MonoBehaviour
{
    // Receiver for Mouse.

    [SerializeField] Texture2D baseCursor;
    [SerializeField] Texture2D sniperCursor;

    public void SetCursor(bool isSniperWeapon)
    {
        if (isSniperWeapon)
            Cursor.SetCursor(sniperCursor, Vector2.zero, CursorMode.Auto);
        else
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    // void OnMouseEnter()
    // {
    //     // if (usingSniperWeapon)
    //     //     Cursor.SetCursor(sniperCursor, Vector2.zero, CursorMode.Auto);
    // }

    // void OnMouseExit()
    // {
    //     if (!usingSniperWeapon)
    //         Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    // }
}
