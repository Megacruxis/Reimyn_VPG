using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Strike", menuName = "Scriptable/Card/Attack/Strike")]
public class Strike : Card
{
    [SerializeField] private int damages;

    public override void DoEffect(FriendlyBehaviour player, EnemyBehaviour opponent)
    {
        opponent.TakeDamage(damages);
        Debug.Log("Strike, HP: " + opponent.GetCurrentHealthPoint());
    }
}
