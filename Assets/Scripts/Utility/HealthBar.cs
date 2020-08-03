using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private Image _fill;

    private Slider _slider;


    private void Awake()
    {
        _slider = GetComponentInChildren<Slider>();
    }

    public void SetMaxHealth(float health)
    {
        _slider.maxValue = health;
        _slider.value = health;
    }

    public void SetHealth(float health)
    {
        _slider.value = health;
    }


}
