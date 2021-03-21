using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

public class CharactersManager : MonoBehaviour
{
    [SerializeField] GameObject characterPrefab;    
    [SerializeField] Transform holder;
 
    
    [ReadOnly]
    public List<Character> characters;


    public void SpawnCharacters(List<CharacterData> data, List<Tile> spawnPoints)
    {
        ClearCharacters();

        for (int i = 0; i < Mathf.Min(data.Count, spawnPoints.Count); i++)
        {
            GameObject obj = Instantiate(characterPrefab, spawnPoints[i].transform.position, Quaternion.identity, holder);
            Character character = obj.GetComponent<Character>();
            SetCharacter(i, character, data[i], spawnPoints[i]);
            characters.Add(character);
        }
    }

    void SetCharacter(int index, Character character, CharacterData data, Tile spawnPoint)
    {
        character.gameObject.name = "Character " + index; 
        character.type = data.type;
        character.SetColor(data.bodyColor);
        character.SetHat(data.hat);
        character.SetSettings(data.settings);
        character.currentTile = spawnPoint;
        spawnPoint.content = TileContentType.Character;

        Text label = character.GetComponentInChildren<Text>();
        label.color = data.labelColor;
        label.text = (index + 1).ToString();
        
        if (character.type == CharacterType.Player) 
        {
            character.gameObject.name += " Player";
            label.text += "P";

        }
        else if (character.type == CharacterType.IA)
        {
            character.gameObject.name += " IA";
            label.text = "CPU";
        }        
    }

    [Button]
    public void ClearCharacters()
    {
        for (int i=0; i < characters.Count; i++)
        {
            if (Application.isPlaying)
            {
                Destroy(characters[i].gameObject);
            }
            else
            {
                DestroyImmediate(characters[i].gameObject);
            }
        }

        characters.Clear();
    }

    public void LockCharacters()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].isLocked = false;
        }
    }

    public void UnlockCharacters()
    {
        
    }

}
