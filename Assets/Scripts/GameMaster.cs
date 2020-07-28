using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    [SerializeField] private Texture2D _cursorSprite;

    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private GameObject _roomContainer;
    [SerializeField] private Player _player;
    

    private void Awake()
    {
        Cursor.SetCursor(_cursorSprite, new Vector2(16, 16), CursorMode.Auto);
    }

    private void Start()
    {
        _mapGenerator.GenerateMap();
        InitialiseRooms();
        // _player.transform.position = new Vector3(0.5f, 0f, 0f);
    }

    private void InitialiseRooms()
    {
        Transform[] rooms = _roomContainer.GetComponentsInChildren<Transform>();

        // Disable all rooms
        foreach (Transform room in rooms)
        {
            room.gameObject.SetActive(false);
        }

        // Enable lobby room
        rooms[0].gameObject.SetActive(true);
        Transform[] lobbyTransforms = rooms[10].GetComponentsInChildren<Transform>(true);
        foreach (Transform trans in lobbyTransforms)
        {
            trans.gameObject.SetActive(true);
        }

    }


}
