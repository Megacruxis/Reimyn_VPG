using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyBehaviour : CharacterBehaviour
{
    protected override void InitialiseHP()
    {
        int start = 80;
        currenthealthPoints = start;
        maxHealthPoints = start;
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
