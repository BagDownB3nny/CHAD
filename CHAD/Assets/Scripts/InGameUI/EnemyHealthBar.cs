using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    public Color low;
    public Color high;
    public Vector2 offset;

    public void SetHealth(float health, float maxHealth) {
        if (slider != null)
        {
            slider.gameObject.SetActive(health < maxHealth);
            slider.minValue = 0;
            slider.value = health;
            slider.maxValue = maxHealth;

            slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
        }
    }

    private void Update() {
        if (slider != null )
        {
            slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + (Vector3)offset);
        }
    }
}
