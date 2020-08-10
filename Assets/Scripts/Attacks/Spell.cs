using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public string Name;
    public int ID;
    public float Damage;
    public float Cooldown;
    public float MissileSpeed;
    public bool Pierce;
    public bool Knockback;
    public Transform Shooter;
    public Vector2 Direction;
    public Sprite Icon;
    public Sprite Sprite;

    [SerializeField] private SoundType _travelSound;
    [SerializeField] private SoundType _landingSound;
    [SerializeField] Effect _deathEffect;
    [SerializeField] private float _maxLifetime = 10f;
    private float _birthTime;

    private Transform _transform;
    private Rigidbody2D _rb;
    private AudioManager _audioManager;

    private bool _dying;

    private void Awake()
    {
        _transform = transform;
        _birthTime = Time.time;    

        _rb = GetComponent<Rigidbody2D>();
        _audioManager = AudioManager.Instance;   
    }

    private void Start()
    {
        // _travelSound = _audioManager.Play(SoundType.FireboltCrackle);
    }

    public void Fire(Vector2 direction)
    {
        _rb.AddForce(direction * MissileSpeed, ForceMode2D.Impulse);
        Direction = direction;
    }

    private void Update() {
        if (Time.time >= _birthTime + _maxLifetime) { Destroy(_transform.gameObject); }
    }

    public void SetShooter(Transform shooter)
    {
        Shooter = shooter;
        if (Shooter.gameObject.layer == 13) 
        {
            _transform.gameObject.layer = 13;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (_transform.IsChildOf(other.transform)) { return; }
        if (_transform.parent == other.transform.parent & _transform.parent != null) { return; }

        // aggro radius colliders
        if (other.gameObject.layer == 15) { return; }
        // terrain colliders
        if (other.gameObject.layer == 16) { Die(); }
        
        IDamageable<float> targetDmg = other.transform.GetComponentInChildren<IDamageable<float>>();
        if (targetDmg != null) {
            targetDmg.Damage(Damage);
        }

        if (!Pierce)
        {
            IKnockable knockableObject = other.transform.GetComponentInChildren<IKnockable>();
            if (knockableObject != null & Knockback) {
                knockableObject.Knockback(Direction * new Vector2(0.5f, 0.5f));
            }

            Die();
        }
    }

    private void Die()
    {
        if (_dying) { return; }
        _dying = true;

        _audioManager.PlayInstant(SoundType.Explosion, 0.3f);
        // _audioManager.Stop(_travelSound);

        if (_deathEffect != null)
        {
            Instantiate(_deathEffect, _transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

}
