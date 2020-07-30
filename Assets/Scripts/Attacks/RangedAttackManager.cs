using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackManager : MonoBehaviour
{

    public GameObject SpellPrefab;
    public Camera Camera;
    public RangedAttackSO[] RangedAttacks;

    public Dictionary<int, RangedAttackSO> IDLookup;

    private Transform _transform;
    private Transform _playerTransform;
    private PlayerInput _playerInput;
    private float shoot = 0f;

    private float shootCooldown;


    private void Awake() 
    {
        _transform = transform;
        IDLookup = new Dictionary<int, RangedAttackSO>();
    }

    private void Start()
    {
        _playerTransform = _transform.GetComponentInParent<Transform>();
        _playerInput = _transform.GetComponentInParent<Player>()._playerInput;
        BuildIDLookup();
        shootCooldown = Time.time;
    }
    
    private void FixedUpdate()
    {
        shoot = _playerInput.Attack.Shoot.ReadValue<float>();

        if (shoot == 0f) { return; }
        if (Time.time >= shootCooldown)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        // Gets mousePos in world coordinates, subtracts player position and normalises value
        // (this removes faster bullets if you click further away)
        Vector2 mousePosRaw = Camera.ScreenToWorldPoint(_playerInput.Mouse.Position.ReadValue<Vector2>());
        Vector3 mousePos = new Vector3(mousePosRaw.x, mousePosRaw.y, 0f);
        Vector3 direction = mousePos - _transform.position;
        direction.Normalize();

        // Calculates the shooting angle and creates a rotation from it
        float shootAngle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        Quaternion rotation = Quaternion.Euler(0, 0, shootAngle - 35f);

        GameObject projectile = GetAttackByIndex(0);
        RangedAttack projectileStats = projectile.GetComponent<RangedAttack>();
        Rigidbody2D rb = projectile.transform.GetComponent<Rigidbody2D>();
            
        projectile.name = projectileStats.Name;
        projectile.transform.position = _playerTransform.position;
        projectile.transform.rotation = rotation;

        float force = projectileStats.MissileSpeed;
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        // set cooldown time
        shootCooldown = Time.time + projectileStats.Cooldown;
    }

    public GameObject GetAttackByIndex(int index)
    {
        GameObject newSpell = Instantiate(SpellPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        RangedAttack spellData = newSpell.GetComponent<RangedAttack>();
        spellData.Shooter = _transform;
        spellData.RangedAttackType = IDLookup[index];
        spellData.SetSOData();
        
        return newSpell;
    }

    public void BuildIDLookup()
    {
        foreach (var attack in RangedAttacks)
        {
            IDLookup.Add(attack.ID, attack);
        }
    }

}