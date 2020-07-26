using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    [SerializeField] private GameObject _roomContainer;
    [SerializeField] private Texture2D _cursorSprite;
    [SerializeField] MapGenerator _mapGenerator;
    [SerializeField] Player _player;
    

    private void Awake()
    {
        Cursor.SetCursor(_cursorSprite, new Vector2(16, 16), CursorMode.Auto);
        _mapGenerator.PopulateGridWithRooms();
    }

    private void Start()
    {
        InitialiseFirstRoom();
    }

    private void InitialiseFirstRoom()
    {
        Transform[] rooms = _roomContainer.GetComponentsInChildren<Transform>();

        foreach (Transform room in rooms)
        {
            room.gameObject.SetActive(false);
        }

        rooms[0].gameObject.SetActive(true);
    }


}
