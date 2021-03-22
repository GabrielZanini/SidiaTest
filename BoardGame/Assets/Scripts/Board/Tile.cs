using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [ReadOnly]
    [Space]
    public SpriteRenderer spriteRenderer;
    [ReadOnly]
    [Space]
    public TileContentType content = TileContentType.Empty;

    [ReadOnly]
    public Character character;
    [ReadOnly]
    public PickUp pickUp;

    [SerializeField]
    public List<Neighbor> Neighbors;


    public int X { get { return Mathf.RoundToInt(transform.position.x); } }
    public int Y { get { return Mathf.RoundToInt(transform.position.y); } }
    public int Z { get { return Mathf.RoundToInt(transform.position.z); } }
    

    private void Reset()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void OnValidate()
    {
        Reset();
    }

    void Update()
    {
        
    }
}

[Serializable]
public class Neighbor
{
    public Neighbor(DirectionType direction, Tile tile)
    {
        this.direction = direction;
        this.tile = tile;
    }


    public DirectionType direction;
    public Tile tile;
}