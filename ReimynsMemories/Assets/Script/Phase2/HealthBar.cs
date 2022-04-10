using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public TMP_Text printHP;

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
        printHP.text = (slider.value + "/" + slider.maxValue);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        printHP.text = (slider.value + "/" + slider.maxValue);
    }
    
}
