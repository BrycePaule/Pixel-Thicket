using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] public Transform MobContainer;
    [SerializeField] public Transform LightContainer;

    [Space(10)]
    public Vector2Int location;
    public int Width;
    public int Height;

    [Space(10)]
    public int[] Gates = new int[4];
    public Gateway North;
    public Gateway East;
    public Gateway South;
    public Gateway West;
    public Gateway NorthBlock;
    public Gateway EastBlock;
    public Gateway SouthBlock;
    public Gateway WestBlock;
    public Vector3 NorthSpawn;
    public Vector3 EastSpawn;
    public Vector3 SouthSpawn;
    public Vector3 WestSpawn;

    [Space(10)]
    public List<Mob> Mobs = new List<Mob>();
    public List<Vector2> MobSpawnLocations = new List<Vector2>();
    public int MobCount;

    private Transform  _transform;
    private MobGenerator _mobGenerator;

    private void Awake() 
    {
        _transform = transform;
        
        Width = Random.Range(RoomGenerator.Instance.roomLowerBound, RoomGenerator.Instance.roomUpperBound);
        Height = Random.Range(RoomGenerator.Instance.roomLowerBound, RoomGenerator.Instance.roomUpperBound);

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

        Mob[] mobs = MobContainer.GetComponentsInChildren<Mob>(true);
        foreach (var mob in mobs)
        {
            Vector2 loc = MobSpawnLocations[Random.Range(0, MobSpawnLocations.Count)];
            mob.transform.position = new Vector3(loc.x, loc.y, 0);
            mob.GetComponentInChildren<MobMovement>().StartCoroutine("MoveWait", Random.Range(0f, 2f));
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

        float top = ((Height - 1) % 2 == 0) ? Height / 2 : Mathf.Floor(Height / 2) - 1;
        float right = ((Width - 1) % 2 == 0) ? Width / 2 : Mathf.Floor(Width / 2) - 1;
        float bot = ((Height - 1) % 2 == 0) ? Height / 2 : Mathf.Floor(Height / 2);
        float left = ((Width - 1) % 2 == 0) ? Width / 2 : Mathf.Floor(Width / 2);

        North.transform.position += new Vector3(0, top, 0) + baseOffsetVector;
        East.transform.position += new Vector3(right, 0, 0) + baseOffsetVector;
        South.transform.position -= new Vector3(0, bot, 0) - baseOffsetVector;
        West.transform.position -= new Vector3(left, 0, 0) - baseOffsetVector;

        NorthBlock.transform.position += new Vector3(0, top + 1, 0) + baseOffsetVector;
        EastBlock.transform.position += new Vector3(right + 1, 0, 0) + baseOffsetVector;
        SouthBlock.transform.position -= new Vector3(0, bot + 1, 0) - baseOffsetVector;
        WestBlock.transform.position -= new Vector3(left + 1, 0, 0) - baseOffsetVector;
    }

    private void RemoveExcessGateways()
    {
        if (Gates[0] == 0) 
        { 
            Destroy(North.gameObject); 
            Destroy(NorthBlock.gameObject); 
        }

        if (Gates[1] == 0) 
        {
            Destroy(East.gameObject); 
            Destroy(EastBlock.gameObject); 
        }

        if (Gates[2] == 0) 
        {
            Destroy(South.gameObject); 
            Destroy(SouthBlock.gameObject); 
        }

        if (Gates[3] == 0) 
        {
            Destroy(West.gameObject); 
            Destroy(WestBlock.gameObject); 
        }
    }

    private void SetPlayerSpawnsInsideGateway()
    {
        // TERRIBLE IMPLEMENTATION BECAUSE I DON'T KNOW WHY LOBBY IS WORKING DIFFERENTLY

        float tileAnchorOffset = 0.5f;
        Vector3 baseOffsetVector = new Vector3(tileAnchorOffset, tileAnchorOffset, 0f);

        float gatewayTop = ((Height - 1) % 2 == 0) ? Height / 2 : Mathf.Floor(Height / 2) - 1;
        float gatewayRight = ((Width - 1) % 2 == 0) ? Width / 2 : Mathf.Floor(Width / 2) - 1;
        float gatewayBot = ((Height - 1) % 2 == 0) ? Height / 2 : Mathf.Floor(Height / 2);
        float gatewayLeft = ((Width - 1) % 2 == 0) ? Width / 2 : Mathf.Floor(Width / 2);

        if (!_transform.name.Contains("Lobby")) 
        {
            NorthSpawn = new Vector3(0, gatewayTop - 1.5f, 0) + baseOffsetVector;
            EastSpawn = new Vector3(gatewayRight - 1.5f, 0, 0) + baseOffsetVector;
            SouthSpawn = new Vector3(0, -gatewayBot + 1.5f, 0) + baseOffsetVector;
            WestSpawn = new Vector3(-gatewayLeft + 1.5f, 0, 0) + baseOffsetVector;
        }

        if (_transform.name.Contains("Lobby")) 
        {
            NorthSpawn = new Vector3(0, gatewayTop - 2.5f, 0) + baseOffsetVector;
            EastSpawn = new Vector3(gatewayRight - 1.5f, -1, 0) + baseOffsetVector;
            SouthSpawn = new Vector3(0, -gatewayBot + 0.5f, 0) + baseOffsetVector;
            WestSpawn = new Vector3(-gatewayLeft + 1.5f, -1, 0) + baseOffsetVector;
        }


    }
}
