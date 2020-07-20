using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;



public class RoomGenerator : MonoBehaviour
{
    /*
        TILE STATES:
        1 = grass
        2 = walls
        3 = rocks
        4 = gateway
    */

    private static RoomGenerator _instance;

    public Room roomPrefab;

    [Space]

    [Range(10, 40)] public int roomLowerBound;
    [Range(10, 40)] public int roomUpperBound;

    [Space]
    public Tile[] grass;
    public Tile[] walls;
    public Tile[] pads;
    public Tile[] rocks;

    [Space]
    public Tile RedTestTile;

    private Dictionary<string, Tile> _wallTiles;

    private Tilemap baseTilemap;
    private Tilemap padTilemap;
    private Tilemap collidableTilemap;

    private int[,] _baseTilemapGrid;
    private int[,] _padTilemapGrid;
    private int[,] _collidableTilemapGrid;

    Room room1;


    // Singleton Pattern
    public static RoomGenerator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<RoomGenerator>();
                if (_instance == null)
                {
                    _instance = new RoomGenerator();
                }
            }

            return _instance;
        }
    }


    // UNITY METHODS
    private void Awake()
    {

        // Singleton Pattern
        if (_instance != null) { Destroy(this); }
        // DontDestroyOnLoad(this);

    }

    private void Start()
    {
        room1 = Instantiate(roomPrefab);

        baseTilemap = room1.transform.Find("Base Tiles").gameObject.GetComponent<Tilemap>();
        padTilemap = room1.transform.Find("Pads").gameObject.GetComponent<Tilemap>();
        collidableTilemap = room1.transform.Find("Collidables").gameObject.GetComponent<Tilemap>();

        _baseTilemapGrid = new int[room1.roomHeight, room1.roomWidth];
        _padTilemapGrid = new int[room1.roomHeight, room1.roomWidth];
        _collidableTilemapGrid = new int[room1.roomHeight, room1.roomWidth];

        _wallTiles = new Dictionary<string, Tile>();
        FillWallsDictionary();

        GenerateRoom(room1);
    }


    // NUMBER MAP GENERATION

    private void GenerateRoom(Room room)
    {

        GenerateEmptyGrid(room, _baseTilemapGrid);
        GenerateEmptyGrid(room, _padTilemapGrid);
        GenerateEmptyGrid(room, _collidableTilemapGrid);

        GenerateGrassArea(room);
        GenerateOuterWalls(room);
        GenerateInnerWalls(room, 40);
        FillEmptiesSurroundedByWalls(room);
        FillSingleSpaceGapsInWalls(room);

        GenerateGateways(room);
        RemoveGatewayBlockers(room);

        SetGridTiles(room);

    }

    private void GenerateEmptyGrid(Room room, int[,] grid)
    {
        for (int y = 0; y < room.roomHeight; y++)
        {
            for (int x = 0; x < room.roomWidth; x++)
            {
                grid[y, x] = 0;
            }
        }
    }

    private void GenerateGrassArea(Room room)
    {
        for (int y = 0; y < room.roomHeight; y++)
        {
            for (int x = 0; x < room.roomWidth; x++)
            {
                _baseTilemapGrid[y, x] = 1;
            }
        }
    }

    private void GenerateOuterWalls(Room room)
    {
        for (int y = 0; y < room.roomHeight; y++)
        {
            for (int x = 0; x < room.roomWidth; x++)
            {
                if (y == 0 | x == 0 | y == (room.roomHeight - 1) | x == (room.roomWidth - 1))
                {
                    _collidableTilemapGrid[y, x] = 2;
                }

            }
        }
    }

    private void GenerateInnerWalls(Room room, int convertThreshold = 33)
    {
        for (int y = 0; y < room.roomHeight; y++)
        {
            for (int x = 0; x < room.roomWidth; x++)
            {
                if (y == 1 | x == 1 | y == (room.roomHeight - 2) | x == (room.roomWidth - 2))
                {
                    if (_collidableTilemapGrid[y, x] == 0)
                    {
                        int roll = (int) Random.Range(0, 100);
                        if (roll <= convertThreshold)
                        {
                            _collidableTilemapGrid[y, x] = 2;
                        }
                    }
                }

            }
        }
    }

    private void GenerateGateways(Room room)
    {

        // extra steps to remove walls near gateways is to make tile selection easier
        // should be improved, but currently manually can't find a fix

        int middleX = Mathf.RoundToInt(room.roomWidth / 2);
        int middleY = Mathf.RoundToInt(room.roomHeight / 2);

        // NORTH
        _padTilemapGrid[room.roomHeight - 1, middleX] = 4;
        _padTilemapGrid[room.roomHeight - 1, middleX - 1] = 4;
        _padTilemapGrid[room.roomHeight - 1, middleX + 1] = 4;

        _collidableTilemapGrid[room.roomHeight - 2, middleX - 2] = 0;
        _collidableTilemapGrid[room.roomHeight - 2, middleX + 2] = 0;

        // SOUTH
        _padTilemapGrid[0, middleX] = 4;
        _padTilemapGrid[0, middleX - 1] = 4;
        _padTilemapGrid[0, middleX + 1] = 4;

        _collidableTilemapGrid[1, middleX - 2] = 0;
        _collidableTilemapGrid[1, middleX + 2] = 0;

        // EAST
        _padTilemapGrid[middleY, room.roomWidth - 1] = 4;
        _padTilemapGrid[middleY - 1, room.roomWidth - 1] = 4;
        _padTilemapGrid[middleY + 1, room.roomWidth - 1] = 4;

        _collidableTilemapGrid[middleY - 2, room.roomWidth - 2] = 0;
        _collidableTilemapGrid[middleY + 2, room.roomWidth - 2] = 0;

        // WEST
        _padTilemapGrid[middleY, 0] = 4;
        _padTilemapGrid[middleY - 1, 0] = 4;
        _padTilemapGrid[middleY + 1, 0] = 4;

        _collidableTilemapGrid[middleY - 2, 1] = 0;
        _collidableTilemapGrid[middleY + 2, 1] = 0;

    }

    private void FillEmptiesSurroundedByWalls(Room room)
    {
        for (int y = 0; y < room.roomHeight; y++)
        {
            for (int x = 0; x < room.roomWidth; x++)
            {

                if (_collidableTilemapGrid[y, x] == 0)
                {
                    Vector2Int loc = new Vector2Int(x, y);
                    int[,] neighbours = GridCalc.GetNeighbours(_collidableTilemapGrid, loc);
                    if (GridCalc.CheckSurrounded(neighbours, 2)) 
                    {
                        _collidableTilemapGrid[y, x] = 2;
                    }
                } 
            }
        }
    }
    
    private void FillSingleSpaceGapsInWalls(Room room)
    {

        bool changed = true;

        while (changed)
        {

            changed = false;

            for (int y = 0; y < room.roomHeight; y++)
            {
                for (int x = 0; x < room.roomWidth; x++)
                {
                    if (_collidableTilemapGrid[y, x] != 0) { continue; }

                    if (_collidableTilemapGrid[y - 1, x] == 2 & _collidableTilemapGrid[y + 1, x] == 2)
                    {
                        _collidableTilemapGrid[y, x] = 2;
                        changed = true;
                    }

                    if (_collidableTilemapGrid[y, x - 1] == 2 & _collidableTilemapGrid[y, x + 1] == 2)
                    {
                        _collidableTilemapGrid[y, x] = 2;
                        changed = true;
                    }                    
                }
            }
        }

    }

    private void RemoveGatewayBlockers(Room room)
    {
        for (int y = 0; y < room.roomHeight; y++)
        {
            for (int x = 0; x < room.roomWidth; x++)
            {
                if (_padTilemapGrid[y, x] == 4)
                {
                    _collidableTilemapGrid[y, x] = 0;
                }

                // NORTH
                if (y == room.roomHeight - 1)
                {
                    if (_padTilemapGrid[y, x] == 4)
                    {
                        _collidableTilemapGrid[y - 1, x] = 0;
                    }
                }

                // SOUTH
                if (y == 0)
                {
                    if (_padTilemapGrid[y, x] == 4)
                    {
                        _collidableTilemapGrid[y + 1, x] = 0;
                    }
                }

                // WEST
                if (x == 0)
                {
                    if (_padTilemapGrid[y, x] == 4)
                    {
                        _collidableTilemapGrid[y, x + 1] = 0;
                    }
                }

                // EAST
                if (x == room.roomWidth - 1)
                {
                    if (_padTilemapGrid[y, x] == 4)
                    {
                        _collidableTilemapGrid[y, x - 1] = 0;
                    }
                }

            }
        }
    }

    private void PrintGrid(Room room, int[,] grid)
    {
        for (int y = 0; y < room.roomHeight; y++)
        {
            for (int x = 0; x < room.roomWidth; x++)
            {
                print(grid[y, x]);
            }
        }
    }
    
    
    // TILE PLACEMENT
    private void SetGridTiles(Room room)
    {
        for (int y = 0; y < room.roomHeight; y++)
        {
            for (int x = 0; x < room.roomWidth; x++)
            {
                if (_baseTilemapGrid[y, x] == 1) 
                {
                    baseTilemap.SetTile(new Vector3Int(x - (room.roomWidth / 2), y - (room.roomHeight / 2), 0), SelectRandomGrassTile());
                }

                if (_collidableTilemapGrid[y, x] == 2) 
                {
                    collidableTilemap.SetTile(new Vector3Int(x - (room.roomWidth / 2), y - (room.roomHeight / 2), 0), SelectWallTile(new Vector2Int(x, y)));
                }

                if (_collidableTilemapGrid[y, x] == 3) 
                {
                    collidableTilemap.SetTile(new Vector3Int(x - (room.roomWidth / 2), y - (room.roomHeight / 2), 0), SelectRandomRockTile());
                }

                if (_collidableTilemapGrid[y, x] == 99) 
                {
                    collidableTilemap.SetTile(new Vector3Int(x - (room.roomWidth / 2), y - (room.roomHeight / 2), 0), RedTestTile);
                }

                if (_padTilemapGrid[y, x] == 4) 
                {
                    padTilemap.SetTile(new Vector3Int(x - (room.roomWidth / 2), y - (room.roomHeight / 2), 0), RedTestTile);
                }

            }
        }
    }

    private Tile SelectRandomGrassTile()
    {
        Tile selectedTile = grass[0];
        int roll = 0;

        // roll for shrub
        roll = (int)Random.Range(0, 100);
        if (roll <= 30)
        {
            selectedTile = grass[(int)Random.Range(4, 7)];

            // roll shrub for flower
            roll = (int)Random.Range(0, 100);
            if (roll <= 8)
            {
                selectedTile = grass[(int)Random.Range(1, 4)];
            }
        }

        return selectedTile;

    }

    private Tile SelectRandomRockTile()
    {
        return rocks[(int) Random.Range(0, rocks.Length)];
    }

    private Tile SelectWallTile(Vector2Int loc)
    {

        int[,] neighbours = GridCalc.GetNeighbours(_collidableTilemapGrid, loc);
        string gridType = GridCalc.WallComparison(neighbours);
        
        if (gridType != null)
        {
            return _wallTiles[gridType];
        }

        return _wallTiles["wall"];
    }


    // UTILITIES
    private void FillWallsDictionary()
    {



        _wallTiles.Add("wall", walls[0]);

        _wallTiles.Add("wall_top", walls[1]);
        _wallTiles.Add("wall_bottom", walls[2]);
        _wallTiles.Add("wall_bottom_grass", walls[16]);
        _wallTiles.Add("wall_left", walls[3]);
        _wallTiles.Add("wall_right", walls[4]);

        _wallTiles.Add("wall_cover", walls[5]);

        _wallTiles.Add("wall_corner_top_left", walls[6]);
        _wallTiles.Add("wall_corner_top_right", walls[7]);
        _wallTiles.Add("wall_corner_bottom_left", walls[8]);
        _wallTiles.Add("wall_corner_bottom_right", walls[9]);

        _wallTiles.Add("wall_bend_bottom_right", walls[10]);
        _wallTiles.Add("wall_bend_bottom_left", walls[11]);
        _wallTiles.Add("wall_bend_top_left", walls[12]);
        _wallTiles.Add("wall_bend_top_right", walls[13]);
        _wallTiles.Add("wall_bend_top_left_grass", walls[14]);
        _wallTiles.Add("wall_bend_top_right_grass", walls[15]);

    }
}

