using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Board : MonoBehaviour
{
    [ReadOnly]
    public List<Tile> tiles;

    public Tile First { get { return tiles[0]; } }
    public Tile Last { get { return tiles[tiles.Count-1]; } }

}
