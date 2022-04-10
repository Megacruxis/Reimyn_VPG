using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterBehaviour : ScriptableObject
{
    [SerializeField] public int maxHealthPoints;
    [SerializeField] public int currenthealthPoints;
    [SerializeField] public int baseDamage;
    [SerializeField] public int shield;

    [SerializeField] public Sprite characterSprite;

    public UnityEvent<int> changeHealth;
    public UnityEvent<int> setHealth;


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
    
    public void TakeDamage(int dmg)
    {
        if (shield>dmg) {shield -= dmg;}
        else
        {
            int realDmg = shield-dmg;
            ChangeHP(realDmg);
            changeHealth.Invoke(realDmg);
        }
    }

    public void HealCharacter(int heal)
    {
        if (heal+currenthealthPoints>maxHealthPoints) 
        {
            ChangeHP(maxHealthPoints-currenthealthPoints);
        }
        else
            ChangeHP(heal);
        changeHealth.Invoke(currenthealthPoints);
    }

    public void AddShield(int bonusShield)
    {
        shield += bonusShield;
    }


    // Start is called before the first frame update
    public void Init()
    {
        changeHealth = new UnityEvent<int>();
        setHealth = new UnityEvent<int>();
        InitialiseBaseDamage();
        InitialiseHP();
        InitialiseShield();
    }    
}
