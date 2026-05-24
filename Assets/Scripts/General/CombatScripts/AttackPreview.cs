using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;

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
    public TextMeshProUGUI battleScreenRightATK;
    public TextMeshProUGUI battleScreenRightHIT;
    public TextMeshProUGUI battleScreenRightCRIT;
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
        scm = FindAnyObjectByType<SaveManager>();
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
                    damageArray = calculateDamage(battleController.characterSelected, battleController.enemySelected, chosenAttack as Attack);
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
    public int[] calculateDamage(GameObject attacker, GameObject defender, Attack attackMove)
    {
        float damage = -1;
        float accuracy = -1;
        float critChance= -1;
        int[] returnArray = {-1, -1, -1};

        try
        {
            EnemyController attackerScript = attacker.GetComponent<EnemyController>();
            PlayerController defenderScript = defender.GetComponent<PlayerController>();

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

            if (attacker.GetComponent<PlayerController>() != null && attacker.GetComponent<PlayerController>().buffs.Any(buff => buff.name == "Charged"))
            {
                damage = damage * 1.5f;
            }
            else if (attacker.GetComponent<EnemyController>() != null && attacker.GetComponent<EnemyController>().buffs.Any(buff => buff.name == "Charged"))
            {
                damage = damage * 1.5f;
            }

            returnArray[0] = (int)Mathf.Round(damage);
            returnArray[1] = (accuracy > 100) ? 100 : (int)Mathf.Round(accuracy);
            returnArray[2] = (critChance > 100) ? 100 : (int)Mathf.Round(critChance);
        

        }

        catch (System.Exception e)
        {
            PlayerController attackerScript = attacker.GetComponent<PlayerController>();
            EnemyController defenderScript = defender.GetComponent<EnemyController>(); 

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
            returnArray[1] = (accuracy > 100) ? 100 : (int)Mathf.Round(accuracy);
            returnArray[2] = (critChance > 100) ? 100 : (int)Mathf.Round(critChance);

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
                    enemyDamageArray = calculateDamage(battleController.enemySelected, battleController.characterSelected, battleController.enemySelected.GetComponent<EnemyController>().knownAttacks[0] as Attack);
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
                    enemyDamageArray = calculateDamage(battleController.enemySelected, battleController.characterSelected, battleController.enemySelected.GetComponent<EnemyController>().knownAttacks[0] as Attack);
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
                StartCoroutine(startAttackSequence(battleController.characterSelected, battleController.enemySelected, chosenAttack as Attack, "left"));
            }
        }
    }
    public IEnumerator startSupportSequence()
    {
        coroutineRunning = true;
        active = false;
        SupportMove supportMove = chosenAttack as SupportMove;

        //Don't skip animations
        if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
        {
            //PopulateBattleScreenInfo();
            
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
            yield return StartCoroutine(battleController.characterSelected.GetComponent<PlayerController>().endTurn());
        }
        coroutineRunning = false;
    }
    public IEnumerator startAttackSequence(GameObject leftPerson, GameObject rightPerson, Attack attackSelected, string initiator)
    {
        coroutineRunning = true;

        //Resetting 
        ResetBattleScreenInfo();

        PlayerController playerScript = leftPerson.GetComponent<PlayerController>();
        EnemyController enemyScript = rightPerson.GetComponent<EnemyController>();
        
        //Populate leftside info. Leftside is always a PlayerCharacter
        battleScreenPlayerName.text = playerScript.title;
        battleScreenLeftHp.text = playerScript.currentHp.ToString();
        battleScreenLeftHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)playerScript.currentHp / playerScript.maxHp, 1f);
        battleScreenLeftMana.text = playerScript.currentMana.ToString();
        battleScreenLeftManaBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)playerScript.currentMana / playerScript.maxMana, 1f);
        if (playerScript.ranged) { battleScreenLeftRangedImage.SetActive(true); battleScreenLeftMeleeImage.SetActive(false);}
        else { battleScreenLeftRangedImage.SetActive(false); battleScreenLeftMeleeImage.SetActive(true); }
        
        //Populate rightside info. Rightside is always an EnemyCharacter
        battleScreenEnemyName.text = enemyScript.title;
        battleScreenRightHp.text = enemyScript.currentHp.ToString();
        battleScreenRightHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)enemyScript.currentHp / enemyScript.maxHp, 1f);   
        battleScreenRightMana.text = enemyScript.currentMana.ToString();
        battleScreenRightManaBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)enemyScript.currentMana / enemyScript.maxMana, 1f);  
        if (enemyScript.ranged) { battleScreenRightRangedImage.SetActive(true); battleScreenRightMeleeImage.SetActive(false);}
        else { battleScreenRightRangedImage.SetActive(false); battleScreenRightMeleeImage.SetActive(true); }

        //TODO: Set leftside and rightside sprites
            //Example: astridSprite.SetActive(true);

        if (initiator == "left")
        {
            //Populate Attack blocks
            int[] leftArray = calculateDamage(leftPerson, rightPerson, attackSelected);
            int[] rightArray = calculateDamage(rightPerson, leftPerson, enemyScript.knownAttacks[0] as Attack);
            battleScreenLeftAttack.text = attackSelected.name;
            battleScreenLeftATK.text = leftArray[0].ToString();
            battleScreenLeftHIT.text = leftArray[1].ToString();
            battleScreenLeftCRIT.text = leftArray[2].ToString();
            if ((playerScript.ranged && enemyScript.ranged) || (!playerScript.ranged && !enemyScript.ranged))
            {
                battleScreenRightAttack.text = enemyScript.knownAttacks[0].name;
                battleScreenRightATK.text = rightArray[0].ToString();
                battleScreenRightHIT.text = rightArray[1].ToString();
                battleScreenRightCRIT.text = rightArray[2].ToString();
            }
        
            //Don't skip animation
            if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
            {                
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
                yield return StartCoroutine(AnimateManaDamage(attackSelected.manaCost, battleScreenLeftManaBar, leftPerson, battleScreenLeftMana));

                //TODO Start attack animation
                    //Example: leftPerson.GetComponent<Animator>().SetBool(attackSelected.name, true);
            }
            
            //Update mana
            playerScript.currentMana = playerScript.currentMana - attackSelected.manaCost;

            //Attack hits
            if (Roll(leftArray[1]))
            {
                if (Roll(leftArray[2]))
                {
                    if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
                    {
                        //Show rightside crit
                        //yield return StartCoroutine(CritAnimation("right"));
                        //heavy screen shake animation?
                        yield return StartCoroutine(AnimateHealthDamage(leftArray[0] * 2, battleScreenRightHpBar, rightPerson, battleScreenRightHp));
                    }
                    enemyScript.currentHp = enemyScript.currentHp - leftArray[0] * 2;
                }
                else
                {
                    if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
                    {
                        //TODO: Hit animation
                        //Example: Slight screen shake
                        yield return StartCoroutine(AnimateHealthDamage(leftArray[0], battleScreenRightHpBar, rightPerson, battleScreenRightHp));
                    }
                    enemyScript.currentHp = enemyScript.currentHp - leftArray[0];
                }
                
                //Try apply debuffs to enemy
                foreach (Debuff debuff in attackSelected.debuffs) 
                {
                    if (Roll(debuff.chanceToApply)) 
                    {
                        //Play animation
                        if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
                        {
                            //TODO: Play debuff animation
                            //Example: rightPerson.GetComponent<Animator>().SetBool(debuff.name, true);
                        }

                        //Overworld animation
                        else
                        {
                            
                        }

                        //Check if debuff is already on the enemy
                        if (enemyScript.HasDebuff(debuff)) {
                            enemyScript.AddTurnsToDebuff(debuff);
                            
                        }
                        else {
                            enemyScript.debuffs.Add(debuff);
                            enemyScript.ApplyDebuffEffects();

                            //Recalculate damage
                            if (debuff.name == "Dazed" || debuff.name == "Confused")
                            {
                                if ((enemyScript.ranged && playerScript.ranged) || (!enemyScript.ranged && !playerScript.ranged))
                                {
                                    rightArray = calculateDamage(rightPerson, leftPerson, enemyScript.knownAttacks[0] as Attack);
                                    battleScreenRightATK.text = rightArray[0].ToString();
                                    battleScreenRightHIT.text = rightArray[1].ToString();
                                    battleScreenRightCRIT.text = rightArray[2].ToString();
                                    battleScreenRightATK.color = Color.red;
                                    battleScreenRightHIT.color = Color.red;
                                    battleScreenRightCRIT.color = Color.red;
                                }
                            }

                        }

                    }
                }

                //Try apply buffs to attacker
                foreach (Buff buff in attackSelected.buffs)
                {
                    if (Roll(buff.chanceToApply))
                    {
                        //Play animation
                        if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
                        {
                            //TODO: Play buff animation
                            //Example: leftPerson.GetComponent<Animator>().SetBool(debuff.name, true);
                        }

                        //Overworld animation
                        else
                        {
                            
                        }

                        //Check if buff is already present
                        if (playerScript.HasBuff(buff))
                        {
                            playerScript.AddTurnsToBuff(buff);
                            
                        }
                        else
                        {
                            playerScript.buffs.Add(buff);
                            playerScript.ApplyBuffEffects();
                        }
                    }
                }
                
                //Remove Charged if present
                RemoveCharged(leftPerson);

            }

            //Attack missed
            else
            {
                if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
                {
                    //Show rightside missed
                    //yield return StartCoroutine(MissedAnimation("right"));
                }

                //TODO: Visual cue when missed.
            }

            yield return new WaitForSeconds(2f);

            //Play enemy death dialogue if necessary
            if (enemyScript.currentHp <= 0)
            {   
                //TODO: Remove sprite on battle screen
                yield return StartCoroutine(DeathSequence(rightPerson));
            }
           
            //********************************************************* START ENEMY ATTACK *******************************************************************
            else
            {
                if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
                {
                    //TODO Start attack animation
                    //Example: rightPerson.GetComponent<Animator>().SetBool(attackSelected.name, true);
                }

                //Attack hits
                if (Roll(rightArray[1]))
                {
                    if (Roll(rightArray[2]))
                    {
                        if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
                        {
                            //Show leftside crit
                            //yield return StartCoroutine(CritAnimation("left"));
                            //heavy screen shake crit animation
                            yield return StartCoroutine(AnimateHealthDamage(rightArray[0] * 2, battleScreenLeftHpBar, leftPerson, battleScreenLeftHp));
                        }
                        playerScript.currentHp = playerScript.currentHp - rightArray[0] * 2;
                    }
                    else
                    {
                        if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
                        {
                            //TODO: hit animation screen shake
                            yield return StartCoroutine(AnimateHealthDamage(rightArray[0], battleScreenLeftHpBar, leftPerson, battleScreenLeftHp));
                        }
                        playerScript.currentHp = playerScript.currentHp - rightArray[0];
                    }
                    
                    //Don't need to check buff/debuffs, because reaction attack is always a basic attack.
                    
                    //Remove Charged if present
                    RemoveCharged(leftPerson);

                }

                //Attack missed
                else
                {
                    if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
                    {
                        //Show leftside missed
                        //yield return StartCoroutine(MissedAnimation("left"));
                    }

                    //TODO: Visual cue when missed.
                }

                yield return new WaitForSeconds(2f);

                //Play enemy character dialogue if necessary
                if (playerScript.currentHp <= 0)
                {   
                    //TODO: Remove sprite on battle screen
                    yield return StartCoroutine(DeathSequence(leftPerson));
                }
           
            }
        
        }
        
        else if (initiator == "right")
        {
            int[] rightArray = calculateDamage(rightPerson, leftPerson, attackSelected);
            int[] leftArray = calculateDamage(leftPerson, rightPerson, playerScript.knownAttacks[0] as Attack);
            battleScreenRightAttack.text = attackSelected.name;
            battleScreenRightATK.text = rightArray[0].ToString();
            battleScreenRightHIT.text = rightArray[1].ToString();
            battleScreenRightCRIT.text = rightArray[2].ToString();
            
            //Populate Attack blocks
            if ((playerScript.ranged && enemyScript.ranged) || (!playerScript.ranged && !enemyScript.ranged))
            {
                battleScreenLeftAttack.text = playerScript.knownAttacks[0].name;
                battleScreenLeftATK.text = leftArray[0].ToString();
                battleScreenLeftHIT.text = leftArray[1].ToString();
                battleScreenLeftCRIT.text = leftArray[2].ToString();  
            }
        
            //Don't skip animation
            if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
            {                
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
                yield return StartCoroutine(AnimateManaDamage(attackSelected.manaCost, battleScreenRightManaBar, rightPerson, battleScreenRightMana));

                //TODO Start attack animation
                    //Example: rightPerson.GetComponent<Animator>().SetBool(attackSelected.name, true);
            }
        
            //Update mana
            enemyScript.currentMana = enemyScript.currentMana - attackSelected.manaCost;

            //Attack hits
            if (Roll(rightArray[1]))
            {
                if (Roll(rightArray[2]))
                {
                    if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
                    {
                        //Show rightside crit
                        //yield return StartCoroutine(CritAnimation("right"));
                        //heavy screen shake animation?
                        yield return StartCoroutine(AnimateHealthDamage(rightArray[0] * 2, battleScreenLeftHpBar, leftPerson, battleScreenLeftHp));
                    }
                    playerScript.currentHp = playerScript.currentHp - rightArray[0] * 2;
                }
                else
                {
                    if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
                    {
                        //TODO: Hit animation
                        //Example: Slight screen shake
                        yield return StartCoroutine(AnimateHealthDamage(rightArray[0], battleScreenLeftHpBar, leftPerson, battleScreenLeftHp));
                    }
                    playerScript.currentHp = playerScript.currentHp - rightArray[0];
                }
                
                //Try apply debuffs to enemy
                foreach (Debuff debuff in attackSelected.debuffs) 
                {
                    if (Roll(debuff.chanceToApply)) 
                    {
                        //Play animation
                        if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
                        {
                            //TODO: Play debuff animation
                            //Example: leftPerson.GetComponent<Animator>().SetBool(debuff.name, true);
                        }

                        //Overworld animation
                        else
                        {
                            
                        }

                        //Check if debuff is already on the enemy
                        if (playerScript.HasDebuff(debuff)) {
                            playerScript.AddTurnsToDebuff(debuff);
                            
                        }
                        else {
                            playerScript.debuffs.Add(debuff);
                            playerScript.ApplyDebuffEffects();

                            //Recalculate damage
                            if (debuff.name == "Dazed" || debuff.name == "Confused")
                            {
                                if ((enemyScript.ranged && playerScript.ranged) || (!enemyScript.ranged && !playerScript.ranged))
                                {
                                    leftArray = calculateDamage(leftPerson, rightPerson, playerScript.knownAttacks[0] as Attack);
                                    battleScreenLeftATK.text = leftArray[0].ToString();
                                    battleScreenLeftHIT.text = leftArray[1].ToString();
                                    battleScreenLeftCRIT.text = leftArray[2].ToString();
                                    battleScreenLeftATK.color = Color.red;
                                    battleScreenLeftHIT.color = Color.red;
                                    battleScreenLeftCRIT.color = Color.red;
                                }
                            }

                        }

                    }
                }

                //Try apply buffs to attacker
                foreach (Buff buff in attackSelected.buffs)
                {
                    if (Roll(buff.chanceToApply))
                    {
                        //Play animation
                        if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
                        {
                            //TODO: Play buff animation
                            //Example: rightPerson.GetComponent<Animator>().SetBool(debuff.name, true);
                        }

                        //Overworld animation
                        else
                        {
                            
                        }

                        //Check if buff is already present
                        if (enemyScript.HasBuff(buff))
                        {
                            enemyScript.AddTurnsToBuff(buff);
                            
                        }
                        else
                        {
                            enemyScript.buffs.Add(buff);
                            enemyScript.ApplyBuffEffects();
                        }
                    }
                }
                
                //Remove Charged if present
                RemoveCharged(rightPerson);

            }
        
            //Attack missed
            else
            {
                if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
                {
                    //Show rightside missed
                    //yield return StartCoroutine(MissedAnimation("left"));
                }

                //TODO: Visual cue when missed.
            }

            yield return new WaitForSeconds(2f);
            
            //Play enemy death dialogue if necessary
            if (playerScript.currentHp <= 0)
            {   
                //TODO: Remove sprite on battle screen
                yield return StartCoroutine(DeathSequence(leftPerson));
            }
           
            //********************************************************* START PLAYER ATTACK *******************************************************************
            else
            {
                if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
                {
                    //TODO Start attack animation
                    //Example: leftPerson.GetComponent<Animator>().SetBool(attackSelected.name, true);
                }

                //Attack hits
                if (Roll(leftArray[1]))
                {
                    if (Roll(leftArray[2]))
                    {
                        if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
                        {
                            //Show rightside crit
                            //yield return StartCoroutine(CritAnimation("right"));
                            //heavy screen shake animation?
                            yield return StartCoroutine(AnimateHealthDamage(leftArray[0] * 2, battleScreenRightHpBar, rightPerson, battleScreenRightHp));
                        }
                        enemyScript.currentHp = enemyScript.currentHp - leftArray[0] * 2;
                    }
                    else
                    {
                        if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
                        {
                            //TODO: Hit animation
                            //Example: Slight screen shake
                            yield return StartCoroutine(AnimateHealthDamage(leftArray[0], battleScreenRightHpBar, rightPerson, battleScreenRightHp));
                        }
                        enemyScript.currentHp = enemyScript.currentHp - leftArray[0];
                    }
                    
                    //Try apply debuffs to enemy
                    foreach (Debuff debuff in attackSelected.debuffs) 
                    {
                        if (Roll(debuff.chanceToApply)) 
                        {
                            //Play animation
                            if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
                            {
                                //TODO: Play debuff animation
                                //Example: rightPerson.GetComponent<Animator>().SetBool(debuff.name, true);
                            }

                            //Overworld animation
                            else
                            {
                                
                            }

                            //Check if debuff is already on the enemy
                            if (enemyScript.HasDebuff(debuff)) {
                                enemyScript.AddTurnsToDebuff(debuff);
                                
                            }
                            else {
                                enemyScript.debuffs.Add(debuff);
                                enemyScript.ApplyDebuffEffects();

                                //Recalculate damage
                                if (debuff.name == "Dazed" || debuff.name == "Confused")
                                {
                                    if ((enemyScript.ranged && playerScript.ranged) || (!enemyScript.ranged && !playerScript.ranged))
                                    {
                                        rightArray = calculateDamage(rightPerson, leftPerson, enemyScript.knownAttacks[0] as Attack);
                                        battleScreenRightATK.text = rightArray[0].ToString();
                                        battleScreenRightHIT.text = rightArray[1].ToString();
                                        battleScreenRightCRIT.text = rightArray[2].ToString();
                                        battleScreenRightATK.color = Color.red;
                                        battleScreenRightHIT.color = Color.red;
                                        battleScreenRightCRIT.color = Color.red;
                                    }
                                }

                            }

                        }
                    }

                    //Try apply buffs to attacker
                    foreach (Buff buff in attackSelected.buffs)
                    {
                        if (Roll(buff.chanceToApply))
                        {
                            //Play animation
                            if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
                            {
                                //TODO: Play buff animation
                                //Example: leftPerson.GetComponent<Animator>().SetBool(debuff.name, true);
                            }

                            //Overworld animation
                            else
                            {
                                
                            }

                            //Check if buff is already present
                            if (playerScript.HasBuff(buff))
                            {
                                playerScript.AddTurnsToBuff(buff);
                                
                            }
                            else
                            {
                                playerScript.buffs.Add(buff);
                                playerScript.ApplyBuffEffects();
                            }
                        }
                    }
                    
                    //Remove Charged if present
                    RemoveCharged(leftPerson);

                }

                //Attack missed
                else
                {
                    if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
                    {
                        //Show rightside missed
                        //yield return StartCoroutine(MissedAnimation("right"));
                    }

                    //TODO: Visual cue when missed.
                }

                yield return new WaitForSeconds(2f);

                //Play enemy death dialogue if necessary
                if (enemyScript.currentHp <= 0)
                {   
                    //TODO: Remove sprite on battle screen
                    yield return StartCoroutine(DeathSequence(rightPerson));
                }
            }
        
        }
    
        //Disable battle screen if necessary.
        if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
        { 
            //Enable the Battle screen and scale it from 1 to 0
            Vector3 startScale = Vector3.one;
            Vector3 endScale = Vector3.zero;
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
        }
    
        //End character turn
        if (initiator == "left")
        {
            yield return StartCoroutine(playerScript.endTurn());
        }
        else if (initiator == "right")
        {
            
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

        //Resetting 
        battleScreenLeftATK.color = Color.white;
        battleScreenLeftHIT.color = Color.white;
        battleScreenLeftCRIT.color = Color.white;
        battleScreenRightATK.color = Color.white;
        battleScreenRightHIT.color = Color.white;
        battleScreenRightCRIT.color = Color.white;
        battleScreenLeftATK.text = "-";
        battleScreenLeftHIT.text = "-";
        battleScreenLeftCRIT.text = "-";
        battleScreenRightATK.text = "-";
        battleScreenRightHIT.text = "-";
        battleScreenRightCRIT.text = "-";
        battleScreenLeftAttack.text = "-";
        battleScreenRightAttack.text = "-";

    }
    private bool Roll(int chance) {
        
        //Determine hit/crit roll for character
        int roll = Random.Range(0, 100);
        if (roll <= chance)
        {
            Debug.Log("Roll success");
            return true;

        }
        else
        {
            Debug.Log("Roll failed");
            return false;
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
    public IEnumerator DeathSequence(GameObject person)
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
    private void RemoveCharged(GameObject person)
    {
        if (person.GetComponent<PlayerController>() != null)
        {
            PlayerController temp = person.GetComponent<PlayerController>();
            foreach (Buff buff in temp.buffs)
            {
                if (buff.name == "Charged")
                {
                    buff.turnsRemaining--;
                }
            }
            temp.buffs.RemoveAll(d => d.turnsRemaining <= 0);
        }
        else
        {
            EnemyController temp = person.GetComponent<EnemyController>();
            foreach (Buff buff in temp.buffs)
            {
                if (buff.name == "Charged")
                {
                    buff.turnsRemaining--;
                }
            }
            temp.buffs.RemoveAll(d => d.turnsRemaining <= 0);
        }
    }
}

