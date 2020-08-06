using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackManager : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;

    public Camera Camera;
    public RangedAttack[] RangedAttacks;

    public Dictionary<int, RangedAttack> IDLookup = new Dictionary<int, RangedAttack>();
    
    private Transform _transform;
    private GameEventSystem _gameEventSystem;
    private InputHandler _inputHandler;
    private Transform _playerTransform;
    private PlayerInput _playerInput;
    private float shoot = 0f;

    private float shootCooldown;

    private void Awake() 
    {
        _transform = transform;

        _inputHandler = GetComponentInParent<InputHandler>();

        _gameEventSystem = GameEventSystem.Instance;

        _gameEventSystem.onShootPress += context => OnShootPress(context);
    }

    private void Start()
    {
        _playerTransform = _transform.GetComponentInParent<Transform>();
        _playerInput = _transform.GetComponentInParent<Player>()._playerInput;
        BuildIDLookup();
        shootCooldown = Time.time;
    }
    
    private void Shoot(Vector2 mousePos)
    {
        RangedAttack selectedSpell = _inventory.SelectedWeapon();
        if (selectedSpell == null) { return; }
        
        Vector3 mousePosWorld = Camera.ScreenToWorldPoint(mousePos);
        mousePosWorld.z = 1f;
        Vector3 direction = mousePosWorld - _transform.position;
        direction.Normalize();

        // Calculates the shooting angle and creates a rotation from it
        float shootAngle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        Quaternion rotation = Quaternion.Euler(0, 0, shootAngle - 35f);

        GameObject projectileObject = GetAttackByIndex(selectedSpell.ID);
        RangedAttack projectile = projectileObject.GetComponent<RangedAttack>();
        Rigidbody2D rb = projectileObject.transform.GetComponent<Rigidbody2D>();
            
        projectile.name = projectile.Name;
        projectile.transform.position = _playerTransform.position;
        projectile.transform.rotation = rotation;

        float force = projectile.MissileSpeed;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
        projectile.Direction = direction * force;

        // set cooldown time
        shootCooldown = Time.time + projectile.Cooldown;
    }

    public GameObject GetAttackByIndex(int index)
    {
        GameObject newSpell = Instantiate(IDLookup[index].gameObject, new Vector3(0, 0, 0), Quaternion.identity);
        newSpell.GetComponent<RangedAttack>().SetShooter(_transform);
        
        return newSpell;
    }

    public void BuildIDLookup()
    {
        foreach (var attack in RangedAttacks)
        {
            IDLookup.Add(attack.ID, attack);
        }
    }

    // EVENTS
    private void OnShootPress(Vector2 mousePos)
    {
        if (Time.time >= shootCooldown) 
        { 
            Shoot(mousePos);
        }
    }

}