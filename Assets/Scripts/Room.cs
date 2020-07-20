using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int roomWidth;
    public int roomHeight;

    private void Awake() 
    {
        roomWidth = Random.Range(RoomGenerator.Instance.roomLowerBound, RoomGenerator.Instance.roomUpperBound);
        roomHeight = Random.Range(RoomGenerator.Instance.roomLowerBound, RoomGenerator.Instance.roomUpperBound);
    }

}
