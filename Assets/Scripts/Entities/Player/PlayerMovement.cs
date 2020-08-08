using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private ParticleSystem _leafParticle;
    [SerializeField] private ParticleSystem _dashParticle;

    [Space(10)]
    [SerializeField] public float MoveSpeed;
    [SerializeField] public float SprintMultiplier;
    [SerializeField] public float DashSpeed;
    [SerializeField] public float DashTime;
    [SerializeField] public float DashCooldown;

    private Vector2 _moveAxis;
    private Vector2 _faceDirection;
    private bool _sprinting;
    private bool _dashing;
    private float _dashPress;
    private float _dashTimeLeft;
    private float _dashCooldownReset;
    private bool _idleChecking;

    private GameEventManager _gameEventManager;
    private AudioManager _audioManager;
    private InputManager _inputManager;
    private Rigidbody2D _rb;
    private Animator _animator;

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _animator = GetComponentInParent<Animator>();

        _audioManager = AudioManager.Instance;
        _inputManager = InputManager.Instance;

        _gameEventManager = GameEventManager.Instance;
        _gameEventManager.onDashPress += OnDashPress;
        _gameEventManager.onSprintPress += OnSprintPress;
        _gameEventManager.onSprintRelease += OnSprintRelease;
    }

    private void Update()
    {
        _moveAxis = _inputManager.PlayerMoveAxis;

        CalculateDirection();
        UpdateAnimatorMovement();
    }

    private void FixedUpdate() 
    {
        if (_dashPress != 0 | _dashing)
        {
            Dash();
        }

        if (_moveAxis != Vector2.zero & !_dashing) {
            Move();
        }

        if (!_idleChecking)
        {
            CheckIdle();
        }
    }

    private void Move()
    {   
        _animator.SetBool("Idle", false);
        StopCoroutine(IdleWaitCheck());

        float timeDelta = Time.deltaTime;
        _leafParticle.Play();

        if (_sprinting)
        {
            _rb.position += new Vector2(_moveAxis.x * MoveSpeed * SprintMultiplier * timeDelta, _moveAxis.y * MoveSpeed * SprintMultiplier * timeDelta);
        }
        else
        {
            _rb.position += new Vector2(_moveAxis.x * MoveSpeed * timeDelta, _moveAxis.y * MoveSpeed * timeDelta);
        }
    }

    private void Dash()
    {
        // start dash
        if (!_dashing & _moveAxis != Vector2.zero)
        {
            if (Time.time < _dashCooldownReset) { return; }

            _dashing = true;
            _dashTimeLeft = DashTime;
            _dashCooldownReset = Time.time + DashCooldown;

            _rb.AddForce(new Vector2(_moveAxis.x * DashSpeed, _moveAxis.y * DashSpeed), ForceMode2D.Impulse);
            _audioManager.PlayInstant(SoundType.Dash);
        }

        // continue dash
        else if (_dashing)
        {
            _dashTimeLeft -= Time.deltaTime;
            _dashParticle.Play();
        }

        // end dash
        if (_dashing & _dashTimeLeft <= 0)
        {
            StopAllMovement();
        }
    }

    public void StopAllMovement()
    {
        _rb.velocity = Vector2.zero;
        _dashing = false;
        _dashPress = 0;
    }

    public void CheckIdle()
    {
        if (_moveAxis == Vector2.zero)
        {
            StartCoroutine(IdleWaitCheck());
        }
    }

    private void CalculateDirection()
    {
        Vector2 targetWorldPos = new Vector2(_inputManager.MousePosWorld.x, _inputManager.MousePosWorld.y);
        Vector2 direction = targetWorldPos - _rb.position;
        direction.Normalize();

        _faceDirection = direction;
    }

    public IEnumerator IdleWaitCheck()
    {
        _idleChecking = true;
        yield return new WaitForSeconds(10);
        _idleChecking = false;

        if (_moveAxis == Vector2.zero)
        {
            _animator.SetBool("Idle", true);
        }
    } 

    // DRAWING
    private void UpdateAnimatorMovement()
    {
        _animator.SetFloat("Horizontal", _faceDirection.x);
        _animator.SetFloat("Vertical", _faceDirection.y);
        _animator.SetFloat("Speed", Mathf.Max(Mathf.Abs(_moveAxis.x), Mathf.Abs(_moveAxis.y)));
    }

    // EVENTS
    private void OnDashPress() => _dashPress = 1;
    private void OnSprintPress() => _sprinting = true; 
    private void OnSprintRelease() => _sprinting = false;
}
