using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Player", menuName = "Scriptable/Player/PlayerSO")]

public class FriendlyBehaviour : CharacterBehaviour
{
    protected override void InitialiseHP()
    {
        currenthealthPoints = maxHealthPoints;
        setHealth.Invoke(maxHealthPoints);
    }


    protected override void InitialiseBaseDamage()
    {
        baseDamage = 20;
    }

    protected override void InitialiseShield()
    {
        shield = 0;
        setShield.Invoke(maxHealthPoints);
    }

}
