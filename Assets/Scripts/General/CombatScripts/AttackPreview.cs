using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class AttackPreview : MonoBehaviour
{
    public BattleController battleController;
    public GameObject attackerPreviewPanel;
    public GameObject defenderPreviewPanel;
    public GameObject confirmButton;
    public GameObject vsText;
    public bool active = false;
    public AudioSource attackWooshAudio;
    public AudioSource attackClangAudio;
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
    public SaveManager scm;
    public GameObject attackSelector;
    private Vector2 attackSelectorInitialPos;
    private int attackIndex = 0;
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
    public GameObject battleScreen;
    public TextMeshProUGUI battleScreenPlayerName;
    public TextMeshProUGUI battleScreenEnemyName;
    public TextMeshProUGUI battleScreenPlayerAttack;
    public TextMeshProUGUI battleScreenEnemyAttack;
    private AttackMoves chosenAttack;
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
    public AudioSource selectBeepAudio;
    private Vector2 originalHpManaBarSize;
    private Vector2 originalEnemyHpManaBarSize;
    private Vector2 originalBattleScreenHpBarSize;
    public AudioSource battleScreenTransitionAudio;
    private int totalDamage = 0;
    private int totalAccuracy = 0;
    private int  totalCrit = 0;
    private int enemyTotalDamage = 0;
    private int enemyTotalAccuracy = 0;
    private int enemyTotalCrit = 0;
    public bool isAssisting = false;
    void Awake()
    {
        scm = FindFirstObjectByType<SaveManager>();
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
            try {
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
        }
        try 
        {
            chosenAttack = battleController.characterSelected.GetComponent<PlayerController>().knownAttacks[attackIndex];
        }
        catch
        {
            chosenAttack = null;
        }

        calculateDamageBlock();
    }
    public void calculateDamageBlock()
    {
        if (chosenAttack != null)
        {
            if (!isAssisting && chosenAttack is Attack attackMove)
            {
                float damage = -1;
                float accuracy = -1;
                float critChance= -1;
                EnemyController enemy = battleController.enemySelected.GetComponent<EnemyController>();
                PlayerController attacker = battleController.characterSelected.GetComponent<PlayerController>();

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
                        damage = ((attacker.attack * attackMove.attackMult) * (attacker.attack * attackMove.attackMult)) / ((attacker.attack * attackMove.attackMult) + enemy.defense);
                    }
                    else if (attackMove.damageType == "magical")
                    {
                        damage = ((attacker.intelligence * attackMove.intMult) * (attacker.intelligence * attackMove.intMult)) / ((attacker.intelligence * attackMove.intMult) + enemy.resistance);
                    }

                    accuracy = attackMove.baseAccuracy + (attacker.skill - enemy.speed) * 2;
                    critChance = attackMove.baseCrit + (attacker.skill * 0.5f);
                }

                totalDamage = (int)Mathf.Floor(damage);
                totalAccuracy = (int)Mathf.Floor(accuracy);
                totalCrit = (int)Mathf.Floor(critChance);

                //Update text visuals
                atkBlock.text = totalDamage.ToString();
                hitBlock.text = totalAccuracy.ToString();
                critBlock.text = totalCrit.ToString();
                manaBlock.text = chosenAttack.manaCost.ToString();
                attackDescription.text = chosenAttack.description;
                confirmAttackButtonScript.validAttack = true;

            }
            else if (isAssisting && chosenAttack is Attack attackMove2)
            {
                atkBlock.text = "-";
                hitBlock.text = "-";
                critBlock.text = "-";
                manaBlock.text = "-";
                attackDescription.text = attackMove2.description;
                confirmAttackButtonScript.validAttack = false;
            }
            else if (!isAssisting && chosenAttack is SupportMove supportMove)
            {
                //Update text visuals
                atkBlock.text = "-";
                hitBlock.text = "-";
                critBlock.text = "-";
                manaBlock.text = chosenAttack.manaCost.ToString();
                attackDescription.text = chosenAttack.description;
                confirmAttackButtonScript.validAttack = false;  
            }
            else if (isAssisting && chosenAttack is SupportMove supportMove2)
            {
                float restorationAmount = 0;

                PlayerController attacker = battleController.characterSelected.GetComponent<PlayerController>();

                //TODO: Special case scenarios like Shield bash etc
                if (supportMove2.name == "Shield Bash")
                {
                }

                //Standard calculation
                else
                {
                    if (supportMove2.restorationAmount > 0)
                    {
                        //Calculate total restoration
                    }

                }

                //Update text visuals
                atkBlock.text = restorationAmount.ToString();
                hitBlock.text = "100";
                critBlock.text = "0";
                manaBlock.text = chosenAttack.manaCost.ToString();
                attackDescription.text = chosenAttack.description;
                confirmAttackButtonScript.validAttack = true;

            }
        }

        else 
        {
            atkBlock.text = "-";
            hitBlock.text = "-";
            critBlock.text = "-";
            manaBlock.text = "-";
            attackDescription.text = "-";
            confirmAttackButtonScript.validAttack = false;
        }

    }
    public void calculateEnemyDamageBlock()
    {
        float damage = -1;
        float accuracy = -1;
        float critChance= -1;

        //character is ranged, but enemy is not ranged
        if (!battleController.enemySelected.GetComponent<EnemyController>().ranged && battleController.characterSelected.GetComponent<PlayerController>().ranged)
        {
            rightsideAtkBlock.text = "-";
            rightsideHitBlock.text = "-";
            rightsideCritBlock.text = "-";
        }
        else if (battleController.enemySelected.GetComponent<EnemyController>().knownAttacks[0] is SupportMove)
        {
            rightsideAtkBlock.text = "-";
            rightsideHitBlock.text = "-";
            rightsideCritBlock.text = "-";
        }
        else if (battleController.enemySelected.GetComponent<EnemyController>().knownAttacks[0] is Attack attackMove)
        {
            EnemyController attacker = battleController.enemySelected.GetComponent<EnemyController>();
            PlayerController enemy = battleController.characterSelected.GetComponent<PlayerController>();

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
                    damage = ((attacker.attack * attackMove.attackMult) * (attacker.attack * attackMove.attackMult)) / ((attacker.attack * attackMove.attackMult) + enemy.defense);
                }
                else if (attackMove.damageType == "magical")
                {
                    damage = ((attacker.intelligence * attackMove.intMult) * (attacker.intelligence * attackMove.intMult)) / ((attacker.intelligence * attackMove.intMult) + enemy.resistance);
                }   

                accuracy = attackMove.baseAccuracy + (attacker.skill - enemy.speed) * 2;
                critChance = attackMove.baseCrit + (attacker.skill * 0.5f);
            }

            enemyTotalDamage = (int)Mathf.Floor(damage);
            enemyTotalAccuracy = (int)Mathf.Floor(accuracy);
            enemyTotalCrit = (int)Mathf.Floor(critChance);

            //Update text visuals
            rightsideAtkBlock.text = enemyTotalDamage.ToString();
            rightsideHitBlock.text = enemyTotalAccuracy.ToString();
            rightsideCritBlock.text = enemyTotalCrit.ToString();
            rightsideManaBlock.text = attackMove.manaCost.ToString();

        }

        rightsideAttackDescription.text = battleController.enemySelected.GetComponent<EnemyController>().knownAttacks[0].description;
        
    }
    public IEnumerator enablePreview(bool isAssisting)
    {
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

        }

        //Update leftside health bars and values
        previewPlayerHp.text = battleController.characterSelected.GetComponent<PlayerController>().currentHp.ToString();
        previewPlayerMaxHp.text = battleController.characterSelected.GetComponent<PlayerController>().maxHp.ToString();
        previewPlayerMana.text = battleController.characterSelected.GetComponent<PlayerController>().currentMana.ToString();
        previewPlayerMaxMana.text = battleController.characterSelected.GetComponent<PlayerController>().maxMana.ToString();
        previewPlayerHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.characterSelected.GetComponent<PlayerController>().currentHp / battleController.characterSelected.GetComponent<PlayerController>().maxHp, 1f);
        previewPlayerManaBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.characterSelected.GetComponent<PlayerController>().currentMana / battleController.characterSelected.GetComponent<PlayerController>().maxMana, 1f);


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
        calculateDamageBlock();
        if (!isAssisting) { calculateEnemyDamageBlock(); }
        else 
        {
            rightsideAtkBlock.text =  "-";
            rightsideHitBlock.text =  "-";
            rightsideCritBlock.text = "-";
            rightsideManaBlock.text = "-";
            rightsideAttackDescription.text = battleController.assistableCharacterSelected.GetComponent<PlayerController>().knownAttacks[0].description;

        }

        //Move UI to visible area
        attackClangAudio.Play();
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
        vsText.SetActive(true);
        active = true;

    }
    public IEnumerator disablePreview()
    {
        isAssisting = false;
        attackWooshAudio.Play();
        //Destroy attack preview prefabs
        foreach (Transform child in attackPreviewSprites.transform)
        {
            Destroy(child.gameObject);
        }

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
        vsText.SetActive(false);
        active = false;

        battleController.characterSelected.GetComponent<PlayerController>().movementEnabled = true;
        yield return null;

    }
    public IEnumerator startAttackSequence()
    {
        //Populate info
        battleScreen.SetActive(true);
        battleScreenTransitionAudio.Play();
        battleScreenPlayerName.text = battleController.characterSelected.GetComponent<PlayerController>().title;
        battleScreenPlayerHealth.text = battleController.characterSelected.GetComponent<PlayerController>().currentHp.ToString();
        battleScreenPlayerHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.characterSelected.GetComponent<PlayerController>().currentHp / battleController.characterSelected.GetComponent<PlayerController>().maxHp, 1f);
        battleScreenPlayerAttack.text = chosenAttack.name;
        battleScreenPlayerATK.text = totalDamage.ToString();
        battleScreenPlayerHIT.text = totalAccuracy.ToString();
        battleScreenPlayerCRIT.text = totalCrit.ToString();
        if (isAssisting)
        {
            battleScreenEnemyName.text = battleController.assistableCharacterSelected.GetComponent<PlayerController>().title;
            battleScreenEnemyHealth.text = battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentHp.ToString();
            battleScreenEnemyHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.assistableCharacterSelected.GetComponent<PlayerController>().currentHp / battleController.assistableCharacterSelected.GetComponent<PlayerController>().maxHp, 1f);
           
        }
        else
        {
            battleScreenEnemyName.text = battleController.enemySelected.GetComponent<EnemyController>().title; 
            battleScreenEnemyHealth.text = battleController.enemySelected.GetComponent<EnemyController>().currentHp.ToString();
            battleScreenEnemyHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.enemySelected.GetComponent<EnemyController>().currentHp / battleController.enemySelected.GetComponent<EnemyController>().maxHp, 1f);
            
            if (!battleController.enemySelected.GetComponent<EnemyController>().ranged && battleController.characterSelected.GetComponent<PlayerController>().ranged )
            {
                battleScreenEnemyAttack.text = "-";
                battleScreenEnemyATK.text =  "-";
                battleScreenEnemyHIT.text =  "-";
                battleScreenEnemyCRIT.text = "-";
            }
            else
            {
                battleScreenEnemyAttack.text = battleController.enemySelected.GetComponent<EnemyController>().knownAttacks[0].name;
                battleScreenEnemyATK.text =  enemyTotalDamage.ToString();
                battleScreenEnemyHIT.text =  enemyTotalAccuracy.ToString();
                battleScreenEnemyCRIT.text = enemyTotalCrit.ToString();
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

        //Determine hit/crit roll for character
        int roll = Random.Range(0, 100);
        if (roll <= totalAccuracy)
        {
            Debug.Log("Character hits with: " + roll);
            roll = Random.Range(0, 100);
            if (roll <= totalCrit)
            {
                Debug.Log("Character crit with: " + roll);
                yield return StartCoroutine(AnimateHealthDamage(totalDamage * 2, battleScreenEnemyHpBar, battleController.enemySelected, battleScreenEnemyHealth));
                battleController.enemySelected.GetComponent<EnemyController>().currentHp = battleController.enemySelected.GetComponent<EnemyController>().currentHp - totalDamage * 2;
            }
            else
            {
                yield return StartCoroutine(AnimateHealthDamage(totalDamage, battleScreenEnemyHpBar, battleController.enemySelected, battleScreenEnemyHealth));
                battleController.enemySelected.GetComponent<EnemyController>().currentHp = battleController.enemySelected.GetComponent<EnemyController>().currentHp - totalDamage;
            }

        }
        else
        {
            Debug.Log("Character Missed!");
        }

        //TODO: Play character attack animation
        yield return new WaitForSeconds(3f);

        //Determine hit/crit for enemy
        if (((battleController.enemySelected.GetComponent<EnemyController>().ranged && battleController.characterSelected.GetComponent<PlayerController>().ranged) || (!battleController.enemySelected.GetComponent<EnemyController>().ranged && !battleController.characterSelected.GetComponent<PlayerController>().ranged)) && !isAssisting)
        {
            //Determine hit/crit roll for enemy
            roll = Random.Range(0, 100);
            if (roll <= enemyTotalAccuracy)
            {
                Debug.Log("Enemy hits with: " + roll);
                roll = Random.Range(0, 100);
                if (roll <= enemyTotalCrit)
                {
                    Debug.Log("Enemy crit with: " + roll);
                    yield return StartCoroutine(AnimateHealthDamage(enemyTotalDamage * 2, battleScreenPlayerHpBar, battleController.characterSelected, battleScreenPlayerHealth));
                    battleController.characterSelected.GetComponent<PlayerController>().currentHp = battleController.characterSelected.GetComponent<PlayerController>().currentHp - enemyTotalDamage * 2; 
                }
                else
                {
                    yield return StartCoroutine(AnimateHealthDamage(enemyTotalDamage, battleScreenPlayerHpBar, battleController.characterSelected, battleScreenPlayerHealth));
                    battleController.characterSelected.GetComponent<PlayerController>().currentHp = battleController.characterSelected.GetComponent<PlayerController>().currentHp - enemyTotalDamage; 

                }
            }
            else
            {
                Debug.Log("Enemy Missed!");
            }
        }
        
        yield return new WaitForSeconds(3f);

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

        //Reset Hp bar sizes
        battleScreenPlayerHpBar.GetComponent<RectTransform>().sizeDelta = originalBattleScreenHpBarSize;
        battleScreenEnemyHpBar.GetComponent<RectTransform>().sizeDelta = originalBattleScreenHpBarSize;

        yield return StartCoroutine(disablePreview());

        chosenAttack = null;

        battleController.characterSelected.GetComponent<PlayerController>().endTurn();
        yield return null;
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
}

