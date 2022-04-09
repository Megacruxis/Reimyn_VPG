using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCombatManager : MonoBehaviour
{
    //[SerializeField] private FriendlyBehaviour player;
    [Header("Emplacement where card can be positioned")]
    [SerializeField] private Transform[] cardSlots;

    private bool[] emptycardSlots;
    private List<Card> deck;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
