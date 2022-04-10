using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boost", menuName = "Scriptable/Card/Utility/Boost")]
public class Boost : Card
{
    [SerializeField] private int strenghtBuff;

    public override void DoEffect(FriendlyBehaviour player, EnemyBehaviour opponent)
    {
        player.GainStrenght(strenghtBuff);
        Debug.Log("LE MUSCLE Billy !!!!!");
    }
}
