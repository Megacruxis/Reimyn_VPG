using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Linkage", menuName = "Scriptable/Card/Attack/Linkage")]
public class Linkage : Card
{
    [SerializeField] private int baseDamage;
    public override void DoEffect(FriendlyBehaviour player, EnemyBehaviour opponent)
    {
        int damages = baseDamage * player.GetMyManager().GetNumberOfPairDiscovered();
        opponent.TakeDamage(player.GetAttackDamage(damages));
        Debug.Log("COCOCOCcombo breaker " + damages);
    }
}
