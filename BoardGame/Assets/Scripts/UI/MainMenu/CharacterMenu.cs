using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    public CharactersManager manager;
    public int id = 0;
    public Text typeLabel;
    public Image head;
    public Image hat;
    public Image body;
    public CharacterType type = CharacterType.Player;

    public List<Sprite> hats;
    public List<CharacterSettings> settings;
    public List<Color> colors;

    public int hatsId = 0;
    public int settingsId = 0;
    public int colorsId = 0;


    public void UpdateCharacterManager()
    {
        manager.charactersData[id].type = type;
        manager.charactersData[id].bodyColor = colors[colorsId];
        manager.charactersData[id].hat = hats[hatsId];
        manager.charactersData[id].settings = settings[settingsId];
    }


    public void ChangeType()
    {
        if (type == CharacterType.Player)
        {
            type = CharacterType.IA;
            typeLabel.text = "CPU";
        }
        else
        {
            type = CharacterType.Player;
            typeLabel.text = "Player";
        }

        UpdateCharacterManager();
    }

    public void ChangeHat(bool back = false)
    {
        if (back && hatsId > 0)
        {
            hatsId -= 1;
        }
        else if (!back && hatsId < hats.Count - 1)
        {
            hatsId += 1;
        }

        hat.enabled = hats[hatsId] != null;
        hat.sprite = hats[hatsId];

        UpdateCharacterManager();
    }

    public void ChangeSetting(bool back = false)
    {
        if (back && settingsId > 0)
        {
            settingsId -= 1;
        }
        else if (!back && settingsId < settings.Count - 1)
        {
            settingsId += 1;
        }

        body.sprite = settings[settingsId].sprite;
        UpdateCharacterManager();
    }

    public void ChangeColors(bool back = false)
    {
        if (back && colorsId > 0)
        {
            colorsId -= 1;
        }
        else if (!back && colorsId < colors.Count - 1)
        {
            colorsId += 1;
        }

        head.color = colors[colorsId];
        hat.color = colors[colorsId];
        body.color = colors[colorsId];
        
        UpdateCharacterManager();
    }



}
