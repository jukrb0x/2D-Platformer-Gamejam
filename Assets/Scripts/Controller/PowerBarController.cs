using System;
using UnityEngine;
using UnityEngine.UI;

public class PowerBarController : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private void Awake()
    {
        slider ??= GetComponent<Slider>();
        fill ??= gameObject.transform.Find("Fill").GetComponent<Image>();
    }

    public void SetMaxPower(float power)
    {
        slider.maxValue = power;
        slider.value = power;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetPower(float health)
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}