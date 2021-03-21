using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField]
    CameraManager cameraManager;
    [SerializeField]
    BoardManager boardManager;
    [SerializeField]
    CharactersManager charactersManager;
    [SerializeField]
    PickUpsManager pickUpsManager;

    [Header("Settings")]
    [SerializeField]
    [Range(4, 16)]
    int size = 16;
    [SerializeField]
    [Range(4, 20)]
    int diceSides = 6;
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
    [SerializeField] [ReadOnly] int turnMoves = 0;
    [SerializeField] [ReadOnly] int turnDices = 0;
    [SerializeField] [ReadOnly] int turnAttack = 0;
    [SerializeField] [ReadOnly] int turnCharacterId = -1;
    [SerializeField] [ReadOnly] List<int> characterDices;
    [SerializeField] [ReadOnly] List<int> enemyDices;

    [SerializeField] UnityEvent onGameOver;

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
        else
        {
            
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
            turnCharacter.canAttack = false;
            turnCharacter.onMove.RemoveListener(RemoveTurnMove);
            turnCharacter.onAttack.RemoveListener(StartBattle);
        }
        
        turnCharacter = character;
        turnCharacter.isTurn = true;
        turnCharacter.canAttack = true;
        turnCharacter.onMove.AddListener(RemoveTurnMove);
        turnCharacter.onAttack.AddListener(StartBattle);

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

    void StartBattle()
    {
        characterDices.Clear();
        enemyDices.Clear();

        // Throw Turn Dices
        for (int i = 0; i < turnDices; i++)
        {
            characterDices.Add(Random.Range(0, diceSides) + 1);
        }

        // Throw Enemy Dices
        for (int i = 0; i < defaulTurnDices; i++)
        {
            enemyDices.Add(Random.Range(0, diceSides) + 1);
        }

        // Order Dices
        OrderListBigger(characterDices);
        OrderListBigger(enemyDices);


        // Compare DICES
        int charDiceWins = 0;
        int charDiceLoses = 0;

        for (int i = 0; i < Mathf.Min(characterDices.Count, enemyDices.Count); i++)
        {
            if (characterDices[i] >= enemyDices[i])
            {
                charDiceWins += 1;
            }
            else
            {
                charDiceLoses += 1;
            }
        }

        // Attack the Loser
        if (charDiceWins > charDiceLoses)
        {
            Attack(turnCharacter, turnCharacter.enemy);
        }
        else if (charDiceWins < charDiceLoses)
        {
            Attack(turnCharacter.enemy, turnCharacter);
        }
        else
        {
            turnCharacter.state = CharacterSate.Waiting;
        }
    }

    void Attack(Character attacker, Character attacked)
    {
        Debug.Log(attacker + " ATK -> " + attacked);
        attacked.TakeDamege(attacker.Attack);
        
        if (attacked.IsDead)
        {
            GameOver(attacker);
        }
        else
        {
            turnCharacter.state = CharacterSate.Waiting;
        }
    }

    void GameOver(Character winer)
    {
        Debug.Log("GAME OVER - " + winer + " wins.");
        onGameOver.Invoke();
    }

    private void OrderListBigger(List<int> list)
    {
        list.Sort();
        list.Reverse();
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
