using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MemoryCombatManager : MonoBehaviour
{
    [Header("Player info")]
    [SerializeField] private FriendlyBehaviour player;
    [SerializeField] private Enemy01 oppo1;
    [SerializeField] private PlayerDeckSO playerDeckSO;

    [Header("Grid info")]
    [SerializeField] private int numberOfLine;
    [SerializeField] private int numberOfColumn;

    [Header("Emplacement where card can be positioned")]
    [SerializeField] private CardDisplayManager[] cardSlots;

    public UnityEvent<CardDisplayManager> cardIsClickedEvent;

    private bool enoughtCardFaceUp;
    private bool isPlayerTurn;
    private bool enemyCanAttack;
    private int gridNumberOfSlots;
    private int faceUpCardIndex;
    private List<int> emptycardSlots;
    private UnityEvent gridIsFilledEvent;


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
        if(cardSlots.Length != numberOfLine * numberOfColumn)
        {
            Debug.LogError("Error in script MemoryCombatManager, not enought card slot for the given number of line and column");
        }

        gridNumberOfSlots = numberOfLine * numberOfColumn;
        if(gridNumberOfSlots % 2 == 1)
        {
            Debug.LogError("Error in script MemoryCombatManager, number of card slots should be an even number");
        }
        cardIsClickedEvent = new UnityEvent<CardDisplayManager>();


        ResetSelectedCard();
        isPlayerTurn = true;
        enemyCanAttack = true;
        emptycardSlots = new List<int>();
        gridIsFilledEvent = new UnityEvent();
        gridIsFilledEvent.AddListener(GridIsFilled);
    }

    private void Start()
    {
        SetEmptyCardSlot();
        playerDeckSO.InitDeckForCombat();
        StartCoroutine(FillGrid());
    }

    private void Update()
    {
        if(!isPlayerTurn && enemyCanAttack)
        {
            enemyCanAttack = false;
            cardIsClickedEvent.RemoveListener(CardIsClicked);
            StartCoroutine(ExectuteEnemyMove());
        }
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
    private IEnumerator FillGrid() 
    {
        cardIsClickedEvent.RemoveListener(CardIsClicked);
        for (int i = 0; i < gridNumberOfSlots/2; i++)
        {
            if(!playerDeckSO.CanDraw())
            {
                playerDeckSO.ShuffleDiscardPileIntoDeck();
            }
            Card selectedCard = playerDeckSO.DrawNextCard();
            if(selectedCard == null)
            {
                Debug.LogError("Error in script MemoryCombatManager, could not fill the grid, deck does not contain enought cards");
                yield break;
            }
            for(int r = 0; r < 2; r++)
            {
                if(emptycardSlots.Count == 0)
                {
                    Debug.LogError("Error in script MemoryCombatManager, no empty card slot left");
                    yield break;
                }
                int selectedSlot = emptycardSlots[Random.Range(0, emptycardSlots.Count)];
                emptycardSlots.Remove(selectedSlot);
                cardSlots[selectedSlot].SetCardInfo(selectedCard, this);
                yield return new WaitForSeconds(0.5f);
            }
        }
        gridIsFilledEvent.Invoke();
    }

    /*
     * Putt all card in the grid in the discard pile and refill it
     */
    public void ResetGrid()
    {
        StartCoroutine(FillGrid());
    }

    /*
     * Used when a card is clicked in order to check if a pair is reveald
     */
    public void CardIsClicked(CardDisplayManager selectedCardManager)
    {
        int selectedCardIndex = numberOfColumn * selectedCardManager.GetSlotY() + selectedCardManager.GetSlotX();
        if (faceUpCardIndex < 0)
        {
            selectedCardManager.FlipCard();
            faceUpCardIndex = selectedCardIndex;
            //apply the effect of the card when she is face up/clicked
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
                        selectedCardManager.GetMyCard().DoEffect(player,oppo1);
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
        ResetSelectedCard();
        // cool end of turn annimation ?
        isPlayerTurn = false;
    }

    private IEnumerator PairFound(CardDisplayManager selectedCardManager)
    {
        yield return new WaitForSeconds(1f);

        HideDiscoveredCard(selectedCardManager);

        playerDeckSO.AddToDiscardPile(selectedCardManager.GetMyCard());
        emptycardSlots.Add(numberOfColumn * cardSlots[faceUpCardIndex].GetSlotY() + cardSlots[faceUpCardIndex].GetSlotX());
        emptycardSlots.Add(numberOfColumn * selectedCardManager.GetSlotY() + selectedCardManager.GetSlotX());

        //cool animation


        if (emptycardSlots.Count == gridNumberOfSlots)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(FillGrid());
        } 
        else
        {
            ResetSelectedCard();
        }       
    }

    private void GridIsFilled()
    {
        cardIsClickedEvent.AddListener(CardIsClicked);
        ResetSelectedCard();
        gridIsFilledEvent.RemoveListener(GridIsFilled);
    }

    /*
     * Reset the variable in order to enable the player to select card again
     */
    private void ResetSelectedCard()
    {
        faceUpCardIndex = -1;
        enoughtCardFaceUp = false;
    }

    private void HideDiscoveredCard(CardDisplayManager selectedCardManager)
    {
        selectedCardManager.FlipCard();
        cardSlots[faceUpCardIndex].FlipCard();
        selectedCardManager.HideCard();
        cardSlots[faceUpCardIndex].HideCard();
    }

    private IEnumerator ExectuteEnemyMove()
    {
        //ennemy annimation start
        yield return new WaitForSeconds(0.1f);
        // ennemy attack

        isPlayerTurn = true;
        cardIsClickedEvent.AddListener(CardIsClicked);
    }
}
