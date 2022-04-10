using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LightProjectile", menuName = "Scriptable/Card/Attack/LightProjectile")]
public class LightProjectile : Card
{
    [SerializeField] private int damages;
    public override void DoEffect(FriendlyBehaviour player, EnemyBehaviour opponent)
    {
        opponent.TakeDamage(damages);
        Debug.Log("Projectile " + opponent.GetCurrentHealthPoint());
    }
}
