using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    [SerializeField] private Texture2D _cursorSprite;
    [SerializeField] private GameObject _roomContainer;
    [SerializeField] private Player _player;

    private SceneLoader _sceneLoader;
    private MapGenerator _mapGenerator;
    private Room[,] _rooms;
    private Room _currentRoom;
    
    private void Awake()
    {
        GameEventSystem.Instance.onGatewayEnter += OnGatewayEnter;

        Cursor.SetCursor(_cursorSprite, new Vector2(16, 16), CursorMode.Auto);
        _sceneLoader = SceneLoader.Instance;
        _mapGenerator = MapGenerator.Instance;
    }

    private void Start()
    {
        _rooms = _mapGenerator.GenerateMap();
        SetupStartRoom();

        _sceneLoader.FadeFromBlack();
    }

    // private void Update()
    // {
    //     print(_currentRoom.name);
    //     // print("(" + _currentRoom.x + ", " + _currentRoom.y + ")");
    // }

    // ROOM MANAGEMENT
    private void SetupStartRoom()
    {
        // Disable all rooms
        foreach (var room in _rooms)
        {
            room.gameObject.SetActive(false);
        }

        SetLobbyToCurrent();
        ActivateCurrentRoom();
    }

    private void SetLobbyToCurrent()
    {
        foreach (var room in _rooms)
        {
            if (room.name.Contains("Lobby"))
            {
                _currentRoom = room;
            }
        }
    }

    private void ActivateCurrentRoom()
    {
        _currentRoom.gameObject.SetActive(true);
    }

    private void DeactivateCurrentRoom()
    {
        _currentRoom.gameObject.SetActive(false);
    }

    // EVENTS
    private void OnGatewayEnter(int direction)
    {
        int currX = _currentRoom.x;
        int currY = _currentRoom.y;

        // NORTH
        if (direction == 0)
        {
            if (currY == _mapGenerator.mapSize - 1) { return; }

            StartCoroutine(_sceneLoader.BlankCrossfade());
            DeactivateCurrentRoom();

            _currentRoom = _rooms[currY + 1, currX];
            ActivateCurrentRoom();
            PlacePlayer(direction);
        }

        // EAST
        if (direction == 1)
        {
            if (currX == _mapGenerator.mapSize - 1) { return; }

            StartCoroutine(_sceneLoader.BlankCrossfade());
            DeactivateCurrentRoom();

            _currentRoom = _rooms[currY, currX + 1];
            ActivateCurrentRoom();
            PlacePlayer(direction);
        }

        // SOUTH
        if (direction == 2)
        {
            if (currY == 0) { return; }

            StartCoroutine(_sceneLoader.BlankCrossfade());
            DeactivateCurrentRoom();

            _currentRoom = _rooms[currY - 1, currX];
            ActivateCurrentRoom();
            PlacePlayer(direction);
        }

        // WEST
        if (direction == 3)
        {
            if (currX == 0) { return; }

            StartCoroutine(_sceneLoader.BlankCrossfade());
            DeactivateCurrentRoom();

            _currentRoom = _rooms[currY, currX - 1];
            ActivateCurrentRoom();
            PlacePlayer(direction);
        }

        print(_currentRoom.name);
    }

    private void PlacePlayer(int direction)
    {
        // NORTH
        if (direction == 0)
        {
            _player.transform.position = _currentRoom._southSpawn;
            // print("southSpawn: " + _currentRoom._southSpawn);
        }

        // EAST
        if (direction == 1)
        {
            _player.transform.position = _currentRoom._westSpawn;
            // print("westSpawn: " + _currentRoom._westSpawn);
        }

        // SOUTH
        if (direction == 2)
        {
            _player.transform.position = _currentRoom._northSpawn;
            // print("northSpawn: " + _currentRoom._northSpawn);
        }

        // WEST
        if (direction == 3)
        {
            _player.transform.position = _currentRoom._eastSpawn;
            // print("eastSpawn: " + _currentRoom._eastSpawn);
        }
    }

}
