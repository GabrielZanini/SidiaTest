using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [ReadOnly]
    public List<Tile> Neighbors;
    [ReadOnly]
    public SpriteRenderer spriteRenderer;


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
