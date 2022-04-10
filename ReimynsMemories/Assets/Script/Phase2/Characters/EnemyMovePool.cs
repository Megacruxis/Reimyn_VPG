using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyMovePool", menuName = "Scriptable/Enemy/EnemyMovePool/BaseMovePool")]
public class EnemyMovePool : ScriptableObject
{
    [SerializeField] private string moveName;
    [SerializeField] private string effectText;

    [SerializeField] private Sprite moveSprite;

    [SerializeField] private int burnDamage = 10;
    [SerializeField] private int internDamage = 0;

    public enum MovePool
    {
        MoveAtk,
        MoveBurn,
        MaxMove
    }

    [SerializeField] private MovePool nextMove = MovePool.MoveAtk;

    public string GetMoveName()
    {
        return moveName;
    }

    public string GetEffectText()
    {
        return effectText;
    }

    public Sprite GetMoveSprite()
    {
        return moveSprite;
    }

    public int GetBurnDamage()
    {
        return burnDamage;
    }

    public MovePool GetNextMove()
    {
        return nextMove;
    }

    private void SetMoveName(string name)
    {
        moveName = name;
    }

    private void SetMoveEffectText(string text)
    {
        effectText = text;
    }

    private void SetMoveSprite(Sprite sprite)
    {
        moveSprite = sprite;
    }

    public void SetBurnDamage(int dmg)
    {
        burnDamage = dmg;
    }
    
    ///
    private void BasicATK(int dmg, FriendlyBehaviour player)
    {
        player.TakeDamage(dmg);
    }
    
    private void BurnCard(Card card)
    {
        card.cardStatus = CardStatus.Burn;
    }

    public void SetNextMove(int dmg)//is the same for all enemies at the minute
    {
        internDamage = dmg;
        nextMove = (MovePool)Random.Range(0, (int)MovePool.MaxMove);
        switch (nextMove)
        {
            case MovePool.MoveAtk:
                SetMoveName("Shadow slash");
                SetMoveEffectText("Attacks Reimyns with an shadow ball");
                //SetMoveSprite(); to do
                break;
            case MovePool.MoveBurn:
                SetMoveName("Burn trap");
                SetMoveEffectText("Puts a burning trap on one your cards at random");
                //SetMoveSprite(); to do
                break;
            default:
                Debug.Log("A new move should have been set");
                break;
        }
    }

    public void ApplyMove(FriendlyBehaviour player, Card card)
    {
        switch (nextMove)
        {
            case MovePool.MoveAtk:
                BasicATK(internDamage,player);
                break;
            case MovePool.MoveBurn:
                BurnCard(card);
                break;
            default:
                Debug.Log("A new move should have been applied");
                break;
        }
    }
}
