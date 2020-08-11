using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Inventory _inventory;

    private GameEventManager _gameEventManager;
    private SpellManager _spellManager;
    private Transform _transform;
    private GlobalContainer _globalContainer;

    private float _shootCooldown;

    private void Awake()
    {
        _transform = transform;
        _gameEventManager = GameEventManager.Instance;
        _spellManager = SpellManager.Instance;
        _globalContainer = GlobalContainer.Instance;

        _gameEventManager.onShootPress += context => OnShootPress(context);
    }

    private bool Shoot(Vector3 mousePosWorld)
    {
        float angleOffset = 90f;

        Spell currSpell = _inventory.SelectedWeapon();
        if (currSpell == null) { return false; }
        
        Vector3 direction = mousePosWorld - _transform.position;
        direction.Normalize();

        float shootAngle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        Quaternion rotation = Quaternion.Euler(0, 0, shootAngle - angleOffset);

        GameObject projectile = Instantiate(_spellManager.GetSpell(currSpell.ID));
        Spell projectileRA = projectile.GetComponent<Spell>();
            
        projectile.transform.position = _transform.position;
        projectile.transform.rotation = rotation;
        projectile.transform.parent = _globalContainer.SpellContainer;
        projectileRA.SetShooter(_transform);
        projectileRA.Fire(direction);

        // set cooldown time
        _shootCooldown = Time.time + projectileRA.Cooldown;

        return true;
    }

    // EVENTS
    private void OnShootPress(Vector3 mousePosWorld)
    {
        if (Time.time >= _shootCooldown) 
        { 
            Shoot(mousePosWorld);
        }
    }
}
