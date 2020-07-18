using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;



public class RoomGenerator : MonoBehaviour
{

    
    public Tilemap tilemap;
    public Texture2D grassland;
    public Tile tileTest;

    [Range(10, 40)]
    public int roomWidth;

    [Range(10, 40)]
    public int roomHeight;

    private int[,] numMap;
    private Sprite[] tiles;


    private Dictionary<string, Tile> tileConvert;


    private void Awake()
    {
        numMap = new int[roomWidth, roomHeight];
        tiles = Resources.LoadAll<Sprite>(grassland.name);
    }

    private void Start()
    {
       
        FillBlank();
        // PrintNumMap();
        ConvertNumToTiles();


    }

    private void InitialiseTileConvert()
    {
        tileConvert = new Dictionary<string, Tile>()
        {
            // {"grass1", tiles[0]},
            
        };
    }

    // NUMBER MAP GENERATION
    private void FillBlank()
    {
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                if (x == 0 | x == (roomWidth - 1) | y == 0 | y == (roomHeight - 1))
                {
                    numMap[x, y] = 1;
                }
                else
                {
                    numMap[x, y] = 0;
                }
            }
        }
    }
    
    private void PrintNumMap()
    {
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                print(numMap[x, y]);
            }
        }
    }

    // TILE PLACEMENT LOGIC
    private void ConvertNumToTiles()
    {
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                if (numMap[x, y] == 1) 
                {
                    tilemap.SetTile(new Vector3Int(x - (roomWidth / 2), y - (roomWidth / 2), 0), tileTest);
                }
            }
        }
    }
}
