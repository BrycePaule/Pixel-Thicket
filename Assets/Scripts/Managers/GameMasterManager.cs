using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterManager : MonoBehaviour
{

    [SerializeField] private GameObject _roomContainer;
    [SerializeField] private Player _player;

    private GameEventManager _gameEventManager;
    private SceneLoader _sceneLoader;
    private AudioManager _audioManager;
    private MapGenerator _mapGenerator;
    private MobGenerator _mobGenerator;

    private Room[,] _rooms;
    private Room _currentRoom;
    
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
        SetupRooms();
        PopulateRoomsWithMobs();

        _audioManager.PlayInstant(SoundType.TemplePath);
        _audioManager.PlayInstant(SoundType.Rain);

        _sceneLoader.FadeFromBlack();
    }

    private void SetupRooms()
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

    private void PlacePlayerOnGatewayEnter(CardinalDirection direction)
    {

        // NORTH
        if (direction == CardinalDirection.North)
        {
            _player.transform.position = _currentRoom.SouthSpawn;
        }

        // EAST
        if (direction == CardinalDirection.East)
        {
            _player.transform.position = _currentRoom.WestSpawn;
        }

        // SOUTH
        if (direction == CardinalDirection.South)
        {
            _player.transform.position = _currentRoom.NorthSpawn;
        }

        // WEST
        if (direction == CardinalDirection.West)
        {
            _player.transform.position = _currentRoom.EastSpawn;
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
    private void OnGatewayEnter(CardinalDirection direction)
    {
        Vector2Int currentRoom = _currentRoom.location;

        // NORTH
        if (direction == CardinalDirection.North)
        {
            if (currentRoom.y == _mapGenerator.mapSize - 1) { return; }

            StartCoroutine(_sceneLoader.BlankCrossfade());
            DeactivateCurrentRoom();

            _currentRoom = _rooms[currentRoom.y + 1, currentRoom.x];

            ActivateCurrentRoom();
            PlacePlayerOnGatewayEnter(direction);
        }

        // EAST
        if (direction == CardinalDirection.East)
        {
            if (currentRoom.x == _mapGenerator.mapSize - 1) { return; }

            StartCoroutine(_sceneLoader.BlankCrossfade());
            DeactivateCurrentRoom();

            _currentRoom = _rooms[currentRoom.y, currentRoom.x + 1];

            ActivateCurrentRoom();
            PlacePlayerOnGatewayEnter(direction);
        }

        // SOUTH
        if (direction == CardinalDirection.South)
        {
            if (currentRoom.y == 0) { return; }

            StartCoroutine(_sceneLoader.BlankCrossfade());
            DeactivateCurrentRoom();

            _currentRoom = _rooms[currentRoom.y - 1, currentRoom.x];

            ActivateCurrentRoom();
            PlacePlayerOnGatewayEnter(direction);
        }

        // WEST
        if (direction == CardinalDirection.West)
        {
            if (currentRoom.x == 0) { return; }

            StartCoroutine(_sceneLoader.BlankCrossfade());
            DeactivateCurrentRoom();

            _currentRoom = _rooms[currentRoom.y, currentRoom.x - 1];
            
            ActivateCurrentRoom();
            PlacePlayerOnGatewayEnter(direction);
        }
    }

    private void OnPlayerDeath()
    {
        StartCoroutine(_sceneLoader.LoadScene((int) SceneIndex.GAME));
        StartCoroutine(_sceneLoader.LoadScene((int) SceneIndex.MAIN_MENU));
    }
}