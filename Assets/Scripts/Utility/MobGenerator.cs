using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobGenerator : MonoBehaviour
{
    
    [SerializeField] private Mob[] _mobPrefabs;

    private Dictionary<MobTypes, Mob> _mobs;

    private static MobGenerator _instance;

    public static MobGenerator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MobGenerator>();
                if (_instance == null)
                {
                    _instance = new MobGenerator();
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
