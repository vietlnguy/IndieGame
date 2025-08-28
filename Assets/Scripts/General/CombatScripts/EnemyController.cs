using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public int offset = 0;
    public BattleController battleController;
    public int hp;
    public int mana;
    public int attack;
    public int defense;
    public int skill;
    public int speed;
    public int attackRange;
    public int moveRange;


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

    }

    void OnMouseExit()
    {

    }
    void OnMouseDown()
    {
        if (battleController.introFinished)
        {
            battleController.selectEnemy(gameObject);
        }

    }
    public void highlightAttackable()
    {
        spriteRenderer.color = Color.red;
    }
    public void unhighlightAttackable()
    {
        spriteRenderer.color = Color.white;
    }

}
