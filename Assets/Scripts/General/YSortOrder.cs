using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class YSortOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public int offset = 0; // Optional manual offset if needed

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Multiply by -100 to invert Y (lower on screen = higher order)
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100) + offset;
    }

}
