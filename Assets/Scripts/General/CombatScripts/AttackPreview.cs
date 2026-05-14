using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class AttackPreview : MonoBehaviour
{
    private BattleController battleController;
    public GameObject attackerPreviewPanel;
    public GameObject defenderPreviewPanel;
    public GameObject confirmButton;
    public GameObject attackIcon;
    public GameObject assistIcon;
    public bool active = false;
    public TextMeshProUGUI characterTitle;
    public TextMeshProUGUI enemyTitle;
    public GameObject portraitBackground;
    public GameObject enemyPortraitBackground;
    public GameObject mainCharacterPrefab;
    public GameObject astridPrefab;
    public GameObject soldierPrefab;
    public GameObject attackPreviewSprites;
    public GameObject attacks;
    public GameObject rightsideAttacks;
    private SaveManager scm;
    public GameObject attackSelector;
    private Vector2 attackSelectorInitialPos;
    private int attackIndex = 0;
    public bool validAttack = false;
    public TextMeshProUGUI atkBlock;
    public TextMeshProUGUI hitBlock;
    public TextMeshProUGUI critBlock;
    public TextMeshProUGUI manaBlock;
    public TextMeshProUGUI attackDescription;
    public TextMeshProUGUI rightsideAtkBlock;
    public TextMeshProUGUI rightsideHitBlock;
    public TextMeshProUGUI rightsideCritBlock;
    public TextMeshProUGUI rightsideManaBlock;
    public TextMeshProUGUI rightsideAttackDescription;
    public GameObject attackPanel;
    public TextMeshProUGUI battleScreenPlayerName;
    public TextMeshProUGUI battleScreenEnemyName;
    public TextMeshProUGUI battleScreenLeftAttack;
    public TextMeshProUGUI battleScreenRightAttack;
    public TextMeshProUGUI battleScreenLeftATK;
    public TextMeshProUGUI battleScreenLeftHIT;
    public TextMeshProUGUI battleScreenLeftCRIT;
    public TextMeshProUGUI battleScreenEnemyATK;
    public TextMeshProUGUI battleScreenEnemyHIT;
    public TextMeshProUGUI battleScreenEnemyCRIT;
    public TextMeshProUGUI battleScreenLeftHp;
    public TextMeshProUGUI battleScreenRightHp;
    public GameObject battleScreenLeftHpBar;
    public GameObject battleScreenRightHpBar;
    public TextMeshProUGUI battleScreenLeftMana;
    public TextMeshProUGUI battleScreenRightMana;
    public GameObject battleScreenLeftManaBar;
    public GameObject battleScreenRightManaBar;
    public TextMeshProUGUI previewPlayerHp;
    public TextMeshProUGUI previewPlayerMaxHp;
    public TextMeshProUGUI previewPlayerMana;
    public TextMeshProUGUI previewPlayerMaxMana;
    public TextMeshProUGUI previewEnemyHp;
    public TextMeshProUGUI previewEnemyMaxHp;
    public TextMeshProUGUI previewEnemyMana;
    public TextMeshProUGUI previewEnemyMaxMana;
    public GameObject previewPlayerHpBar;
    public GameObject previewPlayerManaBar;
    public GameObject previewEnemyHpBar;
    public GameObject previewEnemyManaBar;
    public ConfirmAttackButton confirmAttackButtonScript;
    private Vector2 originalHpManaBarSize;
    private Vector2 originalEnemyHpManaBarSize;
    private Vector2 originalBattleScreenHpBarSize;
    private Vector2 originalBattleScreenManaBarSize;
    public int[] damageArray = {-1, -1, -1};
    public int[] enemyDamageArray = {-1, -1, -1};
    public bool isAssisting = false;
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueBoxTitle;
    public TextMeshProUGUI dialogueBoxText;
    public CanvasGroup dialogueBoxCanvasGroup;
    public RectTransform dialogueBoxRectTransform;
    private AttackMoves chosenAttack;
    public bool coroutineRunning = false;
    public GameObject battleScreenLeftMeleeImage;
    public GameObject battleScreenLeftRangedImage;
    public GameObject battleScreenRightMeleeImage;
    public GameObject battleScreenRightRangedImage;
    public GameObject previewLeftMeleeImage;
    public GameObject previewLeftRangedImage;
    public GameObject previewRightMeleeImage;
    public GameObject previewRightRangedImage;
    public AudioSource overworldAttackAudio;
    public AudioSource typingAudio;
    public AudioSource battleScreenTransitionAudio;
    public AudioSource selectBeepAudio;
    public AudioSource attackWooshAudio;
    public AudioSource attackClangAudio;
    public AudioSource assistPreviewAudio;
    public GameObject backgrounds;
    void Awake()
    {
        scm = FindFirstObjectByType<SaveManager>();
        battleController = GameObject.Find("BattleController").GetComponent<BattleController>();
        attackSelectorInitialPos = attackSelector.GetComponent<RectTransform>().anchoredPosition;
        originalHpManaBarSize = previewPlayerHpBar.GetComponent<RectTransform>().sizeDelta;
        originalEnemyHpManaBarSize = previewEnemyHpBar.GetComponent<RectTransform>().sizeDelta;
        originalBattleScreenHpBarSize = battleScreenRightHpBar.GetComponent<RectTransform>().sizeDelta;
        originalBattleScreenManaBarSize = battleScreenRightManaBar.GetComponent<RectTransform>().sizeDelta;
        
        //Set battle background
        foreach (Transform child in backgrounds.transform)
        {
            if (child.gameObject.name == scm.loadedData.currentChapter)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
    void Start()
    {
    }
    void LateUpdate()
    {
        if (active)
        {
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(disablePreview());
            }
            try 
            {
                handleAttackSelection();
            }
            catch
            {
                //Left blank because deselect happens first and then this errors
            }
            
        }

    }
    public void handleAttackSelection()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (attackIndex > 1)
            {
                selectBeepAudio.Play();
                attackSelector.GetComponent<RectTransform>().anchoredPosition = new Vector2(attackSelector.GetComponent<RectTransform>().anchoredPosition.x, attackSelector.GetComponent<RectTransform>().anchoredPosition.y + 37f);
                if (attackIndex == 2) { attackIndex = 0; }
                else if (attackIndex == 3) { attackIndex = 1; }
            }
            updateAttackSelection();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (attackIndex <= 1)
            {
                selectBeepAudio.Play();
                attackSelector.GetComponent<RectTransform>().anchoredPosition = new Vector2(attackSelector.GetComponent<RectTransform>().anchoredPosition.x, attackSelector.GetComponent<RectTransform>().anchoredPosition.y - 37f);
                if (attackIndex == 0) { attackIndex = 2; }
                else if (attackIndex == 1) { attackIndex = 3; }
            }
            updateAttackSelection();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (attackIndex == 1 || attackIndex == 3)
            {
                selectBeepAudio.Play();
                attackSelector.GetComponent<RectTransform>().anchoredPosition = new Vector2(attackSelector.GetComponent<RectTransform>().anchoredPosition.x - 161f, attackSelector.GetComponent<RectTransform>().anchoredPosition.y);
                if (attackIndex == 1) { attackIndex = 0; }
                else if (attackIndex == 3) { attackIndex = 2; }
            }
            updateAttackSelection();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (attackIndex == 0 || attackIndex == 2)
            {
                selectBeepAudio.Play();
                attackSelector.GetComponent<RectTransform>().anchoredPosition = new Vector2(attackSelector.GetComponent<RectTransform>().anchoredPosition.x + 161f, attackSelector.GetComponent<RectTransform>().anchoredPosition.y);
                if (attackIndex == 0) { attackIndex = 1; }
                else if (attackIndex == 2) { attackIndex = 3; }
            }
            updateAttackSelection();
        }
    }
    private void updateAttackSelection()
    {
        if (isAssisting)
        {
            try
            {
                chosenAttack = battleController.characterSelected.GetComponent<PlayerController>().knownAttacks[attackIndex];
                damageArray = calculateSupport(battleController.characterSelected, battleController.assistableCharacterSelected, chosenAttack);
                atkBlock.text = (-1 * damageArray[0]).ToString();
                hitBlock.text = damageArray[1].ToString();
                critBlock.text = damageArray[2].ToString();
                attackDescription.text = chosenAttack.description;
                manaBlock.text = chosenAttack.manaCost.ToString();
                validAttack = true;
            }
            catch
            {
                chosenAttack = null;
                atkBlock.text = "-";
                hitBlock.text = "-";
                critBlock.text = "-";
                attackDescription.text = "-";
                manaBlock.text = "-";
                validAttack = false;
            }
        }
        else
        {
            try 
            {
                chosenAttack = battleController.characterSelected.GetComponent<PlayerController>().knownAttacks[attackIndex];
                if (chosenAttack is Attack)
                {
                    damageArray = calculateDamage(battleController.characterSelected, battleController.enemySelected, chosenAttack);
                    atkBlock.text = damageArray[0].ToString();
                    hitBlock.text = damageArray[1].ToString();
                    critBlock.text = damageArray[2].ToString();
                    attackDescription.text = chosenAttack.description;
                    manaBlock.text = chosenAttack.manaCost.ToString();
                    if (battleController.characterSelected.GetComponent<PlayerController>().currentMana >= chosenAttack.manaCost) { validAttack = true;}
                    else { validAttack = false; }
                    
                }
                else
                {
                    atkBlock.text = "-";
                    hitBlock.text = "-";
                    critBlock.text = "-";
                    attackDescription.text = "-";
                    manaBlock.text = "-";
                    validAttack = false;
                }
            }
            catch
            {
                chosenAttack = null;
                atkBlock.text = "-";
                hitBlock.text = "-";
                critBlock.text = "-";
                attackDescription.text = "-";
                manaBlock.text = "-";
                validAttack = false;
            } 
        }
       
    }
    public int[] calculateDamage(GameObject attacker, GameObject defender, AttackMoves attack)
    {
        float damage = -1;
        float accuracy = -1;
        float critChance= -1;
        int[] returnArray = {-1, -1, -1};

        try
        {
            EnemyController attackerScript = attacker.GetComponent<EnemyController>();
            PlayerController defenderScript = defender.GetComponent<PlayerController>();

            if (attack is Attack attackMove)
            {
                //TODO: Special case scenarios like Shield bash etc
                if (attackMove.name == "Shield Bash")
                {
                    damage = 0;
                    accuracy = 0;
                    critChance = 0;
                }

                //Standard damage calculation
                else
                {
                    //Factor in attacker buffs/debuffs
                    float attackerATK = attackerScript.attack;
                    float attackerINT = attackerScript.intelligence;
                    float attackerSKL = attackerScript.skill;

                    foreach (Debuff d in attackerScript.debuffs)
                    {
                        if (d.name == "Dazed")
                        {
                            attackerATK = attackerATK * 0.67f;
                            attackerINT = attackerINT * 0.67f;
                        }
                        if (d.name == "Confused")
                        {
                            attackerSKL = attackerSKL * 0.67f;
                        }
                    }
                    foreach (Buff b in attackerScript.buffs)
                    {
                        if (b.name == "Invigorated")
                        {
                            attackerATK = attackerATK * 1.5f;
                            attackerINT = attackerINT * 1.5f;
                        }
                        if (b.name == "Flowing")
                        {
                            attackerSKL = attackerSKL * 1.5f;
                        }
                    }
                    
                    //Factor in defender buffs/debuffs
                    float defenderDEF = defenderScript.defense;
                    float defenderRES = defenderScript.resistance;
                    float defenderSPD = defenderScript.speed;

                    foreach (Debuff d in defenderScript.debuffs)
                    {
                        if (d.name == "Vulnerable")
                        {
                            defenderDEF = defenderDEF * 0.67f;
                            defenderRES = defenderRES * 0.67f;
                        }
                        if (d.name == "Slowed")
                        {
                            defenderSPD = defenderSPD * 0.67f;
                        }
                    }
                    foreach (Buff b in defenderScript.buffs)
                    {
                        if (b.name == "Invigorated")
                        {
                            defenderDEF = defenderDEF * 1.5f;
                            defenderRES = defenderRES * 1.5f;
                        }
                        if (b.name == "Flowing")
                        {
                            attackerSKL = attackerSKL * 1.5f;
                        }
                    }

                    if (attackMove.damageType == "physical") 
                    {
                        damage = attackerATK * attackMove.attackMult * attackerATK * attackMove.attackMult / ((attackerATK * attackMove.attackMult) + defenderDEF);
                    }
                    else if (attackMove.damageType == "magical")
                    {
                        damage = attackerINT * attackMove.intMult * attackerINT * attackMove.intMult / ((attackerINT * attackMove.intMult) + defenderRES);
                    }   

                    accuracy = attackMove.baseAccuracy * (attackerSKL / defenderSPD);
                    critChance = Helpers.CalculateCrit(attackerSKL, attackMove.baseCrit);
                }

                returnArray[0] = (int)Mathf.Round(damage);
                returnArray[1] = (int)Mathf.Round(accuracy);
                returnArray[2] = (int)Mathf.Round(critChance);
            }

        }

        catch (System.Exception e)
        {
            PlayerController attackerScript = attacker.GetComponent<PlayerController>();
            EnemyController defenderScript = defender.GetComponent<EnemyController>(); 

            if (attack is Attack attackMove)
            {
                //TODO: Special case scenarios like Shield bash etc
                if (attackMove.name == "Shield Bash")
                {
                    damage = 0;
                    accuracy = 0;
                    critChance = 0;
                }

                //Standard damage calculation
                else
                {
                    //Factor in attacker buffs/debuffs
                    float attackerATK = attackerScript.attack;
                    float attackerINT = attackerScript.intelligence;
                    float attackerSKL = attackerScript.skill;

                    foreach (Debuff d in attackerScript.debuffs)
                    {
                        if (d.name == "Dazed")
                        {
                            attackerATK = attackerATK * 0.67f;
                            attackerINT = attackerINT * 0.67f;
                        }
                        if (d.name == "Confused")
                        {
                            attackerSKL = attackerSKL * 0.67f;
                        }
                    }
                    foreach (Buff b in attackerScript.buffs)
                    {
                        if (b.name == "Invigorated")
                        {
                            attackerATK = attackerATK * 1.5f;
                            attackerINT = attackerINT * 1.5f;
                        }
                        if (b.name == "Flowing")
                        {
                            attackerSKL = attackerSKL * 1.5f;
                        }
                    }
                    
                    //Factor in defender buffs/debuffs
                    float defenderDEF = defenderScript.defense;
                    float defenderRES = defenderScript.resistance;
                    float defenderSPD = defenderScript.speed;

                    foreach (Debuff d in defenderScript.debuffs)
                    {
                        if (d.name == "Vulnerable")
                        {
                            defenderDEF = defenderDEF * 0.67f;
                            defenderRES = defenderRES * 0.67f;
                        }
                        if (d.name == "Slowed")
                        {
                            defenderSPD = defenderSPD * 0.67f;
                        }
                    }
                    foreach (Buff b in defenderScript.buffs)
                    {
                        if (b.name == "Invigorated")
                        {
                            defenderDEF = defenderDEF * 1.5f;
                            defenderRES = defenderRES * 1.5f;
                        }
                        if (b.name == "Flowing")
                        {
                            attackerSKL = attackerSKL * 1.5f;
                        }
                    }

                    if (attackMove.damageType == "physical") 
                    {
                        damage = attackerATK * attackMove.attackMult * attackerATK * attackMove.attackMult / ((attackerATK * attackMove.attackMult) + defenderDEF);
                    }
                    else if (attackMove.damageType == "magical")
                    {
                        damage = attackerINT * attackMove.intMult * attackerINT * attackMove.intMult / ((attackerINT * attackMove.intMult) + defenderRES);
                    }   

                    accuracy = attackMove.baseAccuracy * (attackerSKL / defenderSPD);
                    critChance = Helpers.CalculateCrit(attackerSKL, attackMove.baseCrit);
                }

                returnArray[0] = (int)Mathf.Round(damage);
                returnArray[1] = (int)Mathf.Round(accuracy);
                returnArray[2] = (int)Mathf.Round(critChance);
            }


        }

        return returnArray;
    }
    public int[] calculateSupport(GameObject attacker, GameObject defender, AttackMoves attack)
    {
        SupportMove supportMove = attack as SupportMove;
        float damage = -1;
        float accuracy = -1;
        float critChance= -1;
        int[] returnArray = {-1, -1, -1};

        try
        {
            PlayerController attackerScript = attacker.GetComponent<PlayerController>();
            PlayerController defenderScript = defender.GetComponent<PlayerController>();

            //TODO: Special cases
            if (supportMove.name == "Mega Heal")
            {
                damage = 0;
                accuracy = 0;
                critChance = 0;
            }

            //Standard support calculation
            else
            {
                //Restoration amount = baseRestoration + 1 for every 3 points of intelligence
                damage = -1 * (supportMove.restorationAmount + attackerScript.intelligence / 3); //negated because it's a heal
                accuracy = 100;
                critChance = 0;
            }

            returnArray[0] = (int)Mathf.Round(damage);
            returnArray[1] = (int)Mathf.Round(accuracy);
            returnArray[2] = (int)Mathf.Round(critChance);
        
        }

        catch
        {
            EnemyController attackerScript = attacker.GetComponent<EnemyController>();
            EnemyController defenderScript = defender.GetComponent<EnemyController>(); 

            //TODO: Special cases
            if (attack.name == "Mega Heal")
            {
                damage = 0;
                accuracy = 0;
                critChance = 0;
            }

            //Standard support calculation
            else
            {
                //Restoration amount = baseRestoration + 1 for every 3 points of intelligence
                damage = supportMove.restorationAmount + attackerScript.intelligence / 3;
                accuracy = 100;
                critChance = 0;
            }

            returnArray[0] = (int)Mathf.Round(damage);
            returnArray[1] = (int)Mathf.Round(accuracy);
            returnArray[2] = (int)Mathf.Round(critChance);
        

        }

        return returnArray;
    } 
    public IEnumerator enablePreview(bool isAssisting)
    {
        battleController.characterSelected.GetComponent<PlayerController>().movementEnabled = false;
        this.isAssisting = isAssisting;

        //Update titles
        characterTitle.text = battleController.characterSelected.GetComponent<PlayerController>().title;

        //Set rightside info
        if (!isAssisting) 
        { 
            enemyTitle.text = battleController.enemySelected.GetComponent<EnemyController>().title;
            previewEnemyHp.text = battleController.enemySelected.GetComponent<EnemyController>().currentHp.ToString();
            previewEnemyMaxHp.text = battleController.enemySelected.GetComponent<EnemyController>().maxHp.ToString();
            previewEnemyMana.text = battleController.enemySelected.GetComponent<EnemyController>().currentMana.ToString();
            previewEnemyMaxMana.text = battleController.enemySelected.GetComponent<EnemyController>().maxMana.ToString();
            previewEnemyHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.enemySelected.GetComponent<EnemyController>().currentHp / battleController.enemySelected.GetComponent<EnemyController>().maxHp, 1f);
            previewEnemyManaBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.enemySelected.GetComponent<EnemyController>().currentMana / battleController.enemySelected.GetComponent<EnemyController>().maxMana, 1f);
            if (battleController.enemySelected.GetComponent<EnemyController>().ranged)
            {
                previewRightMeleeImage.SetActive(false); 
                previewRightRangedImage.SetActive(true); 
                if (battleController.characterSelected.GetComponent<PlayerController>().ranged)
                {
                    enemyDamageArray = calculateDamage(battleController.enemySelected, battleController.characterSelected, battleController.enemySelected.GetComponent<EnemyController>().knownAttacks[0]);
                    rightsideAtkBlock.text = enemyDamageArray[0].ToString();
                    rightsideHitBlock.text = enemyDamageArray[1].ToString();
                    rightsideCritBlock.text = enemyDamageArray[2].ToString();
                    rightsideAttackDescription.text = battleController.enemySelected.GetComponent<EnemyController>().knownAttacks[0].description;
                }
                else
                {
                    rightsideAtkBlock.text = "-";
                    rightsideHitBlock.text = "-";
                    rightsideCritBlock.text = "-";
                    rightsideAttackDescription.text = battleController.enemySelected.GetComponent<EnemyController>().knownAttacks[0].description;
                }
            }
            else 
            { 
                previewRightMeleeImage.SetActive(true);
                previewRightRangedImage.SetActive(false);
                if (!battleController.characterSelected.GetComponent<PlayerController>().ranged)
                {
                    enemyDamageArray = calculateDamage(battleController.enemySelected, battleController.characterSelected, battleController.enemySelected.GetComponent<EnemyController>().knownAttacks[0]);
                    rightsideAtkBlock.text = enemyDamageArray[0].ToString();
                    rightsideHitBlock.text = enemyDamageArray[1].ToString();
                    rightsideCritBlock.text = enemyDamageArray[2].ToString();
                    rightsideAttackDescription.text = battleController.enemySelected.GetComponent<EnemyController>().knownAttacks[0].description;
                }
                else
                {
                    rightsideAtkBlock.text = "-";
                    rightsideHitBlock.text = "-";
                    rightsideCritBlock.text = "-";
                    rightsideAttackDescription.text = battleController.enemySelected.GetComponent<EnemyController>().knownAttacks[0].description;
                }
            }

        }
        else 
        { 
            enemyTitle.text = battleController.assistableCharacterSelected.GetComponent<PlayerController>().title;
            previewEnemyHp.text = battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentHp.ToString();
            previewEnemyMaxHp.text = battleController.assistableCharacterSelected.GetComponent<PlayerController>().maxHp.ToString();
            previewEnemyMana.text = battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentMana.ToString();
            previewEnemyMaxMana.text = battleController.assistableCharacterSelected.GetComponent<PlayerController>().maxMana.ToString();
            previewEnemyHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentHp / battleController.assistableCharacterSelected.GetComponent<PlayerController>().maxHp, 1f);
            previewEnemyManaBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentMana / battleController.assistableCharacterSelected.GetComponent<PlayerController>().maxMana, 1f);
            if (battleController.assistableCharacterSelected.GetComponent<PlayerController>().ranged) { previewRightMeleeImage.SetActive(false); previewRightRangedImage.SetActive(true); }
            else { previewRightMeleeImage.SetActive(true); previewRightRangedImage.SetActive(false); }
        }

        //Set leftside info
        previewPlayerHp.text = battleController.characterSelected.GetComponent<PlayerController>().currentHp.ToString();
        previewPlayerMaxHp.text = battleController.characterSelected.GetComponent<PlayerController>().maxHp.ToString();
        previewPlayerMana.text = battleController.characterSelected.GetComponent<PlayerController>().currentMana.ToString();
        previewPlayerMaxMana.text = battleController.characterSelected.GetComponent<PlayerController>().maxMana.ToString();
        previewPlayerHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.characterSelected.GetComponent<PlayerController>().currentHp / battleController.characterSelected.GetComponent<PlayerController>().maxHp, 1f);
        previewPlayerManaBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.characterSelected.GetComponent<PlayerController>().currentMana / battleController.characterSelected.GetComponent<PlayerController>().maxMana, 1f);
        if (battleController.characterSelected.GetComponent<PlayerController>().ranged) { previewLeftMeleeImage.SetActive(false); previewLeftRangedImage.SetActive(true); }
        else { previewLeftMeleeImage.SetActive(true); previewLeftRangedImage.SetActive(false); }
        
        //Update leftside moveset
        int index = 0;
        foreach (Transform child in attacks.transform)
        {
            foreach (Transform child2 in child.transform)
            {
                try
                {
                    child2.GetComponent<TextMeshProUGUI>().text = battleController.characterSelected.GetComponent<PlayerController>().knownAttacks[index].name;
                    
                    if (isAssisting)
                    {
                        if (battleController.characterSelected.GetComponent<PlayerController>().knownAttacks[index] is Attack)
                        {
                            child2.GetComponent<TextMeshProUGUI>().color = new Color(.5f, .5f, .5f, .5f);
                        }
                        else
                        {
                            child2.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
                        }
                    }
                    else
                    {
                        if (battleController.characterSelected.GetComponent<PlayerController>().knownAttacks[index] is Attack)
                        {
                            child2.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
                        }
                        else
                        {
                            child2.GetComponent<TextMeshProUGUI>().color = new Color(.5f, .5f, .5f, .5f);
                        }
                    }

                }
                catch
                {
                    child2.GetComponent<TextMeshProUGUI>().text = "-";
                }
            }
            index++;
        }

        //Update rightside moveset
        index = 0;
        foreach (Transform child in rightsideAttacks.transform)
        {
            foreach (Transform child2 in child.transform)
            {
                try
                {
                    if (!isAssisting)
                    {
                        child2.GetComponent<TextMeshProUGUI>().text = battleController.enemySelected.GetComponent<EnemyController>().knownAttacks[index].name;
                        if (battleController.enemySelected.GetComponent<EnemyController>().knownAttacks[index] is Attack)
                        {
                            child2.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
                        }
                        else if (battleController.enemySelected.GetComponent<EnemyController>().knownAttacks[index] is SupportMove)
                        {
                            child2.GetComponent<TextMeshProUGUI>().color = new Color(.5f, .5f, .5f, .5f);  
                        }
                    }
                    else
                    {
                        child2.GetComponent<TextMeshProUGUI>().text = battleController.assistableCharacterSelected.GetComponent<PlayerController>().knownAttacks[index].name;
                        child2.GetComponent<TextMeshProUGUI>().color = new Color(.5f, .5f, .5f, .5f);              
                    }

                }
                catch
                {
                    child2.GetComponent<TextMeshProUGUI>().text = "-";
                }
            }
            index++;
        }

        updateAttackSelection();

        //Move UI to visible area
        if (isAssisting) { assistPreviewAudio.Play(); }
        else { attackClangAudio.Play(); }
        float duration = 0.05f;
        Vector2 attackerStartPos = attackerPreviewPanel.transform.localPosition;
        Vector2 defenderStartPos = defenderPreviewPanel.transform.localPosition;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            attackerPreviewPanel.transform.localPosition = Vector2.Lerp(attackerStartPos, new Vector2(-25, 22), t);
            defenderPreviewPanel.transform.localPosition = Vector2.Lerp(defenderStartPos, new Vector2(292, 19), t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        attackerPreviewPanel.transform.localPosition = new Vector2(-25, 22);
        defenderPreviewPanel.transform.localPosition = new Vector2(292, 19);

        //SetPortraits()

        //Enable buttons
        confirmButton.GetComponent<CanvasGroup>().alpha = 1;
        confirmButton.GetComponent<CanvasGroup>().interactable = true;
        confirmButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
        
        if (isAssisting) { assistIcon.SetActive(true); }
        else { attackIcon.SetActive(true); }
        active = true;

    }
    public IEnumerator disablePreview()
    {
        isAssisting = false;
        attackWooshAudio.Play();
        
        //Destroy attack preview prefabs
        //foreach (Transform child in attackPreviewSprites.transform)
        //{
        //    Destroy(child.gameObject);
        //}

        //Move off screen
        float duration = 0.05f;
        Vector2 attackerStartPos = attackerPreviewPanel.transform.localPosition;
        Vector2 defenderStartPos = defenderPreviewPanel.transform.localPosition;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            attackerPreviewPanel.transform.localPosition = Vector2.Lerp(attackerStartPos, new Vector2(-465, 22), t);
            defenderPreviewPanel.transform.localPosition = Vector2.Lerp(defenderStartPos, new Vector2(706, 19), t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        attackerPreviewPanel.transform.localPosition = new Vector2(-465, 22);
        defenderPreviewPanel.transform.localPosition = new Vector2(706, 19);

        //Reset attackSelectorPos
        attackSelector.GetComponent<RectTransform>().anchoredPosition = attackSelectorInitialPos;
        attackIndex = 0;

        //Reset moveset
        int index = 0;
        foreach (Transform child in attacks.transform)
        {
            foreach (Transform child2 in child.transform)
            {
                child2.GetComponent<TextMeshProUGUI>().text = "---";

            }
            index++;
        }

        // Reset damageBlock
        atkBlock.text = "-";
        hitBlock.text = "-";
        critBlock.text = "-";

        //Reset health and mana bars
        previewPlayerHpBar.GetComponent<RectTransform>().sizeDelta = originalHpManaBarSize;
        previewPlayerManaBar.GetComponent<RectTransform>().sizeDelta = originalHpManaBarSize;
        previewEnemyHpBar.GetComponent<RectTransform>().sizeDelta = originalEnemyHpManaBarSize;
        previewEnemyManaBar.GetComponent<RectTransform>().sizeDelta = originalEnemyHpManaBarSize;

        //Disable vs and confirm button
        confirmButton.GetComponent<CanvasGroup>().alpha = 0;
        confirmButton.GetComponent<CanvasGroup>().interactable = false;
        confirmButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
        attackIcon.SetActive(false);
        assistIcon.SetActive(false);

        active = false;

        try {
            battleController.characterSelected.GetComponent<PlayerController>().movementEnabled = true;
        }
        catch
        {
            
        }
        yield return null;

    }
    public void startSequenceHelper()
    {
        if (validAttack)
        {
            if (isAssisting)
            {
                StartCoroutine(startSupportSequence());
            }
            else
            {
                StartCoroutine(startAttackSequence());
            }
        }
    }
    public IEnumerator startAttackSequence()
    {   
        coroutineRunning = true;
        active = false;

        //Don't skip animations
        if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
        {
            PopulateBattleScreenInfo();
            
            //Enable the Battle screen and scale it from 0 to 1
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one;
            float elapsed = 0f;
            float duration = .2f;
            attackPanel.GetComponent<RectTransform>().localScale = startScale;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                attackPanel.GetComponent<RectTransform>().localScale = Vector3.Lerp(startScale, endScale, t);

                yield return null;
            }
            attackPanel.GetComponent<RectTransform>().localScale = endScale; // snap to final value
            
                        
            if (isAssisting)
            {
                
            }
            else
            {
                yield return StartCoroutine(AnimateManaDamage(chosenAttack.manaCost, battleScreenLeftManaBar, battleController.characterSelected, battleScreenLeftMana));
                
                if (RollAttack())
                {
                    if (RollCrit())
                    {
                        //When combat animation is on then animate health bar
                        yield return StartCoroutine(AnimateHealthDamage(damageArray[0] * 2, battleScreenRightHpBar, battleController.enemySelected, battleScreenRightHp));
                        battleController.enemySelected.GetComponent<EnemyController>().currentHp = battleController.enemySelected.GetComponent<EnemyController>().currentHp - damageArray[0] * 2;
                    }
                    else
                    {
                        yield return StartCoroutine(AnimateHealthDamage(damageArray[0], battleScreenRightHpBar, battleController.enemySelected, battleScreenRightHp));
                        battleController.enemySelected.GetComponent<EnemyController>().currentHp = battleController.enemySelected.GetComponent<EnemyController>().currentHp - damageArray[0];
                    }

                    //Roll secondary effects
                    try
                    {
                        Attack attack = chosenAttack as Attack;
                        foreach (Debuff debuff in attack.debuffs) 
                        {
                            if (RollDebuff(debuff.chanceToApply)) 
                            {
                                //Check if debuff is already on the enemy
                                if (battleController.enemySelected.GetComponent<EnemyController>().HasDebuff(debuff)) {
                                    battleController.enemySelected.GetComponent<EnemyController>().AddTurnsToDebuff(debuff);
                                }
                                else {
                                    battleController.enemySelected.GetComponent<EnemyController>().debuffs.Add(debuff);
                                    battleController.enemySelected.GetComponent<EnemyController>().ApplyDebuffEffects();
                                }
                            
                            }
                        }
                    }
                    catch {
                        Debug.Log("Error trying to roll secondary effects");
                    }

                }
            }
        
            //Play enemy death dialogue if necessary
            if (battleController.enemySelected.GetComponent<EnemyController>().currentHp <= 0)
            {   
                //TODO: Remove sprite on battle screen
                yield return StartCoroutine(DeathSequence(battleController.enemySelected));
            }

            //Enemy attacks
            else
            {
                //Enemy can attack back
                if ((battleController.enemySelected.GetComponent<EnemyController>().ranged && battleController.characterSelected.GetComponent<PlayerController>().ranged) || (!battleController.enemySelected.GetComponent<EnemyController>().ranged && !battleController.characterSelected.GetComponent<PlayerController>().ranged))
                {
                    //TODO: attack animation
                    yield return new WaitForSeconds(3f);
                    
                    if (RollEnemyAttack())
                    {
                        if (RollEnemyCrit())
                        {
                            yield return StartCoroutine(AnimateHealthDamage(enemyDamageArray[0] * 2, battleScreenLeftHpBar, battleController.characterSelected, battleScreenLeftHp));
                            battleController.characterSelected.GetComponent<PlayerController>().currentHp = battleController.characterSelected.GetComponent<PlayerController>().currentHp - enemyDamageArray[0] * 2; 
                        }
                        else
                        {
                            yield return StartCoroutine(AnimateHealthDamage(enemyDamageArray[0], battleScreenLeftHpBar, battleController.characterSelected, battleScreenLeftHp));
                            battleController.characterSelected.GetComponent<PlayerController>().currentHp = battleController.characterSelected.GetComponent<PlayerController>().currentHp - enemyDamageArray[0]; 
                        }
                    }
                }
            }
        
            //Play death dialogue if necessary
            if (battleController.characterSelected.GetComponent<PlayerController>().currentHp <= 0)
            {   
                yield return StartCoroutine(DeathSequence(battleController.characterSelected));
            }

            //Enable the Battle screen and scale it from 1 to 0
            startScale = Vector3.one;
            endScale = Vector3.zero;
            elapsed = 0f;
            duration = .2f;
            attackPanel.GetComponent<RectTransform>().localScale = startScale;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                attackPanel.GetComponent<RectTransform>().localScale = Vector3.Lerp(startScale, endScale, t);

                yield return null;
            }
            attackPanel.GetComponent<RectTransform>().localScale = endScale; // snap to final value

            ResetBattleScreenInfo();
        }

        //Skip animations
        else
        {
            
            if (isAssisting)
            {
                //TODO: Assist logic
                yield return new WaitForSeconds(3f);
            }
            else
            {
                overworldAttackAudio.Play();
                if (RollAttack())
                {
                    if (RollCrit())
                    {
                        battleController.enemySelected.GetComponent<EnemyController>().currentHp = battleController.enemySelected.GetComponent<EnemyController>().currentHp - damageArray[0] * 2;
                    }
                    else
                    {
                        battleController.enemySelected.GetComponent<EnemyController>().currentHp = battleController.enemySelected.GetComponent<EnemyController>().currentHp - damageArray[0];
                    }
                }
            }
        
            //Play enemy death dialogue if necessary
            if (battleController.enemySelected.GetComponent<EnemyController>().currentHp <= 0)
            {   
                //TODO: Remove sprite on battle screen
                yield return StartCoroutine(DeathSequence(battleController.enemySelected));
            }

            //Enemy attacks
            else
            {
                //Enemy can attack back
                if ((battleController.enemySelected.GetComponent<EnemyController>().ranged && battleController.characterSelected.GetComponent<PlayerController>().ranged) || (!battleController.enemySelected.GetComponent<EnemyController>().ranged && !battleController.characterSelected.GetComponent<PlayerController>().ranged))
                {
                    if (RollEnemyAttack())
                    {
                        if (RollEnemyCrit())
                        {
                            battleController.characterSelected.GetComponent<PlayerController>().currentHp = battleController.characterSelected.GetComponent<PlayerController>().currentHp - enemyDamageArray[0] * 2; 
                        }
                        else
                        {
                            battleController.characterSelected.GetComponent<PlayerController>().currentHp = battleController.characterSelected.GetComponent<PlayerController>().currentHp - enemyDamageArray[0]; 
                        }
                    }
                }
            }
        
        }

        //Update mana
        battleController.characterSelected.GetComponent<PlayerController>().currentMana = battleController.characterSelected.GetComponent<PlayerController>().currentMana - chosenAttack.manaCost;

        yield return StartCoroutine(disablePreview());

        chosenAttack = null;

        if (battleController.characterSelected != null)
        {
            battleController.characterSelected.GetComponent<PlayerController>().endTurn();
        }

        coroutineRunning = false;
    }
    public IEnumerator startSupportSequence()
    {
        coroutineRunning = true;
        active = false;
        SupportMove supportMove = chosenAttack as SupportMove;

        //Don't skip animations
        if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
        {
            PopulateBattleScreenInfo();
            
            //Enable the Battle screen and scale it from 0 to 1
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one;
            float elapsed = 0f;
            float duration = .2f;
            attackPanel.GetComponent<RectTransform>().localScale = startScale;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                attackPanel.GetComponent<RectTransform>().localScale = Vector3.Lerp(startScale, endScale, t);

                yield return null;
            }
            attackPanel.GetComponent<RectTransform>().localScale = endScale; // snap to final value

            yield return new WaitForSeconds(1.5f);

            //Restores hp
            if (supportMove.restoresHpOrMana == "hp")
            {
                yield return StartCoroutine(AnimateHealthDamage(damageArray[0], battleScreenRightHpBar, battleController.assistableCharacterSelected, battleScreenRightHp));
                battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentHp = battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentHp - damageArray[0];
                if (battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentHp > battleController.assistableCharacterSelected.GetComponent<PlayerController>().maxHp)
                {
                    battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentHp = battleController.assistableCharacterSelected.GetComponent<PlayerController>().maxHp;
                }
            }
            else if (supportMove.restoresHpOrMana == "mana")
            {
                yield return StartCoroutine(AnimateManaDamage(damageArray[0], battleScreenRightManaBar, battleController.assistableCharacterSelected, battleScreenRightMana));
                battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentMana = battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentMana - damageArray[0];
                if (battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentMana > battleController.assistableCharacterSelected.GetComponent<PlayerController>().maxMana)
                {
                    battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentMana = battleController.assistableCharacterSelected.GetComponent<PlayerController>().maxMana;
                }    
            }
            else if (supportMove.restoresHpOrMana == "both")
            {
                
            }

            yield return new WaitForSeconds(1.5f);

            //Enable the Battle screen and scale it from 1 to 0
            startScale = Vector3.one;
            endScale = Vector3.zero;
            elapsed = 0f;
            duration = .2f;
            attackPanel.GetComponent<RectTransform>().localScale = startScale;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                attackPanel.GetComponent<RectTransform>().localScale = Vector3.Lerp(startScale, endScale, t);

                yield return null;
            }
            attackPanel.GetComponent<RectTransform>().localScale = endScale; // snap to final value

            ResetBattleScreenInfo();
        }

        //Skip animations
        else
        {
            battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentHp = battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentHp - damageArray[0];
        }

        //Update mana
        battleController.characterSelected.GetComponent<PlayerController>().currentMana = battleController.characterSelected.GetComponent<PlayerController>().currentMana - chosenAttack.manaCost;

        yield return StartCoroutine(disablePreview());
        chosenAttack = null;
        if (battleController.characterSelected != null)
        {
            battleController.characterSelected.GetComponent<PlayerController>().endTurn();
        }
        coroutineRunning = false;
    }
    public IEnumerator startEnemyAttackSequence(GameObject attacker, GameObject defender, AttackMoves attackSelected)
    {
        PlayerController defenderScript = defender.GetComponent<PlayerController>();
        EnemyController attackerScript = attacker.GetComponent<EnemyController>();
        coroutineRunning = true;
        enemyDamageArray = calculateDamage(attacker, defender, attackSelected);
        damageArray = calculateDamage(defender, attacker, defenderScript.knownAttacks[0]);
        
        
        if (PlayerPrefs.GetInt("combatAnim") == 0)
        {
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one;
            float elapsed = 0f;
            float duration = .2f;

            //Populate info
            battleScreenTransitionAudio.Play();
            battleScreenPlayerName.text = defenderScript.title;
            battleScreenLeftHp.text = defenderScript.currentHp.ToString();
            battleScreenLeftHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)defenderScript.currentHp / defenderScript.maxHp, 1f);
            battleScreenLeftAttack.text = defenderScript.knownAttacks[0].name;
            battleScreenEnemyName.text = attackerScript.title; 
            battleScreenRightHp.text = attackerScript.currentHp.ToString();
            battleScreenRightHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)attackerScript.currentHp / attackerScript.maxHp, 1f);
            battleScreenRightAttack.text = attackSelected.name;
            battleScreenEnemyATK.text = enemyDamageArray[0].ToString();
            battleScreenEnemyHIT.text = enemyDamageArray[1].ToString();
            battleScreenEnemyCRIT.text = enemyDamageArray[2].ToString();

            //battleScreenLeftMana.text = battleController.characterSelected.GetComponent<PlayerController>().currentMana.ToString();
            //battleScreenLeftManaBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.characterSelected.GetComponent<PlayerController>().currentMana / battleController.characterSelected.GetComponent<PlayerController>().maxMana, 1f);
            //battleScreenRightMana.text = attackerScript.currentMana.ToString();
            //battleScreenRightManaBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)attackerScript.currentMana / attackerScript.maxMana, 1f);  
            
            if (attackerScript.ranged) { battleScreenRightRangedImage.SetActive(true); battleScreenRightMeleeImage.SetActive(false);}
            else { battleScreenRightRangedImage.SetActive(false); battleScreenRightMeleeImage.SetActive(true); }
            
            if (defenderScript.ranged) { battleScreenLeftRangedImage.SetActive(true); battleScreenLeftMeleeImage.SetActive(false);}
            else { battleScreenLeftRangedImage.SetActive(false); battleScreenLeftMeleeImage.SetActive(true); }

            //Enemy is ranged and character is not, or vice versa
            if ((attackerScript.ranged && !defenderScript.ranged) || (!attackerScript.ranged && defenderScript.ranged) )
            {
                battleScreenLeftAttack.text = "-";
                battleScreenLeftATK.text =  "-";
                battleScreenLeftHIT.text =  "-";
                battleScreenLeftCRIT.text = "-";
            }
            else
            {
                battleScreenLeftATK.text = damageArray[0].ToString();
                battleScreenLeftHIT.text = damageArray[1].ToString();
                battleScreenLeftCRIT.text = damageArray[2].ToString();  
                battleScreenLeftAttack.text = defenderScript.knownAttacks[0].name;
            }
     
            //Enable the Battle screen and scale it from 0 to 1
            attackPanel.GetComponent<RectTransform>().localScale = startScale;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                attackPanel.GetComponent<RectTransform>().localScale = Vector3.Lerp(startScale, endScale, t);

                yield return null;
            }
            attackPanel.GetComponent<RectTransform>().localScale = endScale; // snap to final value

            //Attack roll sequence
            yield return new WaitForSeconds(1.5f); 

            yield return StartCoroutine(AnimateManaDamage(attackSelected.manaCost, battleScreenRightManaBar, attacker, battleScreenRightMana));
            if (RollEnemyAttack())
            {
                if (RollEnemyCrit())
                {
                    yield return StartCoroutine(AnimateHealthDamage(enemyDamageArray[0] * 2, battleScreenLeftHpBar, defender, battleScreenLeftHp));
                    defenderScript.currentHp = defenderScript.currentHp - enemyDamageArray[0] * 2; 
                }
                else
                {
                    yield return StartCoroutine(AnimateHealthDamage(enemyDamageArray[0], battleScreenLeftHpBar, defender, battleScreenLeftHp));
                    defenderScript.currentHp = defenderScript.currentHp - enemyDamageArray[0]; 
                }
            }
            
            yield return new WaitForSeconds(0.5f);

            //Play character death dialogue if necessary
            if (defenderScript.currentHp <= 0)
            {   
                yield return StartCoroutine(DeathSequence(defender));
                //TODO: Remove sprite on battle screen
                //yield return StartCoroutine(battleController.characterSelected.GetComponent<PlayerController>().Die());
            }

            //Character attacks enemy
            else
            {
                //should only attack back if able to
                if ((defenderScript.ranged && attackerScript.ranged) || (!defenderScript.ranged && !attackerScript.ranged))
                {

                    if (RollAttack())
                    {
                        if (RollCrit())
                        {
                            yield return StartCoroutine(AnimateHealthDamage(damageArray[0] * 2, battleScreenRightHpBar, attacker, battleScreenRightHp));
                            attackerScript.currentHp = attackerScript.currentHp - damageArray[0] * 2;

                        }
                        else
                        {
                            yield return StartCoroutine(AnimateHealthDamage(damageArray[0], battleScreenRightHpBar, attacker, battleScreenRightHp));
                            attackerScript.currentHp = attackerScript.currentHp - damageArray[0];
                        }
                    }

                    //Play enemy death dialogue if necessary
                    if (attackerScript.currentHp <= 0)
                    {   
                        //TODO: Remove sprite on battle screen
                        if (attackerScript.deathDialogue != "")
                        {
                            yield return StartCoroutine(DeathSequence(attacker));
                        }
                    }
                }
            
        
            }
        
            //Enable the Battle screen and scale it from 1 to 0
            attackPanel.GetComponent<RectTransform>().localScale = startScale;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                attackPanel.GetComponent<RectTransform>().localScale = Vector3.Lerp(endScale, startScale, t);

                yield return null;
            }
            attackPanel.GetComponent<RectTransform>().localScale = startScale; // snap to final value

            //Play enemy death dialogue if necessary
            if (attackerScript.currentHp <= 0)
            {   
                //TODO: Remove sprite on battle screen
                if (attackerScript.deathDialogue != "")
                {
                    yield return StartCoroutine(DeathSequence(attacker));
                }
            }

            //Reset Hp bar sizes
            battleScreenLeftHpBar.GetComponent<RectTransform>().sizeDelta = originalBattleScreenHpBarSize;
            battleScreenRightHpBar.GetComponent<RectTransform>().sizeDelta = originalBattleScreenHpBarSize;

            //Reset battlescreen info
            battleScreenPlayerName.text = "-";
            battleScreenLeftHp.text = "-";
            battleScreenLeftAttack.text = "-";
            battleScreenEnemyName.text = "-";
            battleScreenRightHp.text = "-";
            battleScreenRightAttack.text = "-";
            battleScreenEnemyATK.text = "-";
            battleScreenEnemyHIT.text = "-";
            battleScreenEnemyCRIT.text = "-";

        }
        else
        {
            //Attack roll sequence
            yield return new WaitForSeconds(1.5f); 
            if (RollEnemyAttack())
            {
                if (RollEnemyCrit())
                {
                    defenderScript.currentHp = defenderScript.currentHp - enemyDamageArray[0] * 2; 
                }
                else
                {
                    defenderScript.currentHp = defenderScript.currentHp - enemyDamageArray[0]; 
                }
            }
            
            yield return new WaitForSeconds(0.5f);

            //Play character death dialogue if necessary
            if (defenderScript.currentHp <= 0)
            {   
                yield return StartCoroutine(DeathSequence(defender));
                //TODO: Remove sprite on battle screen
                //yield return StartCoroutine(battleController.characterSelected.GetComponent<PlayerController>().Die());
            }

            //Character attacks enemy
            else
            {
                //should only attack back if able to
                if ((defenderScript.ranged && attackerScript.ranged) || (!defenderScript.ranged && !attackerScript.ranged))
                {

                    if (RollAttack())
                    {
                        if (RollCrit())
                        {
                            yield return StartCoroutine(AnimateHealthDamage(damageArray[0] * 2, battleScreenRightHpBar, attacker, battleScreenRightHp));
                            attackerScript.currentHp = attackerScript.currentHp - damageArray[0] * 2;

                        }
                        else
                        {
                            yield return StartCoroutine(AnimateHealthDamage(damageArray[0], battleScreenRightHpBar, attacker, battleScreenRightHp));
                            attackerScript.currentHp = attackerScript.currentHp - damageArray[0];
                        }
                    }

                    //Play enemy death dialogue if necessary
                    if (attackerScript.currentHp <= 0)
                    {   
                        //TODO: Remove sprite on battle screen
                        if (attackerScript.deathDialogue != "")
                        {
                            yield return StartCoroutine(DeathSequence(attacker));
                        }
                    }
                }
            
        
            }
        
        }
        
        coroutineRunning = false;
    }
    public IEnumerator startNeutralAttackSequence(GameObject attacker, GameObject defender, AttackMoves attackSelected)
    {
        EnemyController defenderScript = defender.GetComponent<EnemyController>();
        PlayerController attackerScript = attacker.GetComponent<PlayerController>();
        coroutineRunning = true;
        enemyDamageArray = calculateDamage(attacker, defender, attackSelected);
        damageArray = calculateDamage(defender, attacker, attackerScript.knownAttacks[0]);
        
        
        if (PlayerPrefs.GetInt("combatAnim") == 0)
        {
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one;
            float elapsed = 0f;
            float duration = .2f;

            //Populate leftside info
            battleScreenTransitionAudio.Play();
            battleScreenPlayerName.text = attackerScript.title;
            battleScreenLeftHp.text = attackerScript.currentHp.ToString();
            battleScreenLeftHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)attackerScript.currentHp / attackerScript.maxHp, 1f);
            battleScreenLeftAttack.text = attackSelected.name;
            battleScreenLeftATK.text = damageArray[0].ToString();
            battleScreenLeftHIT.text = damageArray[1].ToString();
            battleScreenLeftCRIT.text = damageArray[2].ToString();

            //Populate rightside info
            battleScreenEnemyName.text = defenderScript.title; 
            battleScreenRightHp.text = defenderScript.currentHp.ToString();
            battleScreenRightHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)defenderScript.currentHp / defenderScript.maxHp, 1f);
 
            if (attackerScript.ranged) { battleScreenLeftRangedImage.SetActive(true); battleScreenLeftMeleeImage.SetActive(false);}
            else { battleScreenLeftRangedImage.SetActive(false); battleScreenLeftMeleeImage.SetActive(true); }
            
            if (defenderScript.ranged) { battleScreenRightRangedImage.SetActive(true); battleScreenRightMeleeImage.SetActive(false);}
            else { battleScreenRightRangedImage.SetActive(false); battleScreenRightMeleeImage.SetActive(true); }
            

            //Enemy is ranged and character is not, or vice versa
            if ((attackerScript.ranged && !defenderScript.ranged) || (!attackerScript.ranged && defenderScript.ranged) )
            {
                battleScreenRightAttack.text = "-";
                battleScreenEnemyATK.text =  "-";
                battleScreenEnemyHIT.text =  "-";
                battleScreenEnemyCRIT.text = "-";
                
            }
            else
            {
                battleScreenEnemyATK.text = enemyDamageArray[0].ToString();
                battleScreenEnemyHIT.text = enemyDamageArray[1].ToString();
                battleScreenEnemyCRIT.text = enemyDamageArray[2].ToString();  
                battleScreenRightAttack.text = defenderScript.knownAttacks[0].name;
            }
     
            //Enable the Battle screen and scale it from 0 to 1
            attackPanel.GetComponent<RectTransform>().localScale = startScale;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                attackPanel.GetComponent<RectTransform>().localScale = Vector3.Lerp(startScale, endScale, t);

                yield return null;
            }
            attackPanel.GetComponent<RectTransform>().localScale = endScale; // snap to final value

            //Player attack roll sequence
            yield return new WaitForSeconds(1.5f); 

            yield return StartCoroutine(AnimateManaDamage(chosenAttack.manaCost, battleScreenLeftManaBar, attacker, battleScreenLeftMana));

            if (RollAttack())
            {
                if (RollCrit())
                {
                    yield return StartCoroutine(AnimateHealthDamage(damageArray[0] * 2, battleScreenRightHpBar, defender, battleScreenRightHp));
                    defenderScript.currentHp = defenderScript.currentHp - damageArray[0] * 2;
                }
                else
                {
                    yield return StartCoroutine(AnimateHealthDamage(damageArray[0], battleScreenRightHpBar, defender, battleScreenRightHp));
                    defenderScript.currentHp = defenderScript.currentHp - damageArray[0]; 
                }
            }
            
            yield return new WaitForSeconds(0.5f);

            //Play enemy death dialogue if necessary
            if (defenderScript.currentHp <= 0)
            {   
                yield return StartCoroutine(DeathSequence(defender));
                //TODO: Remove sprite on battle screen
                //yield return StartCoroutine(battleController.characterSelected.GetComponent<PlayerController>().Die());
            }

            //Enemy attacks enemy
            else
            {
                //should only attack back if able to
                if ((attackerScript.ranged && defenderScript.ranged) || (!attackerScript.ranged && !defenderScript.ranged))
                {

                    if (RollEnemyAttack())
                    {
                        if (RollEnemyCrit())
                        {
                            yield return StartCoroutine(AnimateHealthDamage(enemyDamageArray[0] * 2, battleScreenLeftHpBar, attacker, battleScreenLeftHp));
                            attackerScript.currentHp = attackerScript.currentHp - enemyDamageArray[0] * 2;

                        }
                        else
                        {
                            yield return StartCoroutine(AnimateHealthDamage(enemyDamageArray[0], battleScreenLeftHpBar, attacker, battleScreenLeftHp));
                            attackerScript.currentHp = attackerScript.currentHp - enemyDamageArray[0];
                        }
                    }

                    //Play character death dialogue if necessary
                    if (attackerScript.currentHp <= 0)
                    {   
                        //TODO: Remove sprite on battle screen
                        if (attackerScript.deathDialogue != "")
                        {
                            yield return StartCoroutine(DeathSequence(attacker));
                        }
                    }
                }
            
            }
        
            //Enable the Battle screen and scale it from 1 to 0
            attackPanel.GetComponent<RectTransform>().localScale = startScale;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                attackPanel.GetComponent<RectTransform>().localScale = Vector3.Lerp(endScale, startScale, t);

                yield return null;
            }
            attackPanel.GetComponent<RectTransform>().localScale = startScale; // snap to final value

            //Reset Hp bar sizes
            battleScreenLeftHpBar.GetComponent<RectTransform>().sizeDelta = originalBattleScreenHpBarSize;
            battleScreenRightHpBar.GetComponent<RectTransform>().sizeDelta = originalBattleScreenHpBarSize;

            //Reset battlescreen info
            battleScreenPlayerName.text = "-";
            battleScreenLeftHp.text = "-";
            battleScreenLeftAttack.text = "-";
            battleScreenEnemyName.text = "-";
            battleScreenRightHp.text = "-";
            battleScreenRightAttack.text = "-";
            battleScreenEnemyATK.text = "-";
            battleScreenEnemyHIT.text = "-";
            battleScreenEnemyCRIT.text = "-";

        }
        else
        {
            if (RollAttack())
            {
                if (RollCrit())
                {
                    defenderScript.currentHp = defenderScript.currentHp - damageArray[0] * 2;
                }
                else
                {
                    defenderScript.currentHp = defenderScript.currentHp - damageArray[0]; 
                }
            }
            
            yield return new WaitForSeconds(0.5f);

            //Play enemy death dialogue if necessary
            if (defenderScript.currentHp <= 0)
            {   
                yield return StartCoroutine(DeathSequence(defender));
                //TODO: Remove sprite on battle screen
                //yield return StartCoroutine(battleController.characterSelected.GetComponent<PlayerController>().Die());
            }

            //Enemy attacks enemy
            else
            {
                //should only attack back if able to
                if ((attackerScript.ranged && defenderScript.ranged) || (!attackerScript.ranged && !defenderScript.ranged))
                {
                    if (RollEnemyAttack())
                    {
                        if (RollEnemyCrit())
                        {
                            attackerScript.currentHp = attackerScript.currentHp - enemyDamageArray[0] * 2;

                        }
                        else
                        {
                            attackerScript.currentHp = attackerScript.currentHp - enemyDamageArray[0];
                        }
                    }

                    //Play character death dialogue if necessary
                    if (attackerScript.currentHp <= 0)
                    {   
                        //TODO: Remove sprite on battle screen
                        if (attackerScript.deathDialogue != "")
                        {
                            yield return StartCoroutine(DeathSequence(attacker));
                        }
                    }
                }
            }
        }
        
        coroutineRunning = false;
    }
    private void ResetBattleScreenInfo()
    {
        //Reset Hp bar sizes
        battleScreenLeftHpBar.GetComponent<RectTransform>().sizeDelta = originalBattleScreenHpBarSize;
        battleScreenRightHpBar.GetComponent<RectTransform>().sizeDelta = originalBattleScreenHpBarSize;

        //Reset Mana bar sizes
        battleScreenLeftManaBar.GetComponent<RectTransform>().sizeDelta = originalBattleScreenManaBarSize;
        battleScreenRightManaBar.GetComponent<RectTransform>().sizeDelta = originalBattleScreenManaBarSize;

        //Reset battlescreen info
        battleScreenPlayerName.text = "-";
        battleScreenLeftHp.text = "-";
        battleScreenLeftAttack.text = "-";
        battleScreenEnemyName.text = "-";
        battleScreenRightHp.text = "-";
        battleScreenRightAttack.text = "-";
        battleScreenEnemyATK.text = "-";
        battleScreenEnemyHIT.text = "-";
        battleScreenEnemyCRIT.text = "-";

    }
    private bool RollCrit()
    {
        int roll = Random.Range(0, 100);
        if (roll <= damageArray[2])
        {
            Debug.Log("Character crit with: " + roll);
            return true;
        }
        return false;
    }
    private bool RollAttack()
    {
        //Determine hit/crit roll for character
        int roll = Random.Range(0, 100);
        if (roll <= damageArray[1])
        {
            Debug.Log("Character hits with: " + roll);
            return true;

        }
        else
        {
            //TODO: Show MISS ui
            Debug.Log("Character Missed!");
            return false;
        } 
    }
    private bool RollDebuff(int chance) {
        
        //Determine hit/crit roll for character
        int roll = Random.Range(0, 100);
        if (roll <= chance)
        {
            Debug.Log("Debuff success");
            return true;

        }
        else
        {
            //TODO: Show MISS ui
            Debug.Log("Debuff failed");
            return false;
        } 
    }
    private bool RollEnemyAttack()
    {
        //Determine hit/crit roll for enemy
        int roll = Random.Range(0, 100);
        if (roll <= enemyDamageArray[1])
        {
            Debug.Log("Enemy hits with: " + roll);
            return true;
        }
        else
        {
            Debug.Log("Enemy Missed!");
            return false;
        }
    }
    private bool RollEnemyCrit()
    {
        int roll = Random.Range(0, 100);
        if (roll <= enemyDamageArray[2])
        {
            Debug.Log("Enemy crit with: " + roll);
            return true;
        }
        return false;
    }
    private void PopulateBattleScreenInfo()
    {
        //Populate leftside info
        battleScreenTransitionAudio.Play();
        battleScreenPlayerName.text = battleController.characterSelected.GetComponent<PlayerController>().title;
        battleScreenLeftHp.text = battleController.characterSelected.GetComponent<PlayerController>().currentHp.ToString();
        battleScreenLeftHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.characterSelected.GetComponent<PlayerController>().currentHp / battleController.characterSelected.GetComponent<PlayerController>().maxHp, 1f);
        
        battleScreenLeftMana.text = battleController.characterSelected.GetComponent<PlayerController>().currentMana.ToString();
        battleScreenLeftManaBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.characterSelected.GetComponent<PlayerController>().currentMana / battleController.characterSelected.GetComponent<PlayerController>().maxMana, 1f);
        
        battleScreenLeftAttack.text = chosenAttack.name;
        battleScreenLeftHIT.text = damageArray[1].ToString();
        battleScreenLeftCRIT.text = damageArray[2].ToString();
        if (battleController.characterSelected.GetComponent<PlayerController>().ranged) { battleScreenLeftRangedImage.SetActive(true); battleScreenLeftMeleeImage.SetActive(false);}
        else { battleScreenLeftRangedImage.SetActive(false); battleScreenLeftMeleeImage.SetActive(true); }
        
        //Populate rightside info
        if (isAssisting)
        {
            battleScreenLeftATK.text = (-1 * damageArray[0]).ToString();
            battleScreenEnemyName.text = battleController.assistableCharacterSelected.GetComponent<PlayerController>().title;
            battleScreenRightHp.text = battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentHp.ToString();
            battleScreenRightHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentHp / battleController.assistableCharacterSelected.GetComponent<PlayerController>().maxHp, 1f);   
            battleScreenRightMana.text = battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentMana.ToString();
            battleScreenRightManaBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentMana / battleController.assistableCharacterSelected.GetComponent<PlayerController>().maxMana, 1f);  
            if (battleController.assistableCharacterSelected.GetComponent<PlayerController>().ranged) { battleScreenRightRangedImage.SetActive(true); battleScreenRightMeleeImage.SetActive(false);}
            else { battleScreenRightRangedImage.SetActive(false); battleScreenRightMeleeImage.SetActive(true); }
            battleScreenRightAttack.text = "-";
            battleScreenEnemyATK.text =  "-";
            battleScreenEnemyHIT.text =  "-";
            battleScreenEnemyCRIT.text = "-";
        }
        else
        {
            battleScreenLeftATK.text = damageArray[0].ToString();
            battleScreenEnemyName.text = battleController.enemySelected.GetComponent<EnemyController>().title; 
            battleScreenRightHp.text = battleController.enemySelected.GetComponent<EnemyController>().currentHp.ToString();
            battleScreenRightHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.enemySelected.GetComponent<EnemyController>().currentHp / battleController.enemySelected.GetComponent<EnemyController>().maxHp, 1f);
            battleScreenRightMana.text = battleController.enemySelected.GetComponent<EnemyController>().currentMana.ToString();
            battleScreenRightManaBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.enemySelected.GetComponent<EnemyController>().currentMana / battleController.enemySelected.GetComponent<EnemyController>().maxMana, 1f);
            
            if (battleController.enemySelected.GetComponent<EnemyController>().ranged) { battleScreenRightRangedImage.SetActive(true); battleScreenRightMeleeImage.SetActive(false);}
            else { battleScreenRightRangedImage.SetActive(false); battleScreenRightMeleeImage.SetActive(true); }
        
            //Enemy is not ranged and cannot attack back
            if ((!battleController.enemySelected.GetComponent<EnemyController>().ranged && battleController.characterSelected.GetComponent<PlayerController>().ranged) || (battleController.enemySelected.GetComponent<EnemyController>().ranged && !battleController.characterSelected.GetComponent<PlayerController>().ranged))
            {
                battleScreenRightAttack.text = "-";
                battleScreenEnemyATK.text =  "-";
                battleScreenEnemyHIT.text =  "-";
                battleScreenEnemyCRIT.text = "-";
            }
            else
            {
                battleScreenRightAttack.text = battleController.enemySelected.GetComponent<EnemyController>().knownAttacks[0].name;
                battleScreenEnemyATK.text =  enemyDamageArray[0].ToString();
                battleScreenEnemyHIT.text =  enemyDamageArray[1].ToString();
                battleScreenEnemyCRIT.text = enemyDamageArray[2].ToString();
            }
        }

    }
    private IEnumerator AnimateHealthDamage(int damage, GameObject healthBarObject, GameObject person, TextMeshProUGUI healthText)
    {
        RectTransform rect = healthBarObject.GetComponent<RectTransform>();
        float duration = 1.5f;
        float elapsed = 0f;
        Vector2 startSize = rect.sizeDelta;
        Vector2 endSize;
        int startNumber = 0;
        int targetNumber = 0;

        if (person.GetComponent<PlayerController>() != null)
        {
            float temp = (float)(person.GetComponent<PlayerController>().currentHp - damage) / person.GetComponent<PlayerController>().maxHp;
            if (temp > 1f)
            {
                endSize = originalBattleScreenHpBarSize;
            }
            else if (person.GetComponent<PlayerController>().currentHp - damage <= 0)
            {
                endSize = originalBattleScreenHpBarSize * 0f;
            }
            else
            {
                endSize = originalBattleScreenHpBarSize * temp;
            }
            startNumber = person.GetComponent<PlayerController>().currentHp;
            targetNumber = person.GetComponent<PlayerController>().currentHp - damage;
            if (targetNumber > person.GetComponent<PlayerController>().maxHp)
            {
                targetNumber = person.GetComponent<PlayerController>().maxHp;
            }
            if (targetNumber < 0) { targetNumber = 0; }

        }
        else
        {
            float temp = (float)(person.GetComponent<EnemyController>().currentHp - damage) / person.GetComponent<EnemyController>().maxHp;
            if (temp > 1f)
            {
                endSize = originalBattleScreenHpBarSize;
            }
            else if (person.GetComponent<EnemyController>().currentHp - damage <= 0)
            {
                endSize = originalBattleScreenHpBarSize * 0f;
            }
            else
            {
                endSize = originalBattleScreenHpBarSize * temp;
            }
            
            startNumber = person.GetComponent<EnemyController>().currentHp;
            targetNumber = person.GetComponent<EnemyController>().currentHp - damage;
            if (targetNumber > person.GetComponent<EnemyController>().maxHp)
            {
                targetNumber = person.GetComponent<EnemyController>().maxHp;
            }
            if (targetNumber < 0) { targetNumber = 0; }
        }

        // Track the current printed number
        int currentPrintedNumber = startNumber;

        // Determine direction
        int direction = startNumber > targetNumber ? -1 : 1;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            rect.sizeDelta = Vector2.Lerp(startSize, endSize, t);
            float numberLerp = Mathf.Lerp(startNumber, targetNumber, t);
            int newNumber = direction == -1 
                ? Mathf.FloorToInt(numberLerp)
                : Mathf.CeilToInt(numberLerp);

            if (newNumber != currentPrintedNumber)
            {
                currentPrintedNumber = newNumber;
                healthText.text = currentPrintedNumber.ToString();
            }

            yield return null;
        }

        // Ensure final values are exact
        rect.sizeDelta = endSize;

        if (currentPrintedNumber != targetNumber)
        {
            currentPrintedNumber = targetNumber;
        }
    }
    private IEnumerator AnimateManaDamage(int damage, GameObject healthBarObject, GameObject person, TextMeshProUGUI healthText)
    {
        RectTransform rect = healthBarObject.GetComponent<RectTransform>();
        float duration = 1.5f;
        float elapsed = 0f;
        Vector2 startSize = rect.sizeDelta;
        Vector2 endSize;
        int startNumber = 0;
        int targetNumber = 0;

        if (person.GetComponent<PlayerController>() != null)
        {
            float temp = (float)(person.GetComponent<PlayerController>().currentMana - damage) / person.GetComponent<PlayerController>().maxMana;
            if (temp > 1f)
            {
                endSize = originalBattleScreenManaBarSize;
            }
            else if (person.GetComponent<PlayerController>().currentMana - damage <= 0)
            {
                endSize = originalBattleScreenManaBarSize * 0f;
            }
            else
            {
                endSize = originalBattleScreenManaBarSize * temp;
            }
            startNumber = person.GetComponent<PlayerController>().currentMana;
            targetNumber = person.GetComponent<PlayerController>().currentMana - damage;
            if (targetNumber > person.GetComponent<PlayerController>().maxMana)
            {
                targetNumber = person.GetComponent<PlayerController>().maxMana;
            }
            if (targetNumber < 0) { targetNumber = 0; }

        }
        else
        {
            float temp = (float)(person.GetComponent<EnemyController>().currentMana - damage) / person.GetComponent<EnemyController>().maxMana;
            if (temp > 1f)
            {
                endSize = originalBattleScreenManaBarSize;
            }
            else if (person.GetComponent<EnemyController>().currentMana - damage <= 0)
            {
                endSize = originalBattleScreenManaBarSize * 0f;
            }
            else
            {
                endSize = originalBattleScreenManaBarSize * temp;
            }
            
            startNumber = person.GetComponent<EnemyController>().currentMana;
            targetNumber = person.GetComponent<EnemyController>().currentMana - damage;
            if (targetNumber > person.GetComponent<EnemyController>().maxMana)
            {
                targetNumber = person.GetComponent<EnemyController>().maxMana;
            }
            if (targetNumber < 0) { targetNumber = 0; }
        }

        // Track the current printed number
        int currentPrintedNumber = startNumber;

        // Determine direction
        int direction = startNumber > targetNumber ? -1 : 1;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            rect.sizeDelta = Vector2.Lerp(startSize, endSize, t);
            float numberLerp = Mathf.Lerp(startNumber, targetNumber, t);
            int newNumber = direction == -1 
                ? Mathf.FloorToInt(numberLerp)
                : Mathf.CeilToInt(numberLerp);

            if (newNumber != currentPrintedNumber)
            {
                currentPrintedNumber = newNumber;
                healthText.text = currentPrintedNumber.ToString();
            }

            yield return null;
        }

        // Ensure final values are exact
        rect.sizeDelta = endSize;

        if (currentPrintedNumber != targetNumber)
        {
            currentPrintedNumber = targetNumber;
        }
    }
    private IEnumerator DeathSequence(GameObject person)
    {
        if (person.GetComponent<PlayerController>() != null && person.GetComponent<PlayerController>().deathDialogue != "")
        {
            dialogueBox.SetActive(true);
            float time = .15f;
            float elapsed = 0f;
            Vector2 targetPos = new Vector2(0f, -50f);
            Vector2 startPos = new Vector2(0f, -60f); // Off-screen bottom

            //Player dialogue

            dialogueBoxTitle.text = person.GetComponent<PlayerController>().title;
            yield return new WaitForSeconds(.25f);
            dialogueBoxRectTransform.anchoredPosition = startPos;
            dialogueBoxCanvasGroup.alpha = 0f;

            while (elapsed < time)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / time);
                dialogueBoxCanvasGroup.alpha = t;
                dialogueBoxRectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
                yield return null;
            }

            // Ensure final values are set
            dialogueBoxCanvasGroup.alpha = 1f;
            dialogueBoxRectTransform.anchoredPosition = targetPos;
            yield return StartCoroutine(TypeLine(person.GetComponent<PlayerController>().deathDialogue));
            yield return new WaitForSeconds(2f);

            //Fade box out and reset text
            elapsed = 0f;
            while (elapsed < time)
            {
                elapsed += Time.deltaTime;
                float t2 = Mathf.Clamp01(elapsed / time);
                dialogueBoxCanvasGroup.alpha = dialogueBoxCanvasGroup.alpha = Mathf.Lerp(1, 0f, elapsed / time);
                dialogueBoxRectTransform.anchoredPosition = Vector2.Lerp(targetPos, startPos, t2);
                yield return null;
            }
            dialogueBoxCanvasGroup.alpha = 0f;
            dialogueBoxRectTransform.anchoredPosition = startPos;
            dialogueBoxText.text = "";

            yield return new WaitForSeconds(2f);
        }
        
        else if (person.GetComponent<EnemyController>() != null && person.GetComponent<EnemyController>().deathDialogue != "")
        {
            dialogueBox.SetActive(true);
            float time = .15f;
            float elapsed = 0f;
            Vector2 targetPos = new Vector2(0f, -50f);
            Vector2 startPos = new Vector2(0f, -60f); // Off-screen bottom

            //Enemy dialogue
            dialogueBoxTitle.text = person.GetComponent<EnemyController>().title;
            yield return new WaitForSeconds(.25f);
            dialogueBoxRectTransform.anchoredPosition = startPos;
            dialogueBoxCanvasGroup.alpha = 0f;

            while (elapsed < time)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / time);
                dialogueBoxCanvasGroup.alpha = t;
                dialogueBoxRectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
                yield return null;
            }

            // Ensure final values are set
            dialogueBoxCanvasGroup.alpha = 1f;
            dialogueBoxRectTransform.anchoredPosition = targetPos;
            yield return StartCoroutine(TypeLine(person.GetComponent<EnemyController>().deathDialogue));
            
            //Fade box out and reset text
            elapsed = 0f;
            while (elapsed < time)
            {
                elapsed += Time.deltaTime;
                float t2 = Mathf.Clamp01(elapsed / time);
                dialogueBoxCanvasGroup.alpha = dialogueBoxCanvasGroup.alpha = Mathf.Lerp(1, 0f, elapsed / time);
                dialogueBoxRectTransform.anchoredPosition = Vector2.Lerp(targetPos, startPos, t2);
                yield return null;
            }
            dialogueBoxCanvasGroup.alpha = 0f;
            dialogueBoxRectTransform.anchoredPosition = startPos;
            dialogueBoxText.text = "";

            yield return new WaitForSeconds(2f);
        
        }
        
        //Destroy gameobject
        if (person.GetComponent<PlayerController>() != null)
        {
            person.GetComponent<PlayerController>().Die();
            battleController.characterSelected = null;
        }
        else
        {
            person.GetComponent<EnemyController>().Die(battleController.characterSelected);
            battleController.enemySelected = null;
        }

    }
    private IEnumerator TypeLine(string s) 
    {
        typingAudio.pitch = 0.75f;
        typingAudio.Play();
        foreach (char c in s.ToCharArray()) {
            dialogueBoxText.text += c;
            yield return new WaitForSeconds(.1f);
        }
        typingAudio.Stop();

        typingAudio.pitch = 1f;

    }
}

