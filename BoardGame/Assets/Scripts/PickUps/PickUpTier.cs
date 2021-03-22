using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PickUpTier
{
    public RarityTier tier;
    public Color color = Color.white;
    public int multiplier = 1;
    [Range(1, 100)]
    public int rarity = 1;
}
