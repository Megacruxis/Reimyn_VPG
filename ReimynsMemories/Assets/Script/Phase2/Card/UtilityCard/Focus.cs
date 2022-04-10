using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Focus", menuName = "Scriptable/Card/Utility/Focus")]
public class Focus : Card
{
    public override void DoEffect(FriendlyBehaviour player, EnemyBehaviour opponent)
    {
        player.UpdateAttackMultiplier(2);
    }
}
