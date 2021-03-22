using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class CharacterData
{
    public CharacterType type = CharacterType.Player;
    public Color labelColor = Color.white;
    public Color bodyColor = Color.white;
    [ShowAssetPreview(32, 32)]
    public Sprite hat;
    [Expandable]
    public CharacterSettings settings;
}
