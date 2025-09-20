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
    public bool enabled = false;
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
    public SaveManager scm;
    public GameObject attackSelector;
    private Vector2 attackSelectorInitialPos;
    private int attackIndex = 0;
    public TextMeshProUGUI atkBlock;
    public TextMeshProUGUI hitBlock;
    public TextMeshProUGUI critBlock;
    void Awake()
    {
        scm = FindFirstObjectByType<SaveManager>();
        attackSelectorInitialPos = attackSelector.GetComponent<RectTransform>().anchoredPosition;
    }
    void Start()
    {
    }
    void Update()
    {
        if (enabled)
        {
            handleAttackSelection();
        }

    }
    public void chooseAttack(int index)
    {
        attackIndex = index;
        if (attackIndex == 0)
        {
            attackSelector.GetComponent<RectTransform>().anchoredPosition = attackSelectorInitialPos;
        }
        else if (attackIndex == 1)
        {
            attackSelector.GetComponent<RectTransform>().anchoredPosition = new Vector2(attackSelectorInitialPos.x + 161f, attackSelectorInitialPos.y);
        }
        else if (attackIndex == 2)
        {
            attackSelector.GetComponent<RectTransform>().anchoredPosition = new Vector2(attackSelectorInitialPos.x, attackSelectorInitialPos.y - 37f);
        }
        else if (attackIndex == 3)
        {
            attackSelector.GetComponent<RectTransform>().anchoredPosition = new Vector2(attackSelectorInitialPos.x + 161f, attackSelectorInitialPos.y - 37f);
        }

        calculateDamageBlock();
    }
    public void handleAttackSelection()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (attackIndex > 1)
            {
                attackSelector.GetComponent<RectTransform>().anchoredPosition = new Vector2(attackSelector.GetComponent<RectTransform>().anchoredPosition.x, attackSelector.GetComponent<RectTransform>().anchoredPosition.y + 37f);
                if (attackIndex == 2) { attackIndex = 0; }
                else if (attackIndex == 3) { attackIndex = 1; }
            }

        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (attackIndex <= 1)
            {
                attackSelector.GetComponent<RectTransform>().anchoredPosition = new Vector2(attackSelector.GetComponent<RectTransform>().anchoredPosition.x, attackSelector.GetComponent<RectTransform>().anchoredPosition.y - 37f);
                if (attackIndex == 0) { attackIndex = 2; }
                else if (attackIndex == 1) { attackIndex = 3; }
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (attackIndex == 1 || attackIndex == 3)
            {
                attackSelector.GetComponent<RectTransform>().anchoredPosition = new Vector2(attackSelector.GetComponent<RectTransform>().anchoredPosition.x - 161f, attackSelector.GetComponent<RectTransform>().anchoredPosition.y);
                if (attackIndex == 1) { attackIndex = 0; }
                else if (attackIndex == 3) { attackIndex = 2; }
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (attackIndex == 0 || attackIndex == 2)
            {
                attackSelector.GetComponent<RectTransform>().anchoredPosition = new Vector2(attackSelector.GetComponent<RectTransform>().anchoredPosition.x + 161f, attackSelector.GetComponent<RectTransform>().anchoredPosition.y);
                if (attackIndex == 0) { attackIndex = 1; }
                else if (attackIndex == 2) { attackIndex = 3; }
            }
        }
        calculateDamageBlock();
    }
    public void calculateDamageBlock()
    {
        try
        {
            Attack selectedAttack = battleController.characterSelected.GetComponent<PlayerController>().knownAttacks[attackIndex];
            atkBlock.text = selectedAttack.baseDamage.ToString();
            hitBlock.text = selectedAttack.accuracy.ToString();
            critBlock.text = "10";
        }
        catch
        {
            atkBlock.text = "-";
            hitBlock.text = "-";
            critBlock.text = "-";
        }

    }
    public IEnumerator enablePreview(GameObject enemy)
    {
        //Update titles
        characterTitle.text = battleController.characterSelected.GetComponent<PlayerController>().title;
        enemyTitle.text = enemy.GetComponent<EnemyController>().title;

        //Update moveset
        int index = 0;
        foreach (Transform child in attacks.transform)
        {
            foreach (Transform child2 in child.transform)
            {
                try
                {
                    child2.GetComponent<TextMeshProUGUI>().text = battleController.characterSelected.GetComponent<PlayerController>().knownAttacks[index].name;
                }
                catch
                {

                }
            }
            index++;
        }

        //Update damage stats
        calculateDamageBlock();

        //Update health/mana bars

        //Move UI
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
        GameObject characterPrefab;
        Vector2 temp = new Vector2(portraitBackground.GetComponent<RectTransform>().position.x, portraitBackground.GetComponent<RectTransform>().position.y - 1f);
        if (battleController.characterSelected.GetComponent<PlayerController>().title == scm.loadedData.mainCharacterName) { characterPrefab = Instantiate(mainCharacterPrefab, temp, Quaternion.identity, attackPreviewSprites.transform); }
        else if (battleController.characterSelected.GetComponent<PlayerController>().title == "Astrid") { characterPrefab = Instantiate(astridPrefab, temp, Quaternion.identity, attackPreviewSprites.transform); }
        //TODO: include more prefabs for characters

        GameObject enemyPrefab;
        temp = new Vector2(enemyPortraitBackground.GetComponent<RectTransform>().position.x, enemyPortraitBackground.GetComponent<RectTransform>().position.y - 1f);
        if (enemy.GetComponent<EnemyController>().title == "Soldier") { enemyPrefab = Instantiate(soldierPrefab, temp, Quaternion.identity, attackPreviewSprites.transform); }
        //TODO: include more prefabs for enemies

        //Enable buttons
        confirmButton.SetActive(true);
        vsText.SetActive(true);
        enabled = true;

    }
    public IEnumerator disablePreview()
    {
        //Destroy attack preview prefabs
        foreach (Transform child in attackPreviewSprites.transform)
        {
            Destroy(child.gameObject);
        }
        attackWooshAudio.Play();

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
        //Disable vs and confirm button
        confirmButton.SetActive(false);
        vsText.SetActive(false);
        enabled = false;

    }
    public IEnumerator startAttackSequence()
    {
        //Instantiate player and enemy sprite

        //Populate player name and enemy name

        //Populate player and enemy health

        //Populate player and enemy chosen Attacks

        //Populate player and enemy damage block

        //Enable the Battle screen and scale it from 0 to 1

        yield return null;
    }
}
