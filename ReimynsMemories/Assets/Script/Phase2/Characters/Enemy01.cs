using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy01", menuName = "Scriptable/Enemy/Enemy01SO")]

public class Enemy01 : EnemyBehaviour
{
    protected override void InitialiseHP()
    {
        currenthealthPoints = maxHealthPoints;
    }

    protected override void InitialiseBaseDamage()
    {
        baseDamage = 35;
    }

    protected override void InitialiseShield()
    {
        shield = 0;
    }
}
