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

    public Room LobbyPrefab;
    public Room RoomPrefab;
    [Space(10)]

    public Tile RedTestTile;
    public Tile[] Grass;
    public Tile[] Walls;
    public Tile[] Pads;
    public Tile[] Rocks;

    private Dictionary<string, Tile> _wallTiles;
    private List<Vector2> _spawnLocations;
    private Tilemap _baseTilemap;
    private Tilemap _padTilemap;
    private Tilemap _collideTilemap;
    private int[,] _baseGrid;
    private int[,] _padGrid;
    private int[,] _collideGrid;
    private int[,] _spawnGrid;


    private static RoomGenerator _instance;

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

        if (_instance != null) { Destroy(this.gameObject); }

        _wallTiles = new Dictionary<string, Tile>();

    }

    private void Start()
    {
        FillWallsDictionary();
    }

    // ROOM INITIALISATION
    public Room GenerateRoom(int[] gates)
    {
        Room room = Instantiate(RoomPrefab);

        _baseTilemap = room.transform.Find("Base Tiles").gameObject.GetComponent<Tilemap>();
        _padTilemap = room.transform.Find("Pads").gameObject.GetComponent<Tilemap>();
        _collideTilemap = room.transform.Find("Collidables").gameObject.GetComponent<Tilemap>();

        _baseGrid = new int[room.Height, room.Width];
        _padGrid = new int[room.Height, room.Width];
        _collideGrid = new int[room.Height, room.Width];
        _spawnGrid = new int[room.Height, room.Width];

        // GRID GEN
        GenerateZeroGrid(room, _baseGrid);
        GenerateZeroGrid(room, _padGrid);
        GenerateZeroGrid(room, _collideGrid);
        GenerateZeroGrid(room, _spawnGrid);

        // TILE GEN
        GenerateGrassArea(room, _baseGrid);
        GenerateOuterWalls(room, _collideGrid);
        GenerateInnerWalls(room, _collideGrid, 40);
        FillEmptiesSurroundedByWalls(room, _collideGrid);
        FillSingleSpaceGapsInWalls(room, _collideGrid);
        GenerateGateways(room, _padGrid, _collideGrid, gates);
        RemoveGatewayBlockers(room, _padGrid, _collideGrid);
        SetGridTiles(room, _baseGrid, _collideGrid, _padGrid, _baseTilemap, _collideTilemap, _padTilemap);

        // MOB GEN
        GenerateSpawnGrid(room);
        CalculateMobSlots(room);

        return room;
    }

    public Room GenerateLobbyRoom(int[] gates)
    {
        Room room = Instantiate(LobbyPrefab);

        _baseTilemap = room.transform.Find("Base Tiles").gameObject.GetComponent<Tilemap>();
        _padTilemap = room.transform.Find("Pads").gameObject.GetComponent<Tilemap>();
        _collideTilemap = room.transform.Find("Collidables").gameObject.GetComponent<Tilemap>();

        _baseGrid = new int[room.Height, room.Width];
        _padGrid = new int[room.Height, room.Width];
        _collideGrid = new int[room.Height, room.Width];
        _spawnGrid = new int[room.Height, room.Width];

        GenerateGateways(room, _padGrid, _collideGrid, gates);

        room.MobCount = 0;

        return room;
    }

    // ROOM GENERATION
    private void GenerateZeroGrid(Room room, int[,] grid)
    {
        for (int y = 0; y < room.Height; y++)
        {
            for (int x = 0; x < room.Width; x++)
            {
                grid[y, x] = 0;
            }
        }
    }

    private void GenerateGrassArea(Room room, int[,] baseGrid)
    {
        for (int y = 0; y < room.Height; y++)
        {
            for (int x = 0; x < room.Width; x++)
            {
                baseGrid[y, x] = 1;
            }
        }
    }

    private void GenerateOuterWalls(Room room, int[,] wallGrid)
    {
        for (int y = 0; y < room.Height; y++)
        {
            for (int x = 0; x < room.Width; x++)
            {
                if (y == 0 | x == 0 | y == (room.Height - 1) | x == (room.Width - 1))
                {
                    wallGrid[y, x] = 2;
                }

            }
        }
    }

    private void GenerateInnerWalls(Room room, int[,] wallGrid, int conversionThreshold = 33)
    {
        for (int y = 0; y < room.Height; y++)
        {
            for (int x = 0; x < room.Width; x++)
            {
                if (y == 1 | x == 1 | y == (room.Height - 2) | x == (room.Width - 2))
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

    private void GenerateGateways(Room room, int[,] padGrid, int[,] wallGrid, int[] gates)
    {
        // GATE TILE PLACEMENT MADE OBSELETE
        // now gateways are handled by gameobjects with colliders placed after room gen

        // extra steps to remove walls near gateways is to make tile selection easier
        // should be improved, but currently manually can't find a fix

        int middleX = Mathf.RoundToInt(room.Width / 2);
        int middleY = Mathf.RoundToInt(room.Height / 2);

        // NORTH
        if (gates[0] == 1)
        {
            padGrid[room.Height - 1, middleX] = 4;
            padGrid[room.Height - 1, middleX - 1] = 4;
            padGrid[room.Height - 1, middleX + 1] = 4;

            wallGrid[room.Height - 1, middleX] = 0;
            wallGrid[room.Height - 1, middleX - 1] = 0;
            wallGrid[room.Height - 1, middleX + 1] = 0;

            wallGrid[room.Height - 2, middleX - 2] = 0;
            wallGrid[room.Height - 2, middleX + 2] = 0;
        }

        // EAST
        if (gates[1] == 1)
        {
            padGrid[middleY, room.Width - 1] = 4;
            padGrid[middleY - 1, room.Width - 1] = 4;
            padGrid[middleY + 1, room.Width - 1] = 4;

            wallGrid[middleY, room.Width - 1] = 0;
            wallGrid[middleY - 1, room.Width - 1] = 0;
            wallGrid[middleY + 1, room.Width - 1] = 0;

            wallGrid[middleY - 2, room.Width - 2] = 0;
            wallGrid[middleY + 2, room.Width - 2] = 0;
        }

        // SOUTH
        if (gates[2] == 1)
        {
            padGrid[0, middleX] = 4;
            padGrid[0, middleX - 1] = 4;
            padGrid[0, middleX + 1] = 4;

            wallGrid[0, middleX] = 0;
            wallGrid[0, middleX - 1] = 0;
            wallGrid[0, middleX + 1] = 0;

            wallGrid[1, middleX - 2] = 0;
            wallGrid[1, middleX + 2] = 0;

        }
        
        // WEST
        if (gates[3] == 1)
        {
            padGrid[middleY, 0] = 4;
            padGrid[middleY - 1, 0] = 4;
            padGrid[middleY + 1, 0] = 4;

            wallGrid[middleY, 0] = 0;
            wallGrid[middleY - 1, 0] = 0;
            wallGrid[middleY + 1, 0] = 0;

            wallGrid[middleY - 2, 1] = 0;
            wallGrid[middleY + 2, 1] = 0;
        }

        
    }

    private void FillEmptiesSurroundedByWalls(Room room, int[,] wallGrid)
    {
        for (int y = 0; y < room.Height; y++)
        {
            for (int x = 0; x < room.Width; x++)
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

            for (int y = 0; y < room.Height; y++)
            {
                for (int x = 0; x < room.Width; x++)
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
        for (int y = 0; y < room.Height; y++)
        {
            for (int x = 0; x < room.Width; x++)
            {
                if (padGrid[y, x] == 4)
                {
                    wallGrid[y, x] = 0;
                }

                // NORTH
                if (y == room.Height - 1)
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
                if (x == room.Width - 1)
                {
                    if (padGrid[y, x] == 4)
                    {
                        wallGrid[y, x - 1] = 0;
                    }
                }

            }
        }
    }

    private void GenerateSpawnGrid(Room room)
    {
        for (int y = 0; y < room.Height; y++)
        {
            for (int x = 0; x < room.Width; x++)
            {
                if (_collideGrid[y, x] != 0 | _padGrid[y, x] != 0) 
                {
                    _spawnGrid[y, x] = 0;
                    continue;
                }                

                _spawnGrid[y, x] = 1;
            }
        }
    }

    private void CalculateMobSlots(Room room)
    {
        int spawnTiles = 0;
        _spawnLocations = new List<Vector2>();

        for (int y = 0; y < room.Height; y++)
        {
            for (int x = 0; x < room.Width; x++)
            {
                if (_spawnGrid[y, x] != 1) { continue; } 
                
                // locations are offset by 50% of room width / height to centre them around the middle
                // locations are also already in world coords because each square in the grid is 1 to 1 size
                _spawnLocations.Add(new Vector2(x - Mathf.FloorToInt(room.Width / 2), y - Mathf.FloorToInt(room.Height / 2)));
                spawnTiles++;
            }
        }
        
        room.MobCount = Mathf.FloorToInt(spawnTiles * 0.02f);
        room.MobSpawnLocations = _spawnLocations;
    }

    private void PrintGrid(Room room, int[,] grid)
    {
        for (int y = 0; y < room.Height; y++)
        {
            for (int x = 0; x < room.Width; x++)
            {
                print(grid[y, x]);
            }
        }
    }
    
    // TILE DRAWING
    private void SetGridTiles(Room room, int[,] baseGrid, int[,] wallGrid, int[,] padGrid, Tilemap baseTilemap, Tilemap wallTilemap, Tilemap padTilemap)
    {
        for (int y = 0; y < room.Height; y++)
        {
            for (int x = 0; x < room.Width; x++)
            {
                if (baseGrid[y, x] == 1) 
                {
                    baseTilemap.SetTile(new Vector3Int(x - (room.Width / 2), y - (room.Height / 2), 0), SelectRandomGrassTile());
                }

                if (wallGrid[y, x] == 2) 
                {
                    wallTilemap.SetTile(new Vector3Int(x - (room.Width / 2), y - (room.Height / 2), 0), SelectWallTile(new Vector2Int(x, y), wallGrid));
                }

                if (wallGrid[y, x] == 3) 
                {
                    wallTilemap.SetTile(new Vector3Int(x - (room.Width / 2), y - (room.Height / 2), 0), SelectRandomRockTile());
                }

                if (wallGrid[y, x] == 99) 
                {
                    wallTilemap.SetTile(new Vector3Int(x - (room.Width / 2), y - (room.Height / 2), 0), RedTestTile);
                }

                if (padGrid[y, x] == 4) 
                {
                    padTilemap.SetTile(new Vector3Int(x - (room.Width / 2), y - (room.Height / 2), 0), RedTestTile);
                }

            }
        }
    }

    private Tile SelectRandomGrassTile()
    {
        Tile selectedTile = Grass[0];
        int roll = 0;

        // roll for shrub
        roll = (int)Random.Range(0, 100);
        if (roll <= 30)
        {
            selectedTile = Grass[(int)Random.Range(4, 7)];

            // roll shrub for flower
            roll = (int)Random.Range(0, 100);
            if (roll <= 8)
            {
                selectedTile = Grass[(int)Random.Range(1, 4)];
            }
        }

        return selectedTile;

    }

    private Tile SelectRandomRockTile()
    {
        return Rocks[(int) Random.Range(0, Rocks.Length)];
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

        _wallTiles.Add("wall", Walls[0]);

        _wallTiles.Add("wall_top", Walls[1]);
        _wallTiles.Add("wall_bottom", Walls[2]);
        _wallTiles.Add("wall_bottom_grass", Walls[16]);
        _wallTiles.Add("wall_left", Walls[3]);
        _wallTiles.Add("wall_right", Walls[4]);

        _wallTiles.Add("wall_cover", Walls[5]);

        _wallTiles.Add("wall_corner_top_left", Walls[6]);
        _wallTiles.Add("wall_corner_top_right", Walls[7]);
        _wallTiles.Add("wall_corner_bottom_left", Walls[8]);
        _wallTiles.Add("wall_corner_bottom_right", Walls[9]);

        _wallTiles.Add("wall_bend_bottom_right", Walls[10]);
        _wallTiles.Add("wall_bend_bottom_left", Walls[11]);
        _wallTiles.Add("wall_bend_top_left", Walls[12]);
        _wallTiles.Add("wall_bend_top_right", Walls[13]);
        _wallTiles.Add("wall_bend_top_left_grass", Walls[14]);
        _wallTiles.Add("wall_bend_top_right_grass", Walls[15]);

    }
}

