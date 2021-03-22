using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] DirectionType direction;
    
    [SerializeField] bool visible = false;
    public bool Visible { 
        get { return visible; }
        set {
            visible = value;
            ShowArrow();
        }
    }

    Character character;
    SpriteRenderer spriteRenderer;
    Collider boxCollider;

    private void Reset()
    {
        if (character == null)
        {
            character = GetComponentInParent<Character>();
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (boxCollider == null)
        {
            boxCollider = GetComponent<Collider>();
        }
    }

    private void OnValidate()
    {
        Reset();
        ShowArrow();
    }

    private void Awake()
    {
        OnValidate();
    }

    private void OnMouseDown()
    {
        Debug.Log("Click Arrow");
        character.MoveTo(direction);
    }

    void ShowArrow()
    {
        spriteRenderer.enabled = visible;
        boxCollider.enabled = visible;
    }
}
