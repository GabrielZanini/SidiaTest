using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CharactersManager : MonoBehaviour
{
    [SerializeField]
    GameObject characterPrefab;
    [SerializeField] Transform holder;
 
    [SerializeField]
    [ReadOnly]
    List<Character> characters;

    [SerializeField]
    [ReadOnly]
    int turnCharacter = 0;




    public void SpawnCharacters(List<CharacterType> types, List<Tile> spawnPoints)
    {
        ClearCharacters();

        for (int i = 0; i < Mathf.Min(types.Count, spawnPoints.Count); i++)
        {
            GameObject obj = Instantiate(characterPrefab, spawnPoints[i].transform.position, Quaternion.identity, holder);
            obj.name = obj.name + " " + i;
            Character character = obj.GetComponent<Character>();
            character.type = types[i];
            character.currentTile = spawnPoints[i];
            characters.Add(character);
        }
    }

    [Button]
    void ClearCharacters()
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

}
