using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public int x;
    public int y;
    public int roomWidth;
    public int roomHeight;
    public Gateway north;
    public Gateway east;
    public Gateway south;
    public Gateway west;
    public Vector3 _northSpawn;
    public Vector3 _eastSpawn;
    public Vector3 _southSpawn;
    public Vector3 _westSpawn;

    private Transform  _transform;

    private void Awake() 
    {

        _transform = transform;
        
        if (!_transform.name.Contains("Lobby")) 
        {
            roomWidth = Random.Range(RoomGenerator.Instance.roomLowerBound, RoomGenerator.Instance.roomUpperBound);
            roomHeight = Random.Range(RoomGenerator.Instance.roomLowerBound, RoomGenerator.Instance.roomUpperBound);
        }
        
        SetGateways();
        SetPlayerSpawnsInsideGateway();
    }

    private void OnEnable()
    {
        Transform[] children = _transform.GetComponentsInChildren<Transform>(true);
        foreach (var child in children)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        Transform[] children = _transform.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            child.gameObject.SetActive(false);
        }
    }


    private void SetGateways()
    {
        float tileAnchorOffset = 0.5f;
        Vector3 baseOffsetVector = new Vector3(tileAnchorOffset, tileAnchorOffset, 0f);

        float top = ((roomHeight - 1) % 2 == 0) ? roomHeight / 2 : Mathf.Floor(roomHeight / 2) - 1;
        float right = ((roomWidth - 1) % 2 == 0) ? roomWidth / 2 : Mathf.Floor(roomWidth / 2) - 1;
        float bot = ((roomHeight - 1) % 2 == 0) ? roomHeight / 2 : Mathf.Floor(roomHeight / 2);
        float left = ((roomWidth - 1) % 2 == 0) ? roomWidth / 2 : Mathf.Floor(roomWidth / 2);

        north.transform.position += new Vector3(0, top, 0) + baseOffsetVector;
        east.transform.position += new Vector3(right, 0, 0) + baseOffsetVector;
        south.transform.position -= new Vector3(0, bot, 0) - baseOffsetVector;
        west.transform.position -= new Vector3(left, 0, 0) - baseOffsetVector;
    }

    private void SetPlayerSpawnsInsideGateway()
    {
        // TERRIBLE IMPLEMENTATION BECAUSE I DON'T KNOW WHY LOBBY IS WORKING DIFFERENTLY

        float tileAnchorOffset = 0.5f;
        Vector3 baseOffsetVector = new Vector3(tileAnchorOffset, tileAnchorOffset, 0f);

        float gatewayTop = ((roomHeight - 1) % 2 == 0) ? roomHeight / 2 : Mathf.Floor(roomHeight / 2) - 1;
        float gatewayRight = ((roomWidth - 1) % 2 == 0) ? roomWidth / 2 : Mathf.Floor(roomWidth / 2) - 1;
        float gatewayBot = ((roomHeight - 1) % 2 == 0) ? roomHeight / 2 : Mathf.Floor(roomHeight / 2);
        float gatewayLeft = ((roomWidth - 1) % 2 == 0) ? roomWidth / 2 : Mathf.Floor(roomWidth / 2);

        if (!_transform.name.Contains("Lobby")) 
        {
            _northSpawn = new Vector3(0, gatewayTop - 1.5f, 0) + baseOffsetVector;
            _eastSpawn = new Vector3(gatewayRight - 1.5f, 0, 0) + baseOffsetVector;
            _southSpawn = new Vector3(0, -gatewayBot + 1.5f, 0) + baseOffsetVector;
            _westSpawn = new Vector3(-gatewayLeft + 1.5f, 0, 0) + baseOffsetVector;
        }

        if (_transform.name.Contains("Lobby")) 
        {
            _northSpawn = new Vector3(0, gatewayTop - 2.5f, 0) + baseOffsetVector;
            _eastSpawn = new Vector3(gatewayRight - 1.5f, -1, 0) + baseOffsetVector;
            _southSpawn = new Vector3(0, -gatewayBot + 0.5f, 0) + baseOffsetVector;
            _westSpawn = new Vector3(-gatewayLeft + 1.5f, -1, 0) + baseOffsetVector;
        }


    }
}
