using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : ScriptableObject
{
    [SerializeField] private int cardId;
    [SerializeField] private bool exile;

    [SerializeField] private Sprite cardFrontSprite;

    public CardStatus cardStatus = CardStatus.Clear;

    public abstract void DoEffect(FriendlyBehaviour player, EnemyBehaviour opponent);

    public Sprite GetCardFrontSprite()
    {
        return cardFrontSprite;
    }

    public int GetCardId()
    {
        return cardId;
    }

    public bool GetExile()
    {
        return exile;
    }

    public string GetCardStatus()
    {
        switch (cardStatus)
        {
            case CardStatus.Clear:
                return "Clear";
            case CardStatus.Burn:
                return "Burn";  
            default:
                return "Error Status undifined"; 
        }
    }
}
