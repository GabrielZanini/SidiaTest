using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    GameObject tilePrefab;

    [SerializeField]
    Transform holder;

    [SerializeField]
    Sprite squareTile;

    [Range(4, 16)]
    public int size = 16;


    [ReorderableList]
    [SerializeField]
    List<Color> tileColors;

    [ReorderableList]
    public List<Tile> spawnPoints;

    [ReadOnly]
    public List<Tile> tiles;

    int nextColorId = -1;

    private void Reset()
    {
        if (tileColors.Count == 0)
        {
            tileColors.Add(Color.white);
        }
    }

    private void OnValidate()
    {
        Reset();
    }

    void Start()
    {

    }


    public void CreateBoard()
    {
        ClearBoardTiles();
        CreateSquareBoard();
    }

    void CreateSquareBoard()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GameObject obj = Instantiate(tilePrefab, new Vector3(j, 0, i), Quaternion.Euler(90, 0, 0), holder);
                obj.name = "Tile " + j + "x" + i;
                Tile tile = obj.GetComponent<Tile>();

                tile.spriteRenderer.sprite = squareTile;
                tile.spriteRenderer.color = GetNextColor();

                // Vertical Neighbors
                if (i > 0)
                {
                    tile.Neighbors.Add(new Neighbor(DirectionType.Down, tiles[(i - 1) * size + j]));
                    tiles[(i - 1) * size + j].Neighbors.Add(new Neighbor(DirectionType.Up, tile));
                }

                // Horizontal Neighbors
                if (j > 0)
                {
                    tile.Neighbors.Add(new Neighbor(DirectionType.Left, tiles[i * size + j - 1]));
                    tiles[i * size + j - 1].Neighbors.Add(new Neighbor(DirectionType.Right, tile));
                }

                tiles.Add(tile);
            }

            if (size % 2 == 0)
            {
                GetNextColor();
            }            
        }

        spawnPoints.Add(tiles[0]);
        spawnPoints.Add(tiles[tiles.Count - 1]);
    }

    [Button]
    public void ClearBoardTiles()
    {
        nextColorId = -1;
        spawnPoints.Clear();

        while (tiles.Count > 0)
        {
            if (Application.isPlaying)
            {
                Destroy(tiles[0].gameObject);
            }
            else
            {
                DestroyImmediate(tiles[0].gameObject);
            }
            
            tiles.Remove(tiles[0]);
        }
    }

    public void SetSize(float s)
    {
        size = (int)s;
    }

    Color GetNextColor()
    {
        //Debug.Log("nextColorId: " + nextColorId);
        nextColorId += 1;

        if (nextColorId >= tileColors.Count)
        {
            nextColorId = 0;
        }

        return tileColors[nextColorId];
    }



}



