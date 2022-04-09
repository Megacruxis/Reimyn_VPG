using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDeck", menuName = "Scriptable/Player/PlayerDeckSO")]
public class PlayerDeckSO : ScriptableObject
{
    [SerializeField] private List<Card> playerDecklist;
    [SerializeField] private List<Card> deckCurrentStauts;
    [SerializeField] private List<Card> discardPile;
}
