using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplayManager : MonoBehaviour
{
    [Header("Required reference to child component")]
    [SerializeField] private SpriteRenderer mySpriteRenderer;

    [Header("Card Sprite")]
    [SerializeField] private Sprite frontSprite;
    [SerializeField] private Sprite backSprite;

    [Header("Slot Coordinate")]
    [SerializeField] private int x;
    [SerializeField] private int y;

    private bool coroutineAllowed, isFacedUp, isHidden;
    private Card myCard;
    private MemoryCombatManager myManager;

    private void Awake()
    {
        if(mySpriteRenderer == null)
        {
            Debug.LogError("Missing reference to SpriteRenderer mySpriteRenderer in script CardDisplayManager");
        }
        if(frontSprite == null)
        {
            Debug.LogError("Missing reference to Sprite frontSprite in script CardDisplayManager");
        }
        if(backSprite == null)
        {
            Debug.LogError("Missing reference to Sprite backSprite in script CardDisplayManager");
        }
        mySpriteRenderer.sprite = backSprite;
        myCard = null;
        coroutineAllowed = true;
        isFacedUp = false;
        HideCard();
    }

    public int GetSlotX()
    {
        return x;
    }

    public int GetSlotY()
    {
        return y;
    }

    public Card GetMyCard()
    {
        return myCard;
    }

    /*
     * Used by game manager to set the script info
     */
    public void SetCardInfo(Card myCard, MemoryCombatManager myManager)
    {
        frontSprite = myCard.GetCardFrontSprite();
        this.myCard = myCard;
        this.myManager = myManager;
        if (isFacedUp)
        {
            StartCoroutine(RotateCard());
        }
        DisplayCard(0.18f);
    }



    /*
     * Start the coroutine that flip the card when clicked
     */
    private void OnMouseDown()
    {
        if(!isHidden)
        {
            myManager.cardIsClickedEvent.Invoke(this);
        }
    }

    public void FlipCard()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(RotateCard());
        }
    }

    /*
     * Coroutine are function that run in parallel to the script (similar to a new thread)
     * A coroutine has its execution stoped at the yield return statement. Execution is resumed when the condition written
     * after it is met (here it's when 0.01 second have passed since execution was stop).
     * The goal here is to rotate the sprite 10 degree each 0.01 second until it did half a circle
    */
    private IEnumerator RotateCard()
    {
        coroutineAllowed = false;

        if (!isFacedUp)
        {
            for (float i = 0f; i <= 180f; i += 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    mySpriteRenderer.sprite = frontSprite;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }

        else if (isFacedUp)
        {
            for (float i = 180f; i >= 0f; i -= 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    mySpriteRenderer.sprite = backSprite;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }

        coroutineAllowed = true;
        isFacedUp = !isFacedUp;
    }

    public void HideCard()
    {
        isHidden = true;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void DisplayCard(float delay)
    {
        isHidden = false;
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
