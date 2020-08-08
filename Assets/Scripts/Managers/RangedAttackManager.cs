using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackManager : MonoBehaviour
{
    private static RangedAttackManager _instance;

    public Camera Camera;
    public GameObject[] RangedAttacks;

    public Dictionary<int, GameObject> IDLookup = new Dictionary<int, GameObject>();
    
    private Transform _transform;
    private InputManager _inputManager;

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

        _inputManager = GetComponentInParent<InputManager>();
    }

    private void Start()
    {
        BuildIDLookup();
        shootCooldown = Time.time;
    }
    
    // private bool Shoot(Transform shooter, Vector2 mousePos)
    // {
    //     RangedAttack selectedSpell = _inventory.SelectedWeapon();
    //     if (selectedSpell == null) { return false; }
        
    //     Vector3 mousePosWorld = Camera.ScreenToWorldPoint(mousePos);
    //     mousePosWorld.z = 0f;
    //     Vector3 direction = mousePosWorld - _transform.position;
    //     direction.Normalize();

    //     // Calculates the shooting angle and creates a rotation from it
    //     float shootAngle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    //     Quaternion rotation = Quaternion.Euler(0, 0, shootAngle - 35f);

    //     RangedAttack projectile = GetAttackByIndex(selectedSpell.ID);
            
    //     projectile.transform.position = shooter.position;
    //     projectile.transform.rotation = rotation;

    //     projectile.Fire(direction);

    //     // set cooldown time
    //     shootCooldown = Time.time + projectile.Cooldown;

    //     return true;
    // }

    public GameObject GetAttackByIndex(int index)
    {
        GameObject newSpell = Instantiate(IDLookup[index]);
        newSpell.GetComponent<RangedAttack>().SetShooter(_transform);
        
        return newSpell;
    }

    public void BuildIDLookup()
    {
        foreach (var attack in RangedAttacks)
        {
            IDLookup.Add(attack.GetComponent<RangedAttack>().ID, attack);
        }
    }

}