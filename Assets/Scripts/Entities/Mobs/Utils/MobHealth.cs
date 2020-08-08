using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobHealth : MonoBehaviour, IDamageable<float>, IKnockable, IKillable
{

    [SerializeField] private HealthBar _healthBar;

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

    private void Start() 
    {
        _healthBar.SetMaxHealth(_health);
        _mob.DisableHealthbar();
    }

    public void Damage(float damageTaken)
    {
        _animator.SetTrigger("Hit");
        _mob._hit = true;

        _health = Mathf.Clamp(_health - damageTaken, 0, 99999);
        _healthBar.SetHealth(_health);

        if (_health <= 0) { Kill(); }

        if (!_mob._aggro) 
        {
            _mob._aggro = true; 
        }

        if (_health != _maxHealth) 
        { 
            _mob.EnableHealthbar(); 
        }
    }

    public void Knockback(Vector2 force)
    {
        _rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void Kill() => Destroy(_mob.gameObject);

}
