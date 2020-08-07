using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Mob
{

    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Color _color;
    [SerializeField] private ParticleSystem _jellyParticle;

    [SerializeField] [Range(0, 3)] private float _hopCooldown;

    private AudioManager _audioManager;
    private int _generation;

    protected override void Awake()
    {
        _audioManager = AudioManager.Instance;

        _healthBar = GetComponentInChildren<HealthBar>();
    }

    protected override void Start()
    {
        _healthBar.SetMaxHealth(Health);
        DisableHealthbar();

        Color randomColour = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 255f);

        _sr.color = randomColour;
        // _jellyParticle.main.startColor = randomColour;
        _jellyParticle.startColor = randomColour;
    }

    private IEnumerator HopEnd()
    {
        _hopWait = true;
        _hopInAir = false;

        _audioManager.PlayInstant(SoundType.SlimeBounce, 1, 1, true);
        _jellyParticle.Play();

        yield return new WaitForSeconds(_hopCooldown);
        _hopWait = false;
    }

}