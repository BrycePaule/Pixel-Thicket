using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private CircleCollider2D _aggroRadius;
    [SerializeField] private Animator _animator;
    [Space(10)]
    [SerializeField] private float _moveSpeedAggro;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private MovementStyle _moveStyle;
    [SerializeField] private MovementPattern _movePattern;
    [SerializeField] [Range(0, 3)] private float _hopCooldown;
    [Space(10)]

    // TESTING
    [SerializeField] private Player _player;

    private Transform _transform;
    private bool _aggro = false;
    private bool _hopWait;

    private Vector2 _dest;
    private Vector2 _direction;
    private bool _destArrived = true;

    // UNITY METHODS
    private void Awake() 
    {
        _transform = transform;
    }

    public virtual void Start() 
    {
        // _direction = Vector2.zero;
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
                // _animator.speed =
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
        if (_movePattern == MovementPattern.FollowPlayer)
        {
            _dest = _player.GetComponent<Rigidbody2D>().position;
        }

        else if (_movePattern == MovementPattern.RandomLocation)
        {
            _dest = new Vector2((int) Random.Range(-10, 10), (int) Random.Range(-10, 10));
        }
        
    }

    private void SetDirection()
    {
        _direction = _dest - _rb.position;
        _direction.Normalize();

        // Update direction in animator
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
    }

    private IEnumerator HopWait()
    {
        _hopWait = true;
        yield return new WaitForSeconds(_hopCooldown);
        _hopWait = false;
    }
    
    // UTILITIES
    private enum MovementStyle
    {
        Standard,
        Hop
    }

    private enum MovementPattern
    {
        RandomLocation,
        FollowPlayer,
    }
}
