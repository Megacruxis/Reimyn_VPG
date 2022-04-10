using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public CharacterBehaviour linkedCharacter;

    private void Start()
    {
        linkedCharacter.setHealth.AddListener(SetMaxHealth);
        linkedCharacter.changeHealth.AddListener(SetHealth);
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
    
}
