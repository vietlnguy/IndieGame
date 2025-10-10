using UnityEngine;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public int offset = 0;
    public BattleController battleController;
    public CharacterToolTip characterToolTipScript;
    public MoveRangeCircle moveRangeCircleScript;
    public AttackRangeCircle attackRangeCircleScript;
    public AttackPreview attackPreviewScript;
    public CharacterMenu characterMenuScript;
    private Rigidbody2D rigidBody;
    private bool isHovered = false;
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
    public AudioSource deselectAudio;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        characterToolTipScript = GameObject.Find("characterInfoToolTip").GetComponent<CharacterToolTip>();
        moveRangeCircleScript = GameObject.Find("MoveRangeCircle").GetComponent<MoveRangeCircle>();
        attackRangeCircleScript = GameObject.Find("AttackRangeCircle").GetComponent<AttackRangeCircle>();
        characterMenuScript = GameObject.Find("CharacterMenu").GetComponent<CharacterMenu>();
        attackPreviewScript = GameObject.Find("AttackPreview").GetComponent<AttackPreview>();
        deselectAudio = GameObject.Find("AttackPreviewWoosh").GetComponent<AudioSource>();
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
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int layerMask = LayerMask.GetMask("Characters"); // ignore AttackRange layer
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            // Hover logic
            if (!isHovered)
            {
                isHovered = true;
                OnHoverEnter();
            }

            // Click logic
            if (Input.GetMouseButtonDown(0))
            {
                OnClick();
            }
        }
        else if (isHovered)
        {
            isHovered = false;
            OnHoverExit();
        }

    }
    void LateUpdate()
    {
        // Multiply by -100 to invert Y (lower on screen = higher order)
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100) + offset;
    }
    void OnHoverEnter()
    {
        if (battleController.introFinished)
        {
            if (!characterMenuScript.active && !attackPreviewScript.active)
            {
                characterToolTipScript.enableCharacterToolTip(gameObject);
            }
        }

    }
    void OnHoverExit()
    {
        characterToolTipScript.disableCharacterToolTip();
    }
    public void OnClick()
    {
        if (battleController.introFinished && !battleController.isPaused && !battleController.attackPreviewScript.active && !characterMenuScript.active && !battleController.isEnemyTurn)
        {
            //no character or enemy selected
            if (battleController.characterSelected == null && battleController.enemySelected == null)
            {
                selectEnemy();
            }

            //no character selected but another enemy is selected
            else if (battleController.characterSelected == null && battleController.enemySelected != null)
            {
                battleController.enemySelected.GetComponent<EnemyController>().deselectEnemy();
                selectEnemy();
            }

            //another character is selected and this enemy is in attack range
            if (battleController.characterSelected != null && attackRangeCircleScript.enemiesInRange.Contains(gameObject))
            {
                battleController.enemySelected = gameObject;
                StartCoroutine(attackPreviewScript.enablePreview());
            }

            //another character is selected and this enemy is not in attack range
            else if (battleController.characterSelected != null && !attackRangeCircleScript.enemiesInRange.Contains(gameObject))
            {
                battleController.characterSelected.GetComponent<PlayerController>().deselectCharacter();
                selectEnemy();
            }
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
    public void graySpriteAndFreeze()
    {
        spriteRenderer.color = Color.gray;
        rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    public void selectEnemy()
    {
        battleController.enemySelected = gameObject;
        moveRangeCircleScript.enableMoveRange(gameObject);
        attackRangeCircleScript.enableAttackRange(gameObject);
    }
    public void deselectEnemy()
    {
        deselectAudio.Play();
        moveRangeCircleScript.disableMoveRange();
        attackRangeCircleScript.disableAttackRange();
        battleController.enemySelected = null;
        unhighlight();
    }
}
