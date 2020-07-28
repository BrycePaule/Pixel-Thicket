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


    [Range(0, 100)] public int roomLowerBound;
    [Range(0, 100)] public int roomUpperBound;
    [Space(10)]

    public Room lobbyPrefab;
    public Room roomPrefab;
    [Space(10)]

    public Tile RedTestTile;
    public Tile[] grass;
    public Tile[] walls;
    public Tile[] pads;
    public Tile[] rocks;

    private static RoomGenerator _instance;
    private Dictionary<string, Tile> _wallTiles;
    private Tilemap _baseTilemap;
    private Tilemap _padTilemap;
    private Tilemap _collideTilemap;
    private int[,] _baseGrid;
    private int[,] _padGrid;
    private int[,] _collideGrid;


    // Singleton Pattern
    public static RoomGenerator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<RoomGenerator>();
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

        _wallTiles = new Dictionary<string, Tile>();
        FillWallsDictionary();

    }

    // ROOM INITIALISATION
    public Room GenerateRoom()
    {

        Room room = Instantiate(roomPrefab);

        _baseTilemap = room.transform.Find("Base Tiles").gameObject.GetComponent<Tilemap>();
        _padTilemap = room.transform.Find("Pads").gameObject.GetComponent<Tilemap>();
        _collideTilemap = room.transform.Find("Collidables").gameObject.GetComponent<Tilemap>();

        _baseGrid = new int[room.roomHeight, room.roomWidth];
        _padGrid = new int[room.roomHeight, room.roomWidth];
        _collideGrid = new int[room.roomHeight, room.roomWidth];

        GenerateZeroGrid(room, _baseGrid);
        GenerateZeroGrid(room, _padGrid);
        GenerateZeroGrid(room, _collideGrid);

        GenerateGrassArea(room, _baseGrid);
        GenerateOuterWalls(room, _collideGrid);
        GenerateInnerWalls(room, _collideGrid, 40);
        FillEmptiesSurroundedByWalls(room, _collideGrid);
        FillSingleSpaceGapsInWalls(room, _collideGrid);
        GenerateGateways(room, _padGrid, _collideGrid);
        RemoveGatewayBlockers(room, _padGrid, _collideGrid);

        SetGridTiles(room, _baseGrid, _collideGrid, _padGrid, _baseTilemap, _collideTilemap, _padTilemap);

        return room;

    }

    public Room GenerateLobbyRoom()
    {
        return Instantiate(lobbyPrefab);
    }

    // ROOM GENERATION
    private void GenerateZeroGrid(Room room, int[,] grid)
    {
        for (int y = 0; y < room.roomHeight; y++)
        {
            for (int x = 0; x < room.roomWidth; x++)
            {
                grid[y, x] = 0;
            }
        }
    }

    private void GenerateGrassArea(Room room, int[,] baseGrid)
    {
        for (int y = 0; y < room.roomHeight; y++)
        {
            for (int x = 0; x < room.roomWidth; x++)
            {
                baseGrid[y, x] = 1;
            }
        }
    }

    private void GenerateOuterWalls(Room room, int[,] wallGrid)
    {
        for (int y = 0; y < room.roomHeight; y++)
        {
            for (int x = 0; x < room.roomWidth; x++)
            {
                if (y == 0 | x == 0 | y == (room.roomHeight - 1) | x == (room.roomWidth - 1))
                {
                    wallGrid[y, x] = 2;
                }

            }
        }
    }

    private void GenerateInnerWalls(Room room, int[,] wallGrid, int conversionThreshold = 33)
    {
        for (int y = 0; y < room.roomHeight; y++)
        {
            for (int x = 0; x < room.roomWidth; x++)
            {
                if (y == 1 | x == 1 | y == (room.roomHeight - 2) | x == (room.roomWidth - 2))
                {
                    if (wallGrid[y, x] == 0)
                    {
                        int roll = (int) Random.Range(0, 100);
                        if (roll <= conversionThreshold)
                        {
                            wallGrid[y, x] = 2;
                        }
                    }
                }

            }
        }
    }

    private void GenerateGateways(Room room, int[,] padGrid, int[,] wallGrid)
    {
        // GATE TILE PLACEMENT MADE OBSELETE
        // now gateways are handled by gameobjects with colliders placed after room gen

        // extra steps to remove walls near gateways is to make tile selection easier
        // should be improved, but currently manually can't find a fix

        int middleX = Mathf.RoundToInt(room.roomWidth / 2);
        int middleY = Mathf.RoundToInt(room.roomHeight / 2);

        // NORTH
        padGrid[room.roomHeight - 1, middleX] = 4;
        padGrid[room.roomHeight - 1, middleX - 1] = 4;
        padGrid[room.roomHeight - 1, middleX + 1] = 4;

        wallGrid[room.roomHeight - 2, middleX - 2] = 0;
        wallGrid[room.roomHeight - 2, middleX + 2] = 0;

        // SOUTH
        padGrid[0, middleX] = 4;
        padGrid[0, middleX - 1] = 4;
        padGrid[0, middleX + 1] = 4;

        wallGrid[1, middleX - 2] = 0;
        wallGrid[1, middleX + 2] = 0;

        // EAST
        padGrid[middleY, room.roomWidth - 1] = 4;
        padGrid[middleY - 1, room.roomWidth - 1] = 4;
        padGrid[middleY + 1, room.roomWidth - 1] = 4;

        wallGrid[middleY - 2, room.roomWidth - 2] = 0;
        wallGrid[middleY + 2, room.roomWidth - 2] = 0;

        // WEST
        padGrid[middleY, 0] = 4;
        padGrid[middleY - 1, 0] = 4;
        padGrid[middleY + 1, 0] = 4;

        wallGrid[middleY - 2, 1] = 0;
        wallGrid[middleY + 2, 1] = 0;
    }

    private void FillEmptiesSurroundedByWalls(Room room, int[,] wallGrid)
    {
        for (int y = 0; y < room.roomHeight; y++)
        {
            for (int x = 0; x < room.roomWidth; x++)
            {
                if (wallGrid[y, x] == 0)
                {
                    Vector2Int loc = new Vector2Int(x, y);
                    int[,] neighbours = GridMath.GetNeighbours(wallGrid, loc);
                    if (GridMath.CheckSurroundedCardinal(neighbours, 2)) 
                    {
                        wallGrid[y, x] = 2;
                    }
                } 
            }
        }
    }
    
    private void FillSingleSpaceGapsInWalls(Room room, int[,] wallGrid)
    {

        bool changed = true;

        while (changed)
        {

            changed = false;

            for (int y = 0; y < room.roomHeight; y++)
            {
                for (int x = 0; x < room.roomWidth; x++)
                {
                    if (wallGrid[y, x] != 0) { continue; }

                    if (wallGrid[y - 1, x] == 2 & wallGrid[y + 1, x] == 2)
                    {
                        wallGrid[y, x] = 2;
                        changed = true;
                    }

                    if (wallGrid[y, x - 1] == 2 & wallGrid[y, x + 1] == 2)
                    {
                        wallGrid[y, x] = 2;
                        changed = true;
                    }                    
                }
            }
        }

    }

    private void RemoveGatewayBlockers(Room room, int[,] padGrid, int[,] wallGrid)
    {
        for (int y = 0; y < room.roomHeight; y++)
        {
            for (int x = 0; x < room.roomWidth; x++)
            {
                if (padGrid[y, x] == 4)
                {
                    wallGrid[y, x] = 0;
                }

                // NORTH
                if (y == room.roomHeight - 1)
                {
                    if (padGrid[y, x] == 4)
                    {
                        wallGrid[y - 1, x] = 0;
                    }
                }

                // SOUTH
                if (y == 0)
                {
                    if (padGrid[y, x] == 4)
                    {
                        wallGrid[y + 1, x] = 0;
                    }
                }

                // WEST
                if (x == 0)
                {
                    if (padGrid[y, x] == 4)
                    {
                        wallGrid[y, x + 1] = 0;
                    }
                }

                // EAST
                if (x == room.roomWidth - 1)
                {
                    if (padGrid[y, x] == 4)
                    {
                        wallGrid[y, x - 1] = 0;
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
    
    // TILE DRAWING
    private void SetGridTiles(Room room, int[,] baseGrid, int[,] wallGrid, int[,] padGrid, Tilemap baseTilemap, Tilemap wallTilemap, Tilemap padTilemap)
    {
        for (int y = 0; y < room.roomHeight; y++)
        {
            for (int x = 0; x < room.roomWidth; x++)
            {
                if (baseGrid[y, x] == 1) 
                {
                    baseTilemap.SetTile(new Vector3Int(x - (room.roomWidth / 2), y - (room.roomHeight / 2), 0), SelectRandomGrassTile());
                }

                if (wallGrid[y, x] == 2) 
                {
                    wallTilemap.SetTile(new Vector3Int(x - (room.roomWidth / 2), y - (room.roomHeight / 2), 0), SelectWallTile(new Vector2Int(x, y), wallGrid));
                }

                if (wallGrid[y, x] == 3) 
                {
                    wallTilemap.SetTile(new Vector3Int(x - (room.roomWidth / 2), y - (room.roomHeight / 2), 0), SelectRandomRockTile());
                }

                if (wallGrid[y, x] == 99) 
                {
                    wallTilemap.SetTile(new Vector3Int(x - (room.roomWidth / 2), y - (room.roomHeight / 2), 0), RedTestTile);
                }

                if (padGrid[y, x] == 4) 
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

    private Tile SelectWallTile(Vector2Int loc, int[,] wallGrid)
    {

        int[,] neighbours = GridMath.GetNeighbours(wallGrid, loc);
        string gridType = GridMath.WallComparison(neighbours);
        
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

