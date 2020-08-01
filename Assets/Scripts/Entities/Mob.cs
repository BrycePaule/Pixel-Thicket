using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour, IDamageable<float>, IKillable, IKnockable
{

    public float MaxHealth;
    public float Health;
    public Animator _animator;

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private CircleCollider2D _aggroRadius;

    [Space(10)]
    [SerializeField] private float _moveSpeedAggro;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private MovementStyle _moveStyle;
    [SerializeField] private MovementPattern _movePattern;
    [SerializeField] private float _damage;

    // TESTING
    [Space(10)]
    [SerializeField] private Player _player;

    private Transform _transform;
    private bool _aggro = false;

    private Vector2 _dest;
    private Vector2 _direction;
    private bool _destArrived = true;

    // SLIME
    public bool _hopWait;

    // UNITY METHODS
    private void Awake() 
    {
        _transform = transform;
        _player = FindObjectOfType<Player>();
    }

    public virtual void Start() 
    {

    }

    private void Update()
    {
        Move();
    }

    // MOVEMENT
    public virtual void Move() {

        if (_movePattern == MovementPattern.FollowPlayer)
        {
            SetDestination();
            SetDirection();
        }

        else if (_movePattern == MovementPattern.RandomLocation)
        {
            CheckArrived();

            if (_destArrived | _dest == null | _direction == Vector2.zero) 
            { 
                SetDestination(); 
            }

            SetDirection();
            _destArrived = false;
        }

        if (_moveStyle == MovementStyle.Standard) 
        {
            _animator.SetBool("Hopping", false);

            if (_aggro)
            {
                _rb.position += new Vector2(_direction.x * _moveSpeedAggro * Time.deltaTime, _direction.y * _moveSpeedAggro * Time.deltaTime);
            }
            else
            {
                _rb.position += new Vector2(_direction.x * _moveSpeed * Time.deltaTime, _direction.y * _moveSpeed * Time.deltaTime);
            }

        }

        else if (_moveStyle == MovementStyle.Hop)
        {
            _animator.SetBool("Hopping", true);

            if (!_hopWait)
            {
                if (_aggro)
                {
                    _rb.position += new Vector2(_direction.x * _moveSpeedAggro * Time.deltaTime, _direction.y * _moveSpeedAggro * Time.deltaTime);
                }
                else
                {
                    _rb.position += new Vector2(_direction.x * _moveSpeed * Time.deltaTime, _direction.y * _moveSpeed * Time.deltaTime);
                }
            }
        } 
    }

    private void SetDestination()
    {

         if (_movePattern == MovementPattern.RandomLocation)
        {
            // IMPLEMENT PROPER ROOM BASED RANDOM DESTINATION
            _dest = new Vector2(Random.Range(-20, 20), Random.Range(-20, 20));
        }

        else if (_movePattern == MovementPattern.FollowPlayer)
        {
            _dest = _player.GetComponent<Rigidbody2D>().position;
        }


    }

    private void SetDirection()
    {
        _direction = _dest - _rb.position;
        _direction.Normalize();

        // Update face direction in animator
        _animator.SetFloat("Horizontal", _direction.x);
        _animator.SetFloat("Vertical", _direction.y);
    }

    private void CheckArrived()
    {
        if (Mathf.RoundToInt(_rb.position.x) != _dest.x) { return; }
        if (Mathf.RoundToInt(_rb.position.y) != _dest.y) { return; }

        _destArrived = true;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        _destArrived = true;

        if (other.gameObject.layer != 12) { return; } 

        IDamageable<float> target = other.transform.GetComponent<IDamageable<float>>();
        if (target == null) { return; }

        target.Damage(_damage);

    }
    
    // HEALTH
    public void Damage(float damageTaken)
    {

        _animator.SetTrigger("Hit");

        Health = ((Health - damageTaken) < 0) ? 0 : Health - damageTaken;

        if (Health <= 0)
        {
            Kill();
        }
    }

    public void Knockback(Vector2 force)
    {
        _rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void Kill()
    {
        Destroy(this.gameObject);
    }

    // CALLBACKS
    private void ShootFinish()
    {
        _animator.ResetTrigger("Shoot");
    }

    private void HitFinish()
    {
        _animator.ResetTrigger("Hit");
    }

    // UTILITIES
    public enum MovementStyle
    {
        Standard,
        Hop
    }

    public enum MovementPattern
    {
        RandomLocation,
        FollowPlayer,
    }
}
