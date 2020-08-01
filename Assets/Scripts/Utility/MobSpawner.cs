using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{

    [SerializeField] private Mob[] _mobPrefabs;

    private Dictionary<MobTypes, Mob> _mobs;

    private static MobSpawner _instance;

    public static MobSpawner Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MobSpawner>();
                if (_instance == null)
                {
                    _instance = new MobSpawner();
                }
            }
            return _instance;
        }
    }

    // UNITY METHODS
    private void Awake()
    {
        if (_instance != null) { Destroy(this.gameObject); }

        _mobs = new Dictionary<MobTypes, Mob>();
        FillMobsDictionary();
    }

    // SPAWNING
    public Mob Spawn(MobTypes mobType)
    {
        Mob newMob = Instantiate(_mobs[mobType]);
        return newMob;
    }

    // UTILITIES
    private void FillMobsDictionary()
    {
        _mobs.Add(MobTypes.Slime, _mobPrefabs[1]);
        _mobs.Add(MobTypes.Shade, _mobPrefabs[0]);
    }

}
