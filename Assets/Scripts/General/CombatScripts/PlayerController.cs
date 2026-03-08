using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Animator animator;
    private SaveManager saveManager;
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
    public int baseMaxHp;
    public int baseMaxMana;
    public int baseAttack;
    public int baseIntelligence;
    public int baseDefense;
    public int baseResistance;
    public int baseSkill;
    public int baseSpeed;
    public int baseAttackRange;
    public int baseMoveRange;
    public int totalAttackMod;
    public int totalIntelligenceMod;
    public int totalDefenseMod;
    public int totalResistanceMod;
    public int totalSkillMod;
    public int totalSpeedMod;
    public float totalAttackMult;
    public float totalIntelligenceMult;
    public float totalDefenseMult;
    public float totalResistanceMult;
    public float totalSkillMult;
    public float totalSpeedMult;
    public float totalAttackRangeMult;
    public float totalMoveRangeMult;
    public float totalHpMod;
    public float totalHpMult;
    public float totalManaMod;
    public float totalManaMult;
    public List<Subquest> subquests;
    public bool owned;
    public string title;
    public bool ranged;
    public List<string> statuses;
    public Equipment weaponEquiped;
    public Equipment armorEquiped;
    public Equipment accessoryEquiped;
    public List<AttackMoves> knownAttacks;
    public List<Item> inventory;
    private Vector3 originalPosition;
    private bool isHovered = false;
    public AudioSource selectAudio;
    public AudioSource deselectAudio;
    public static event Action<string> OnCharacterDied;
    public string deathDialogue;
    public GameOver gameOverScript;
    public bool hoverable = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        walkingAudio = GameObject.Find("WalkingAudio").GetComponent<AudioSource>();
        battleController = GameObject.Find("BattleController").GetComponent<BattleController>();
        characterToolTipScript = GameObject.Find("CharacterInfoToolTip").GetComponent<CharacterToolTip>();
        saveManager = FindFirstObjectByType<SaveManager>();
        moveRangeCircleScript = GameObject.Find("MoveRangeCircle").GetComponent<MoveRangeCircle>();
        attackRangeCircleScript = GameObject.Find("AttackRangeCircle").GetComponent<AttackRangeCircle>();
        attackPreviewScript = GameObject.Find("AttackPreview").GetComponent<AttackPreview>();
        characterMenuScript = GameObject.Find("CharacterMenu").GetComponent<CharacterMenu>();
        characterAssistMenuScript = GameObject.Find("CharacterAssistMenu").GetComponent<CharacterAssistMenu>();
        inventoryMenuScript = GameObject.Find("InventoryMenu").GetComponent<InventoryMenu>();
        selectAudio = GameObject.Find("SelectBeep").GetComponent<AudioSource>();
        deselectAudio = GameObject.Find("DeselectAudio").GetComponent<AudioSource>();
        gameOverScript = GameObject.Find("GameOverScreen").GetComponent<GameOver>();
        populateCharacterData();
    }
    void Update()
    {
        if (!gameOverScript.active && hoverable) {
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
                StartCoroutine(characterMenuScript.enableCharacterMenu(gameObject));
                movementEnabled = false;
                walkingAudio.Stop();
                animator.SetBool("isWalking", false);
                characterToolTipScript.disableCharacterToolTip();
                selectAudio.Play();
            }

            //different character already selected and this character is clicked
            else if (battleController.characterSelected != null && battleController.characterSelected != gameObject)
            {
                //if this character is in assistable range. Should bring up assistable UI
                if (attackRangeCircleScript.alliesInRange.Contains(gameObject))
                {
                    walkingAudio.Stop();
                    battleController.characterSelected.GetComponent<PlayerController>().animator.SetBool("isWalking", false);
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
        Character savedCharacter;
        if (temp == "MainCharacter")
        {
            savedCharacter = saveManager.loadedData.characters.Find(c => c.characterName == saveManager.loadedData.mainCharacterName);
        }
        else
        {
            savedCharacter = saveManager.loadedData.characters.Find(c => c.characterName == temp);
        } 
        baseMaxHp = savedCharacter.baseMaxHp;
        baseMaxMana = savedCharacter.baseMaxMana;
        baseAttack = savedCharacter.baseAttack;
        baseIntelligence = savedCharacter.baseIntelligence;
        baseDefense = savedCharacter.baseDefense;
        baseResistance = savedCharacter.baseResistance;
        baseSkill = savedCharacter.baseSkill;
        baseSpeed = savedCharacter.baseSpeed;
        baseAttackRange = savedCharacter.baseAttackRange;
        baseMoveRange = savedCharacter.baseMoveRange;
        owned = savedCharacter.owned;
        ranged = savedCharacter.ranged;
        title = savedCharacter.characterName;
        knownAttacks = savedCharacter.knownAttacks;
        inventory = savedCharacter.inventory;
        weaponEquiped = savedCharacter.weaponEquiped;
        armorEquiped = savedCharacter.armorEquiped;
        accessoryEquiped = savedCharacter.accessoryEquiped;
        subquests = savedCharacter.subquests;
        
        if (savedCharacter.characterName == "Astrid")
        {
            deathDialogue = "Ah-- I suppose this is it. Stay safe everyone...";
        }
        else if (savedCharacter.characterName == "Amara")
        {
            deathDialogue = "Guh- I guess my luck finally ran out...";
        }
        else if (savedCharacter.characterName == "Celeste")
        {
            deathDialogue = "Ah- goddess... I come to you.. Protect Luc--";
        }
        else if (savedCharacter.characterName == "Gerard")
        {
            deathDialogue = "Urgh- no. I cannot fall here. I must protect the princess...";
        }
        else if (savedCharacter.characterName == "Ivy")
        {
            deathDialogue = "Ah- protect... forest.. please.";
        }
        else if (savedCharacter.characterName == "Katherine")
        {
            deathDialogue = "Ach- Not yet.. I can still fight.. I am strong..";   
        }
        else if (savedCharacter.characterName == "Lucas")
        {
            deathDialogue = "Guh- Bummer... Looks like I won't be a legend...";
        }
        else if (savedCharacter.characterName == "Penelope")
        {
            deathDialogue = "Oh- that's my blood... and I was just getting to the good part...";   
        }
        else if (savedCharacter.characterName == "Vanessa")
        {
            deathDialogue = "Ah- my time is up. I knew you'd come for me one day, Lady Death...";
        }
        else
        {
            deathDialogue = "Ack- I think this is it for me.. I'm sorry, everyone... ";
        }


        calculateStats();
    }
    public void calculateStats()
    {   
        Equipment tempWeapon;
        Equipment tempArmor;
        Equipment tempAccessory;

        //When no equipment then use default
        if (weaponEquiped == null)
        {
            tempWeapon = new Equipment("temp", "weapon", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "temp");
        }
        else
        {
            tempWeapon = weaponEquiped;
        }       
        if (armorEquiped == null)
        {
            tempArmor = new Equipment("temp", "weapon", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "temp");
        }
        else
        {
            tempArmor = armorEquiped;
        }
        if (accessoryEquiped == null)
        {            
            tempAccessory = new Equipment("temp", "weapon", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "temp");
        }
        else
        {
            tempAccessory = accessoryEquiped;
        }
        
        totalAttackMod = tempWeapon.attackMod + tempArmor.attackMod + tempAccessory.attackMod;
        totalIntelligenceMod = tempWeapon.intelligenceMod + tempArmor.intelligenceMod + tempAccessory.intelligenceMod;
        totalDefenseMod = tempWeapon.defenseMod + tempArmor.defenseMod + tempAccessory.defenseMod;
        totalResistanceMod = tempWeapon.resistanceMod + tempArmor.resistanceMod + tempAccessory.resistanceMod;
        totalSkillMod = tempWeapon.skillMod + tempArmor.skillMod + tempAccessory.skillMod;
        totalSpeedMod = tempWeapon.speedMod + tempArmor.speedMod + tempAccessory.speedMod;
        totalHpMod = tempWeapon.hpMod + tempArmor.hpMod + tempAccessory.hpMod;
        totalManaMod = tempWeapon.manaMod + tempArmor.manaMod + tempAccessory.manaMod;
        
        totalAttackMult = 1 + tempWeapon.attackMult + tempArmor.attackMult + tempAccessory.attackMult;
        totalIntelligenceMult = 1 + tempWeapon.intelligenceMult + tempArmor.intelligenceMult + tempAccessory.intelligenceMult;
        totalDefenseMult = 1 + tempWeapon.defenseMult + tempArmor.defenseMult + tempAccessory.defenseMult;
        totalResistanceMult = 1 + tempWeapon.resistanceMult + tempArmor.resistanceMult + tempAccessory.resistanceMult;
        totalSkillMult = 1 + tempWeapon.skillMult + tempArmor.skillMult + tempAccessory.skillMult;
        totalSpeedMult = 1 +tempWeapon.speedMult + tempArmor.speedMult + tempAccessory.speedMult;
        totalAttackRangeMult = 1 + tempWeapon.attackRangeMult + tempArmor.attackRangeMult + tempAccessory.attackRangeMult;
        totalMoveRangeMult = 1 + tempWeapon.moveRangeMult + tempArmor.moveRangeMult + tempAccessory.moveRangeMult;
        totalHpMult = 1 + tempWeapon.hpMult + tempArmor.hpMult + tempAccessory.hpMult;
        totalManaMult = 1 + tempWeapon.manaMult + tempArmor.manaMult + tempAccessory.manaMult;

        maxHp = Mathf.RoundToInt((float)(baseMaxHp + totalHpMod) * totalHpMult);
        currentHp = maxHp;
        maxMana = Mathf.RoundToInt((float)(baseMaxMana + totalManaMod) * totalManaMult);
        currentMana = maxMana;
        attack = Mathf.RoundToInt((float)(baseAttack + totalAttackMod) * totalAttackMult);
        intelligence = Mathf.RoundToInt((float)(baseIntelligence + totalIntelligenceMod) * totalIntelligenceMult);
        defense = Mathf.RoundToInt((float)(baseDefense + totalDefenseMod) * totalDefenseMult);
        resistance = Mathf.RoundToInt((float)(baseResistance + totalResistanceMod) * totalResistanceMult);
        skill = Mathf.RoundToInt((float)(baseSkill + totalSkillMod) * totalSkillMult);
        speed = Mathf.RoundToInt((float)(baseSpeed + totalSpeedMod) * totalSpeedMult);
        moveRange = Mathf.RoundToInt((float)baseMoveRange* totalMoveRangeMult);
        attackRange = Mathf.RoundToInt((float)baseAttackRange * totalAttackRangeMult);
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
    public void Die()
    {
        OnCharacterDied?.Invoke(title);
    }
}
