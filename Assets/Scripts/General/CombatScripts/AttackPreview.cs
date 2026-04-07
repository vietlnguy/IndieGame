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
    public TextMeshProUGUI battleScreenPlayerAttack;
    public TextMeshProUGUI battleScreenEnemyAttack;
    public TextMeshProUGUI battleScreenPlayerATK;
    public TextMeshProUGUI battleScreenPlayerHIT;
    public TextMeshProUGUI battleScreenPlayerCRIT;
    public TextMeshProUGUI battleScreenEnemyATK;
    public TextMeshProUGUI battleScreenEnemyHIT;
    public TextMeshProUGUI battleScreenEnemyCRIT;
    public TextMeshProUGUI battleScreenPlayerHealth;
    public TextMeshProUGUI battleScreenEnemyHealth;
    public GameObject battleScreenPlayerHpBar;
    public GameObject battleScreenEnemyHpBar;
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
    void Awake()
    {
        scm = FindFirstObjectByType<SaveManager>();
        battleController = GameObject.Find("BattleController").GetComponent<BattleController>();
        attackSelectorInitialPos = attackSelector.GetComponent<RectTransform>().anchoredPosition;
        originalHpManaBarSize = previewPlayerHpBar.GetComponent<RectTransform>().sizeDelta;
        originalEnemyHpManaBarSize = previewEnemyHpBar.GetComponent<RectTransform>().sizeDelta;
        originalBattleScreenHpBarSize = battleScreenEnemyHpBar.GetComponent<RectTransform>().sizeDelta;
        
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
        try 
        {
            chosenAttack = battleController.characterSelected.GetComponent<PlayerController>().knownAttacks[attackIndex];
            calculateDamage(battleController.characterSelected, battleController.enemySelected, chosenAttack);
            atkBlock.text = damageArray[0].ToString();
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
                    if (attackMove.damageType == "physical") 
                    {
                        damage = attackerScript.attack * attackMove.attackMult * attackerScript.attack * attackMove.attackMult / ((attackerScript.attack * attackMove.attackMult) + defenderScript.defense);
                    }
                    else if (attackMove.damageType == "magical")
                    {
                        damage = attackerScript.intelligence * attackMove.intMult * attackerScript.intelligence * attackMove.intMult / ((attackerScript.intelligence * attackMove.intMult) + defenderScript.resistance);
                    }   

                    accuracy = attackMove.baseAccuracy + (attackerScript.skill - defenderScript.speed) * 2;
                    critChance = attackMove.baseCrit + (attackerScript.skill * 0.5f);
                }

                returnArray[0] = (int)Mathf.Round(damage);
                returnArray[1] = (int)Mathf.Round(accuracy);
                returnArray[2] = (int)Mathf.Round(critChance);
            }
            else if (attack is SupportMove)
            {
                
            }
        }

        catch (System.Exception e)
        {
            Debug.Log(e);
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
                    if (attackMove.damageType == "physical") 
                    {
                        damage = attackerScript.attack * attackMove.attackMult * attackerScript.attack * attackMove.attackMult / ((attackerScript.attack * attackMove.attackMult) + defenderScript.defense);
                    }
                    else if (attackMove.damageType == "magical")
                    {
                        damage = attackerScript.intelligence * attackMove.intMult * attackerScript.intelligence * attackMove.intMult / ((attackerScript.intelligence * attackMove.intMult) + defenderScript.resistance);
                    }   

                    accuracy = attackMove.baseAccuracy + (attackerScript.skill - defenderScript.speed) * 2;
                    critChance = attackMove.baseCrit + (attackerScript.skill * 0.5f);
                }

                returnArray[0] = (int)Mathf.Round(damage);
                returnArray[1] = (int)Mathf.Round(accuracy);
                returnArray[2] = (int)Mathf.Round(critChance);
            }
            else if (attack is SupportMove)
            {
                
            }

        }

        return returnArray;
    }
    public IEnumerator enablePreview(bool isAssisting)
    {
        updateAttackSelection();
        battleController.characterSelected.GetComponent<PlayerController>().movementEnabled = false;
        this.isAssisting = isAssisting;

        //Update titles
        characterTitle.text = battleController.characterSelected.GetComponent<PlayerController>().title;

        if (!isAssisting) 
        { 
            enemyTitle.text = battleController.enemySelected.GetComponent<EnemyController>().title;
            previewEnemyHp.text = battleController.enemySelected.GetComponent<EnemyController>().currentHp.ToString();
            previewEnemyMaxHp.text = battleController.enemySelected.GetComponent<EnemyController>().maxHp.ToString();
            previewEnemyMana.text = battleController.enemySelected.GetComponent<EnemyController>().currentMana.ToString();
            previewEnemyMaxMana.text = battleController.enemySelected.GetComponent<EnemyController>().maxMana.ToString();
            previewEnemyHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.enemySelected.GetComponent<EnemyController>().currentHp / battleController.enemySelected.GetComponent<EnemyController>().maxHp, 1f);
            previewEnemyManaBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.enemySelected.GetComponent<EnemyController>().currentMana / battleController.enemySelected.GetComponent<EnemyController>().maxMana, 1f);
            if (battleController.enemySelected.GetComponent<EnemyController>().ranged) { previewRightMeleeImage.SetActive(false); previewRightRangedImage.SetActive(true); }
            else { previewRightMeleeImage.SetActive(true); previewRightRangedImage.SetActive(false); }
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

        //Update leftside health bars and values
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
                    if (battleController.characterSelected.GetComponent<PlayerController>().knownAttacks[index] is Attack)
                    {
                        if (!isAssisting) { child2.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f); }
                        else { child2.GetComponent<TextMeshProUGUI>().color = new Color(.5f, .5f, .5f, .5f); }
                    }
                    else if (battleController.characterSelected.GetComponent<PlayerController>().knownAttacks[index] is SupportMove)
                    {
                        if (!isAssisting) { child2.GetComponent<TextMeshProUGUI>().color = new Color(.5f, .5f, .5f, .5f); }
                        else { child2.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);  }
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

        //Update damage stats
        if (!isAssisting)
        {
            //Check attack validity
            if (battleController.characterSelected.GetComponent<PlayerController>().knownAttacks[attackIndex] is Attack)
            {
                validAttack = true;
                damageArray = calculateDamage(battleController.characterSelected, battleController.enemySelected, battleController.characterSelected.GetComponent<PlayerController>().knownAttacks[attackIndex]);
                atkBlock.text = damageArray[0].ToString();
                hitBlock.text = damageArray[1].ToString();
                critBlock.text = damageArray[2].ToString();
                attackDescription.text = battleController.characterSelected.GetComponent<PlayerController>().knownAttacks[attackIndex].description;

                //When character is ranged, and enemy is not or vice versa
                if ((battleController.characterSelected.GetComponent<PlayerController>().ranged && !battleController.enemySelected.GetComponent<EnemyController>().ranged) || (!battleController.characterSelected.GetComponent<PlayerController>().ranged && battleController.enemySelected.GetComponent<EnemyController>().ranged))
                {
                    rightsideAtkBlock.text = "-";
                    rightsideHitBlock.text = "-";
                    rightsideCritBlock.text = "-";
                    rightsideAttackDescription.text = "-";
                }
                else
                {
                    enemyDamageArray = calculateDamage(battleController.enemySelected, battleController.characterSelected, battleController.enemySelected.GetComponent<EnemyController>().knownAttacks[0]);
                    rightsideAtkBlock.text = enemyDamageArray[0].ToString();
                    rightsideHitBlock.text = enemyDamageArray[1].ToString();
                    rightsideCritBlock.text = enemyDamageArray[2].ToString();
                    rightsideAttackDescription.text = battleController.enemySelected.GetComponent<EnemyController>().knownAttacks[0].description;

                }
            }
            else
            {
                validAttack = false;
                atkBlock.text = "-";
                hitBlock.text = "-";
                critBlock.text = "-";
                attackDescription.text = battleController.characterSelected.GetComponent<PlayerController>().knownAttacks[attackIndex].description;
            }

        }

        else
        {
            //TODO: Assisting logic
        }



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

        //Instantiate potrait prefabs
        //GameObject characterPrefab;
        //Vector2 temp = new Vector2(portraitBackground.GetComponent<RectTransform>().position.x, portraitBackground.GetComponent<RectTransform>().position.y - 1f);
        //if (battleController.characterSelected.GetComponent<PlayerController>().title == scm.loadedData.mainCharacterName) { characterPrefab = Instantiate(mainCharacterPrefab, temp, Quaternion.identity, attackPreviewSprites.transform); }
        //else if (battleController.characterSelected.GetComponent<PlayerController>().title == "Astrid") { characterPrefab = Instantiate(astridPrefab, temp, Quaternion.identity, attackPreviewSprites.transform); }
        ////TODO: include more prefabs for characters
