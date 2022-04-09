using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : CharacterBehaviour
{
    protected override void InitialiseHP()
    {
        int start = 150;
        currenthealthPoints = start;
        maxHealthPoints = start;
    }

    protected override void InitialiseBaseDamage()
    {
        baseDamage = 35;
    }

    protected override void InitialiseShield()
    {
        shield = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
