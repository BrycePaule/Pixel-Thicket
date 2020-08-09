using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightColourFlicker : MonoBehaviour
{

    [SerializeField] private Light2D _light;

    [Space(10)]
    [SerializeField] private float _interval;
    [SerializeField] [Range(0, 1)] private float _intervalVariance;
    [SerializeField] [Range(1, 4)] private float _colourShiftSpeed;

    [Space(10)]
    [SerializeField] private Color _colour1;
    [SerializeField] private Color _colour2;

    private Vector2 _R;
    private Vector2 _G;
    private Vector2 _B;
    private float _updateTime;

    private Color _currentColour;
    private Color _shiftColour;

    private void Start()
    {
        _R = new Vector2(Mathf.Min(_colour1.r, _colour2.r), Mathf.Max(_colour1.r, _colour2.r));
        _G = new Vector2(Mathf.Min(_colour1.g, _colour2.g), Mathf.Max(_colour1.g, _colour2.g));
        _B = new Vector2(Mathf.Min(_colour1.b, _colour2.b), Mathf.Max(_colour1.b, _colour2.b));

        _shiftColour = SelectNewRandomColour();
        _currentColour = _shiftColour;
        SetLightColor(_currentColour);
        SetUpdateTime();
    }

    private void Update()
    {
        if (Time.time >= _updateTime)
        {
            _shiftColour = SelectNewRandomColour();
            SetUpdateTime();
        }

        _currentColour.r += (_light.color.r < _shiftColour.r) ? 0.01f * _colourShiftSpeed : -0.01f * _colourShiftSpeed;
        _currentColour.g += (_light.color.g < _shiftColour.g) ? 0.01f * _colourShiftSpeed : -0.01f * _colourShiftSpeed;
        _currentColour.b += (_light.color.b < _shiftColour.b) ? 0.01f * _colourShiftSpeed : -0.01f * _colourShiftSpeed;

        SetLightColor(_currentColour);
    }

    private Color SelectNewRandomColour()
    {
        return new Color(Random.Range(_R.x, _R.y), Random.Range(_G.x, _G.y), Random.Range(_B.x, _B.y));
    }

    private void SetLightColor(Color color)
    {
        _light.color = color;
        _currentColour = color;
    } 
    
    private void SetUpdateTime()
    {
        _updateTime = Time.time + _interval + (_interval * Random.Range(-_intervalVariance, _intervalVariance));
    }


}
