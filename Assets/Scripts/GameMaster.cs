using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    [SerializeField] private Texture2D cursorSprite;

    private void Start()
    {
        Cursor.SetCursor(cursorSprite, new Vector2(16, 16), CursorMode.Auto);
    }


}
