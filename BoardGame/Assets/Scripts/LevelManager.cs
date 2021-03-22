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
    [Range(4, 20)]
    int diceSides = 6;
    [Space]
    [SerializeField] int defaulTurnMoves = 3;
    [SerializeField] int defaulTurnDices = 3;
    [SerializeField] int defaulTurnAttack = 1;
    [SerializeField] [Range(0f,1f)] float battleStateTime = 0.5f;

    [Header("Turn")]
    [ReadOnly] public Character turnCharacter;
    [SerializeField] [ReadOnly] int turnMoves = 0;
    [SerializeField] [ReadOnly] int turnDices = 0;
    [SerializeField] [ReadOnly] int turnAttack = 0;
    [SerializeField] [ReadOnly] int turnCharacterId = -1;
    [ReadOnly] public List<int> characterDices;
    [ReadOnly] public List<int> enemyDices;
    [ReadOnly] public List<bool> characterDiceWins;
    [ReadOnly] public Character attacker;
    [ReadOnly] public Character attacked;
    [ReadOnly] public Character winner;
    [SerializeField] [ReadOnly] int maxPickUps = 0;
    [ReadOnly] public BattleState battleState = BattleState.NONE;

    [Space]
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
        //StartLevel();
    }

    private void Update()
    {
        if (turnMoves == 0 && turnCharacter.state == CharacterSate.Waiting)
        {
            EndTurn();
        }

        if (((float)pickUpsManager.pickUps.Count / maxPickUps) < 0.1f)
        {
            RespawnPickUps();
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

        maxPickUps = boardManager.tiles.Count - charactersManager.characters.Count;

        onStart.Invoke();
    }
        
    //[Button]
    void CreateBoard()
    {
        boardManager.CreateBoard();
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
    void RespawnPickUps()
    {
        pickUpsManager.RespawnPickUps(boardManager.tiles);
    }

    [Button]
    public void ClearLevel()
    {
        turnCharacterId = -1;
        maxPickUps = 0;
        attacker = null;
        attacked = null;
        winner = null;
        boardManager.ClearBoardTiles();
        charactersManager.ClearCharacters();
        pickUpsManager.ClearPickUps();
    }

    //[Button]
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

        Debug.Log("Turn: " + turnCharacter.name);
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
        Debug.Log("StartBattle");
        StartCoroutine(Battle());
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
    }

    void GameOver()
    {
        Debug.Log("GAME OVER - " + winner + " wins.");
        StopAllCoroutines();
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


    IEnumerator Battle()
    {
        battleState = BattleState.NONE;
        yield return new WaitForSeconds(battleStateTime);

        characterDices.Clear();
        enemyDices.Clear();
        characterDiceWins.Clear();

        // Throw Turn Dices
        for (int i = 0; i < turnDices; i++)
        {
            characterDices.Add(Random.Range(0, diceSides) + 1);
        }

        battleState = BattleState.ThrowingDicesAttacker;
        yield return new WaitForSeconds(battleStateTime);

        // Throw Enemy Dices
        for (int i = 0; i < defaulTurnDices; i++)
        {
            enemyDices.Add(Random.Range(0, diceSides) + 1);
        }

        battleState = BattleState.ThrowingDicesAttacked;
        yield return new WaitForSeconds(battleStateTime);

        // Order Dices
        OrderListBigger(characterDices);
        OrderListBigger(enemyDices);

        battleState = BattleState.SortingDices;
        yield return new WaitForSeconds(battleStateTime);

        // Compare DICES
        int charDiceWins = 0;
        int charDiceLoses = 0;
        characterDiceWins.Clear();

        for (int i = 0; i < Mathf.Min(characterDices.Count, enemyDices.Count); i++)
        {
            characterDiceWins.Add(characterDices[i] >= enemyDices[i]);

            if (characterDiceWins[i])
            {
                charDiceWins += 1;
            }
            else
            {
                charDiceLoses += 1;
            }
        }

        battleState = BattleState.ComparingDices;
        yield return new WaitForSeconds(battleStateTime);

        // Attack the Loser
        if (charDiceWins > charDiceLoses)
        {
            attacker = turnCharacter;
            attacked = turnCharacter.enemy;
        }
        else if (charDiceWins < charDiceLoses)
        {
            attacker = turnCharacter.enemy;
            attacked = turnCharacter;
        }

        battleState = BattleState.ShowingWinner;
        yield return new WaitForSeconds(battleStateTime);

        if (attacker != null && attacked != null) 
        {
            Attack(attacker, attacked);
        }        

        battleState = BattleState.NONE;
        yield return new WaitForSeconds(battleStateTime);

        turnCharacter.state = CharacterSate.Waiting;

        attacker = null;
        attacked = null;
    }
}
