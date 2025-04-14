using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSetter : MonoBehaviour
{
    // Receiver for Mouse.

    [SerializeField] Texture2D baseCursor;
    [SerializeField] Texture2D sniperCursor;

    public bool hasSniperWeapon;

    void OnMouseEnter()
    {
        if (hasSniperWeapon)
            Cursor.SetCursor(sniperCursor, Vector2.zero, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        if (!hasSniperWeapon)
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
