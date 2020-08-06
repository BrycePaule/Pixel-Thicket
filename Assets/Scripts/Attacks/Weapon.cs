using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Inventory _inventory;

    private GameEventManager _gameEventManager;
    private RangedAttackManager _rangedAttackManager;
    private Transform _transform;

    private float _shootCooldown;

    private void Awake()
    {
        _transform = transform;
        _gameEventManager = GameEventManager.Instance;
        _rangedAttackManager = RangedAttackManager.Instance;

        _gameEventManager.onShootPress += context => OnShootPress(context);
    }

    private bool Shoot(Vector3 target)
    {
        RangedAttack selectedSpell = _inventory.SelectedWeapon();
        if (selectedSpell == null) { return false; }
        
        Vector3 direction = target - _transform.position;
        direction.Normalize();

        float shootAngle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        Quaternion rotation = Quaternion.Euler(0, 0, shootAngle - 35f);

        GameObject projectile = _rangedAttackManager.GetAttackByIndex(selectedSpell.ID);
        RangedAttack projectileRA = projectile.GetComponent<RangedAttack>();
            
        projectile.transform.position = _transform.position;
        projectile.transform.rotation = rotation;
        projectileRA.Fire(direction);

        // set cooldown time
        _shootCooldown = Time.time + projectileRA.Cooldown;

        return true;
    }

    // EVENTS
    private void OnShootPress(Vector2 mousePos)
    {
        if (Time.time >= _shootCooldown) 
        { 
            Vector3 mousePosWorld = _camera.ScreenToWorldPoint(mousePos);
            mousePosWorld.z = 0f;

            Shoot(mousePosWorld);
        }
    }
}
