using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : CharacterBehaviour
{
    public EnemyMovePool interPool;

    public void SetBaseBurnDamage(int dmg)
    {
        interPool.SetBurnDamage(dmg);
    }

    public void EnemyNextMove()
    {
        interPool.SetNextMove(baseDamage);
    }

    public void EnemyApplyMove(FriendlyBehaviour player, Card card)
    {
        interPool.ApplyMove(player, card);
    }

    public void EnemyApplyBurn(FriendlyBehaviour player)
    {
        player.TakeDamage(interPool.GetBurnDamage());
    }

}
