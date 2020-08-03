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
        Color randomColour = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 255f);

        _sr.color = randomColour;
        // _jellyParticle.main.startColor = randomColour;
        _jellyParticle.startColor = randomColour;
    }

    private IEnumerator HopEnd()
    {
        _hopWait = true;
        _hopInAir = false;

        _audioManager.Play(SoundType.SlimeBounce, 1, true);
        _jellyParticle.Play();

        yield return new WaitForSeconds(_hopCooldown);
        _hopWait = false;
    }

}