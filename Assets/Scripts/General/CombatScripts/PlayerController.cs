using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public SaveManager saveManager;
    private Rigidbody2D rigidBody;
    public bool movementEnabled = false;
    private float moveSpeed = 6.0f;
    public AudioSource walkingAudio;
    public BattleController battleController;
    public CharacterToolTip characterToolTipScript;
    public MoveRangeCircle moveRangeCircleScript;
    public AttackRangeCircle attackRangeCircleScript;
    public AttackPreview attackPreviewScript;
    public CharacterMenu characterMenuScript;
    public CharacterAssistMenu characterAssistMenuScript;
    public InventoryMenu inventoryMenuScript;
    public int offset = 0;
    public int hp;
    public int maxHp;
    public int mana;
    public int maxMana;
    public int attack;
    public int defense;
    public int specialDefense;
    public int skill;
    public int speed;
    public int attackRange;
    public int moveRange;
    public int relationship;
    public bool owned;
    public string title;
    public List<string> statuses;
    public Equipment weaponEquiped;
    public Equipment armorEquiped;
    public Equipment accessoryEquiped;
    public List<Attack> knownAttacks;
    public List<Item> inventory;
    private Vector3 originalPosition;
    private bool isHovered = false;
    public AudioSource selectAudio;
    public AudioSource deselectAudio;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        walkingAudio = GameObject.Find("WalkingAudio").GetComponent<AudioSource>();
        battleController = GameObject.Find("BattleController").GetComponent<BattleController>();
        characterToolTipScript = GameObject.Find("characterInfoToolTip").GetComponent<CharacterToolTip>();
        saveManager = FindFirstObjectByType<SaveManager>();
        moveRangeCircleScript = GameObject.Find("MoveRangeCircle").GetComponent<MoveRangeCircle>();
        attackRangeCircleScript = GameObject.Find("AttackRangeCircle").GetComponent<AttackRangeCircle>();
        attackPreviewScript = GameObject.Find("AttackPreview").GetComponent<AttackPreview>();
        characterMenuScript = GameObject.Find("CharacterMenu").GetComponent<CharacterMenu>();
        characterAssistMenuScript = GameObject.Find("CharacterAssistMenu").GetComponent<CharacterAssistMenu>();
        inventoryMenuScript = GameObject.Find("InventoryMenu").GetComponent<InventoryMenu>();
        selectAudio = GameObject.Find("SelectBeep").GetComponent<AudioSource>();
        deselectAudio = GameObject.Find("AttackPreviewWoosh").GetComponent<AudioSource>();
        populateCharacterData();
    }
    void Start()
    {

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

        if (movementEnabled)
        {
            handleMovement();
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
            if (!battleController.disabledCharacters.Contains(gameObject) && battleController.characterSelected == null)
            {
                spriteRenderer.color = Color.yellow;
            }

            if (!characterMenuScript.active && !attackPreviewScript.active)
            {
                characterToolTipScript.enableCharacterToolTip(gameObject);
            }
        }

    }
    void OnHoverExit()
    {
        if (battleController.introFinished && !battleController.disabledCharacters.Contains(gameObject) && battleController.characterSelected == null)
        {
            spriteRenderer.color = Color.white;
        }
        characterToolTipScript.disableCharacterToolTip();

    }
    void OnClick()
    {
        //Intro is finished, not paused, not attack preview, not characterMenu, and not enemies turn
        if (battleController.introFinished && !battleController.isPaused && !battleController.isEnemyTurn && !attackPreviewScript.active && !characterMenuScript.active && !inventoryMenuScript.active && !characterAssistMenuScript.active)
        {
            //no character selected yet. Should select this character.
            if (battleController.characterSelected == null)
            {
                selectCharacter();
            }

            //this character already selected and this character is clicked again. Should bring up inventory and end turn menu
            else if (battleController.characterSelected == gameObject && !characterAssistMenuScript.active)
            {
                characterMenuScript.enableCharacterMenu(gameObject);
                movementEnabled = false;
                characterToolTipScript.disableCharacterToolTip();
                selectAudio.Play();
            }

            //different character already selected and this character is clicked
            else if (battleController.characterSelected != null && battleController.characterSelected != gameObject)
            {
                //if this character is in assistable range. Should bring up assistable UI
                if (attackRangeCircleScript.alliesInRange.Contains(gameObject))
                {
                    characterAssistMenuScript.enableCharacterAssistMenu(gameObject);
                    battleController.characterSelected.GetComponent<PlayerController>().movementEnabled = false;
                    battleController.assistableCharacterSelected = gameObject;
                    characterToolTipScript.disableCharacterToolTip();
                    selectAudio.Play();
                }

                //this character is not in assistable range. Should deselect other character and select this character
                else
                {
                    battleController.characterSelected.GetComponent<PlayerController>().deselectCharacter();
                    selectCharacter();
                }
            }

        }
    }
    void populateCharacterData()
    {

        string temp = gameObject.name.Substring(0, gameObject.name.IndexOf("Prefab"));
        Character hero;
        if (temp == "MainCharacter")
        {
            hero = saveManager.loadedData.characters.Find(c => c.characterName == saveManager.loadedData.mainCharacterName);
        }
        else
        {
            hero = saveManager.loadedData.characters.Find(c => c.characterName == temp);
        }
        hp = hero.maxHp;
        maxHp = hero.maxHp;
        mana = hero.maxMana;
        maxMana = hero.maxMana;
        attack = hero.attack;
        defense = hero.defense;
        specialDefense = hero.specialDefense;
        skill = hero.skill;
        speed = hero.speed;
        attackRange = hero.attackRange;
        moveRange = hero.moveRange;
        relationship = hero.relationship;
        owned = hero.owned;
        title = hero.characterName;
        knownAttacks = hero.knownAttacks;
        inventory = hero.inventory;
    }
    public void highlightAssistable()
    {
        spriteRenderer.color = Color.green;
    }
    public void unhighlight()
    {
        spriteRenderer.color = Color.white;
        animator.SetBool("isFrozen", false);
    }
    public void highlightAttackable()
    {
        spriteRenderer.color = Color.red;
    }
    public void handleMovement()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) { direction.y += 1; if (!walkingAudio.isPlaying) { walkingAudio.Play(); } }
        if (Input.GetKey(KeyCode.S))
        {
            direction.y -= 1;
            if (!walkingAudio.isPlaying)
            {
                walkingAudio.Play();
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.x -= 1;
            if (!walkingAudio.isPlaying)
            {
                walkingAudio.Play();
            }
            Vector3 localScale = transform.localScale;
            localScale.x = -Mathf.Abs(localScale.x);
            transform.localScale = localScale;

        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x += 1;
            if (!walkingAudio.isPlaying)
            {
                walkingAudio.Play();
            }
            Vector3 localScale = transform.localScale;
            localScale.x = Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }

        if (Input.GetKeyUp(KeyCode.W)) { walkingAudio.Stop(); }
        if (Input.GetKeyUp(KeyCode.S)) { walkingAudio.Stop(); }
        if (Input.GetKeyUp(KeyCode.A)) { walkingAudio.Stop(); }
        if (Input.GetKeyUp(KeyCode.D)) { walkingAudio.Stop(); }

        if (direction != Vector3.zero)
        {
            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
    public void graySpriteAndFreeze()
    {
        spriteRenderer.color = Color.gray;
        animator.SetBool("isFrozen", true);
        rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;

    }
    public void deselectCharacter()
    {
        //reset position and freeze
        deselectAudio.Play();
        transform.position = originalPosition;
        rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        moveRangeCircleScript.disableMoveRange();
        attackRangeCircleScript.disableAttackRange();
        movementEnabled = false;
        battleController.characterSelected = null;
        animator.SetBool("isWalking", false);
        walkingAudio.Stop();
        if (battleController.disabledCharacters.Contains(gameObject))
        {
            graySpriteAndFreeze();
        }
        else
        {
            unhighlight();
        }

    }
    public void selectCharacter()
    {
        originalPosition = transform.position;
        battleController.characterSelected = gameObject;
        moveRangeCircleScript.enableMoveRange(gameObject);
        attackRangeCircleScript.enableAttackRange(gameObject);
        if (!battleController.disabledCharacters.Contains(gameObject))
        {
            movementEnabled = true;
            rigidBody.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
            rigidBody.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        }
    }
    public void endTurn()
    {
        moveRangeCircleScript.disableMoveRange();
        attackRangeCircleScript.disableAttackRange();
        graySpriteAndFreeze();
        characterMenuScript.disableCharacterMenu();
        battleController.disabledCharacters.Add(gameObject);
        battleController.characterSelected = null;
        originalPosition = transform.position;
        movementEnabled = false;
    }
}
