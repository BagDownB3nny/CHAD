using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void Initialize(float _maxHp) {
        slider.maxValue = _maxHp;
    }
    
    public void SetHealth(float _hp) {
        slider.value = _hp;
    }
}
