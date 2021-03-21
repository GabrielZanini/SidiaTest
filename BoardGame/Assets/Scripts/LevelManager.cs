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
    List<CharacterData> charactersData;
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
    [ShowNonSerializedField] int turnCharacterId = -1;



    private void OnValidate()
    {
        if (charactersManager.characters.Count > 0)
        {
            EndTurn();
        }
    }

    void Start()
    {
        CreateLevel();
    }

    private void Update()
    {
        if (turnMoves == 0 && turnCharacter.state == CharacterSate.Waiting)
        {
            EndTurn();
        }
    }

    [Button]
    void CreateLevel()
    {
        ClearLevel();

        CreateBoard();
        SpawnCharacters();

        EndTurn();
    }
        
    //[Button]
    void CreateBoard()
    {
        boardManager.CreateBoard(size);
    }

    //[Button]
    void SpawnCharacters()
    {
        charactersManager.SpawnCharacters(charactersData, boardManager.spawnPoints);
    }

    [Button]
    void ClearLevel()
    {
        turnCharacterId = -1;
        boardManager.ClearBoardTiles();
        charactersManager.ClearCharacters();
    }

    [Button]
    void EndTurn()
    {
        ResetTurn();
        SetTurnCharacter(NextCharacter());
    }

    void ResetTurn()
    {
        turnDices = defaulTurnDices;
        turnMoves = defaulTurnMoves;
        turnAttack = defaulTurnAttack;
    }

    void SetTurnCharacter(Character character)
    {
        ResetTurn();

        if (turnCharacter != null)
        {
            turnCharacter.isTurn = false;
            turnCharacter.onMove.RemoveListener(RemoveTurnMove);
        }
        
        turnCharacter = character;
        turnCharacter.isTurn = true;
        turnCharacter.onMove.AddListener(RemoveTurnMove);

        cameraManager.Target = turnCharacter.transform;
    }

    Character NextCharacter()
    {
        turnCharacterId += 1;

        if (turnCharacterId >= charactersManager.characters.Count)
        {
            turnCharacterId = 0;
        }

        return charactersManager.characters[turnCharacterId];
    }


    void RemoveTurnMove()
    {
        turnMoves -= 1;
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
