using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class RoomGenerator : MonoBehaviour
{

    public Tilemap tilemap;

    [Range(10, 40)]
    public int roomWidth;

    [Range(10, 40)]
    public int roomHeight;

    private int[,] numMap;


    private void Start()
    {
       numMap = new int[roomWidth, roomHeight];

       FillBlank();
       PrintMap();
    }


    private void FillBlank()
    {
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                if (x == 0 | x == roomWidth - 1 | y == 0 | y == roomHeight)
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

    private void PrintMap()
    {
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                print(numMap[x, y]);
            }
        }
    }
}
