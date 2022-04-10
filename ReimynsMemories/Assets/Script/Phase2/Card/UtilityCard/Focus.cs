using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Focus : Card
{
    public override void DoEffect(FriendlyBehaviour player, EnemyBehaviour opponent)
    {
        player.UpdateAttackMultiplier(2);
    }
}
