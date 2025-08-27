using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public int offset = 0;
    public BattleController battleController;
    public int hp;
    public int maxHp;
    public int mana;
    public int maxMana;
    public int attack;
    public int defense;
    public int skill;
    public int speed;
    public int attackRange;
    public int moveRange;
    public int relationship;
    public bool owned;
    public Equipment weaponEquiped;
    public Equipment armorEquiped;
    public Equipment accessoryEquiped;
    public GameObject attackRangeObj;

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
        if (battleController.introFinished)
        {
            spriteRenderer.color = Color.yellow;
        }
    }

    void OnMouseExit()
    {
        if (battleController.introFinished)
        {
            spriteRenderer.color = Color.white;     
        }

    }
    void OnMouseDown()
    {
        if (battleController.introFinished)
        {
            battleController.selectCharacter(gameObject); 
        }

    }

}
