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

public enum DirectionType
{
    Up,
    Down,
    Left,
    Right
}

public enum PickUpType
{
    Move,
    Attack,
    Health,
    Dice
}


public enum TileContentType
{
    Empty,
    Collectable,
    Character,
}