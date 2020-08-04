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
    [SerializeField] private Inventory _inventory;

    [Space(10)]
    [SerializeField] private float healthRegen;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;

    [Space(10)]
    // TESTING
    [SerializeField] private RangedAttack FIREBALL;

    private Vector2 _faceDirection;
    private bool _idle;
    private bool _idleChecking;
    private bool _regenChecking;

    private bool _sprinting;
    private float _dashPress;
    private float dashTimeCounter;
    private float dashCooldownTracker;
    private bool _dashing;

    private float _inventoryPress;

    private Transform _transform;
    private Vector2 _moveAxis;
    private AudioManager _audioManager;
    private InputHandler _inputHandler;

    private void Awake()
    {
        _transform = transform;
        _gameEventSystem = GameEventSystem.Instance;
        _audioManager = AudioManager.Instance;
        _healthBar = GetComponentInChildren<HealthBar>();
        _inputHandler = GetComponent<InputHandler>();

        _gameEventSystem.onDashPress += OnDashPress;
        _gameEventSystem.onSprintPress += OnSprintPress;
        _gameEventSystem.onSprintRelease += OnSprintRelease;
    }

    private void Start()
    {
        _healthBar.SetMaxHealth(Health);
    }

    private void Update()
    {
        _moveAxis = _inputHandler.PlayerMoveAxis;

        CalculateDirection();
        UpdateAnimator();
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

        CheckIdle();
    }

    // PLAYER MOVEMENT
    private void Move()
    {   
        _idle = false;
        _animator.SetBool("Idle", false);

        float timeDelta = Time.deltaTime;
        _leafParticle.Play();

        if (_sprinting)
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
            _dashPress = 0;
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

    private void CalculateDirection()
    {
        Vector2 mousePos = _camera.ScreenToWorldPoint(_inputHandler.MousePos);
        Vector2 direction = mousePos - _rb.position;
        direction.Normalize();

        _faceDirection = direction;
    }

    public IEnumerator GatewayLock()
    {
        GatewayTravelLocked = true;

        yield return new WaitForSeconds(3);

        GatewayTravelLocked = false;
    } 

    public IEnumerator IdleWait()
    {
        _regenChecking = true;

        yield return new WaitForSeconds(10);

        _regenChecking = false;

        if (_moveAxis == Vector2.zero)
        {
            _idle = true;
            _animator.SetBool("Idle", true);
        }
    } 

    // DRAWING
    private void UpdateAnimator()
    {
        _animator.SetFloat("Horizontal", _faceDirection.x);
        _animator.SetFloat("Vertical", _faceDirection.y);
        _animator.SetFloat("Speed", Mathf.Max(Mathf.Abs(_moveAxis.x), Mathf.Abs(_moveAxis.y)));
    }
    
    // HEALTH
    public void Damage(float damageTaken)
    {
        Health = ((Health - damageTaken) < 0) ? 0 : Health - damageTaken;
        _healthBar.SetHealth(Health);

        _inventory.Add(FIREBALL);

        if (Health <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        _gameEventSystem.OnPlayerDeath();
    }

    public IEnumerator RegenTimer()
    {
        
        yield return new WaitForSeconds(10);

    } 

    // COLLISION
    private void OnTriggerEnter2D(Collider2D other) {
        // print(other.name);
    }

    // EVENTS
    private void OnDashPress() => _dashPress = 1;
    private void OnSprintPress() => _sprinting = true; 
    private void OnSprintRelease() => _sprinting = false;
    
}