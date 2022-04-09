using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDeck", menuName = "Scriptable/Player/PlayerDeckSO")]
public class PlayerDeckSO : ScriptableObject
{
    [SerializeField] private List<Card> playerDecklist;
    private List<Card> currentDeck;
    private List<Card> discardPile;
    private List<Card> exilePile;

    public Card DrawNextCard()
    {
        if(currentDeck.Count == 0)
        {
            Debug.LogError("Cannot draw a card, deck is empty");
            return null;
        }
        Card card = currentDeck[Random.Range(0, currentDeck.Count)];
        currentDeck.Remove(card);
        return card;
    }

    public void AddCardToDeck(Card cardToAdd)
    {
        playerDecklist.Add(cardToAdd);
    }

    public void ShuffleDeck()
    {
        List<Card> tmpDeck = new List<Card>();
        for(int i = 0; i < currentDeck.Count; i++)
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
