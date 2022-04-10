using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LightBall", menuName = "Scriptable/Card/Attack/LightBall")]
public class LightBall : Card
{
    [SerializeField] private int damages;

    public override void DoEffect(FriendlyBehaviour player, EnemyBehaviour opponent)
    {
        opponent.TakeDamage(player.GetAttackDamage(damages));
        Debug.Log("Bouboule " + opponent.GetCurrentHealthPoint());
    }
}
