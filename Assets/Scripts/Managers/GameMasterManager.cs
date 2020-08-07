﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterManager : MonoBehaviour
{

    [SerializeField] private Texture2D _cursorSprite;
    [SerializeField] private GameObject _roomContainer;
    [SerializeField] private Player _player;
    [SerializeField] private MobGenerator _mobGenerator;
    [SerializeField] private AudioManager _audioManager;

    [SerializeField] private SceneLoader _sceneLoader;
    private MapGenerator _mapGenerator;
    private GameEventManager _gameEventManager;
    private Room[,] _rooms;
    private Room _currentRoom;
    
    private void Awake()
    {
        // Cursor.SetCursor(_cursorSprite, new Vector2(16, 16), CursorMode.Auto);
    }

    private void Start()
    {
        _gameEventManager = GameEventManager.Instance;
        _sceneLoader = SceneLoader.Instance;
        _audioManager = AudioManager.Instance;
        _mapGenerator = MapGenerator.Instance;
        _mobGenerator = MobGenerator.Instance;

        _gameEventManager.onGatewayEnter += OnGatewayEnter;
        _gameEventManager.onPlayerDeath += OnPlayerDeath;

        _rooms = _mapGenerator.GenerateMap();
        SetupStartRoom();
        PopulateRoomsWithMobs();

        _audioManager.PlayInstant(SoundType.TemplePath);
        _audioManager.PlayInstant(SoundType.Rain);

        _sceneLoader.FadeFromBlack();
    }

    // ROOM MANAGEMENT
    private void SetupStartRoom()
    {
        // Disable all rooms
        foreach (var room in _rooms)
        {
            if (room == null) { continue; }

            if (room.name.Contains("Lobby"))
            {
                _currentRoom = room;
            }
            else
            {
                room.gameObject.SetActive(false);
            }
        }

        // SetLobbyToCurrent();
        // ActivateCurrentRoom();
    }

    private void SetLobbyToCurrent()
    {
        foreach (var room in _rooms)
        {
            if (room == null) { continue; }
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

    private void PlacePlayerOnGatewayEnter(int direction)
    {

        // NORTH
        if (direction == 0)
        {
            _player.transform.position = _currentRoom.SouthSpawn;
            // print("southSpawn: " + _currentRoom._southSpawn);
        }

        // EAST
        if (direction == 1)
        {
            _player.transform.position = _currentRoom.WestSpawn;
            // print("westSpawn: " + _currentRoom._westSpawn);
        }

        // SOUTH
        if (direction == 2)
        {
            _player.transform.position = _currentRoom.NorthSpawn;
            // print("northSpawn: " + _currentRoom._northSpawn);
        }

        // WEST
        if (direction == 3)
        {
            _player.transform.position = _currentRoom.EastSpawn;
            // print("eastSpawn: " + _currentRoom._eastSpawn);
        }
    }


    // MOB SPAWNING
    private void PopulateRoomsWithMobs()
    {
        foreach (Room room in _rooms)
        {
            if (room == null) { continue; }
            if (room.name.Contains("Lobby")) { continue; }
            
            int spawnCount = room.MobCount;

            while (spawnCount > 0)
            {
                Mob newMob = _mobGenerator.Spawn(MobTypes.Slime);
                newMob.transform.SetParent(room.MobContainer);
                // newMob.GetComponent<Slime>().Color;
                newMob.gameObject.SetActive(false);
                room.Mobs.Add(newMob);

                spawnCount --;
            }
        }
    }

    // EVENTS
    private void OnGatewayEnter(int direction)
    {
        Vector2Int currLocation = _currentRoom.location;
        // StartCoroutine(_sceneLoader.Transition());

        // NORTH
        if (direction == 0)
        {
            if (currLocation.y == _mapGenerator.mapSize - 1) { return; }

            StartCoroutine(_sceneLoader.BlankCrossfade());
            DeactivateCurrentRoom();

            _currentRoom = _rooms[currLocation.y + 1, currLocation.x];

            ActivateCurrentRoom();
            PlacePlayerOnGatewayEnter(direction);
        }

        // EAST
        if (direction == 1)
        {
            if (currLocation.x == _mapGenerator.mapSize - 1) { return; }

            StartCoroutine(_sceneLoader.BlankCrossfade());
            DeactivateCurrentRoom();

            _currentRoom = _rooms[currLocation.y, currLocation.x + 1];

            ActivateCurrentRoom();
            PlacePlayerOnGatewayEnter(direction);
        }

        // SOUTH
        if (direction == 2)
        {
            if (currLocation.y == 0) { return; }

            StartCoroutine(_sceneLoader.BlankCrossfade());
            DeactivateCurrentRoom();

            _currentRoom = _rooms[currLocation.y - 1, currLocation.x];

            ActivateCurrentRoom();
            PlacePlayerOnGatewayEnter(direction);
        }

        // WEST
        if (direction == 3)
        {
            if (currLocation.x == 0) { return; }

            StartCoroutine(_sceneLoader.BlankCrossfade());
            DeactivateCurrentRoom();

            _currentRoom = _rooms[currLocation.y, currLocation.x - 1];
            
            ActivateCurrentRoom();
            PlacePlayerOnGatewayEnter(direction);
        }

        // _sceneLoader._readyOut = true;

    }

    private void OnPlayerDeath()
    {
        StartCoroutine(_sceneLoader.LoadScene((int) SceneIndex.GAME));
        StartCoroutine(_sceneLoader.LoadScene((int) SceneIndex.MAIN_MENU));
    }
}