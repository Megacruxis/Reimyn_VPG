using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwordOfLight", menuName = "Scriptable/Card/Attack/SwordOfLight")]
public class SwordOfLight : Card
{
    [SerializeField] private int damages;

    public override void DoEffect(FriendlyBehaviour player, EnemyBehaviour opponent)
    {
        opponent.TakeDamage(player.GetAttackDamage(damages));
        Debug.Log("Sword " + opponent.GetCurrentHealthPoint());
        if(opponent.GetCurrentHealthPoint() > 0)
        {
            player.GetMyManager().SetCanResetGrid(false);
            player.GetMyManager().ResetGrid();
        }
    }
}
