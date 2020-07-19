﻿using System.Collections;
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
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                grid[x, y] = 0;
            }
        }
    }

    private void GenerateGrassArea()
    {
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                _baseTilemapGrid[x, y] = 1;
            }
        }
    }

    private void GenerateOuterWalls()
    {
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                if (x == 0 | y == 0 | x == (roomWidth - 1) | y == (roomHeight - 1))
                {
                    _collidableTilemapGrid[x, y] = 3;
                }

            }
        }
    }

    private void GenerateInnerWalls()
    {
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                if (x == 1 | y == 1 | x == (roomWidth - 2) | y == (roomHeight - 2))
                {
                    int roll = (int) Random.Range(0, 100);
                    if (roll <= 33)
                    {
                        _collidableTilemapGrid[x, y] = 3;
                    }
                }

            }
        }
    }
    
    private void PrintGrid(int[,] grid)
    {
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                print(grid[x, y]);
            }
        }
    }

    // TILE PLACEMENT LOGIC
    private void SetGridTiles()
    {
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                if (_baseTilemapGrid[x, y] == 1) 
                {
                    baseTilemap.SetTile(new Vector3Int(x - (roomWidth / 2), y - (roomWidth / 2), 0), SelectRandomGrassTile());
                }

                if (_collidableTilemapGrid[x, y] == 2) 
                {
                    collidableTilemap.SetTile(new Vector3Int(x - (roomWidth / 2), y - (roomWidth / 2), 0), SelectRandomRockTile());
                }

                if (_collidableTilemapGrid[x, y] == 3) 
                {
                    collidableTilemap.SetTile(new Vector3Int(x - (roomWidth / 2), y - (roomWidth / 2), 0), CalculateWallTileDirection(new Vector2Int(x, y)));
                }

                if (_collidableTilemapGrid[x, y] == 99) 
                {
                    collidableTilemap.SetTile(new Vector3Int(x - (roomWidth / 2), y - (roomWidth / 2), 0), DISPLAYTILE);
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

    private Tile CalculateWallTileDirection(Vector2Int location)
    {

        if (location.x == 0 & location.y == 0) { return _walls["wall_corner_bottom_left"]; }
        if (location.x == 0 & location.y == roomHeight - 1) { return _walls["wall_corner_top_left"]; }
        if (location.x == roomWidth - 1 & location.y == 0) { return _walls["wall_corner_bottom_right"]; }
        if (location.x == roomWidth - 1 & location.y == roomHeight - 1) { return _walls["wall_corner_top_right"]; }

        if (location.x == 0) { return _walls["wall_left"]; }
        if (location.x == roomWidth - 1) { return _walls["wall_right"]; }
        if (location.y == 0) { return _walls["wall_bottom"]; }
        if (location.y == roomHeight - 1) { return _walls["wall_top"]; }


        return _walls["wall"];
    }

    private void GetTileNeighbours(int[,] grid, Vector2Int location)
    {
        
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
