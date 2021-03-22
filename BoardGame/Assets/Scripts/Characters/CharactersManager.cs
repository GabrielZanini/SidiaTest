using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

public class CharactersManager : MonoBehaviour
{
    [SerializeField] GameObject characterPrefab;    
    [SerializeField] Transform holder;
    [SerializeField]
    [ReorderableList]
    List<CharacterData> charactersData;


    [ReadOnly]
    public List<Character> characters;


    public void SpawnCharacters(List<Tile> spawnPoints)
    {
        ClearCharacters();

        for (int i = 0; i < Mathf.Min(charactersData.Count, spawnPoints.Count); i++)
        {
            GameObject obj = Instantiate(characterPrefab, spawnPoints[i].transform.position, Quaternion.identity, holder);
            Character character = obj.GetComponent<Character>();
            SetCharacter(i, character, charactersData[i], spawnPoints[i]);
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

        CharacterCanvas canvas = character.GetComponentInChildren<CharacterCanvas>();
        canvas.label.color = data.labelColor;
        canvas.label.text = (index + 1).ToString();
        
        if (character.type == CharacterType.Player) 
        {
            character.gameObject.name += " Player";
            canvas.label.text += "P";

        }
        else if (character.type == CharacterType.IA)
        {
            character.gameObject.name += " IA";
            canvas.label.text = "CPU";
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
            characters[i].isLocked = true;
        }
    }

    public void UnlockCharacters()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].isLocked = false;
        }
    }

}
