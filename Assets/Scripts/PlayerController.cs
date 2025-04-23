using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public int offset = 0; // Optional manual offset if needed
    public MainPlayerController parent;

    void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
    }

    void Update()
    {

    }

    void LateUpdate()
    {
        // Multiply by -100 to invert Y (lower on screen = higher order)
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100) + offset;
    }

    void OnMouseEnter()
    {   
        parent.OnChildMouseEnter(gameObject);
    }

    void OnMouseExit()
    {
        parent.OnChildMouseExit(gameObject);
    }

}
