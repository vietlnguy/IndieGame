using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public int offset = 0;
    public BattleController battleController;
    public CharacterToolTip characterToolTipScript;
    public MoveRangeCircle moveRangeCircleScript;
    public AttackRangeCircle attackRangeCircleScript;
    public EffectiveAttackRangeCircle effectiveAttackRangeCircleScript;
    public AttackPreview attackPreviewScript;
    public CharacterMenu characterMenuScript;
    public CharacterAssistMenu characterAssistMenuScript;
    public InventoryMenu inventoryMenuScript;
    private Rigidbody2D rigidBody;
    private bool isHovered = false;
    public int currentHp;
    public int maxHp;
    public int currentMana;
    public int maxMana;
    public int attack;
    public int intelligence;
    public int defense;
    public int resistance;
    public int skill;
    public int speed;
    public int attackRange;
    public int moveRange;
    public string title;
    public bool roams;
    public bool ranged;
    public bool boss = false;
    public bool support;
    public bool hybrid;
    public AudioSource deselectAudio;
    public List<AttackMoves> knownAttacks;
    public string deathDialogue;
    public GameOver gameOverScript;
    public AudioSource walkingAudio;
    public static event Action<GameObject[]> OnEnemyDied;
    public bool hoverable = true;
    public GameObject bossIconPrefab;

    void Awake()
    {
        walkingAudio = GameObject.Find("WalkingAudio").GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        battleController = GameObject.Find("BattleController").GetComponent<BattleController>();
        characterToolTipScript = GameObject.Find("CharacterInfoToolTip").GetComponent<CharacterToolTip>();
        moveRangeCircleScript = GameObject.Find("MoveRangeCircle").GetComponent<MoveRangeCircle>();
        attackRangeCircleScript = GameObject.Find("AttackRangeCircle").GetComponent<AttackRangeCircle>();
        effectiveAttackRangeCircleScript = GameObject.Find("EffectiveAttackRangeCircle").GetComponent<EffectiveAttackRangeCircle>();
        characterMenuScript = GameObject.Find("CharacterMenu").GetComponent<CharacterMenu>();
        characterAssistMenuScript = GameObject.Find("CharacterAssistMenu").GetComponent<CharacterAssistMenu>();
        attackPreviewScript = GameObject.Find("AttackPreview").GetComponent<AttackPreview>();
        inventoryMenuScript = GameObject.Find("InventoryMenu").GetComponent<InventoryMenu>();
        deselectAudio = GameObject.Find("DeselectAudio").GetComponent<AudioSource>();
        gameOverScript = GameObject.Find("GameOverScreen").GetComponent<GameOver>();


    }
    void Start()
    {
        if (boss)
        {
            Instantiate(bossIconPrefab, gameObject.transform, false);
        }
    }
    void Update()
    {
        if (!gameOverScript.active && hoverable) 
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
        if (battleController.introFinished && !battleController.isPaused && !battleController.isEnemyTurn && !attackPreviewScript.active && !characterMenuScript.active && !characterAssistMenuScript.active && !inventoryMenuScript.active)
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

            //another character is selected and this enemy is in attack range and character can still act
            if (battleController.characterSelected != null && attackRangeCircleScript.enemiesInRange.Contains(gameObject))
            {
                if (battleController.disabledCharacters.Contains(battleController.characterSelected))
                {
                    battleController.characterSelected.GetComponent<PlayerController>().deselectCharacter();
                    selectEnemy();
                }
                else
                {
                    battleController.enemySelected = gameObject;
                    walkingAudio.Stop();
                    battleController.characterSelected.GetComponent<PlayerController>().animator.SetBool("isWalking", false);
                    StartCoroutine(attackPreviewScript.enablePreview(false));
                }
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
        effectiveAttackRangeCircleScript.enableEffectiveAttackRange(gameObject);
    }
    public void deselectEnemy()
    {
        deselectAudio.Play();
        moveRangeCircleScript.disableMoveRange();
        attackRangeCircleScript.disableAttackRange();
        effectiveAttackRangeCircleScript.disableEffectiveAttackRange();
        battleController.enemySelected = null;
        unhighlight();
    }
    public void Die(GameObject killer)
    {
        GameObject[] list = { gameObject, killer };
        OnEnemyDied?.Invoke(list);
        Destroy(gameObject);
    }

}
