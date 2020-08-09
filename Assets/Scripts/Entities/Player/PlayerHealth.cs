using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable<float>, IKillable
{
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private ParticleSystem _healthRegenParticles;

    [Space(10)]
    public float MaxHealth;
    public float Health;
    public float HealthRegenPerSecond;
    public float HealthRegenOutOfCombatTimer;
    
    private bool _regen;
    private bool _regenChecking;
    private bool _hit;

    private GameEventManager _gameEventManager;
    private AudioManager _audioManager;
    private Transform _transform;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInParent<Animator>();

        _transform = transform;
        _gameEventManager = GameEventManager.Instance;
        _audioManager = AudioManager.Instance;
    }

    private void Start()
    {
        _healthBar.SetMaxHealth(MaxHealth);
    }
   
    private void FixedUpdate()
    {
        if (_regen)
        {
            RegenerateHealth();
        }
    }

    private void RegenerateHealth()
    {
        Health += HealthRegenPerSecond * Time.deltaTime;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        _healthBar.SetHealth(Health);

        if (Health >= MaxHealth | _hit | HealthRegenPerSecond == 0)
        {
            StopRegen();
        }
    }

    private void StartRegen()
    {
        _regen = true;
        _healthRegenParticles.gameObject.SetActive(true);
    }

    private void StopRegen()
    {
        _regen = false;
        _healthRegenParticles.gameObject.SetActive(false);
    }

    public void Damage(float damageTaken)
    {
        StopAllCoroutines();
        StartCoroutine(RegenTimer());

        _hit = true;
        _animator.SetTrigger("Hit");
        _audioManager.PlayInstant(SoundType.PlayerHit, 1, 1, true);

        Health = Mathf.Clamp(Health - damageTaken, 0, 99999);
        _healthBar.SetHealth(Health);

        if (Health <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        _gameEventManager.OnPlayerDeath();
        // _animator.SetTrigger("Death");
    }

    public IEnumerator RegenTimer()
    {
        yield return new WaitForSeconds(HealthRegenOutOfCombatTimer);

        StartRegen();
    } 

    // CALLBACKS
    public void HitFinish()
    {
        _animator.ResetTrigger("Hit");
        _hit = false;
    }

}
