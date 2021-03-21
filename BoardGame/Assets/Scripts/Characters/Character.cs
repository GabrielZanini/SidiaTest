using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Character : MonoBehaviour
{
    [SerializeField]
    CharacterType type = CharacterType.Player;

    [SerializeField]
    Color color = Color.white;

    [SerializeField]
    [Expandable]
    CharacterSettings settings;

    [SerializeField]
    int health = 0;

    [Header("Sprites")]
    [SerializeField]
    SpriteRenderer head;
    [SerializeField]
    SpriteRenderer hat;
    [SerializeField]
    SpriteRenderer body;

    [SerializeField] Tile currentTile;
    [SerializeField] float moveSpeed;

    public bool isMoving = false;

    Arrows arrows;
    CharactersManager manager;

    private void Reset()
    {
        if (settings != null)
        {
            health = settings.MaxHealth;
            body.sprite = settings.sprite;
        }
        else
        {
            health = 0;
            body.sprite = null;
        }

        head.color = color;
        hat.color = color;
        body.color = color;

        arrows = GetComponentInChildren<Arrows>();
        manager = GetComponentInParent<CharactersManager>();
    }

    private void OnValidate()
    {
        Reset();
    }

    private void Awake()
    {
        OnValidate();
    }

    void Start()
    {
        
    }

    void Update()
    {
        isMoving = currentTile.transform.position != transform.position;

        if (isMoving)
        {
            Move();
            arrows.HideArrows();
        }
        else
        {
            arrows.ShowArrows(currentTile);
        }
    }

    void Move()
    {
        Vector3 currentPosition = transform.position;

        currentPosition.x = Mathf.MoveTowards(currentPosition.x, currentTile.transform.position.x, moveSpeed * Time.deltaTime);
        currentPosition.z = Mathf.MoveTowards(currentPosition.z, currentTile.transform.position.z, moveSpeed * Time.deltaTime);

        Vector3 offset = currentTile.transform.position - transform.position;

        transform.position = currentPosition;

        if (offset.sqrMagnitude < 0.001f)
        {
            transform.position = currentTile.transform.position;
        }
    }

    public void MoveTo(DirectionType direction)
    {        
        for (int i=0; i<currentTile.Neighbors.Count; i++)
        {
            if (currentTile.Neighbors[i].direction == direction)
            {
                currentTile = currentTile.Neighbors[i].tile;
                break;
            }
        }
    }

    public void AddHealth(int hp)
    {
        health += hp;
        if (health > settings.MaxHealth)
        {
            health = settings.MaxHealth;
        }
    }

}
