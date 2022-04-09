using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDeck", menuName = "Scriptable/Player/PlayerDeckSO")]
public class PlayerDeckSO : ScriptableObject
{
    [SerializeField] private List<Card> playerDecklist;
    private List<Card> currentDeck = new List<Card>();
    private List<Card> discardPile = new List<Card>();
    private List<Card> exilePile = new List<Card>();

    public void InitDeckForCombat()
    {
        exilePile.Clear();
        ResetDeck();
    }

    public bool CanDraw()
    {
        return currentDeck.Count > 0;
    }

    public Card DrawNextCard()
    {
        if(currentDeck.Count == 0)
        {
            ShuffleDiscardPileIntoDeck();
            Debug.LogError("Cannot draw a card, deck is empty");
            return null;
        }
        Card card = currentDeck[Random.Range(0, currentDeck.Count)];
        currentDeck.Remove(card);
        return card;
    }

    public void AddToDiscardPile(Card cardToAdd)
    {
        if(playerDecklist.Contains(cardToAdd))
        {
            discardPile.Add(cardToAdd);
        }
    }

    public void AddCardToDeck(Card cardToAdd)
    {
        playerDecklist.Add(cardToAdd);
    }

    public void ShuffleDeck()
    {
        List<Card> tmpDeck = new List<Card>();
        int max = currentDeck.Count;
        for(int i = 0; i < max; i++)
        {
            Card selectedCard = currentDeck[Random.Range(0, currentDeck.Count)];
            currentDeck.Remove(selectedCard);
            tmpDeck.Add(selectedCard);
        }
        currentDeck = tmpDeck;
    }

    public void ShuffleDiscardPileIntoDeck()
    {
        foreach(Card c in discardPile)
        {
            currentDeck.Add(c);
        }
        discardPile.Clear();
        ShuffleDeck();
    }

    public void ResetDeck()
    {
        currentDeck.Clear();
        discardPile.Clear();
        foreach(Card c in playerDecklist)
        {
            if(!exilePile.Contains(c))
            {
                currentDeck.Add(c);
            }
        }
        ShuffleDeck();
    }
}
