using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCombatManager : MonoBehaviour
{
    [Header("Player info")]
    [SerializeField] private FriendlyBehaviour player;
    [SerializeField] private PlayerDeckSO playerDeckSO;

    [Header("Grid info")]
    [SerializeField] private int numberOfLine;
    [SerializeField] private int numberOfColumn;

    [Header("Emplacement where card can be positioned")]
    [SerializeField] private Transform[] cardSlots;

    private bool[] emptycardSlots;

    private void Awake()
    {
        if(player == null)
        {
            Debug.LogError("Missing reference to FriendlyBehaviour player in script MemoryCombatManager");
        }
        if(playerDeckSO == null)
        {
            Debug.LogError("Missing reference to PlayerDeckSO playerDeckSO in script MemoryCombatManager");
        }
        if(numberOfColumn <= 0)
        {

        }
        if(numberOfColumn <= 0)
        {

        }
        if(cardSlots.Length < numberOfLine * numberOfColumn)
        {

        }
    }

    private void Start()
    {
        emptycardSlots = new bool[numberOfLine * numberOfColumn];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
