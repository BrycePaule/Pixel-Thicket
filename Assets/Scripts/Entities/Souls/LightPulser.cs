using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightPulser : MonoBehaviour
{

    [SerializeField] private float _speed;
    [SerializeField] private float _flashTime;
    [SerializeField] private Vector2 _intensityMinMax;

    private Light2D _light;
    private float _random;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
        _random = transform.parent.GetInstanceID();
    }

    private void FixedUpdate() 
    {
        _light.intensity = Mathf.Clamp(Mathf.Sin(_random + Time.time * _speed) * _flashTime, _intensityMinMax.x, _intensityMinMax.y);
    }
}
