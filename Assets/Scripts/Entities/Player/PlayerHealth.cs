using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable<float>, IKillable
{
    [SerializeField] private HealthBar _healthBar;

    [Space(10)]
    public float MaxHealth;
    public float Health;
    public float HealthRegen;
    
    private bool _regenChecking;
    private bool _hit;

    private GameEventManager _gameEventManager;
    private AudioManager _audioManager;
    private Transform _transform;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _transform = transform;
        _gameEventManager = GameEventManager.Instance;
        _audioManager = AudioManager.Instance;
    }

    private void Start()
    {
        _healthBar.SetMaxHealth(Health);
    }
   
    // HEALTH
    public void Damage(float damageTaken)
    {
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
        yield return new WaitForSeconds(10);
    } 

    // CALLBACKS
    private void HitFinish()
    {
        _animator.ResetTrigger("Hit");
        _hit = false;
    }

}
