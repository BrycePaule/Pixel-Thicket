using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackManager : MonoBehaviour
{
    private static RangedAttackManager _instance;

    public Camera Camera;
    public GameObject[] RangedAttacks;

    private Dictionary<int, GameObject> SpellLookup = new Dictionary<int, GameObject>();
    
    private Transform _transform;

    private float shootCooldown;

    public static RangedAttackManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<RangedAttackManager>();
                if (_instance == null)
                {
                    _instance = new RangedAttackManager();
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
    
    public GameObject GetAttackByID(int ID)
    {      
        return Instantiate(SpellLookup[ID]);
    }

    public void BuildIDLookup()
    {
        foreach (var attack in RangedAttacks)
        {
            SpellLookup.Add(attack.GetComponent<RangedAttack>().ID, attack);
        }
    }

}