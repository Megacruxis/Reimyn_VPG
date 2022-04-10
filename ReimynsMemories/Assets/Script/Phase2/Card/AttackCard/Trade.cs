using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trade", menuName = "Scriptable/Card/Attack/Trade")]
public class Trade : Card
{
    [SerializeField] private int damages;
    [SerializeField] private int heal;

    public override void DoEffect(FriendlyBehaviour player, EnemyBehaviour opponent)
    {
        int dealtDamages = damages - opponent.GetShield();
        opponent.TakeDamage(player.GetAttackDamage(damages));
        if (dealtDamages > 0)
        {
            player.HealCharacter(dealtDamages);
        }
        Debug.Log("Trade " + damages + " healed " + dealtDamages);
    }
}
