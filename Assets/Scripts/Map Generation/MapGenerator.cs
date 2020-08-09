using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{

    [Range(1, 20)] public int mapSize;

    public Tilemap tilemap;
    public Tilemap tilemap2;
    public Tile empty;
    public Tile node;
    public Transform roomContainer;

    private int[,] _mapGrid;
    private Room[,] _Rooms;

    private int _nodeCap;
    private int _nodeSize;
    private int _nodeDist;

    private static MapGenerator _instance;

    public static MapGenerator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MapGenerator>();
                if (_instance == null)
                {
                    _instance = new MapGenerator();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null) { Destroy(this.gameObject); }

        _nodeCap = Mathf.RoundToInt(mapSize / 2) + 1;
        _nodeSize = 3;
        _nodeDist = _nodeSize + 1;

    }

    private void Start() {

        // TESTING (runs map gen (just the locations) X number of times)
        // TESTERNodeGeneration(1000);

        // GenerateNodes();
        // // DEBUGGING
        // GenerateNodeTilemap();
        // ExpandNode();

        // ExpandNodeChance(33);
        // ExpandNodeChance(33);
        // RemoveSurroundedNodes();
        // RemoveUnreachableNodes();
        // // DEBUGGING
        // GenerateNodeTilemap2();

        // _Rooms[0, 0] = RoomGenerator.Instance.GenerateRoom();

    }

    // GRID GENERATION
    private int[,] GenerateZeroGrid()
    {

        int[,] grid = new int[mapSize, mapSize];

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                grid[y, x] = 0;
            }
        }

        return grid;
    }

    private void GenerateNodes()
    {
        // TESTS - 2/100
        // TESTS - 16/1000

        List<Vector2Int> nodes = new List<Vector2Int>();
        
        bool isolated = true;
        int counter = 1;
        int attempts = 0;

        while (counter <= _nodeCap)
        {
            Vector2Int loc = SelectRandomGridLocation(0, mapSize - _nodeSize, 0, mapSize - _nodeSize);
            isolated = true;

            foreach (var node in nodes)
            {
                if (GridMath.PointDistance(node, loc) < _nodeDist)
                {
                    isolated = false;
                }
            }

            if (isolated)
            {
                _mapGrid[loc.y, loc.x] = 1;
                nodes.Add(loc);
                counter++;
            }

            attempts++;
            if (attempts > 10000) 
            { 
            print("COULDN'T FINISH CREATING NODES"); 
            break; 
            }
        }
    }

    private void ExpandNode()
    {
        int[,] _newMapGrid = new int[mapSize, mapSize];

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                if (_mapGrid[y, x] != 1) { continue; }
               
                for (int j = 0; j < _nodeSize; j++)
                {
                    for (int i = 0; i < _nodeSize; i++)
                    {
                        _newMapGrid[y + j, x + i] = 1;
                    }
                }
            }
        }

        _mapGrid = _newMapGrid;

    }

    private void ExpandNodeChance(int expandThreshold = 33)
    {
        int[,] _newMapGrid = _mapGrid;

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                if (_mapGrid[y, x] != 0) { continue; }

                int[,] neighbours = GridMath.GetNeighbours(_mapGrid, new Vector2Int(x, y));
                if (!GridMath.NeighboursContainsX(neighbours, 1)) { continue; }

                
                if (Roll(33))
                {
                    _newMapGrid[y, x] = 1;
                }
            }
        }

        _mapGrid = _newMapGrid;
    }

    private void RemoveSurroundedNodes()
    {
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                int[,] neighbours = GridMath.GetNeighbours(_mapGrid, new Vector2Int(x, y), 1);

                if (GridMath.CheckSurroundedFully(neighbours, 1))
                {
                    _mapGrid[y, x] = 0;
                }
            }
        }
    }

    private void RemoveUnreachableNodes()
    {
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                int[,] neighbours = GridMath.GetNeighbours(_mapGrid, new Vector2Int(x, y), 0);

                if (GridMath.CheckSurroundedCardinal(neighbours, 0))
                {
                    _mapGrid[y, x] = 0;
                }
            }
        }
    }

    private int[] CalculateGatesNeeded(Vector2Int loc)
    {
        int[] gates = {0, 0, 0, 0};
        int[,] neighbours = GridMath.GetNeighbours(_mapGrid, new Vector2Int(loc.x, loc.y), 0);

        if (neighbours[0, 1] == 1) { gates[0] = 1; }
        if (neighbours[1, 2] == 1) { gates[1] = 1; }
        if (neighbours[2, 1] == 1) { gates[2] = 1; }
        if (neighbours[1, 0] == 1) { gates[3] = 1; }

        return gates;
    }

    // UTILITIES
    private Vector2Int SelectRandomGridLocation(int lowerBoundX = 0, int upperBoundX = 0, int lowerBoundY = 0, int upperBoundY = 0)
    {
        if (upperBoundX == 0) { upperBoundX = mapSize; }
        if (upperBoundY == 0) { upperBoundY = mapSize; }

        int x = Mathf.RoundToInt(Random.Range(lowerBoundX, upperBoundX + 1));
        int y = Mathf.RoundToInt(Random.Range(lowerBoundY, upperBoundY + 1));

        return new Vector2Int(x, y);
    }

    private bool Roll(int successChance = 50)
    {
        int roll = Mathf.RoundToInt(Random.Range(0, 100));

        if (roll <= successChance)
        {
            return true;
        }

        return false;
    }

    private void TESTERGenerateNodeTilemap()
    {
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                if (_mapGrid[y, x] == 1) 
                {
                    tilemap.SetTile(new Vector3Int(x - (mapSize / 2), y - (mapSize / 2), 0), node);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(x - (mapSize / 2), y - (mapSize / 2), 0), empty);
                }
            }
        }
    }

    private void TESTERGenerateNodeTilemap2()
    {
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                if (_mapGrid[y, x] == 1) 
                {
                    tilemap2.SetTile(new Vector3Int(x - (mapSize / 2), y - (mapSize / 2), 0), node);
                }
                else
                {
                    tilemap2.SetTile(new Vector3Int(x - (mapSize / 2), y - (mapSize / 2), 0), empty);
                }
            }
        }
    }

    private void TESTERNodeGeneration(int loops)
    {
        for (int i = 0; i < loops; i++)
        {
            if ((i + 1) % 1000 == 0) { print("Test #" + (i + 1)); }

            _mapGrid = GenerateZeroGrid();
            GenerateNodes();
        }
    }

    private void TESTERPrintMapGrid()
    {
        foreach (var roomValue in _mapGrid)
        {
            print(roomValue);
        }
    }

    // MAP INITIALISATION
    public Room[,] GenerateMap()
    {
        _Rooms = new Room[mapSize,mapSize];

        _mapGrid = GenerateZeroGrid();
        GenerateNodes();
        ExpandNode();
        ExpandNodeChance(33);
        ExpandNodeChance(33);
        RemoveSurroundedNodes();
        RemoveUnreachableNodes();

        bool placedLobby = false;
        bool placedEnd = false;

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {

                if (_mapGrid[y, x] == 0) { continue; }

                int[] gates = CalculateGatesNeeded(new Vector2Int(x, y));
                
                if (!placedLobby)
                {
                    Room room = RoomGenerator.Instance.GenerateLobbyRoom(gates);
                    room.Gates = gates;
                    room.location = new Vector2Int(x, y);
                    room.name = "Room (" + x + ", " + y + ") - Lobby";
                    room.transform.SetParent(roomContainer);
                    _Rooms[y, x] = room;

                    placedLobby = true;
                }

                else 
                {
                    Room room = RoomGenerator.Instance.GenerateRoom(gates);
                    room.Gates = gates;
                    room.location = new Vector2Int(x, y);
                    room.name = "Room (" + x + ", " + y + ")";
                    room.transform.SetParent(roomContainer);
                    _Rooms[y, x] = room;
                }

            }
        }

        for (int y = mapSize - 1; y > 0; y--)
        {
            if (placedEnd) { break; }

            for (int x = mapSize - 1; x > 0; x--)
            {
                if (placedEnd) { break; }
                if (_mapGrid[y, x] == 0) { continue; }

                Destroy(_Rooms[y, x].gameObject);

                int[] gates = CalculateGatesNeeded(new Vector2Int(x, y));

                Room room = RoomGenerator.Instance.GenerateEndRoom(gates);
                room.Gates = gates;
                room.location = new Vector2Int(x, y);
                room.name = "Room (" + x + ", " + y + ") - End";
                room.transform.SetParent(roomContainer);
                _Rooms[y, x] = room;

                placedEnd = true;
            }
        }

        return _Rooms;
    }
}
