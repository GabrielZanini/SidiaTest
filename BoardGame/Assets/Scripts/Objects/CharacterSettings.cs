﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharcaterSettings", menuName = "Character Settings")]
public class CharacterSettings : ScriptableObject
{
    public Sprite sprite;
    public int MaxHealth = 100;
    public int Attack = 20;
}
