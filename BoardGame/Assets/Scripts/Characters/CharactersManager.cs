using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CharactersManager : MonoBehaviour
{
    [SerializeField]
    [ReadOnly]
    List<Character> characters;
    [SerializeField]
    [ReadOnly]
    int numberCharacters = 2;

    [SerializeField]
    Character characterTurn;
    [SerializeField]
    Board board;

    [SerializeField] int defaulTurnMoves = 3;
    [SerializeField] int defaulTurnDices = 3;
    [SerializeField] int defaulTurnAttack = 1;
    [ShowNonSerializedField] int turnMoves = 0;
    [ShowNonSerializedField] int turnDices = 0;
    [ShowNonSerializedField] int turnAttack = 0;


    private void OnValidate()
    {
        ResetTurn();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void ResetTurn()
    {
        turnDices = defaulTurnDices;
        turnMoves = defaulTurnMoves;
    }





    // PickUps

    public void AddTurnMove()
    {
        turnMoves += 1;
    }

    public void AddTurnDice()
    {
        turnDices += 1;
    }

    public void AddTurnAttack()
    {
        turnAttack += 1;
    }

    public void AddHealth()
    {
        characterTurn.AddHealth(20);
    }


}
