using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrows : MonoBehaviour
{
    [SerializeField] Arrow Up;
    [SerializeField] Arrow Down;
    [SerializeField] Arrow Left;
    [SerializeField] Arrow Right;


    public void ShowArrows(Tile tile)
    {
        HideArrows();

        for (int i=0; i<tile.Neighbors.Count; i++)
        {
            if (tile.Neighbors[i].direction == DirectionType.Up)
            {
                Up.Visible = true;
            }
            else if (tile.Neighbors[i].direction == DirectionType.Down)
            {
                Down.Visible = true;
            }
            else if (tile.Neighbors[i].direction == DirectionType.Left)
            {
                Left.Visible = true;
            }
            else if (tile.Neighbors[i].direction == DirectionType.Right)
            {
                Right.Visible = true;
            }
        }
    }


    public void HideArrows()
    {
        Up.Visible = false;
        Down.Visible = false;
        Left.Visible = false;
        Right.Visible = false;
    }
}
