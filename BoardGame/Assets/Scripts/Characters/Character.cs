using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("Settings")]
    [ReadOnly] public string charName;
    public bool isLocked = false;
    public CharacterType type = CharacterType.Player;

    public Color color = Color.white;

    [Expandable]
    public CharacterSettings settings;    
    
    public Tile currentTile;
    [SerializeField]
    float moveSpeed;

    [ReadOnly]
    public int health = 0;
    [ReadOnly]
    public int attack = 0;

    [Header("Turn")]
    [SerializeField]
    [ReadOnly]
    public CharacterSate state = CharacterSate.Waiting;
    [ReadOnly]
    public bool isTurn = false;
    //[ReadOnly]
    //public bool canAttack = false;
    [ReadOnly]
    [SerializeField] bool hasMoved = false;
    [ReadOnly]
    public Character enemy;

    [Header("Sprites")]
    [SerializeField]
    SpriteRenderer head;
    [SerializeField]
    SpriteRenderer hat;
    [SerializeField]
    SpriteRenderer body;

    [Foldout("Events")]
    public UnityEvent onMove;
    [Foldout("Events")]
    public UnityEvent onAttack;
    [Foldout("Events")]
    public UnityEvent onPickUp;
    [Foldout("Events")]
    public UnityEvent onDeath;

    Arrows arrows;

    [HideInInspector]
    public CharacterCanvas canvas;
    [HideInInspector]
    public LevelManager level;

    public int Attack { get { return settings.Attack; } }
    public bool IsDead { get { return health <= 0; } }


    private void Reset()
    {
        arrows = GetComponentInChildren<Arrows>();
        canvas = GetComponentInChildren<CharacterCanvas>();

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

    //[Button]
    void Update()
    {
        if (isTurn && !isLocked)
        {
            if (state == CharacterSate.Moving)
            {
                if (currentTile.transform.position != transform.position)
                {                    
                    Move();
                    arrows.HideArrows();
                    hasMoved = true;
                }
                else
                {                    
                    CollectPickUp();
                    WaitIfNotAttack();
                }
            }
            else if (state == CharacterSate.Waiting)
            {
                WaitIfNotAttack();

                if (type == CharacterType.Player)
                {
                    arrows.ShowArrows(currentTile);
                }
                else if (type == CharacterType.IA)
                {
                    MoveTo((DirectionType)Random.Range(0,4));
                }
            }
            else if (state == CharacterSate.Fighting)
            {
                arrows.HideArrows();
            }
        }
        else
        {
            transform.position = currentTile.transform.position;
            state = CharacterSate.Waiting;
            arrows.HideArrows();
            enemy = null;
            attack = settings.Attack;
            hasMoved = false;
        }
    }

    void WaitIfNotAttack()
    {
        state = CharacterSate.Waiting;

        if (hasMoved)
        {
            enemy = EnemyNear();

            if (enemy != null)
            {
                state = CharacterSate.Fighting;
                hasMoved = false;
                onAttack.Invoke();
            }
        }
        else
        {
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

    void CollectPickUp()
    {
        if (currentTile.content == TileContentType.Collectable)
        {
            PickUp pickUp = currentTile.pickUp.RemoveFromTileAndList();

            Debug.Log(gameObject.name +  " Picked: " + pickUp.type.ToString());
            
            if (pickUp.type == PickUpType.Attack)
            {
                AddAttack(pickUp.value);
            }
            else if (pickUp.type == PickUpType.Dice)
            {
                level.AddTurnDice(pickUp.value);
            }
            else if (pickUp.type == PickUpType.Health)
            {
                AddHealth(pickUp.value);
            }
            else if (pickUp.type == PickUpType.Move)
            {
                level.AddTurnMove(pickUp.value);
            }

            Destroy(pickUp.gameObject);

            onPickUp.Invoke(); 
        }

        currentTile.content = TileContentType.Character;
        currentTile.character = this;
    }

    public void MoveTo(DirectionType direction)
    {        
        for (int i=0; i<currentTile.Neighbors.Count; i++)
        {
            if (currentTile.Neighbors[i].direction == direction && currentTile.Neighbors[i].tile.content != TileContentType.Character)
            {
                currentTile.content = TileContentType.Empty;
                currentTile.character = null;
                currentTile = currentTile.Neighbors[i].tile;
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
        else if (health <= 0)
        {
            onDeath.Invoke();
        }

        //Debug.Log(gameObject.name + " HP = " + health + "/" + settings.MaxHealth);
    }

    public void TakeDamege(int damage)
    {
        AddHealth(-damage);
    }

    public void AddAttack(int atk)
    {
        attack += atk;

        if (attack < 0)
        {
            attack = 0;
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
            attack = settings.Attack;
            body.sprite = settings.sprite;
        }
        else
        {
            health = 0;
            attack = 0;
            body.sprite = null;
        }
    }
}
