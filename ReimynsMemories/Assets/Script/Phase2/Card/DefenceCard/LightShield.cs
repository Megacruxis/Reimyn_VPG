using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LightShield", menuName = "Scriptable/Card/Defence/LightShield")]
public class LightShield : Card
{
    [SerializeField] private int shieldValue;

    public override void DoEffect(FriendlyBehaviour player, EnemyBehaviour opponent)
    {
        player.AddShield(shieldValue);
    }
}
