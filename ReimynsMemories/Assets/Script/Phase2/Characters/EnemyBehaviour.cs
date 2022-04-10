using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GeneralEnemy", menuName = "Scriptable/Enemy/GeneralEnemy")]

public class EnemyBehaviour : CharacterBehaviour
{
    [SerializeField] private float failRate;// scale the chance for the opponent to fail
    [SerializeField] private float atkBlockRatio;
    [SerializeField] private float healShieldRatio;
    
    [SerializeField] private int healValue;
    [SerializeField] private int blockValue;

    public string ExectuteNextMove(FriendlyBehaviour player)
    {
        float rollFail = ((float)Random.Range(0, 100))/100f;
        if (rollFail>failRate)
        {
            float rollATK = ((float)Random.Range(0, 100))/100f;
            if (rollATK<atkBlockRatio)
            {
                player.TakeDamage(baseDamage);
                return "The enemy attack you !";
            }
            else
            {
                float rollHP = ((float)Random.Range(0, 100))/100f;
                if (rollHP<healShieldRatio)
                {
                    HealCharacter(healValue);
                    return "The enemy heals !";
                }
                else
                {
                    AddShield(blockValue);
                    return "The enemy shields itself !";
                }
            }
        }
        return "The enemy fail his attack !";
    }

    protected override void InitialiseHP()
    {
        currenthealthPoints = maxHealthPoints;
        setHealth.Invoke(maxHealthPoints);
    }

    protected override void InitialiseBaseDamage()
    {

    }

    protected override void InitialiseShield()
    {
        setShield.Invoke(maxHealthPoints);
    }

}
