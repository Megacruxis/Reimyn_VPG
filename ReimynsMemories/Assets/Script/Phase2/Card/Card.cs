using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card
{
    [SerializeField] private string cardName;
    [SerializeField] private string effectText;

    [SerializeField] private Sprite cardFrontSprite;

    public abstract void DoEffect(); //take hostile behaviour + player as parameter

    public string GetCardName()
    {
        return cardName;
    }

    public string GetEffectText()
    {
        return effectText;
    }

    public Sprite GetCardFrontSprite()
    {
        return cardFrontSprite;
    }
}
