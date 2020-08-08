using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobMovement : MonoBehaviour
{
    public MovementPattern MovePattern;
    public MovementStyle _moveStyle;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _moveSpeedAggro;
    [SerializeField] private int _chanceToMoveWait;

    [Tooltip("Slime")]
    [SerializeField] [Range(0, 3)] private float _hopCooldown;
    [SerializeField] ParticleSystem _particle;

    private Vector2 _dest;
    private Vector2 _direction;
    public bool _destArrived = true;
    private bool _moveWait;

    // SLIME
    private bool _hopInAir;
    private bool _hopWait;

    private AudioManager _audioManager;
    private Transform _transform;
    private Animator _animator;
    private Rigidbody2D _rb;
    private Mob _mob;
    private Rigidbody2D _playerRB;

    private void Awake() 
    {
        _transform = transform;

        _audioManager = AudioManager.Instance;

        _mob = GetComponentInParent<Mob>();
        _rb = GetComponentInParent<Rigidbody2D>();
        _animator = GetComponentInParent<Animator>();
    }

    private void OnEnable()
    {
        _playerRB = FindObjectOfType<Player>().GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

    // MOVEMENT
    private void Move() 
    {
        if (_mob._hit) { return; }

        DoMovePattern();
        DoMoveStyle();

        UpdateAnimatorFaceDirection();
    }

    private void DoMovePattern()
    {
        // FOLLOW PLAYER
        if (MovePattern == MovementPattern.FollowPlayer)
        {
            _moveWait = false;
            _animator.SetBool("MoveWait", false);

            if (_hopInAir) { return; }

            SetDestination();
            SetDirection();
        }

        // RANDOM LOCATION
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
        }
    }

    private void DoMoveStyle()
    {
        if (_moveStyle == MovementStyle.Standard) 
        {
            _animator.SetBool("Hopping", false);

            if (_mob._aggro)
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

                if (_mob._aggro)
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

        _destArrived = false;
    }

    private void SetDirection()
    {
        _direction = _dest - _rb.position;
        _direction.Normalize();
    }

    private void UpdateAnimatorFaceDirection()
    {
        _animator.SetFloat("Horizontal", _direction.x);
        _animator.SetFloat("Vertical", _direction.y);
    }

    private void CheckArrived()
    {
        if (Mathf.RoundToInt(_rb.position.x) != _dest.x) { return; }
        if (Mathf.RoundToInt(_rb.position.y) != _dest.y) { return; }

        _destArrived = true;
    }
    
    private IEnumerator MoveWait(float seconds = 2f)
    {
        _moveWait = true;
        _animator.SetBool("MoveWait", true);

        yield return new WaitForSeconds(seconds);

        _moveWait = false;
        _animator.SetBool("MoveWait", false);
    }

    private IEnumerator HopEnd()
    {
        print("hop end");
        _hopWait = true;
        _hopInAir = false;

        _audioManager.PlayInstant(SoundType.SlimeBounce, 1, 1, true);
        _particle.Play();

        yield return new WaitForSeconds(_hopCooldown);
        _hopWait = false;
    }

}
