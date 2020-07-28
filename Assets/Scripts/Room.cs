using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int roomWidth;
    public int roomHeight;

    public Gateway north;
    public Gateway east;
    public Gateway south;
    public Gateway west;

    private void Awake() 
    {
        if (transform.name != "Lobby(Clone)") 
        {
            roomWidth = Random.Range(RoomGenerator.Instance.roomLowerBound, RoomGenerator.Instance.roomUpperBound);
            roomHeight = Random.Range(RoomGenerator.Instance.roomLowerBound, RoomGenerator.Instance.roomUpperBound);
        }
        
        SetGateways();
    }


    private void SetGateways()
    {
        float top = ((roomHeight - 1) % 2 == 0) ? roomHeight / 2 : Mathf.Floor(roomHeight / 2) - 1;
        float bot = ((roomHeight - 1) % 2 == 0) ? roomHeight / 2 : Mathf.Floor(roomHeight / 2);
        float left = ((roomWidth - 1) % 2 == 0) ? roomWidth / 2 : Mathf.Floor(roomWidth / 2);
        float right = ((roomWidth - 1) % 2 == 0) ? roomWidth / 2 : Mathf.Floor(roomWidth / 2) - 1;

        north.transform.position += new Vector3(0, top, 0);
        east.transform.position += new Vector3(right, 0, 0);
        south.transform.position -= new Vector3(0, bot, 0);
        west.transform.position -= new Vector3(left, 0, 0);
    }

}
