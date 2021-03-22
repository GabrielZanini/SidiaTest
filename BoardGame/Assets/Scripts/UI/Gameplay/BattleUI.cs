using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
    [SerializeField] GameObject charDicePrefab;
    [SerializeField] CharactersManager charactersManager;
    public LevelManager levelManager;

    public List<CharDiceUI> charDices;

    BattleState lastUpdate = BattleState.NONE;

    private void Update()
    {
        if (lastUpdate != levelManager.battleState)
        {
            lastUpdate = levelManager.battleState;

            if (lastUpdate == BattleState.NONE)
            {
                ClearCharDices();
            }
            else if(lastUpdate == BattleState.ThrowingDicesAttacker)
            {
                CreateCharactersDices();
            }
        }
    }

    public void CreateCharactersDices()
    {
        Debug.Log("Create CHAR Dices");

        ClearCharDices();

        for (int i=0; i<charactersManager.characters.Count; i++)
        {
            CharDiceUI charDice = Instantiate(charDicePrefab, transform).GetComponent<CharDiceUI>();
            charDice.character = charactersManager.characters[i];
            charDice.battleUI = this;
            charDices.Add(charDice);
        }
    }

    void ClearCharDices()
    {
        Debug.Log("Clear CHAR Dices");

        while (charDices.Count > 0)
        {
            CharDiceUI charDice = charDices[0];
            charDices.Remove(charDice);
            Destroy(charDice.gameObject);
        }
    }
}
