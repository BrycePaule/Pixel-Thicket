﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomGrassTile : Tile
{

    [SerializeField]
    private Sprite[] grassSprites;


    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {

    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
