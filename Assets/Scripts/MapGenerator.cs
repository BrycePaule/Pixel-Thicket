using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{

    [Range(0, 20)] public int mapSize;

    public Tilemap tilemap;
    public Tilemap tilemap2;
    public Tile empty;
    public Tile node;

    private static MapGenerator _instance;
    private int[,] _mapGrid;
    private Room[,] _Rooms;

    private int _nodeCap;
    private int _nodeSize;
    private int _nodeDist;


    public static MapGenerator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<MapGenerator>();
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
        if (_instance != null) { Destroy(this); }
        // DontDestroyOnLoad(this);

        _mapGrid = GenerateZeroGrid();
        _Rooms = new Room[mapSize,mapSize];

        _nodeCap = Mathf.RoundToInt(mapSize / 2) + 1;
        _nodeSize = 3;
        _nodeDist = _nodeSize + 1;
    }

    private void Start() {
        // PopulateGridWithRooms();

        // TESTING
        // TestNodeGeneration(50000);

        GenerateNodes();
        // DEBUGGING
        GenerateNodeTilemap();
        ExpandNode();

        ExpandNodeChance(33);
        ExpandNodeChance(33);
        RemoveSurroundedNodes();
        RemoveUnreachableNodes();
        // DEBUGGING
        GenerateNodeTilemap2();

        _Rooms[0, 0] = RoomGenerator.Instance.GenerateRoom();

    }

    private void PopulateGridWithRooms()
    {
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                _Rooms[y, x] = RoomGenerator.Instance.GenerateRoom();
            }
        }
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

    private void GenerateNodeTilemap()
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

    private void GenerateNodeTilemap2()
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

    private void TestNodeGeneration(int loops)
    {
        for (int i = 0; i < loops; i++)
        {
            _mapGrid = GenerateZeroGrid();
            GenerateNodes();
        }
    }
}
