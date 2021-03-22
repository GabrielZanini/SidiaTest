using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCanvas : MonoBehaviour
{
    public Text label;
    public Slider healthBar;
    public Text dices;
    public Text steps;

    Character character;
    Camera mainCamera;

    private void OnValidate()
    {
        character = GetComponentInParent<Character>();
        mainCamera = Camera.main;

        if (character != null && character.settings != null)
        {
            ShowStatus();
        }
    }

    private void Awake()
    {
        OnValidate();
    }

    private void Start()
    {
        transform.rotation = mainCamera.transform.rotation;
        ShowStatus();
    }


    void Update()
    {
        ShowStatus();
    }

    void ShowStatus()
    {
        healthBar.maxValue = character.settings.MaxHealth;
        healthBar.value = character.health;
        //label.enabled = character.isTurn;
        dices.gameObject.SetActive(character.isTurn);
        steps.gameObject.SetActive(character.isTurn);
    }
}
