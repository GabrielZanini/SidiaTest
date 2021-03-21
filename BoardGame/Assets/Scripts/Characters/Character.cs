using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("Settings")]
    public CharacterType type = CharacterType.Player;

    [SerializeField]
    Color color = Color.white;

    [SerializeField]
    [Expandable]
    CharacterSettings settings;
    
    
    public UnityEvent onMove;

    public Tile currentTile;
    [SerializeField] float moveSpeed;

    [SerializeField]
    [ReadOnly]
    int health = 0;
    [ReadOnly]
    [Header("Turn")]
    public CharacterSate state = CharacterSate.Waiting;
    [ReadOnly]
    public bool isTurn = false;
    [SerializeField]
    [ReadOnly]
    Character enemy;

    [Header("Sprites")]
    [SerializeField]
    SpriteRenderer head;
    [SerializeField]
    SpriteRenderer hat;
    [SerializeField]
    SpriteRenderer body;


    Arrows arrows;
    CharactersManager manager;

    private void Reset()
    {
        arrows = GetComponentInChildren<Arrows>();
        manager = GetComponentInParent<CharactersManager>();

        SetSettings(settings);
        SetColor(color);
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

    [Button]
    void Update()
    {
        if (isTurn)
        {
            if (state == CharacterSate.Moving)
            {
                Move();
                arrows.HideArrows();
                
                if (currentTile.transform.position == transform.position)
                {
                    enemy = EnemyNear();

                    if (enemy != null && false)
                    {
                        state = CharacterSate.Fighting;
                    }
                    else
                    {
                        state = CharacterSate.Waiting;
                    }
                }
            }
            else if (state == CharacterSate.Waiting)
            {
                if (type == CharacterType.Player)
                {
                    arrows.ShowArrows(currentTile);
                }
            }
            else if (state == CharacterSate.Fighting)
            {
                
            }
        }
        else
        {
            transform.position = currentTile.transform.position;
            state = CharacterSate.Waiting;
            arrows.HideArrows();
            enemy = null;
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

    Character EnemyNear()
    {
        for (int i = 0; i < currentTile.Neighbors.Count; i++)
        {
            if (currentTile.Neighbors[i].tile.content == TileContentType.Character)
            {
                return currentTile.Neighbors[i].tile.character;
            }
        }

        return null;
    }


    public void MoveTo(DirectionType direction)
    {        
        for (int i=0; i<currentTile.Neighbors.Count; i++)
        {
            if (currentTile.Neighbors[i].direction == direction)
            {
                currentTile.content = TileContentType.Empty;
                currentTile.character = null;
                currentTile = currentTile.Neighbors[i].tile;
                currentTile.content = TileContentType.Character;
                currentTile.character = this;
                state = CharacterSate.Moving;
                onMove.Invoke();
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


    public void SetColor(Color newColor)
    {
        color = newColor;

        head.color = color;
        hat.color = color;
        body.color = color;
    }

    public void SetHat(Sprite sprite)
    {
        hat.sprite = sprite;
    }

    public void SetSettings(CharacterSettings settings)
    {
        this.settings = settings;

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
    }
}
