using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{

    [Range(0, 20)] public int mapSize;

    public Tilemap tilemap;
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

        _mapGrid = new int[mapSize,mapSize];
        _Rooms = new Room[mapSize,mapSize];

        _nodeCap = Mathf.RoundToInt(mapSize / 2);
        _nodeSize = 3;
        _nodeDist = _nodeSize + 1;
    }

    private void Start() {
        // PopulateGridWithRooms();

        GenerateNodes();
        ExpandNode();
        // ExpandNodeChance(100);
        // ExpandNodeChance(100);
        ExpandNodeChance(50);
        ExpandNodeChance(50);
        RemoveSurroundedNodes();
        RemoveUnreachableNodes();

        _Rooms[0, 0] = RoomGenerator.Instance.GenerateRoom();

        // DEBUGGING
        GenerateNodeTilemap();
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
    private void GenerateZeroGrid()
    {
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                _mapGrid[y, x] = 0;
            }
        }
    }

    private void GenerateNodes()
    {

        print(_nodeCap);

        List<Vector2Int> nodes = new List<Vector2Int>();
        
        bool isolated = true;
        int counter = 1;

        while (counter <= _nodeCap)
        {
            Vector2Int loc = SelectRandomGridLocation(0, mapSize - 3, 2, mapSize);
            isolated = true;

            foreach (var node in nodes)
            {
                if (GridMath.PointDistance(node, loc) <= _nodeSize)
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
        }
    }

    private void ExpandNodeChance(int expandThreshold = 33)
    {
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                if (_mapGrid[y, x] != 0) { continue; }

                int[,] neighbours = GridMath.GetNeighbours(_mapGrid, new Vector2Int(x, y));
                if (!GridMath.NeighboursContainsX(neighbours, 1)) { continue; }

                
                if (Roll(33))
                {
                    _mapGrid[y, x] = 1;
                }
            }
        }
    }

    private void ExpandNode()
    {
        for (int y = mapSize - 1; y >= 0; y--)
        {
            for (int x = mapSize - 1; x >= 0; x--)
            {
                _mapGrid[y, x + 1] = 1;
                _mapGrid[y, x + 2] = 1;

                _mapGrid[y + 1, x] = 1;
                _mapGrid[y + 1, x + 1] = 1;
                _mapGrid[y + 1, x + 2] = 1;

                _mapGrid[y + 2, x] = 1;
                _mapGrid[y + 2, x + 1] = 1;
                _mapGrid[y + 2, x + 2] = 1;
            }
        }
    }

    private void RemoveSurroundedNodes()
    {
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                int[,] neighbours = GridMath.GetNeighbours(_mapGrid, new Vector2Int(x, y));

                if (GridMath.CheckSurroundedCardinal(neighbours, 1))
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
                int[,] neighbours = GridMath.GetNeighbours(_mapGrid, new Vector2Int(x, y));

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

        int x = Mathf.RoundToInt(Random.Range(lowerBoundX, upperBoundX));
        int y = Mathf.RoundToInt(Random.Range(lowerBoundY, upperBoundY));

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

}
