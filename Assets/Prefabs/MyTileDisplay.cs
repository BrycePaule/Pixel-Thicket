using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTileDisplay : MonoBehaviour
{

    public MyTile tile;

    void Start()
    {
        print(tile.biome);
        print(tile.type);
    }

}
