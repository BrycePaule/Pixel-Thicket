using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobHealth : MonoBehaviour, IDamageable<float>, IKnockable, IKillable
{

    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private Canvas _healthBarCanvas;

    [Space(10)]
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _health;

    private Transform _transform;
    private Rigidbody2D _rb;
    private Animator _animator;
    private Mob _mob;

    private void Awake() 
    {
        _transform = transform;

        _mob = GetComponentInParent<Mob>();
        _rb = GetComponentInParent<Rigidbody2D>();
        _animator = GetComponentInParent<Animator>();
    }

    private void OnEnable()
    {
        if (_health == _maxHealth)
        {
            DisableHealthbar();
        }
    }

    private void Start() 
    {
        _healthBar.SetMaxHealth(_health);
    }

    public void Damage(float damageTaken)
    {
        _animator.SetTrigger("Hit");
        _mob.Hit = true;

        _health = Mathf.Clamp(_health - damageTaken, 0, 99999);
        _healthBar.SetHealth(_health);

        if (_health <= 0) { Kill(); }

        if (!_mob.Aggro) 
        {
            _mob.Aggro = true; 
        }

        if (_health != _maxHealth) 
        { 
            EnableHealthbar(); 
        }
    }

    public void Knockback(Vector2 force)
    {
        _rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void Kill() => Destroy(_mob.gameObject);

    // UI
    public void EnableHealthbar() => _healthBarCanvas.enabled = true;

    public void DisableHealthbar() => _healthBarCanvas.enabled = false;
}
