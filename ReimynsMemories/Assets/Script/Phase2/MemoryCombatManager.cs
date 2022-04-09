using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MemoryCombatManager : MonoBehaviour
{
    [Header("Player info")]
    [SerializeField] private FriendlyBehaviour player;
    [SerializeField] private PlayerDeckSO playerDeckSO;

    [Header("Grid info")]
    [SerializeField] private int numberOfLine;
    [SerializeField] private int numberOfColumn;

    [Header("Emplacement where card can be positioned")]
    [SerializeField] private CardDisplayManager[] cardSlots;

    public UnityEvent<CardDisplayManager> cardIsClickedEvent;

    private List<int> emptycardSlots;
    private int gridNumberOfSlots;
    private int faceUpCardIndex;
    private bool enoughtCardFaceUp;

    private void Awake()
    {
        if(player == null)
        {
            Debug.LogError("Missing reference to FriendlyBehaviour player in script MemoryCombatManager");
        }
        if(playerDeckSO == null)
        {
            Debug.LogError("Missing reference to PlayerDeckSO playerDeckSO in script MemoryCombatManager");
        }
        if(numberOfColumn <= 0)
        {
            Debug.LogError("Error in script MemoryCombatManager, numberOfColumn = " + numberOfColumn 
                + " but should be greater than 0");
        }
        if(numberOfColumn <= 0)
        {
            Debug.LogError("Error in script MemoryCombatManager, numberOfLine = " + numberOfLine
                + " but should be greater than 0");
        }
        if(cardSlots.Length < numberOfLine * numberOfColumn)
        {
            Debug.LogError("Error in script MemoryCombatManager, not enought card slot for the given number of line and column");
        }

        gridNumberOfSlots = numberOfLine * numberOfColumn;
        if(gridNumberOfSlots % 2 == 1)
        {
            Debug.LogError("Error in script MemoryCombatManager, number of card slots should be an even number");
        }
        cardIsClickedEvent = new UnityEvent<CardDisplayManager>();
        cardIsClickedEvent.AddListener(CardIsClicked);
        emptycardSlots = new List<int>();
        faceUpCardIndex = -1;
        enoughtCardFaceUp = false;
    }

    private void Start()
    {
        SetEmptyCardSlot();
        playerDeckSO.InitDeckForCombat();
        FillGrid();
    }

    private void SetEmptyCardSlot()
    {
        emptycardSlots.Clear();
        for (int i = 0; i < gridNumberOfSlots; i++)
        {
            emptycardSlots.Add(i);
        }
    }

    /*
     * Fill the grids with card from the player deck
     */
    private void FillGrid() 
    {
        for (int i = 0; i < gridNumberOfSlots/2; i++)
        {
            Card selectedCard = playerDeckSO.DrawNextCard();
            if(selectedCard == null)
            {
                //maybe shuffle ?
                Debug.LogError("Error in script MemoryCombatManager, could not fill the grid, deck is empty");
                return;
            }
            for(int r = 0; r < 2; r++)
            {
                if(emptycardSlots.Count == 0)
                {
                    Debug.LogError("Error in script MemoryCombatManager, no empty card slot left");
                    return;
                }
                int selectedSlot = emptycardSlots[Random.Range(0, emptycardSlots.Count)];
                emptycardSlots.Remove(selectedSlot);
                cardSlots[selectedSlot].SetCardInfo(selectedCard, this);
            }
        }
    }

    public void CardIsClicked(CardDisplayManager selectedCardManager)
    {
        int selectedCardIndex = numberOfColumn * selectedCardManager.GetSlotY() + selectedCardManager.GetSlotX();
        if (faceUpCardIndex < 0)
        {
            selectedCardManager.FlipCard();
            faceUpCardIndex = selectedCardIndex;
            //apply the effect of the card when she is face up
        }
        else
        {
            if(!enoughtCardFaceUp)
            {
                if(selectedCardIndex != faceUpCardIndex)
                {
                    enoughtCardFaceUp = true;
                    if (selectedCardManager.GetMyCard().GetCardId() == cardSlots[faceUpCardIndex].GetMyCard().GetCardId())
                    {
                        selectedCardManager.FlipCard();
                        StartCoroutine(PairFound(selectedCardManager));
                    }
                    else
                    {
                        selectedCardManager.FlipCard();
                        StartCoroutine(NotAPair(selectedCardManager));
                    }
                }
            } 
        }
    }

    private IEnumerator NotAPair(CardDisplayManager selectedCardManager)
    {
        yield return new WaitForSeconds(1f);
        selectedCardManager.FlipCard();
        cardSlots[faceUpCardIndex].FlipCard();
        faceUpCardIndex = -1;
        enoughtCardFaceUp = false;
        //pass turn
    }

    private IEnumerator PairFound(CardDisplayManager selectedCardManager)
    {
        yield return new WaitForSeconds(1f);
        selectedCardManager.FlipCard();
        cardSlots[faceUpCardIndex].FlipCard();
        selectedCardManager.HideCard();
        cardSlots[faceUpCardIndex].HideCard();
        faceUpCardIndex = -1;
        enoughtCardFaceUp = false;
        //cool animation
        //pair effect
    }
}
