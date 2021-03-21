using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    GameObject tilePrefab;

    [SerializeField]
    Sprite squareTile;


    [ReorderableList]
    [SerializeField]
    List<Color> tileColors;

    [SerializeField]
    Board board;

    [ReorderableList]
    public List<Tile> spawnPoints;

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


    public void CreateBoard(int size)
    {
        ClearBoardTiles();
        CreateSquareBoard(size);
    }

    void CreateSquareBoard(int size)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GameObject obj = Instantiate(tilePrefab, new Vector3(j, 0, i), Quaternion.Euler(90, 0, 0), board.transform);
                obj.name = "Tile " + j + "x" + i;
                Tile tile = obj.GetComponent<Tile>();

                tile.spriteRenderer.sprite = squareTile;
                tile.spriteRenderer.color = GetNextColor();

                // Vertical Neighbors
                if (i > 0)
                {
                    tile.Neighbors.Add(new Neighbor(DirectionType.Down, board.tiles[(i - 1) * size + j]));
                    board.tiles[(i - 1) * size + j].Neighbors.Add(new Neighbor(DirectionType.Up, tile));
                }

                // Horizontal Neighbors
                if (j > 0)
                {
                    tile.Neighbors.Add(new Neighbor(DirectionType.Left, board.tiles[i * size + j - 1]));
                    board.tiles[i * size + j - 1].Neighbors.Add(new Neighbor(DirectionType.Right, tile));
                }

                board.tiles.Add(tile);
            }

            if (size % 2 == 0)
            {
                GetNextColor();
            }            
        }

        spawnPoints.Add(board.First);
        spawnPoints.Add(board.Last);
    }

    [Button]
    public void ClearBoardTiles()
    {
        nextColorId = -1;
        spawnPoints.Clear();

        while (board.tiles.Count > 0)
        {
            if (Application.isPlaying)
            {
                Destroy(board.First.gameObject);
            }
            else
            {
                DestroyImmediate(board.First.gameObject);
            }
            
            board.tiles.Remove(board.First);
        }
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



