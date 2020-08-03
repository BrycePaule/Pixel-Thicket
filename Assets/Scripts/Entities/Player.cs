using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class Player : MonoBehaviour, IDamageable<float>, IKillable
{
        
    public float MaxHealth;
    public float Health;
    
    public bool GatewayTravelLocked;

    [Space(10)]
    public PlayerInput _playerInput;
    [SerializeField] private Animator _animator;
    [SerializeField] public Rigidbody2D _rb;
    [SerializeField] private ParticleSystem _leafParticle;
    [SerializeField] private ParticleSystem _dashParticle;
    [SerializeField] private Camera _camera;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private GameEventSystem _gameEventSystem;

    [Space(10)]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;

    private Vector2 _faceDirection;
    private bool _idle;
    private bool _idleChecking;

    private float _sprinting;
    private float _dashPress;
    private float dashTimeCounter;
    private float dashCooldownTracker;
    private bool _dashing;

    private Transform _transform;
    private Vector2 _moveAxis;
    private AudioManager _audioManager;

    void OnEnable()
    { 
        _playerInput.Enable();
    }

    void OnDisable()
    { 
        _playerInput.Disable(); 
    }

    private void Awake()
    {
        _transform = transform;
        _playerInput = new PlayerInput();
        _playerInput.Enable();

        _audioManager = AudioManager.Instance;

        _healthBar = GetComponentInChildren<HealthBar>();
        _gameEventSystem = FindObjectOfType<GameEventSystem>();
    }

    private void Start()
    {
        _healthBar.SetMaxHealth(Health);
    }

    private void Update()
    {
        _moveAxis = _playerInput.Movement.Move.ReadValue<Vector2>();
        _sprinting = _playerInput.Movement.Sprint.ReadValue<float>();
        _dashPress = _playerInput.Movement.Dash.ReadValue<float>();

        Vector2 mousePos = _camera.ScreenToWorldPoint(_playerInput.Mouse.Position.ReadValue<Vector2>());
        Vector2 direction = mousePos - _rb.position;
        direction.Normalize();
        _animator.SetFloat("Horizontal", direction.x);
        _animator.SetFloat("Vertical", direction.y);
        
        _animator.SetFloat("Speed", Mathf.Max(Mathf.Abs(_moveAxis.x), Mathf.Abs(_moveAxis.y)));
    }

    private void FixedUpdate() {
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

    // PLAYER MOVEMENT
    private void Move()
    {   
        _idle = false;
        _animator.SetBool("Idle", false);

        float timeDelta = Time.deltaTime;
        _leafParticle.Play();

        if (_sprinting != 0f)
        {
            _rb.position += new Vector2(_moveAxis.x * moveSpeed * sprintMultiplier * timeDelta, _moveAxis.y * moveSpeed * sprintMultiplier * timeDelta);
        }
        else
        {
            _rb.position += new Vector2(_moveAxis.x * moveSpeed * timeDelta, _moveAxis.y * moveSpeed * timeDelta);
        }
    }

    private void Dash()
    {
        // start dash
        if (!_dashing & _moveAxis != Vector2.zero)
        {
            if (Time.time <= dashCooldownTracker) { return; }

            _dashing = true;
            dashTimeCounter = dashTime;
            dashCooldownTracker = Time.time + dashCooldown;
            _rb.AddForce(new Vector2(_moveAxis.x * dashSpeed, _moveAxis.y * dashSpeed), ForceMode2D.Impulse);
            _audioManager.Play(SoundType.Dash);
        }

        // continue dash
        else if (_dashing)
        {
            dashTimeCounter -= Time.deltaTime;
            _dashParticle.Play();
        }

        // end dash
        if (_dashing & dashTimeCounter <= 0)
        {
            _rb.velocity = Vector2.zero;
            _dashing = false;
        }
    }

    public void StopAllMovement()
    {
        _rb.velocity = Vector2.zero;
        _dashing = false;
    }

    public void CheckIdle()
    {
        if (_moveAxis == Vector2.zero)
        {
            StartCoroutine("IdleWait");
        }
    }

    public IEnumerator GatewayLock()
    {
        GatewayTravelLocked = true;

        yield return new WaitForSeconds(3);

        GatewayTravelLocked = false;
    } 

    public IEnumerator IdleWait()
    {
        _idleChecking = true;

        yield return new WaitForSeconds(10);

        _idleChecking = false;

        if (_moveAxis == Vector2.zero)
        {
            _idle = true;
            _animator.SetBool("Idle", true);
        }
    } 

    // HEALTH
    public void Damage(float damageTaken)
    {
        Health = ((Health - damageTaken) < 0) ? 0 : Health - damageTaken;
        _healthBar.SetHealth(Health);

        if (Health <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        _gameEventSystem.OnPlayerDeath();
    }

    // COLLISION
    private void OnTriggerEnter2D(Collider2D other) {
        // print(other.name);
    }
}
