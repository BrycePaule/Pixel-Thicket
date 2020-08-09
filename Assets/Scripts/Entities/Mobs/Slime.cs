using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Mob
{

    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Color _color;
    [SerializeField] private ParticleSystem _jellyParticle;

    private AudioManager _audioManager;
    private int _generation;

    protected override void Awake()
    {
        _audioManager = AudioManager.Instance;

        _transform = transform;
        _animator = GetComponentInParent<Animator>();
    }

    protected override void Start()
    {
        Color randomColour = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 255f);

        _sr.color = randomColour;
        // _jellyParticle.main.startColor = randomColour;
        _jellyParticle.startColor = randomColour;
    }
}