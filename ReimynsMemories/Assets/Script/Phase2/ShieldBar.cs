using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ShieldBar : MonoBehaviour
{
    public Slider slider;

    public TMP_Text printSD;

    public CharacterBehaviour linkedCharacterShield;

    private void Start()
    {
        linkedCharacterShield.setShield.AddListener(SetMaxShield);
        linkedCharacterShield.changeShield.AddListener(SetShield);
    }

    public void SetMaxShield(int shield)
    {
        slider.maxValue = shield;
        slider.value = 0;
        printSD.text = (slider.value + "/" + slider.maxValue);
    }

    public void SetShield(int shield)
    {
        slider.value = shield;
        if(shield > 0)
        {
            printSD.text = (slider.value + "/" + slider.maxValue);
        } else
        {
            printSD.text = "";
        }
    }

    public void SetLinkedCharacter(CharacterBehaviour linkedCharacter)
    {
        linkedCharacterShield.setShield.RemoveListener(SetMaxShield);
        linkedCharacterShield.changeShield.RemoveListener(SetShield);
        linkedCharacterShield = linkedCharacter;
        linkedCharacterShield.setShield.AddListener(SetMaxShield);
        linkedCharacterShield.changeShield.AddListener(SetShield);
        SetMaxShield(linkedCharacter.GetMaxHealthPoints());
        SetShield(linkedCharacter.GetShield());
    }
}
