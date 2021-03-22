using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class PickUpData
{
    public PickUpType type;
    [ShowAssetPreview(32, 32)]
    public Sprite sprite;
    public int value = 10;
    [Range(1, 100)]
    public int rarity = 1;
}
