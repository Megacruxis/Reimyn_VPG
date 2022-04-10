using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public CharacterBehaviour linkedHealthStart;
    public CharacterBehaviour linkedHealthChange;

    private void Start()
    {
        linkedHealthStart.setHealth.AddListener(SetMaxHealth);
    }

    private void Update()
    {
        linkedHealthChange.changeHealth.AddListener(SetHealth);
    }

    public void SetMaxHealth(int health)
    {
        slider.value = health;
        slider.maxValue = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
    
}
