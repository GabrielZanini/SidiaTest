using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class LevelScreen : MonoBehaviour
{
    public LevelManager levelManager;
    public Text battle;
    public Text attack;

    BattleState lastUpdate = BattleState.NONE;

    void Update()
    {
        if (lastUpdate != levelManager.battleState)
        {
            lastUpdate = levelManager.battleState;

            if (lastUpdate == BattleState.NONE)
            {
                attack.enabled = false;
            }
            else if (lastUpdate == BattleState.ThrowingDicesAttacker)
            {
                battle.enabled = true;
            }
            else if (lastUpdate == BattleState.ThrowingDicesAttacked)
            {

            }
            else if (lastUpdate == BattleState.SortingDices)
            {
                battle.enabled = false;
            }
            else if (lastUpdate == BattleState.ComparingDices)
            {

            }
            else if (lastUpdate == BattleState.ShowingWinner)
            {
                if (levelManager.attacker != null)
                {
                    attack.text = levelManager.attacker.charName + " Attack!";
                }
                else
                {
                    attack.text = "Draw!";
                }
                
                attack.enabled = true;
            }
            else
            {

            }


        }
    }
}
