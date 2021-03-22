using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PickUp : MonoBehaviour
{
    [ReadOnly]
    public PickUpType type;
    [ReadOnly]
    public int value = 10;
    [ReadOnly]
    public SpriteRenderer spriterRenderer;
    [ReadOnly]
    public Tile tile;

    private void OnValidate()
    {
        spriterRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Awake()
    {
        OnValidate();
    }

    public void RemoveFromTile()
    {
        tile.pickUp = null;

        if (tile.content == TileContentType.Collectable)
        {
            tile.content = TileContentType.Empty;
        }
    }
}
