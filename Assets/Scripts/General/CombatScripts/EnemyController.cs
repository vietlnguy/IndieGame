using UnityEngine;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public int offset = 0;
    public BattleController battleController;
    public CharacterToolTip characterToolTipScript;
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
    public string title;
    public bool roams;
    public Attack attackMove;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterToolTipScript = GameObject.Find("characterInfoToolTip").GetComponent<CharacterToolTip>();
    }
    void Start()
    {
        //Crate moveset
        if (title == "Soldier")
        {
            attackMove = new Attack("Slash", "physical", 10, 100, 0, 0, "Slash the enemy with sword");
        }
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
            characterToolTipScript.enableCharacterToolTip(gameObject);    
        }

    }
    void OnMouseExit()
    {
        characterToolTipScript.disableCharacterToolTip();    
    }
    void OnMouseDown()
    {
        if (battleController.introFinished)
        {
            
        }

    }
    public void highlightAttackable()
    {
        spriteRenderer.color = Color.red;
    }
    public void highlightAssistable()
    {
        spriteRenderer.color = Color.green;
    }
    public void unhighlight()
    {
        spriteRenderer.color = Color.white;
    }

}
