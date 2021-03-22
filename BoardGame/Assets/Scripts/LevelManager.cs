﻿using NaughtyAttributes;
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
    [ReadOnly] public Character winner;

    [SerializeField] UnityEvent onStart;
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
        StartLevel();
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
    public void StartLevel()
    {
        ClearLevel();

        CreateBoard();
        SpawnCharacters();
        SpawnPickUps();
        EndTurn();

        onStart.Invoke();
    }
        
    //[Button]
    void CreateBoard()
    {
        boardManager.CreateBoard(size);
    }

    //[Button]
    void SpawnCharacters()
    {
        charactersManager.SpawnCharacters(boardManager.spawnPoints);
    }

    void SpawnPickUps()
    {
        pickUpsManager.SpawnPickUps(boardManager.tiles);
    }

    [Button]
    void ClearLevel()
    {
        turnCharacterId = -1;
        winner = null;
        boardManager.ClearBoardTiles();
        charactersManager.ClearCharacters();
        pickUpsManager.ClearPickUps();
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
            turnCharacter.canvas.steps.text = "0";
            turnCharacter.canvas.dices.text = "0";
            turnCharacter.onMove.RemoveListener(RemoveTurnMove);
            turnCharacter.onAttack.RemoveListener(StartBattle);
        }
        
        turnCharacter = character;
        turnCharacter.isTurn = true;
        turnCharacter.canvas.steps.text = turnMoves.ToString();
        turnCharacter.canvas.dices.text = turnDices.ToString();
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
        turnCharacter.canvas.steps.text = turnMoves.ToString();
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
            winner = attacker;
            GameOver();
        }
        else
        {
            turnCharacter.state = CharacterSate.Waiting;
        }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER - " + winner + " wins.");
        onGameOver.Invoke();
    }

    private void OrderListBigger(List<int> list)
    {
        list.Sort();
        list.Reverse();
    }

    



    // PickUps

    public void AddTurnMove(int mv = 1)
    {
        turnMoves += mv;
        turnCharacter.canvas.steps.text = turnMoves.ToString();
    }

    public void AddTurnDice(int dices)
    {
        turnDices += dices;
        turnCharacter.canvas.dices.text = turnDices.ToString();
    }

    public void AddTurnAttack(int atk)
    {
        turnCharacter.AddAttack(atk);
    }

    public void AddHealth(int hp = 20)
    {
        turnCharacter.AddHealth(hp);
    }
}
