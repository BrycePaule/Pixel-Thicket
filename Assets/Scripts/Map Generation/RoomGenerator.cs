using System;
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

        70 = red pad
        71 = purple pad
    */

    private static RoomGenerator _instance;

    [Range(0, 100)] public int roomLowerBound;
    [Range(0, 100)] public int roomUpperBound;
    [Space(10)]

    public Room LobbyPrefab;
    public Room RoomPrefab;

    [Space(10)]
    public Tile RedTestTile;
    public Tile PurpleTestTile;
    public Tile[] Grass;
    public Tile[] Walls;
    public Tile[] Pads;
    public Tile[] Rocks;

    [Space(10)]
    [SerializeField] private GameObject _torchPrefab;

    [Space(10)]
    public List<Vector2> SpawnLocations;

    private Dictionary<string, Tile> _wallTiles = new Dictionary<string, Tile>();
    private Dictionary<string, Tile> _padTiles = new Dictionary<string, Tile>();
    private Tilemap _groundTilemap;
    private Tilemap _padTilemap;
    private Tilemap _collideTilemap;
    private int[,] _groundGrid;
    private int[,] _padGrid;
    private int[,] _collideGrid;
    private int[,] _spawnGrid;
    private RoomType _roomType;

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
        if (_instance != null) { Destroy(gameObject); }
    }

    private void Start()
    {
        FillWallsDictionary();
        FillPadsDictionary();
    }

    // ROOM INITIALISATION
    public Room GenerateRoom(int[] gates)
    {
        Room room = Instantiate(RoomPrefab);

        _groundTilemap = room.transform.Find("Ground").gameObject.GetComponent<Tilemap>();
        _padTilemap = room.transform.Find("Pads").gameObject.GetComponent<Tilemap>();
        _collideTilemap = room.transform.Find("Collidables").gameObject.GetComponent<Tilemap>();

        _groundGrid = new int[room.Height, room.Width];
        _padGrid = new int[room.Height, room.Width];
        _collideGrid = new int[room.Height, room.Width];
        _spawnGrid = new int[room.Height, room.Width];

        // GRID GEN
        GenerateZeroGrid(room, _groundGrid);
        GenerateZeroGrid(room, _padGrid);
        GenerateZeroGrid(room, _collideGrid);
        GenerateZeroGrid(room, _spawnGrid);
        SelectRoomType(room);

        // TILE GEN
        GrassGenerate(room, _groundGrid);
        WallsGenerateOuter(room, _collideGrid);
        WallsGenerateInner(room, _collideGrid, 40);
        WallsFillSurrounded(room, _collideGrid);
        WallsFillSingleGaps(room, _collideGrid);

        RoomTypeGenerate(room, _collideGrid, _padGrid);

        GatewaysGenerate(room, _padGrid, _collideGrid, gates);
        GatewaysClearWallBlockers(room, _padGrid, _collideGrid);
        SetGridTiles(room, _groundGrid, _collideGrid, _padGrid, _groundTilemap, _collideTilemap, _padTilemap);

        // MOB GEN
        SpawnableGridGenerate(room);
        MobSpawnLocGenerate(room);

        // ROOM OBJECTS
        LightGenerate(room, _collideGrid);

        return room;
    }

    public Room GenerateLobbyRoom(int[] gates)
    {
        Room room = Instantiate(RoomPrefab);

        _groundTilemap = room.transform.Find("Ground").gameObject.GetComponent<Tilemap>();
        _padTilemap = room.transform.Find("Pads").gameObject.GetComponent<Tilemap>();
        _collideTilemap = room.transform.Find("Collidables").gameObject.GetComponent<Tilemap>();

        _groundGrid = new int[room.Height, room.Width];
        _padGrid = new int[room.Height, room.Width];
        _collideGrid = new int[room.Height, room.Width];
        _spawnGrid = new int[room.Height, room.Width];

        // GRID GEN
        GenerateZeroGrid(room, _groundGrid);
        GenerateZeroGrid(room, _padGrid);
        GenerateZeroGrid(room, _collideGrid);
        GenerateZeroGrid(room, _spawnGrid);
        SelectRoomType(room, RoomType.Lobby);

        // TILE GEN
        GrassGenerate(room, _groundGrid);
        WallsGenerateOuter(room, _collideGrid);
        WallsGenerateInner(room, _collideGrid, 40);
        WallsFillSurrounded(room, _collideGrid);
        WallsFillSingleGaps(room, _collideGrid);

        RoomTypeGenerate(room, _collideGrid, _padGrid);

        GatewaysGenerate(room, _padGrid, _collideGrid, gates);
        GatewaysClearWallBlockers(room, _padGrid, _collideGrid);
        SetGridTiles(room, _groundGrid, _collideGrid, _padGrid, _groundTilemap, _collideTilemap, _padTilemap);

        room.MobCount = 0;

        return room;
    }

    public Room GenerateEndRoom(int[] gates)
    {
        Room room = Instantiate(RoomPrefab);

        _groundTilemap = room.transform.Find("Ground").gameObject.GetComponent<Tilemap>();
        _padTilemap = room.transform.Find("Pads").gameObject.GetComponent<Tilemap>();
        _collideTilemap = room.transform.Find("Collidables").gameObject.GetComponent<Tilemap>();

        _groundGrid = new int[room.Height, room.Width];
        _padGrid = new int[room.Height, room.Width];
        _collideGrid = new int[room.Height, room.Width];
        _spawnGrid = new int[room.Height, room.Width];

        // GRID GEN
        GenerateZeroGrid(room, _groundGrid);
        GenerateZeroGrid(room, _padGrid);
        GenerateZeroGrid(room, _collideGrid);
        GenerateZeroGrid(room, _spawnGrid);
        SelectRoomType(room, RoomType.End);

        // TILE GEN
        GrassGenerate(room, _groundGrid);
        WallsGenerateOuter(room, _collideGrid);
        WallsGenerateInner(room, _collideGrid, 40);
        WallsFillSurrounded(room, _collideGrid);
        WallsFillSingleGaps(room, _collideGrid);

        RoomTypeGenerate(room, _collideGrid, _padGrid);

        GatewaysGenerate(room, _padGrid, _collideGrid, gates);
        GatewaysClearWallBlockers(room, _padGrid, _collideGrid);
        SetGridTiles(room, _groundGrid, _collideGrid, _padGrid, _groundTilemap, _collideTilemap, _padTilemap);

        // MOB GEN
        SpawnableGridGenerate(room);
        MobSpawnLocGenerate(room);

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

    private void GrassGenerate(Room room, int[,] baseGrid)
    {
        for (int y = 0; y < room.Height; y++)
        {
            for (int x = 0; x < room.Width; x++)
            {
                baseGrid[y, x] = 1;
            }
        }
    }

    private void WallsGenerateOuter(Room room, int[,] collideGrid)
    {
        for (int y = 0; y < room.Height; y++)
        {
            for (int x = 0; x < room.Width; x++)
            {
                if (y == 0 | x == 0 | y == (room.Height - 1) | x == (room.Width - 1))
                {
                    collideGrid[y, x] = 2;
                }

            }
        }
    }

    private void WallsGenerateInner(Room room, int[,] collideGrid, int conversionThreshold = 33)
    {
        for (int y = 0; y < room.Height; y++)
        {
            for (int x = 0; x < room.Width; x++)
            {
                if (y == 1 | x == 1 | y == (room.Height - 2) | x == (room.Width - 2))
                {
                    if (collideGrid[y, x] == 0)
                    {
                        if (Roll.Chance(conversionThreshold))
                        {
                            collideGrid[y, x] = 2;
                        }
                    }
                }

            }
        }
    }

    private void WallsFillSurrounded(Room room, int[,] collideGrid)
    {
        for (int y = 0; y < room.Height; y++)
        {
            for (int x = 0; x < room.Width; x++)
            {
                if (collideGrid[y, x] == 0)
                {
                    Vector2Int loc = new Vector2Int(x, y);
                    int[,] neighbours = GridMath.GetNeighbours(collideGrid, loc);
                    if (GridMath.CheckSurroundedCardinal(neighbours, 2)) 
                    {
                        collideGrid[y, x] = 2;
                    }
                } 
            }
        }
    }
    
    private void WallsFillSingleGaps(Room room, int[,] collideGrid)
    {

        bool changed = true;

        while (changed)
        {

            changed = false;

            for (int y = 0; y < room.Height; y++)
            {
                for (int x = 0; x < room.Width; x++)
                {
                    if (collideGrid[y, x] != 0) { continue; }

                    if (collideGrid[y - 1, x] == 2 & collideGrid[y + 1, x] == 2)
                    {
                        collideGrid[y, x] = 2;
                        changed = true;
                    }

                    if (collideGrid[y, x - 1] == 2 & collideGrid[y, x + 1] == 2)
                    {
                        collideGrid[y, x] = 2;
                        changed = true;
                    }                    
                }
            }
        }

    }

    private void GatewaysClearWallBlockers(Room room, int[,] padGrid, int[,] collideGrid)
    {
        for (int y = 0; y < room.Height; y++)
        {
            for (int x = 0; x < room.Width; x++)
            {
                if (padGrid[y, x] == 4)
                {
                    collideGrid[y, x] = 0;
                }

                // NORTH
                if (y == room.Height - 1)
                {
                    if (padGrid[y, x] == 4)
                    {
                        collideGrid[y - 1, x] = 0;
                    }
                }

                // SOUTH
                if (y == 0)
                {
                    if (padGrid[y, x] == 4)
                    {
                        collideGrid[y + 1, x] = 0;
                    }
                }

                // WEST
                if (x == 0)
                {
                    if (padGrid[y, x] == 4)
                    {
                        collideGrid[y, x + 1] = 0;
                    }
                }

                // EAST
                if (x == room.Width - 1)
                {
                    if (padGrid[y, x] == 4)
                    {
                        collideGrid[y, x - 1] = 0;
                    }
                }

            }
        }
    }

    private void GatewaysGenerate(Room room, int[,] padGrid, int[,] collideGrid, int[] gates)
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

            collideGrid[room.Height - 1, middleX] = 0;
            collideGrid[room.Height - 1, middleX - 1] = 0;
            collideGrid[room.Height - 1, middleX + 1] = 0;

            collideGrid[room.Height - 2, middleX - 2] = 0;
            collideGrid[room.Height - 2, middleX + 2] = 0;
        }

        // EAST
        if (gates[1] == 1)
        {
            padGrid[middleY, room.Width - 1] = 4;
            padGrid[middleY - 1, room.Width - 1] = 4;
            padGrid[middleY + 1, room.Width - 1] = 4;

            collideGrid[middleY, room.Width - 1] = 0;
            collideGrid[middleY - 1, room.Width - 1] = 0;
            collideGrid[middleY + 1, room.Width - 1] = 0;

            collideGrid[middleY - 2, room.Width - 2] = 0;
            collideGrid[middleY + 2, room.Width - 2] = 0;
        }

        // SOUTH
        if (gates[2] == 1)
        {
            padGrid[0, middleX] = 4;
            padGrid[0, middleX - 1] = 4;
            padGrid[0, middleX + 1] = 4;

            collideGrid[0, middleX] = 0;
            collideGrid[0, middleX - 1] = 0;
            collideGrid[0, middleX + 1] = 0;

            collideGrid[1, middleX - 2] = 0;
            collideGrid[1, middleX + 2] = 0;

        }
        
        // WEST
        if (gates[3] == 1)
        {
            padGrid[middleY, 0] = 4;
            padGrid[middleY - 1, 0] = 4;
            padGrid[middleY + 1, 0] = 4;

            collideGrid[middleY, 0] = 0;
            collideGrid[middleY - 1, 0] = 0;
            collideGrid[middleY + 1, 0] = 0;

            collideGrid[middleY - 2, 1] = 0;
            collideGrid[middleY + 2, 1] = 0;
        }
    }

    private void SelectRoomType(Room room, RoomType roomTypeSelection = RoomType.Null)
    {
        if (roomTypeSelection == RoomType.Null)
        {
            int roomTypeCount = Enum.GetNames(typeof(RoomType)).Length;

            // 0, 1, 2 are Null, Lobby and End - shouldn't randomly roll them
            _roomType = (RoomType) UnityEngine.Random.Range(3, roomTypeCount);
        }

        else
        {
            _roomType = roomTypeSelection;
        }

        room.RoomType = _roomType;

        // _roomType = RoomType.End;

    }

    private void RoomTypeGenerate(Room room, int[,] collideGrid, int[,] padGrid)
    {
        if (_roomType == RoomType.Lobby)
        {
            int boxHalfWidth = 6;
            int boxHalfHeight = 6;

            int xCentre = Mathf.FloorToInt(room.Width / 2);
            int yCentre = Mathf.FloorToInt(room.Height / 2);

            int xLower = xCentre - boxHalfWidth;
            int xUpper = xCentre + boxHalfWidth;

            int yLower = yCentre - boxHalfHeight;
            int yUpper = yCentre + boxHalfHeight;

            for (int y = 0; y < room.Height; y++)
            {
                for (int x = 0; x < room.Width; x++)
                {
                    // bounds to centre box
                    if (x < xLower | x > xUpper) { continue; }
                    if (y < yLower | y > yUpper) { continue; }

                    // create box
                    collideGrid[y, x] = 3;

                    // cut out entrances
                    if ((xCentre - 1 <= x & x <= xCentre + 1) | (yCentre - 1 <= y & y <= yCentre + 1))
                    {
                        collideGrid[y, x] = 0;
                    }

                    // cutout centre
                    if ((x > xLower & x < xUpper) & (y > yLower & y < yUpper))
                    {
                        collideGrid[y, x] = 0;
                    }

                    // place lips on entrances
                    if ((x == xCentre - 2 | x == xCentre + 2) & (y == yLower + 1 | y == yUpper - 1)) { collideGrid[y, x] = 3; }
                    if ((y == yCentre - 2 | y == yCentre + 2) & (x == xLower + 1 | x == xUpper - 1)) { collideGrid[y, x] = 3; }

                    // centre pad
                    if ((xCentre - 1 <= x & x <= xCentre + 1) & (yCentre - 1 <= y & y <= yCentre + 1))
                    {
                        padGrid[y, x] = 70;
                    }

                    // bottom left pad
                    // if ((x > xLower & x < xCentre - 2) & (y > yLower & y < yCentre - 2))
                    // {
                    //     padGrid[y, x] = 71;
                    // }

                    // bottom right pad
                    if ((x < xUpper & x > xCentre + 2) & (y > yLower & y < yCentre - 2))
                    {
                        padGrid[y, x] = 71;
                    }

                    // top right pad
                    // if ((x < xUpper & x > xCentre + 2) & (y < yUpper & y > yCentre + 2))
                    // {
                    //     padGrid[y, x] = 71;
                    // }

                    // top left pad
                    // if ((x > xLower & x < xCentre - 2) & (y < yUpper & y > yCentre + 2))
                    // {
                    //     padGrid[y, x] = 71;
                    // }
                }
            }
        
            // LIGHTS
            List<Vector2> lightLocations = new List<Vector2>
            {
                new Vector2(-5, 2),
                new Vector2(-5, -2),
                new Vector2(5, 2),
                new Vector2(5, -2),
            };

            foreach (Vector2 loc in lightLocations)
            {
                Vector3 worldLocation = new Vector3(loc.x + 0.5f, loc.y + 0.5f, 0f);
                GameObject torch = Instantiate(_torchPrefab, worldLocation, Quaternion.identity);
                torch.transform.parent = room.LightContainer;
            }
        }

        if (_roomType == RoomType.End)
        {
            int boxHalfWidth = 4;
            int boxHalfHeight = 4;

            int xCentre = Mathf.FloorToInt(room.Width / 2);
            int yCentre = Mathf.FloorToInt(room.Height / 2);

            int xLower = xCentre - boxHalfWidth;
            int xUpper = xCentre + boxHalfWidth;

            int yLower = yCentre - boxHalfHeight;
            int yUpper = yCentre + boxHalfHeight;

            for (int y = 0; y < room.Height; y++)
            {
                for (int x = 0; x < room.Width; x++)
                {
                    // bounds to centre box
                    if (x < xLower | x > xUpper) { continue; }
                    if (y < yLower | y > yUpper) { continue; }
                    

                    // bound the centre pad
                    if (x == xCentre - 2 | x == xCentre + 2 | y == yCentre - 2 | y == yCentre + 2 |
                        x == xCentre - 1 | x == xCentre + 1 | y == yCentre - 1 | y == yCentre + 1)
                    {
                        collideGrid[y, x] = 3;
                    }

                    // cut out entrances
                    if (x == xCentre| y == yCentre)
                    {
                        collideGrid[y, x] = 0;
                    }

                    // centre pad
                    if ((xCentre - 1 <= x & x <= xCentre + 1) & (yCentre - 1 <= y & y <= yCentre + 1))
                    {
                        collideGrid[y, x] = 0;
                        padGrid[y, x] = 70;
                    }
                }
            }

            // LIGHTS
            List<Vector2> lightLocations = new List<Vector2>
            {
                new Vector2(-4, 1),
                new Vector2(-2, 1),

                new Vector2(2, 1),
                new Vector2(4, 1),
            };

            foreach (Vector2 loc in lightLocations)
            {
                Vector3 worldLocation = new Vector3(loc.x + 0.5f, loc.y + 0.5f, 0f);
                GameObject torch = Instantiate(_torchPrefab, worldLocation, Quaternion.identity);
                torch.transform.parent = room.LightContainer;
            }
        }

        if (_roomType == RoomType.Clear)
        {

        }
        
        if (_roomType == RoomType.Box)
        {
            int boxWidth = UnityEngine.Random.Range(2, Mathf.FloorToInt(room.Width * 0.25f));
            int boxHeight = UnityEngine.Random.Range(2, Mathf.FloorToInt(room.Height * 0.25f));

            int xCentre = Mathf.FloorToInt(room.Width / 2);
            int yCentre = Mathf.FloorToInt(room.Height / 2);

            int xLower = xCentre - boxWidth;
            int xUpper = xCentre + boxWidth;

            int yLower = yCentre - boxHeight;
            int yUpper = yCentre + boxHeight;

            for (int y = 0; y < room.Height; y++)
            {
                for (int x = 0; x < room.Width; x++)
                {
                    // bounds to centre box
                    if (x < xLower | x > xUpper) { continue; }
                    if (y < yLower | y > yUpper) { continue; }
                    
                    collideGrid[y, x] = 3;
                }
            }
        }

        if (_roomType == RoomType.RingClosed)
        {
            int boxWidth = UnityEngine.Random.Range(4, Mathf.FloorToInt(room.Width * 0.25f));
            int boxHeight = UnityEngine.Random.Range(4, Mathf.FloorToInt(room.Height * 0.25f));

            int xCentre = Mathf.FloorToInt(room.Width / 2);
            int yCentre = Mathf.FloorToInt(room.Height / 2);

            int xLower = xCentre - boxWidth;
            int xUpper = xCentre + boxWidth;

            int yLower = yCentre - boxHeight;
            int yUpper = yCentre + boxHeight;

            for (int y = 0; y < room.Height; y++)
            {
                for (int x = 0; x < room.Width; x++)
                {
                    // bounds to centre box
                    if (x < xLower | x > xUpper) { continue; }
                    if (y < yLower | y > yUpper) { continue; }

                    // creates ring
                    if (x == xLower | x == xUpper | y == yLower | y == yUpper) 
                    {
                        collideGrid[y, x] = 3;
                    }
                }
            }
        }

        if (_roomType == RoomType.RingCutout)
        {
            int boxWidth = UnityEngine.Random.Range(4, Mathf.FloorToInt(room.Width * 0.25f));
            int boxHeight = UnityEngine.Random.Range(4, Mathf.FloorToInt(room.Height * 0.25f));

            int xCentre = Mathf.FloorToInt(room.Width / 2);
            int yCentre = Mathf.FloorToInt(room.Height / 2);

            int xLower = xCentre - boxWidth;
            int xUpper = xCentre + boxWidth;

            int yLower = yCentre - boxHeight;
            int yUpper = yCentre + boxHeight;

            for (int y = 0; y < room.Height; y++)
            {
                for (int x = 0; x < room.Width; x++)
                {
                    // bounds to centre box
                    if (x < xLower | x > xUpper) { continue; }
                    if (y < yLower | y > yUpper) { continue; }

                    // create box
                    collideGrid[y, x] = 3;

                    // cut out entrances
                    if ((xCentre - 1 <= x & x <= xCentre + 1) | (yCentre - 1 <= y & y <= yCentre + 1))
                    {
                        collideGrid[y, x] = 0;
                    }

                    // cutout centre
                    if ((x > xLower & x < xUpper) & (y > yLower & y < yUpper))
                    {
                        collideGrid[y, x] = 0;
                    }
                }
            }
        }

        if (_roomType == RoomType.RingCutoutLips)
        {
            if (room.Width <= 24 | room.Height <= 24) { return; }

            int boxWidth = UnityEngine.Random.Range(6, Mathf.FloorToInt(room.Width * 0.35f));
            int boxHeight = UnityEngine.Random.Range(6, Mathf.FloorToInt(room.Height * 0.35f));

            int xCentre = Mathf.FloorToInt(room.Width / 2);
            int yCentre = Mathf.FloorToInt(room.Height / 2);

            int xLower = xCentre - boxWidth;
            int xUpper = xCentre + boxWidth;

            int yLower = yCentre - boxHeight;
            int yUpper = yCentre + boxHeight;

            for (int y = 0; y < room.Height; y++)
            {
                for (int x = 0; x < room.Width; x++)
                {
                    // bounds to centre box
                    if (x < xLower | x > xUpper) { continue; }
                    if (y < yLower | y > yUpper) { continue; }

                    // create box
                    collideGrid[y, x] = 3;

                    // cut out entrances
                    if ((xCentre - 1 <= x & x <= xCentre + 1) | (yCentre - 1 <= y & y <= yCentre + 1))
                    {
                        collideGrid[y, x] = 0;
                    }

                    // cutout centre
                    if ((x > xLower & x < xUpper) & (y > yLower & y < yUpper))
                    {
                        collideGrid[y, x] = 0;
                    }

                    // place lips on entrances
                    if ((x == xCentre - 2 | x == xCentre + 2) & (y == yLower + 1 | y == yUpper - 1)) { collideGrid[y, x] = 3; }
                    if ((y == yCentre - 2 | y == yCentre + 2) & (x == xLower + 1 | x == xUpper - 1)) { collideGrid[y, x] = 3; } 
                }
            }
        }

        if (_roomType == RoomType.Houses)
        {
            if (room.Width <= 24 | room.Height <= 24) { return; }

            int boxWidth = UnityEngine.Random.Range(6, Mathf.FloorToInt(room.Width * 0.25f));
            int boxHeight = UnityEngine.Random.Range(6, Mathf.FloorToInt(room.Height * 0.25f));

            int xCentre = Mathf.FloorToInt(room.Width / 2);
            int yCentre = Mathf.FloorToInt(room.Height / 2);

            int xLower = xCentre - boxWidth;
            int xUpper = xCentre + boxWidth;

            int yLower = yCentre - boxHeight;
            int yUpper = yCentre + boxHeight;

            for (int y = 0; y < room.Height; y++)
            {
                for (int x = 0; x < room.Width; x++)
                {
                    // bounds to centre box
                    if (x < xLower | x > xUpper) { continue; }
                    if (y < yLower | y > yUpper) { continue; }

                    // create box
                    collideGrid[y, x] = 3;

                    // cut out entrances
                    if ((xCentre - 1 <= x & x <= xCentre + 1) | (yCentre - 1 <= y & y <= yCentre + 1))
                    {
                        collideGrid[y, x] = 0;
                    }

                    // bottom left box
                    if ((x > xLower & x < xCentre - 2) & (y > yLower & y < yCentre - 2))
                    {
                        collideGrid[y, x] = 0;
                    }
                    
                    // top left box
                    if ((x > xLower & x < xCentre - 2) & (y < yUpper & y > yCentre + 2))
                    {
                        collideGrid[y, x] = 0;
                    }

                    // top right box
                    if ((x < xUpper & x > xCentre + 2) & (y < yUpper & y > yCentre + 2))
                    {
                        collideGrid[y, x] = 0;
                    }

                    // bottom left box
                    if ((x < xUpper & x > xCentre + 2) & (y > yLower & y < yCentre - 2))
                    {
                        collideGrid[y, x] = 0;
                    }

                    // Doors
                    if ((x == xCentre - 2 | x == xCentre + 2) & (y == yLower + 1 | y == yUpper - 1)) { collideGrid[y, x] = 0; }
                    if ((y == yCentre - 2 | y == yCentre + 2) & (x == xLower + 1 | x == xUpper - 1)) { collideGrid[y, x] = 0; }
                }
            }
        }
        
        if (_roomType == RoomType.PillarsFilled)
        {
            int boxWidth = UnityEngine.Random.Range(5, Mathf.FloorToInt(room.Width * 0.25f));
            int boxHeight = UnityEngine.Random.Range(5, Mathf.FloorToInt(room.Height * 0.25f));

            int xCentre = Mathf.FloorToInt(room.Width / 2);
            int yCentre = Mathf.FloorToInt(room.Height / 2);

            int xLower = xCentre - boxWidth;
            int xUpper = xCentre + boxWidth;

            int yLower = yCentre - boxHeight;
            int yUpper = yCentre + boxHeight;

            for (int y = 0; y < room.Height; y++)
            {
                for (int x = 0; x < room.Width; x++)
                {
                    // bounds to centre box
                    if (x < xLower | x > xUpper) { continue; }
                    if (y < yLower | y > yUpper) { continue; }
                    
                    // fills box
                    collideGrid[y, x] = 3;

                    // cuts walkways
                    if ((xCentre - 1 <= x & x <= xCentre + 1) | (yCentre - 1 <= y & y <= yCentre + 1))
                    {
                        collideGrid[y, x] = 0;
                    }
                }
            }
        }

        if (_roomType == RoomType.PillarsCutout)
        {
            int boxWidth = UnityEngine.Random.Range(4, Mathf.FloorToInt(room.Width * 0.25f));
            int boxHeight = UnityEngine.Random.Range(4, Mathf.FloorToInt(room.Height * 0.25f));

            int xCentre = Mathf.FloorToInt(room.Width / 2);
            int yCentre = Mathf.FloorToInt(room.Height / 2);

            int xLower = xCentre - boxWidth;
            int xUpper = xCentre + boxWidth;

            int yLower = yCentre - boxHeight;
            int yUpper = yCentre + boxHeight;

            for (int y = 0; y < room.Height; y++)
            {
                for (int x = 0; x < room.Width; x++)
                {
                    // bounds to centre box
                    if (x < xLower | x > xUpper) { continue; }
                    if (y < yLower | y > yUpper) { continue; }

                    // create box
                    collideGrid[y, x] = 3;

                    // cutout walkways
                    if ((xCentre - 1 <= x & x <= xCentre + 1) | (yCentre - 1 <= y & y <= yCentre + 1))
                    {
                        collideGrid[y, x] = 0;
                    }

                    // cutout bottom left box
                    if ((x > xLower & x < xCentre - 2) & (y > yLower & y < yCentre - 2))
                    {
                        collideGrid[y, x] = 0;
                    }
                    
                    // cutout top left box
                    if ((x > xLower & x < xCentre - 2) & (y < yUpper & y > yCentre + 2))
                    {
                        collideGrid[y, x] = 0;
                    }

                    // cutout top right box
                    if ((x < xUpper & x > xCentre + 2) & (y < yUpper & y > yCentre + 2))
                    {
                        collideGrid[y, x] = 0;
                    }

                    // cutout bottom left box
                    if ((x < xUpper & x > xCentre + 2) & (y > yLower & y < yCentre - 2))
                    {
                        collideGrid[y, x] = 0;
                    }
                }
            }
        }
        
    }

    private void LightGenerate(Room room, int[,] collideGrid)
    {
        for (int y = 0; y < room.Height; y++)
        {
            for (int x = 0; x < room.Width; x++)
            {
                if (collideGrid[y, x] != 3) { continue; }

                int[,] neighbours = GridMath.GetNeighbours(collideGrid, new Vector2Int(x, y), 0);

                // if empty below
                if (neighbours[2, 1] == 0)
                {
                    if (Roll.Chance(5))
                    {
                        Vector3 worldLocation = new Vector3(x - Mathf.FloorToInt(room.Width / 2) + 0.5f, y - Mathf.FloorToInt(room.Height / 2) + 0.5f, 0f);

                        GameObject torch = Instantiate(_torchPrefab, worldLocation, Quaternion.identity);
                        torch.transform.parent = room.LightContainer;
                    }
                }
            }
        }
    }

    // SPAWN LOCATIONS
    private void SpawnableGridGenerate(Room room)
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

    private void MobSpawnLocGenerate(Room room)
    {
        int spawnTiles = 0;
        SpawnLocations = new List<Vector2>();

        for (int y = 0; y < room.Height; y++)
        {
            for (int x = 0; x < room.Width; x++)
            {
                if (_spawnGrid[y, x] != 1) { continue; } 
                
                // locations are offset by 50% of room width / height to centre them around the middle
                // locations are also already in world coords because each square in the grid is 1 to 1 size
                SpawnLocations.Add(new Vector2(x - Mathf.FloorToInt(room.Width / 2), y - Mathf.FloorToInt(room.Height / 2)));
                spawnTiles++;
            }
        }
        
        room.MobCount = Mathf.FloorToInt(spawnTiles * 0.02f);
        room.MobSpawnLocations = SpawnLocations;
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
    private void SetGridTiles(Room room, int[,] baseGrid, int[,] collideGrid, int[,] padGrid, Tilemap baseTilemap, Tilemap collideTilemap, Tilemap padTilemap)
    {
        for (int y = 0; y < room.Height; y++)
        {
            for (int x = 0; x < room.Width; x++)
            {
                if (baseGrid[y, x] == 1) 
                {
                    baseTilemap.SetTile(new Vector3Int(x - (room.Width / 2), y - (room.Height / 2), 0), SelectRandomGrassTile());
                }

                if (collideGrid[y, x] == 2) 
                {
                    collideTilemap.SetTile(new Vector3Int(x - (room.Width / 2), y - (room.Height / 2), 0), SelectWallTile(new Vector2Int(x, y), collideGrid));
                }

                if (collideGrid[y, x] == 3) 
                {
                    collideTilemap.SetTile(new Vector3Int(x - (room.Width / 2), y - (room.Height / 2), 0), SelectRandomRockTile());
                }

                if (collideGrid[y, x] == 99) 
                {
                    collideTilemap.SetTile(new Vector3Int(x - (room.Width / 2), y - (room.Height / 2), 0), RedTestTile);
                }

                if (padGrid[y, x] == 4) 
                {
                    padTilemap.SetTile(new Vector3Int(x - (room.Width / 2), y - (room.Height / 2), 0), SelectGatewayTile(room, new Vector2Int(x, y), padGrid));
                }

                if (padGrid[y, x] == 70) 
                {
                    padTilemap.SetTile(new Vector3Int(x - (room.Width / 2), y - (room.Height / 2), 0), SelectPadTile(new Vector2Int(x, y), padGrid));
                }

                if (padGrid[y, x] == 71) 
                {
                    padTilemap.SetTile(new Vector3Int(x - (room.Width / 2), y - (room.Height / 2), 0), SelectPadTile(new Vector2Int(x, y), padGrid));
                }

            }
        }
    }

    private Tile SelectRandomGrassTile()
    {
        Tile selectedTile = Grass[0];

        // roll for shrub
        if (Roll.Chance(30))
        {
            selectedTile = Grass[(int) UnityEngine.Random.Range(4, 7)];

            // roll shrub for flower
            if (Roll.Chance(8))
            {
                selectedTile = Grass[(int) UnityEngine.Random.Range(1, 4)];
            }
        }

        return selectedTile;

    }

    private Tile SelectRandomRockTile()
    {
        return Rocks[(int) UnityEngine.Random.Range(0, Rocks.Length)];
        // return Rocks[0];
    }

    private Tile SelectWallTile(Vector2Int loc, int[,] collideGrid)
    {
        int[,] neighbours = GridMath.GetNeighbours(collideGrid, loc);
        string gridType = GridMath.Comparison(neighbours, GridMath.WallComparisonGrids);
        
        if (gridType != null)
        {
            return _wallTiles[gridType];
        }

        return _wallTiles["wall"];
    }

    private Tile SelectPadTile(Vector2Int loc, int[,] padGrid)
    {
        int[,] neighbours = GridMath.GetNeighbours(padGrid, loc);
        string gridType = GridMath.Comparison(neighbours, GridMath.PadComparisonGrids, 1);

        if (gridType != null)
        {
            return _padTiles[gridType];
        }

        return _wallTiles["wall"];
    }

    private Tile SelectGatewayTile(Room room, Vector2Int loc, int[,] padGrid)
    {
        // if (loc.x == 0) { return _wallTiles["wall_gateway_left"]; }
        // if (loc.y == 0) { return _wallTiles["wall_gateway_bottom"]; }

        // if (loc.x == room.Width - 1) { return _wallTiles["wall_gateway_right"]; }
        // if (loc.y == room.Height - 1) { return _wallTiles["wall_gateway_top"]; }

        return null;
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

        _wallTiles.Add("wall_gateway_top", Walls[17]);
        _wallTiles.Add("wall_gateway_bottom", Walls[18]);
        _wallTiles.Add("wall_gateway_left", Walls[19]);
        _wallTiles.Add("wall_gateway_right", Walls[20]);
    }

    private void FillPadsDictionary()
    {
        _padTiles.Add("pad_red_top", Pads[0]);
        _padTiles.Add("pad_red_bottom", Pads[1]);
        _padTiles.Add("pad_red_left", Pads[2]);
        _padTiles.Add("pad_red_right", Pads[3]);

        _padTiles.Add("pad_red_middle", Pads[10]);
        _padTiles.Add("pad_red_middle_dot", Pads[11]);

        _padTiles.Add("pad_red_corner_top_left", Pads[4]);
        _padTiles.Add("pad_red_corner_top_right", Pads[5]);
        _padTiles.Add("pad_red_corner_bottom_left", Pads[6]);
        _padTiles.Add("pad_red_corner_bottom_right", Pads[7]);

        _padTiles.Add("pad_red_bend_top_left", Pads[8]);
        _padTiles.Add("pad_red_bend_top_right", Pads[9]);


        _padTiles.Add("pad_purple_top", Pads[12]);
        _padTiles.Add("pad_purple_bottom", Pads[13]);
        _padTiles.Add("pad_purple_left", Pads[14]);
        _padTiles.Add("pad_purple_right", Pads[15]);

        _padTiles.Add("pad_purple_middle", Pads[22]);
        _padTiles.Add("pad_purple_middle_dot", Pads[23]);

        _padTiles.Add("pad_purple_corner_top_left", Pads[16]);
        _padTiles.Add("pad_purple_corner_top_right", Pads[17]);
        _padTiles.Add("pad_purple_corner_bottom_left", Pads[18]);
        _padTiles.Add("pad_purple_corner_bottom_right", Pads[19]);

        _padTiles.Add("pad_purple_bend_top_left", Pads[20]);
        _padTiles.Add("pad_purple_bend_top_right", Pads[21]);
    }
}

