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

    protected int currenthealthPoints;
    protected MemoryCombatManager myManager;

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
        if (shield>dmg) {shield -= dmg;}
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
    }

    // Used to set the stat of the player at their default value 
    public void Init(MemoryCombatManager myManager)
    {
        this.myManager = myManager;
        InitialiseBaseDamage();
        InitialiseHP();
        InitialiseShield();
    }

    public void OnEnable()
    {
        changeHealth = new UnityEvent<int>();
        setHealth = new UnityEvent<int>();
    }
}
