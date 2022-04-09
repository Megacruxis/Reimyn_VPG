using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBehaviour : MonoBehaviour
{
    [SerializeField] protected int maxHealthPoints;
    [SerializeField] protected int currenthealthPoints;
    [SerializeField] protected int baseDamage;
    [SerializeField] protected int shield;

    private enum passiveCapacities
    {
        protection,
        atkBoost
    }

    protected abstract void InitialiseHP();
    protected abstract void InitialiseBaseDamage();
    protected abstract void InitialiseShield();

    private void ChangeHP(int amount)
    {
        currenthealthPoints += amount;
    }
    
    protected void TakeDamage(int dmg)
    {
        if (shield>dmg) {shield -= dmg;}
        else
        {
            int realDmg = shield-dmg;
            ChangeHP(realDmg);
        }
    }

    protected void HealCharacter(int heal)
    {
        if (heal+currenthealthPoints>maxHealthPoints) 
        {
            ChangeHP(maxHealthPoints-currenthealthPoints);
        }
        else
            ChangeHP(heal);
    }

    protected void AddShield(int bonusShield)
    {
        shield += bonusShield;
    }


    // Start is called before the first frame update
    protected void Start()
    {
        InitialiseBaseDamage();
        InitialiseHP();
        InitialiseShield();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
