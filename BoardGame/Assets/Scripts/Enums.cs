using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Triangle,
    Square,
    Hexagon
}

public enum CharacterType
{
    Player,
    IA
}

public enum CharacterSate
{
    Waiting,
    Moving,
    Fighting,
}

public enum TurnActions
{
    Move,
    Attack
}

public enum DirectionType : int
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3,
}

public enum PickUpType
{
    Move,
    Attack,
    Health,
    Dice
}

public enum RarityTier
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
}

public enum TileContentType
{
    Empty,
    Collectable,
    Character,
}

public enum BattleState
{
    NONE,
    ThrowingDicesAttacker,
    ThrowingDicesAttacked,
    SortingDices,
    ComparingDices,
    ShowingWinner,
}