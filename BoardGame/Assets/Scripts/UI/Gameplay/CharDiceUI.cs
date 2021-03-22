using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharDiceUI : MonoBehaviour
{
    public GameObject prefabDiceUI;
    public Character character;
    public BattleUI battleUI;
    public List<DiceUI> dices;

    BattleState lastUpdate = BattleState.NONE;


    private void Update()
    {
        if (lastUpdate != battleUI.levelManager.battleState)
        {
            lastUpdate = battleUI.levelManager.battleState;

            if (lastUpdate == BattleState.NONE)
            {
                ClearCharDices();
            }
            else if (lastUpdate == BattleState.ThrowingDicesAttacker)
            {
                CreateDices();
            }
            else if (lastUpdate == BattleState.ThrowingDicesAttacked)
            {
                CreateDices();
            }
            else if (lastUpdate == BattleState.SortingDices)
            {
                ShowNumbers();
            }
            else if (lastUpdate == BattleState.ComparingDices)
            {
                ShowVictory(battleUI.levelManager.characterDiceWins);
            }
            else
            {
                ClearCharDices();
            }
        }
    }

    public void CreateDices()
    {
        ClearCharDices();

        if (battleUI == null || battleUI.levelManager.turnCharacter == null || character == null)
        {
            return;
        }

        if (battleUI.levelManager.turnCharacter == character)
        {
            CreateDices(battleUI.levelManager.characterDices);
        }
        else if (battleUI.levelManager.turnCharacter.enemy == character)
        {
            CreateDices(battleUI.levelManager.enemyDices);
        }
    }

    void CreateDices(List<int> vals)
    {
        for (int i = 0; i < vals.Count; i++)
        {
            DiceUI diceUI = Instantiate(prefabDiceUI, transform).GetComponent<DiceUI>();
            diceUI.dice.color = character.color;
            diceUI.number.text = vals[i].ToString();
            dices.Add(diceUI);
        }
    }

    void ShowNumbers()
    {
        List<int> vals = new List<int>();
        
        if (battleUI.levelManager.turnCharacter == character)
        {
            vals = battleUI.levelManager.characterDices;
        }
        else if (battleUI.levelManager.turnCharacter.enemy == character)
        {
            vals = battleUI.levelManager.enemyDices;
        }

        for (int i = 0; i < Mathf.Min(vals.Count,dices.Count); i++)
        {
            dices[i].number.text = vals[i].ToString();
        }
    }

    void ShowVictory(List<bool> wins)
    {
        for (int i = 0; i < Mathf.Min(wins.Count, dices.Count); i++)
        {
            bool win = true;
               
            if (battleUI.levelManager.turnCharacter == character)
            {
                win = wins[i];
            }
            else if (battleUI.levelManager.turnCharacter.enemy == character)
            {
                win = !wins[i];
            }

            dices[i].cross.enabled = !win;
        }
    }

    void ClearCharDices()
    {
        while (dices.Count > 0)
        {
            DiceUI diceUI = dices[0];
            dices.Remove(diceUI);
            Destroy(diceUI.gameObject);
        }
    }
}
