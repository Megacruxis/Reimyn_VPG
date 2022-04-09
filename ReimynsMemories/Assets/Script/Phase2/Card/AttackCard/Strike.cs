using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Strike", menuName = "Scriptable/Card/Attack/Strike")]
public class Strike : Card
{
    public override void DoEffect(FriendlyBehaviour player, EnemyBehaviour opponent)
    {
        opponent.TakeDamage(player.baseDamage);
        Debug.Log("PAFFF la patate !!!");
        Debug.Log(opponent.currenthealthPoints);
    }
}
