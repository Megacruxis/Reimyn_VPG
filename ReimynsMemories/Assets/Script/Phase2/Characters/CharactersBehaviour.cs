using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterBehaviour : ScriptableObject
{
    [SerializeField] protected int maxHealthPoints;
    [SerializeField] protected int baseDamage;
    [SerializeField] protected int shield;

    [SerializeField] public Sprite characterSprite;

    public UnityEvent<int> changeHealth;
    public UnityEvent<int> setHealth;
    public UnityEvent<int> changeShield;
    public UnityEvent<int> setShield;

    protected int currenthealthPoints;
    protected int attackMultiplier;
    protected MemoryCombatManager myManager;
    protected float currentStrenght = 0;
    private enum passiveCapacities
    {
        protection,
        atkBoost
    }

    protected abstract void InitialiseHP();
    protected abstract void InitialiseBaseDamage();
    protected abstract void InitialiseShield();

    public int GetCurrentHealthPoint()
    {
        return currenthealthPoints;
    }

    public void GainStrenght(int value)
    {
        currentStrenght += value;
    }

    public int GetMaxHealthPoints()
    {
        return maxHealthPoints;
    }

    public int GetShield()
    {
        return shield;
    }

    public MemoryCombatManager GetMyManager()
    {
        return myManager;
    }

    private void ChangeHP(int amount)
    {
        currenthealthPoints += amount;
    }
    
    public void TakeDamage(int dmg)
    {
        if (shield>0) 
        {
            if (shield>dmg)
            {
                shield -= dmg;
            }
            else
            {
                int realDmg = shield-dmg;
                ChangeHP(realDmg);
                changeHealth.Invoke(currenthealthPoints);
                shield = 0;
            }
            changeShield.Invoke(shield);    
        }
        else
        {
            int realDmg = shield-dmg;
            ChangeHP(realDmg);
            changeHealth.Invoke(currenthealthPoints);
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
        changeShield.Invoke(shield);
    }

    public int GetAttackMultiplier()
    {
        return attackMultiplier;
    }

    public void ResetAttackMultiplier()
    {
        attackMultiplier = 1;
    }

    public void UpdateAttackMultiplier(int multiply)
    {
        attackMultiplier *= multiply;
    }

    public void NewTurn()
    {
        shield /= 2;
        ResetAttackMultiplier();
    }

    public int GetAttackDamage(int originalValue)
    {
        int val = originalValue * attackMultiplier;
        ResetAttackMultiplier();
        return val;
    }

    // Used to set the stat of the player at their default value 
    public void Init(MemoryCombatManager myManager)
    {
        this.myManager = myManager;
        currentStrenght = 0,
        InitialiseBaseDamage();
        InitialiseHP();
        InitialiseShield();
        ResetAttackMultiplier();
    }

    public void OnEnable()
    {
        changeHealth = new UnityEvent<int>();
        setHealth = new UnityEvent<int>();
        changeShield = new UnityEvent<int>();
        setShield = new UnityEvent<int>();
    }
}
