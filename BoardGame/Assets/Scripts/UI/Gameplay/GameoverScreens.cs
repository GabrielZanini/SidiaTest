using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverScreens : MonoBehaviour
{
    [SerializeField] LevelManager level;
    [SerializeField] Text winner;
    

    private void OnEnable()
    {
        winner.text = level.winner.canvas.label.text;
        winner.color = level.winner.canvas.label.color;
    }
}
