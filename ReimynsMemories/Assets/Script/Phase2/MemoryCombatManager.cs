using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MemoryCombatManager : MonoBehaviour
{
    [Header("Player info")]
    [SerializeField] private FriendlyBehaviour player;
    [SerializeField] private EnemyBehaviour opponent;
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
    private bool canResetGrid;
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
        if(opponent == null)
        {
            Debug.LogError("Missing reference to EnemyBehaviour opponent in script MemoryCombatManager");
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
        StartCoroutine(InitPlayerAndOpponent());
        SetEmptyCardSlot();
        playerDeckSO.InitDeckForCombat();
        StartCoroutine(FillGrid(0));
    }

    private void Update()
    {
        if(!isPlayerTurn && enemyCanAttack)
        {
            opponent.NewTurn();
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
    private IEnumerator FillGrid(float delay) 
    {
        cardIsClickedEvent.RemoveListener(CardIsClicked);
        yield return new WaitForSeconds(delay);
        List<int> filledSlots = new List<int>();
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
                filledSlots.Add(selectedSlot);
            }
        }

        // make the card appear one by one
        int max = filledSlots.Count;
        for (int i = 0; i < max; i++)
        {
            int selected = filledSlots[Random.Range(0, filledSlots.Count)];
            filledSlots.Remove(selected);
            cardSlots[selected].DisplayCard(0);
            yield return new WaitForSeconds(0.05f);
        }
        gridIsFilledEvent.Invoke();
    }

    /*
     * Putt all card in the grid in the discard pile and refill it
     */
    public void ResetGrid()
    {
        cardIsClickedEvent.RemoveListener(CardIsClicked);
        DiscardAllCard();
        HideAllCard();
        StartCoroutine(FillGrid(1.4f));
    }

    /*
     * Used when a card is clicked in order to check if a pair is reveald
     */
    public void CardIsClicked(CardDisplayManager selectedCardManager)
    {
        int selectedCardIndex = CalculateCardIndex(selectedCardManager);
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
                        PairFound(selectedCardManager);
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

    public void SetCanResetGrid(bool value)
    {
        canResetGrid = value;
    }

    private void PairFound(CardDisplayManager selectedCardManager)
    {
        HideDiscoveredCard(selectedCardManager);

        if(selectedCardManager.GetMyCard().GetExile())
        {
            playerDeckSO.ExileCard(selectedCardManager.GetMyCard());
        } 
        else
        {
            playerDeckSO.AddToDiscardPile(selectedCardManager.GetMyCard());
        }
        emptycardSlots.Add(faceUpCardIndex);
        emptycardSlots.Add(CalculateCardIndex(selectedCardManager));

        //cool animation
        selectedCardManager.GetMyCard().DoEffect(player, opponent);

        if (emptycardSlots.Count == gridNumberOfSlots && canResetGrid)
        {
            StartCoroutine(FillGrid(1f));
        } 
        else
        {
            canResetGrid = true;
            ResetSelectedCard();
        }       
    }

    private void GridIsFilled()
    {
        cardIsClickedEvent.AddListener(CardIsClicked);
        ResetSelectedCard();
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
        selectedCardManager.HideCard(1f);
        cardSlots[faceUpCardIndex].HideCard(1f);
    }

    public void HideAllCard()
    {
        foreach(CardDisplayManager card in cardSlots)
        {
            card.HideCard(0.2f);
        }
    }

    public void DiscardAllCard()
    {
        List<int> discardedId = new List<int>();
        foreach (CardDisplayManager card in cardSlots)
        {
            if(!card.GetIsHidden())
            {
                int cardId = card.GetMyCard().GetCardId();
                if (!discardedId.Contains(cardId))
                {
                    playerDeckSO.AddToDiscardPile(card.GetMyCard());
                    discardedId.Add(cardId);
                } else
                {
                    discardedId.Remove(cardId);
                }
                emptycardSlots.Add(CalculateCardIndex(card));
            }
        }
    }

    public int CalculateCardIndex(CardDisplayManager selectedCardManager)
    {
        return numberOfColumn* selectedCardManager.GetSlotY() + selectedCardManager.GetSlotX();
    }

    private IEnumerator ExectuteEnemyMove()
    {
        //ennemy annimation start
        yield return new WaitForSeconds(0.1f);
        // ennemy attack

        isPlayerTurn = true;
        player.NewTurn();
        cardIsClickedEvent.AddListener(CardIsClicked);
    }

    public IEnumerator InitPlayerAndOpponent() 
    {
        yield return new WaitForSeconds(0.1f);
        player.Init(this);
        opponent.Init(this);
    }
    
}
