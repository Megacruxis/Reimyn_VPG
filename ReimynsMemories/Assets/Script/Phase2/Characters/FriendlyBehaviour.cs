using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Player", menuName = "Scriptable/Player/PlayerSO")]

public class FriendlyBehaviour : CharacterBehaviour
{
    protected override void InitialiseHP()
    {
        int start = 80;
        currenthealthPoints = start;
        maxHealthPoints = start;
        setHealth = new UnityEvent<int>();
        setHealth.Invoke(start);
    }


    protected override void InitialiseBaseDamage()
    {
        baseDamage = 20;
    }

    protected override void InitialiseShield()
    {
        shield = 0;
    }

}
