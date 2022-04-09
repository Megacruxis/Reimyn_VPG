using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : CharacterBehaviour
{
    protected override void InitialiseHP()
    {
        int start = 150;
        currenthealthPoints = start;
        maxHealthPoints = start;
    }

    protected override void InitialiseBaseDamage()
    {
        baseDamage = 35;
    }

    protected override void InitialiseShield()
    {
        shield = 0;
    }

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
