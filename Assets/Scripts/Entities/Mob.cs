using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour, IDamageable<float>, IKillable, IKnockable
{

    public float MaxHealth;
    public float Health;
    public Animator _animator;

    [SerializeField] private Rigidbody2D _rb;

    [Space(10)]
    [SerializeField] private float _moveSpeedAggro;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private MovementStyle _moveStyle;
    [SerializeField] public MovementPattern MovePattern;
    [SerializeField] private int _chanceToMoveWait;
    [SerializeField] private float _damage;

    // TESTING
    [Space(10)]
    [SerializeField] private Player _player;

    private Transform _transform;
    private Rigidbody2D _playerRB;
    private bool _aggro;
    private bool _hit;

    private Vector2 _dest;
    private Vector2 _direction;
    private bool _destArrived = true;
    private bool _moveWait;

    // SLIME
    protected bool _hopInAir;
    protected bool _hopWait;

    // UNITY METHODS
    private void Awake() 
    {
        _transform = transform;
    }

    public virtual void Start() 
    {

    }

    private void OnEnable()
    {
        _player = FindObjectOfType<Player>();
        _playerRB = _player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

    // MOVEMENT
    public virtual void Move() 
    {

        if (_hit) { return; }

        if (MovePattern == MovementPattern.FollowPlayer)
        {
            _moveWait = false;
            _animator.SetBool("MoveWait", false);

            if (!_hopInAir)
            {
                SetDestination();
                SetDirection();
            }
        }

        else if (MovePattern == MovementPattern.RandomLocation)
        {
            if (_moveWait) { return; }

            CheckArrived();

            if (_destArrived | _dest == null | _direction == Vector2.zero) 
            { 
                SetDestination();

                if (Roll.Chance(_chanceToMoveWait)) 
                { 
                    StartCoroutine("MoveWait", Random.Range(0f, 2f));
                }
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
                _hopInAir = true;

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
         if (MovePattern == MovementPattern.RandomLocation)
        {
            // IMPLEMENT PROPER ROOM BASED RANDOM DESTINATION
            _dest = new Vector2(Random.Range(-20, 20), Random.Range(-20, 20));
        }

        else if (MovePattern == MovementPattern.FollowPlayer)
        {
            _dest = _playerRB.position;
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
    
    public IEnumerator MoveWait(float seconds = 2f)
    {
        _moveWait = true;
        _animator.SetBool("MoveWait", true);

        yield return new WaitForSeconds(seconds);

        _moveWait = false;
        _animator.SetBool("MoveWait", false);
    }

    // COLLISION
    private void OnCollisionEnter2D(Collision2D other) 
    {
        _destArrived = true;

        if (other.gameObject.layer == 12) 
        {
            IDamageable<float> target = other.transform.GetComponent<IDamageable<float>>();
            if (target == null) { return; }

            target.Damage(_damage);
        } 
    }

    // HEALTH
    public void Damage(float damageTaken)
    {

        _animator.SetTrigger("Hit");
        _hit = true;
        if (!_aggro) { _aggro = true; }

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
        _hit = false;
    }
}
