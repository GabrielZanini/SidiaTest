using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [Header("Managers")]
    [SerializeField]
    CameraManager cameraManager;
    [SerializeField]
    BoardManager boardManager;
    [SerializeField]
    CharactersManager charactersManager;

    [Header("Settings")]
    [SerializeField]
    [Range(4, 16)]
    int size = 16;
    [SerializeField]
    [ReorderableList]
    List<CharacterType> charactersTypes;
    [Space]
    [SerializeField] int defaulTurnMoves = 3;
    [SerializeField] int defaulTurnDices = 3;
    [SerializeField] int defaulTurnAttack = 1;

    [Header("Turn")]
    [SerializeField]
    Character turnCharacter;
    [ShowNonSerializedField] int turnMoves = 0;
    [ShowNonSerializedField] int turnDices = 0;
    [ShowNonSerializedField] int turnAttack = 0;

    [Button]
    void Start()
    {
        CreateBoard();
        SpawnCharacters();
    }

    [Button]
    void CreateBoard()
    {
        boardManager.CreateBoard(size);
    }

    [Button]
    void SpawnCharacters()
    {
        charactersManager.SpawnCharacters(charactersTypes, boardManager.spawnPoints);
    }

    void ResetTurn()
    {
        turnDices = defaulTurnDices;
        turnMoves = defaulTurnMoves;
        turnAttack = defaulTurnAttack;
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
        turnCharacter.AddHealth(20);
    }
}
