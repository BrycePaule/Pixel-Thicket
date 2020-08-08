using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    private static SpellManager _instance;

    public Camera Camera;
    public GameObject[] Spells;

    private Dictionary<int, GameObject> SpellLookup = new Dictionary<int, GameObject>();
    
    private Transform _transform;

    private float shootCooldown;

    public static SpellManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SpellManager>();
                if (_instance == null)
                {
                    _instance = new SpellManager();
                }
            }
            return _instance;
        }
    }

    private void Awake() 
    {
        if (_instance != null) { Destroy(this.gameObject); }

        _transform = transform;
    }

    private void Start()
    {
        BuildIDLookup();
        shootCooldown = Time.time;
    }
    
    public GameObject GetSpell(int ID)
    {      
        return SpellLookup[ID];
    }

    public void BuildIDLookup()
    {
        foreach (var attack in Spells)
        {
            SpellLookup.Add(attack.GetComponent<Spell>().ID, attack);
        }
    }

}