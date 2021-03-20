﻿using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    GameObject tilePrefab;

    [SerializeField]
    Sprite squareTile;

    [SerializeField] [Range(4, 16)]
    int size = 16;

    [ReorderableList] [SerializeField]
    List<Color> tileColors;

    [SerializeField] [ReadOnly]
    List<Tile> board;

    int totalTiles = 0;

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
        CreateBoard();
    }

    void Update()
    {

    }

    [Button]
    void CreateBoard()
    {
        ClearBoardTiles();
        CreateSquareBoard();
    }

    void CreateSquareBoard()
    {
        //Debug.Log("CreateBoard");
        totalTiles = size * size;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GameObject obj = Instantiate(tilePrefab, new Vector3(j, 0, i), Quaternion.Euler(90, 0, 0), transform);
                Tile tile = obj.GetComponent<Tile>();

                tile.spriteRenderer.sprite = squareTile;
                tile.spriteRenderer.color = GetNextColor();

                tile.Neighbors = GetNeighbors();

                board.Add(tile);
            }
            GetNextColor();
        }
    }

    [Button]
    void ClearBoardTiles()
    {
        nextColorId = -1;

        while (board.Count > 0)
        {
            if (Application.isPlaying)
            {
                Destroy(board[0].gameObject);
            }
            else
            {
                DestroyImmediate(board[0].gameObject);
            }
            
            board.RemoveAt(0);
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


    List<Tile> GetNeighbors()
    {
        Debug.Log("-------------- Visinhos ------------");
        return null;
    }

}



