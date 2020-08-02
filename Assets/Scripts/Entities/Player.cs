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

    [Space(10)]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;

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
    }

    private void Update()
    {
        _moveAxis = _playerInput.Movement.Move.ReadValue<Vector2>();
        _sprinting = _playerInput.Movement.Sprint.ReadValue<float>();
        _dashPress = _playerInput.Movement.Dash.ReadValue<float>();

        _animator.SetFloat("Horizontal", _moveAxis.x);
        _animator.SetFloat("Vertical", _moveAxis.y);
        _animator.SetFloat("Speed", Mathf.Max(Mathf.Abs(_moveAxis.x), Mathf.Abs(_moveAxis.y)));
    }

    private void FixedUpdate() {
        // DASHING
        if (_dashPress != 0 | _dashing)
        {
            Dash();
        }

        // MOVING
        if (_moveAxis != Vector2.zero & !_dashing) {
            Move();
        }
    }

    // PLAYER MOVEMENT
    private void Move()
    {   
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
            _audioManager.Play(SoundTypes.Dash);
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

    public IEnumerator GatewayLock()
    {
        GatewayTravelLocked = true;

        yield return new WaitForSeconds(3);

        GatewayTravelLocked = false;
    } 

    // HEALTH
    public void Damage(float damageTaken)
    {
        Health = ((Health - damageTaken) < 0) ? 0 : Health - damageTaken;

        if (Health <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        // Animator
    }

    // COLLISION
    private void OnTriggerEnter2D(Collider2D other) {
        // print(other.name);
    }
}
