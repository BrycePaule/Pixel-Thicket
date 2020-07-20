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

    public Tilemap baseTilemap;
    public Tilemap padTilemap;
    public Tilemap collidableTilemap;

    [Range(10, 40)] public int roomWidth;
    [Range(10, 40)] public int roomHeight;

    public Tile[] grass;
    public Tile[] walls;
    public Tile[] pads;
    public Tile[] rocks;

    [Space]
    public Tile DISPLAYTILE;
    // public BaseRuleTile ruleTile;
    [Space]

    private Dictionary<string, Tile> _walls;

    private int[,] _baseTilemapGrid;
    private int[,] _padTilemapGrid;
    private int[,] _collidableTilemapGrid;


    private void Awake()
    {
        _baseTilemapGrid = new int[roomWidth, roomHeight];
        _padTilemapGrid = new int[roomWidth, roomHeight];
        _collidableTilemapGrid = new int[roomWidth, roomHeight];

        _walls = new Dictionary<string, Tile>();
        FillWallsDictionary();
    }

    private void Start()
    {
       
        GenerateEmptyGrid(_baseTilemapGrid);
        GenerateEmptyGrid(_padTilemapGrid);
        GenerateEmptyGrid(_collidableTilemapGrid);

        GenerateGrassArea();
        GenerateOuterWalls();
        GenerateInnerWalls();

        SetGridTiles();

        // PrintGrid(_collidableTilemapGrid);
    }

    // NUMBER MAP GENERATION
    private void GenerateEmptyGrid(int[,] grid)
    {
        for (int y = 0; y < roomWidth; y++)
        {
            for (int x = 0; x < roomHeight; x++)
            {
                grid[y, x] = 0;
            }
        }
    }

    private void GenerateGrassArea()
    {
        for (int y = 0; y < roomWidth; y++)
        {
            for (int x = 0; x < roomHeight; x++)
            {
                _baseTilemapGrid[y, x] = 1;
            }
        }
    }

    private void GenerateOuterWalls()
    {
        for (int y = 0; y < roomWidth; y++)
        {
            for (int x = 0; x < roomHeight; x++)
            {
                if (y == 0 | x == 0 | y == (roomWidth - 1) | x == (roomHeight - 1))
                {
                    _collidableTilemapGrid[y, x] = 2;
                }

            }
        }
    }

    private void GenerateInnerWalls()
    {
        for (int y = 0; y < roomWidth; y++)
        {
            for (int x = 0; x < roomHeight; x++)
            {
                if (y == 1 | x == 1 | y == (roomWidth - 2) | x == (roomHeight - 2))
                {
                    int roll = (int) Random.Range(0, 100);
                    if (roll <= 33)
                    {
                        _collidableTilemapGrid[y, x] = 2;
                    }
                }

            }
        }
    }
    
    private void PrintGrid(int[,] grid)
    {
        for (int y = 0; y < roomWidth; y++)
        {
            for (int x = 0; x < roomHeight; x++)
            {
                print(grid[y, x]);
            }
        }
    }

    // TILE PLACEMENT
    private void SetGridTiles()
    {
        for (int y = 0; y < roomWidth; y++)
        {
            for (int x = 0; x < roomHeight; x++)
            {
                if (_baseTilemapGrid[y, x] == 1) 
                {
                    baseTilemap.SetTile(new Vector3Int(x - (roomWidth / 2), y - (roomHeight / 2), 0), SelectRandomGrassTile());
                }

                if (_collidableTilemapGrid[y, x] == 2) 
                {
                    collidableTilemap.SetTile(new Vector3Int(x - (roomWidth / 2), y - (roomHeight / 2), 0), CalculateWallTileDirection(new Vector2Int(x, y)));
                }

                if (_collidableTilemapGrid[y, x] == 3) 
                {
                    collidableTilemap.SetTile(new Vector3Int(x - (roomWidth / 2), y - (roomHeight / 2), 0), SelectRandomRockTile());
                }

                if (_collidableTilemapGrid[y, x] == 99) 
                {
                    collidableTilemap.SetTile(new Vector3Int(x - (roomWidth / 2), y - (roomHeight / 2), 0), DISPLAYTILE);
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

    private Tile CalculateWallTileDirection(Vector2Int loc)
    {

        int[,] neighbours = GridCalc.GetNeighbours(_collidableTilemapGrid, loc);
        string gridType = GridCalc.CompareGrid(neighbours);
        
        if (gridType != null)
        {
            return _walls[gridType];
        }

        // if (loc.x == 0 & loc.y == 0) { return _walls["wall_corner_bottom_left"]; }
        // if (loc.x == 0 & loc.y == roomHeight - 1) { return _walls["wall_corner_top_left"]; }
        // if (loc.x == roomWidth - 1 & loc.y == 0) { return _walls["wall_corner_bottom_right"]; }
        // if (loc.x == roomWidth - 1 & loc.y == roomHeight - 1) { return _walls["wall_corner_top_right"]; }

        // if (loc.x == 0) { return _walls["wall_left"]; }
        // if (loc.x == roomWidth - 1) { return _walls["wall_right"]; }
        // if (loc.y == 0) { return _walls["wall_bottom"]; }
        // if (loc.y == roomHeight - 1) { return _walls["wall_top"]; }

        return _walls["wall"];
    }

    // UTILITIES
    private void FillWallsDictionary()
    {
        _walls.Add("wall", walls[0]);
        _walls.Add("wall_cover", walls[6]);

        _walls.Add("wall_left", walls[8]);
        _walls.Add("wall_right", walls[7]);
        _walls.Add("wall_top", walls[9]);
        _walls.Add("wall_bottom", walls[1]);

        _walls.Add("wall_corner_bottom_left", walls[2]);
        _walls.Add("wall_corner_bottom_right", walls[4]);
        _walls.Add("wall_corner_top_left", walls[10]);
        _walls.Add("wall_corner_top_right", walls[12]);

        _walls.Add("wall_bend_top_right", walls[3]);
        _walls.Add("wall_bend_top_left", walls[5]);
        _walls.Add("wall_bend_bottom_right", walls[11]);
        _walls.Add("wall_bend_bottom_left", walls[13]);
    }
}

