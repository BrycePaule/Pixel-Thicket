using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{

    public SpriteRenderer SR;

    [Header("These should be null")]
    public RangedAttackSO RangedAttackType;
    public string Name;
    public float Damage;
    public float Cooldown;
    public float MissileSpeed;
    public bool Pierce;

    public Transform Shooter;
    public Vector2 Direction;

    private float _maxLifetime = 10f;
    private float _birthTime;

    private AudioManager _audioManager;

    // public Sprite Icon;
    // public Sprite Sprite;

    private Transform _transform;


    private void Awake()
    {
        _transform = transform;
        _birthTime = Time.time;    

        _audioManager = AudioManager.Instance;   
    }

    private void Start() 
    {
        ParticleSystem[] particles = _transform.GetComponentsInChildren<ParticleSystem>(true);
        foreach (var particle in particles)
        {
            if (particle.name == Name)
            {
                particle.gameObject.SetActive(true);
            }
        }
    }

    private void Update() {
        if (Time.time >= _birthTime + _maxLifetime) { Destroy(_transform.gameObject); }
    }

    public void SetSOData()
    {
        Name = RangedAttackType.Name;
        Damage = RangedAttackType.Damage;
        Cooldown = RangedAttackType.Cooldown;
        MissileSpeed = RangedAttackType.MissileSpeed;
        Pierce = RangedAttackType.Pierce;

        SR.sprite = RangedAttackType.Sprite;

        if (Shooter.gameObject.layer == 13) {
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
        if (other.gameObject.layer == 16) { Destroy(this.gameObject); }
        
        IDamageable<float> targetDmg = other.transform.GetComponent<IDamageable<float>>();
        if (targetDmg != null) {
            targetDmg.Damage(Damage);
        }

        if (!Pierce)
        {
            IKnockable targetKnock = other.transform.GetComponent<IKnockable>();
            if (targetKnock != null) {
                targetKnock.Knockback(Direction * new Vector2(0.5f, 0.5f));
            }

            _audioManager.Play(SoundTypes.Explosion, 0.3f);
            Destroy(this.gameObject);
        }
    }

}
