using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile", menuName = "Tiles/My Tile")]
public class MyTile : ScriptableObject
{

    public string biome;
    public string type;
    public Sprite sprite;

}