//
        //GameObject enemyPrefab;
        //temp = new Vector2(enemyPortraitBackground.GetComponent<RectTransform>().position.x, enemyPortraitBackground.GetComponent<RectTransform>().position.y - 1f);
        //if (battleController.enemySelected.GetComponent<EnemyController>().title == "Soldier") { enemyPrefab = Instantiate(soldierPrefab, temp, Quaternion.identity, attackPreviewSprites.transform); }
        ////TODO: include more prefabs for enemies

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
    public IEnumerator startAttackSequence()
    {   
        coroutineRunning = true;

        //Populate battle info if showing animations. 0 means don't skip animations
        if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
        {
            //Populate info
            battleScreenTransitionAudio.Play();
            battleScreenPlayerName.text = battleController.characterSelected.GetComponent<PlayerController>().title;
            battleScreenPlayerHealth.text = battleController.characterSelected.GetComponent<PlayerController>().currentHp.ToString();
            battleScreenPlayerHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.characterSelected.GetComponent<PlayerController>().currentHp / battleController.characterSelected.GetComponent<PlayerController>().maxHp, 1f);
            battleScreenPlayerAttack.text = chosenAttack.name;
            battleScreenPlayerATK.text = damageArray[0].ToString();
            battleScreenPlayerHIT.text = damageArray[1].ToString();
            battleScreenPlayerCRIT.text = damageArray[2].ToString();
            if (battleController.characterSelected.GetComponent<PlayerController>().ranged) { battleScreenLeftRangedImage.SetActive(true); battleScreenLeftMeleeImage.SetActive(false);}
            else { battleScreenLeftRangedImage.SetActive(false); battleScreenLeftMeleeImage.SetActive(true); }
            
            //Support sequence info population
            if (isAssisting)
            {
                battleScreenEnemyName.text = battleController.assistableCharacterSelected.GetComponent<PlayerController>().title;
                battleScreenEnemyHealth.text = battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentHp.ToString();
                battleScreenEnemyHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentHp / battleController.assistableCharacterSelected.GetComponent<PlayerController>().maxHp, 1f);  
                if (battleController.assistableCharacterSelected.GetComponent<PlayerController>().ranged) { battleScreenRightRangedImage.SetActive(true); battleScreenRightMeleeImage.SetActive(false);}
                else { battleScreenRightRangedImage.SetActive(false); battleScreenRightMeleeImage.SetActive(true); }
            
            }
            
            //Attack sequence info population
            else
            {
                battleScreenEnemyName.text = battleController.enemySelected.GetComponent<EnemyController>().title; 
                battleScreenEnemyHealth.text = battleController.enemySelected.GetComponent<EnemyController>().currentHp.ToString();
                battleScreenEnemyHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.enemySelected.GetComponent<EnemyController>().currentHp / battleController.enemySelected.GetComponent<EnemyController>().maxHp, 1f);
                if (battleController.enemySelected.GetComponent<EnemyController>().ranged) { battleScreenRightRangedImage.SetActive(true); battleScreenRightMeleeImage.SetActive(false);}
                else { battleScreenRightRangedImage.SetActive(false); battleScreenRightMeleeImage.SetActive(true); }
            
                //Enemy is not ranged and cannot attack back
                if ((!battleController.enemySelected.GetComponent<EnemyController>().ranged && battleController.characterSelected.GetComponent<PlayerController>().ranged) || (battleController.enemySelected.GetComponent<EnemyController>().ranged && !battleController.characterSelected.GetComponent<PlayerController>().ranged))
                {
                    battleScreenEnemyAttack.text = "-";
                    battleScreenEnemyATK.text =  "-";
                    battleScreenEnemyHIT.text =  "-";
                    battleScreenEnemyCRIT.text = "-";
                }
                else
                {
                    battleScreenEnemyAttack.text = battleController.enemySelected.GetComponent<EnemyController>().knownAttacks[0].name;
                    battleScreenEnemyATK.text =  enemyDamageArray[0].ToString();
                    battleScreenEnemyHIT.text =  enemyDamageArray[1].ToString();
                    battleScreenEnemyCRIT.text = enemyDamageArray[2].ToString();
                }

            }

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
        }
        
        //Support roll sequence
        if (isAssisting)
        {
            //TODO: Assist logic
            yield return new WaitForSeconds(3f);
        }

        //Attack roll sequence
        else
        {

            if (PlayerPrefs.GetInt("combatAnim") == 0)
            {
                //TODO: Attack animations
                yield return new WaitForSeconds(3f);  
            }
            else
            {
                overworldAttackAudio.Play();
            }

            //Determine hit/crit roll for character
            int roll = Random.Range(0, 100);
            if (roll <= damageArray[1])
            {
                Debug.Log("Character hits with: " + roll);
                roll = Random.Range(0, 100);
                if (roll <= damageArray[2])
                {
                    Debug.Log("Character crit with: " + roll);
                    //When combat animation is on then animate health bar
                    if (PlayerPrefs.GetInt("combatAnim") == 0) {
                        yield return StartCoroutine(AnimateHealthDamage(damageArray[0] * 2, battleScreenEnemyHpBar, battleController.enemySelected, battleScreenEnemyHealth));
                    }
                    battleController.enemySelected.GetComponent<EnemyController>().currentHp = battleController.enemySelected.GetComponent<EnemyController>().currentHp - damageArray[0] * 2;
                }
                else
                {   
                    //When combat animation is on then animate health bar
                    if (PlayerPrefs.GetInt("combatAnim") == 0) {
                        yield return StartCoroutine(AnimateHealthDamage(damageArray[0], battleScreenEnemyHpBar, battleController.enemySelected, battleScreenEnemyHealth));
                    }
                    battleController.enemySelected.GetComponent<EnemyController>().currentHp = battleController.enemySelected.GetComponent<EnemyController>().currentHp - damageArray[0];
                }

            }
            else
            {
                //TODO: Show MISS ui
                Debug.Log("Character Missed!");

            } 
        
            //Play enemy death dialogue if necessary
            if (battleController.enemySelected.GetComponent<EnemyController>().currentHp <= 0)
            {   
                //TODO: Remove sprite on battle screen
                yield return StartCoroutine(DeathSequence(battleController.enemySelected));
            }

            //Enemy attacks character
            else
            {
                //should only attack back if able to
                if ((battleController.enemySelected.GetComponent<EnemyController>().ranged && battleController.characterSelected.GetComponent<PlayerController>().ranged) || (!battleController.enemySelected.GetComponent<EnemyController>().ranged && !battleController.characterSelected.GetComponent<PlayerController>().ranged))
                {
                    if (PlayerPrefs.GetInt("combatAnim", -1) == 0) 
                    {
                        //TODO: Enemy attack animation
                        yield return new WaitForSeconds(3f);
                    }
                    //Determine hit/crit roll for enemy
                    roll = Random.Range(0, 100);
                    if (roll <= enemyDamageArray[1])
                    {
                        Debug.Log("Enemy hits with: " + roll);
                        roll = Random.Range(0, 100);
                        if (roll <= enemyDamageArray[2])
                        {
                            Debug.Log("Enemy crit with: " + roll);
                            yield return StartCoroutine(AnimateHealthDamage(enemyDamageArray[0] * 2, battleScreenPlayerHpBar, battleController.characterSelected, battleScreenPlayerHealth));
                            battleController.characterSelected.GetComponent<PlayerController>().currentHp = battleController.characterSelected.GetComponent<PlayerController>().currentHp - enemyDamageArray[0] * 2; 
                        }
                        else
                        {
                            yield return StartCoroutine(AnimateHealthDamage(enemyDamageArray[0], battleScreenPlayerHpBar, battleController.characterSelected, battleScreenPlayerHealth));
                            battleController.characterSelected.GetComponent<PlayerController>().currentHp = battleController.characterSelected.GetComponent<PlayerController>().currentHp - enemyDamageArray[0]; 

                        }
                    }
                    else
                    {
                        Debug.Log("Enemy Missed!");
                    }
                    
                    //Play death dialogue if necessary
                    if (battleController.characterSelected.GetComponent<PlayerController>().currentHp <= 0)
                    {   
                        yield return StartCoroutine(DeathSequence(battleController.characterSelected));
                        //TODO: Remove sprite on battle screen
                        //yield return StartCoroutine(battleController.characterSelected.GetComponent<PlayerController>().Die());
                    }
                }
            }
        
        
        }
       
        if (PlayerPrefs.GetInt("combatAnim", -1) == 0)
        {
            //Scale Panel from 1 to 0
            float elapsed = 0f;
            float duration = .2f;
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                attackPanel.GetComponent<RectTransform>().localScale = Vector3.Lerp(endScale, startScale, t);

                yield return null;
            }
            attackPanel.GetComponent<RectTransform>().localScale = startScale; // snap to final value


        }

        //Reset Hp bar sizes
        battleScreenPlayerHpBar.GetComponent<RectTransform>().sizeDelta = originalBattleScreenHpBarSize;
        battleScreenEnemyHpBar.GetComponent<RectTransform>().sizeDelta = originalBattleScreenHpBarSize;

        //Reset battlescreen info
        battleScreenPlayerName.text = "-";
        battleScreenPlayerHealth.text = "-";
        battleScreenPlayerAttack.text = "-";
        battleScreenEnemyName.text = "-";
        battleScreenEnemyHealth.text = "-";
        battleScreenEnemyAttack.text = "-";
        battleScreenEnemyATK.text = "-";
        battleScreenEnemyHIT.text = "-";
        battleScreenEnemyCRIT.text = "-";

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
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;
        float elapsed = 0f;
        float duration = .2f;

        enemyDamageArray = calculateDamage(attacker, defender, attackSelected);
        damageArray = calculateDamage(defender, attacker, defenderScript.knownAttacks[0]);
        
        if (PlayerPrefs.GetInt("combatAnim") == 0) 
        {
            //Populate info
            battleScreenTransitionAudio.Play();
            battleScreenPlayerName.text = defenderScript.title;
            battleScreenPlayerHealth.text = defenderScript.currentHp.ToString();
            battleScreenPlayerHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)defenderScript.currentHp / defenderScript.maxHp, 1f);
            battleScreenPlayerAttack.text = defenderScript.knownAttacks[0].name;
            battleScreenEnemyName.text = attackerScript.title; 
            battleScreenEnemyHealth.text = attackerScript.currentHp.ToString();
            battleScreenEnemyHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)attackerScript.currentHp / attackerScript.maxHp, 1f);
            battleScreenEnemyAttack.text = attackSelected.name;
            battleScreenEnemyATK.text = enemyDamageArray[0].ToString();
            battleScreenEnemyHIT.text = enemyDamageArray[1].ToString();
            battleScreenEnemyCRIT.text = enemyDamageArray[2].ToString();
            

            if (attackerScript.ranged) { battleScreenRightRangedImage.SetActive(true); battleScreenRightMeleeImage.SetActive(false);}
            else { battleScreenRightRangedImage.SetActive(false); battleScreenRightMeleeImage.SetActive(true); }
            
            if (defenderScript.ranged) { battleScreenLeftRangedImage.SetActive(true); battleScreenLeftMeleeImage.SetActive(false);}
            else { battleScreenLeftRangedImage.SetActive(false); battleScreenLeftMeleeImage.SetActive(true); }
            

            //Enemy is ranged and character is not, or vice versa
            if ((attackerScript.ranged && !defenderScript.ranged) || (!attackerScript.ranged && defenderScript.ranged) )
            {
                battleScreenPlayerAttack.text = "-";
                battleScreenPlayerATK.text =  "-";
                battleScreenPlayerHIT.text =  "-";
                battleScreenPlayerCRIT.text = "-";
            }
            else
            {
                battleScreenPlayerATK.text = damageArray[0].ToString();
                battleScreenPlayerHIT.text = damageArray[1].ToString();
                battleScreenPlayerCRIT.text = damageArray[2].ToString();  
                battleScreenPlayerAttack.text = defenderScript.knownAttacks[0].name;
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
            //TODO: Play enemy attack animation
            yield return new WaitForSeconds(3f);  
        }

        //Determine hit roll for enemy
        int roll = Random.Range(0, 100);
        if (roll <= enemyDamageArray[1])
        {
            //Determine crit roll for enemy
            roll = Random.Range(0, 100);
            if (roll <= enemyDamageArray[2])
            {

                Debug.Log(attackerScript.title + " crit " + defenderScript.title + " for " + (enemyDamageArray[0] * 2).ToString() + " damage.");
                if (PlayerPrefs.GetInt("combatAnim") == 0) {
                    yield return StartCoroutine(AnimateHealthDamage(enemyDamageArray[0] * 2, battleScreenPlayerHpBar, defender, battleScreenPlayerHealth));
                }
                defenderScript.currentHp = defenderScript.currentHp - enemyDamageArray[0] * 2; 
            }
            
            else
            {
                Debug.Log(attackerScript.title + " hit " + defenderScript.title + " for " + enemyDamageArray[0].ToString() + " damage.");
                if (PlayerPrefs.GetInt("combatAnim") == 0)
                {
                    yield return StartCoroutine(AnimateHealthDamage(enemyDamageArray[0], battleScreenPlayerHpBar, defender, battleScreenPlayerHealth));
                }
                defenderScript.currentHp = defenderScript.currentHp - enemyDamageArray[0]; 
            }

        }
        else
        {
            //TODO: Show MISS ui
            Debug.Log(attackerScript.title + " missed attack on " + defenderScript.title);

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
                //Determine hit roll for character
                roll = Random.Range(0, 100);
                if (roll <= damageArray[1])
                {
                    //Determine crit roll for character
                    roll = Random.Range(0, 100);
                    if (roll <= damageArray[2])
                    {
                        Debug.Log(defenderScript.title + " crit " + attackerScript.title + " for " + (damageArray[0] * 2).ToString() + " damage.");
                        if (PlayerPrefs.GetInt("combatAnim") == 0)
                        {
                            yield return StartCoroutine(AnimateHealthDamage(damageArray[0] * 2, battleScreenEnemyHpBar, attacker, battleScreenEnemyHealth));
                        }
                        attackerScript.currentHp = attackerScript.currentHp - damageArray[0] * 2;
                    }
                    else
                    {
                        Debug.Log(defenderScript.title + " hit " + attackerScript.title + " for " + damageArray[0].ToString() + " damage.");

                        if (PlayerPrefs.GetInt("combatAnim") == 0)
                        {
                            yield return StartCoroutine(AnimateHealthDamage(damageArray[0], battleScreenEnemyHpBar, attacker, battleScreenEnemyHealth));
                        }
                        attackerScript.currentHp = attackerScript.currentHp - damageArray[0];
                    }

                }
                else
                {
                    //TODO: Show MISS ui
                    Debug.Log(defenderScript.title + " missed attack on " + attackerScript.title);
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
    
        if (PlayerPrefs.GetInt("combatAnim") == 0)
        {
            //Scale Panel from 1 to 0
            elapsed = 0f;
            duration = .2f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                attackPanel.GetComponent<RectTransform>().localScale = Vector3.Lerp(endScale, startScale, t);

                yield return null;
            }
            attackPanel.GetComponent<RectTransform>().localScale = startScale; // snap to final value
        }

        //Reset Hp bar sizes
        battleScreenPlayerHpBar.GetComponent<RectTransform>().sizeDelta = originalBattleScreenHpBarSize;
        battleScreenEnemyHpBar.GetComponent<RectTransform>().sizeDelta = originalBattleScreenHpBarSize;

        //Reset battlescreen info
        battleScreenPlayerName.text = "-";
        battleScreenPlayerHealth.text = "-";
        battleScreenPlayerAttack.text = "-";
        battleScreenEnemyName.text = "-";
        battleScreenEnemyHealth.text = "-";
        battleScreenEnemyAttack.text = "-";
        battleScreenEnemyATK.text = "-";
        battleScreenEnemyHIT.text = "-";
        battleScreenEnemyCRIT.text = "-";
        
        coroutineRunning = false;
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
            endSize = originalBattleScreenHpBarSize * new Vector2(temp, 1f);
            startNumber = person.GetComponent<PlayerController>().currentHp;
            targetNumber = person.GetComponent<PlayerController>().currentHp - damage;
        }
        else
        {
            float temp = (float)(person.GetComponent<EnemyController>().currentHp - damage) / person.GetComponent<EnemyController>().maxHp;
            endSize = originalBattleScreenHpBarSize * new Vector2(temp, 1f);
            startNumber = person.GetComponent<EnemyController>().currentHp;
            targetNumber = person.GetComponent<EnemyController>().currentHp - damage;
        }

        if (targetNumber < 0) { targetNumber = 0; }

        // Track the current printed number
        int currentPrintedNumber = startNumber;

        // Determine direction
        int direction = startNumber > targetNumber ? -1 : 1;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // Calculate lerp percentage
            float t = elapsed / duration;

            // Update size
            rect.sizeDelta = Vector2.Lerp(startSize, endSize, t);

            // Calculate the interpolated number based on progress
            float numberLerp = Mathf.Lerp(startNumber, targetNumber, t);

            // Convert to int in correct direction
            int newNumber = direction == -1 
                ? Mathf.FloorToInt(numberLerp)
                : Mathf.CeilToInt(numberLerp);

            // Only print when the number changes
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

