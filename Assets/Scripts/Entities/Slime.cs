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

    private void Awake()
    {
        _audioManager = AudioManager.Instance;
    }

    public override void Start()
    {
        _sr.color = _color;
        _sr.color = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255), 255f);
        _jellyParticle.startColor = _color;
    }

    private IEnumerator HopEnd()
    {
        _hopWait = true;

        _audioManager.Play(SoundType.SlimeBounce, 1, true);
        _jellyParticle.Play();

        yield return new WaitForSeconds(_hopCooldown);
        _hopWait = false;
    }

}