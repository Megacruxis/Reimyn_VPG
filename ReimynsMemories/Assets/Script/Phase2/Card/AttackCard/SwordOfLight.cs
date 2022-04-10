using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwordOfLight", menuName = "Scriptable/Card/Attack/SwordOfLight")]
public class SwordOfLight : Card
{
    [SerializeField] private int damages;

    public override void DoEffect(FriendlyBehaviour player, EnemyBehaviour opponent)
    {
        opponent.TakeDamage(damages);
        Debug.Log("Sword " + opponent.GetCurrentHealthPoint());
        player.GetMyManager().SetCanResetGrid(false);
        player.GetMyManager().ResetGrid();
    }
}
