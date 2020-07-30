using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public Vector2Int location;
    public int roomWidth;
    public int roomHeight;
    public int[] Gates = new int[4];
    public Gateway North;
    public Gateway East;
    public Gateway South;
    public Gateway West;
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

    private void Start()
    {
        RemoveExcessGateways();
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

        North.transform.position += new Vector3(0, top, 0) + baseOffsetVector;
        East.transform.position += new Vector3(right, 0, 0) + baseOffsetVector;
        South.transform.position -= new Vector3(0, bot, 0) - baseOffsetVector;
        West.transform.position -= new Vector3(left, 0, 0) - baseOffsetVector;
    }

    private void RemoveExcessGateways()
    {

        if (Gates[0] == 0) { Destroy(North.gameObject); }
        if (Gates[1] == 0) { Destroy(East.gameObject); }
        if (Gates[2] == 0) { Destroy(South.gameObject); }
        if (Gates[3] == 0) { Destroy(West.gameObject); }
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
