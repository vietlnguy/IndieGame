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
    private bool selectorXPos = true;
    private bool selectorYPos = true;
    public GameObject attackSelector;
    private Vector2 attackSelectorInitialPos;
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
            if (Input.GetKeyDown(KeyCode.W)){
                if (!selectorYPos)
                {
                    attackSelector.GetComponent<RectTransform>().anchoredPosition = new Vector2(attackSelector.GetComponent<RectTransform>().anchoredPosition.x, attackSelector.GetComponent<RectTransform>().anchoredPosition.y + 37f);
                    selectorYPos = true;
                } 
            }
            if (Input.GetKeyDown(KeyCode.S)){
                if (selectorYPos)
                {
                    attackSelector.GetComponent<RectTransform>().anchoredPosition = new Vector2(attackSelector.GetComponent<RectTransform>().anchoredPosition.x, attackSelector.GetComponent<RectTransform>().anchoredPosition.y - 37f);
                    selectorYPos = false;
                } 
            }
            if (Input.GetKeyDown(KeyCode.A)){
                if (!selectorXPos)
                {
                    attackSelector.GetComponent<RectTransform>().anchoredPosition = new Vector2(attackSelector.GetComponent<RectTransform>().anchoredPosition.x - 161f, attackSelector.GetComponent<RectTransform>().anchoredPosition.y);
                    selectorXPos = true;
                } 
            }
            if (Input.GetKeyDown(KeyCode.D)){
                if (selectorXPos)
                {
                    attackSelector.GetComponent<RectTransform>().anchoredPosition = new Vector2(attackSelector.GetComponent<RectTransform>().anchoredPosition.x + 161f, attackSelector.GetComponent<RectTransform>().anchoredPosition.y);
                    selectorXPos = false;
                } 
            }
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
        if (battleController.characterSelected.GetComponent<PlayerController>().title == scm.loadedData.mainCharacterName) { characterPrefab = Instantiate(mainCharacterPrefab, portraitBackground.GetComponent<RectTransform>().anchoredPosition, Quaternion.identity, attackPreviewSprites.transform); }
        else if (battleController.characterSelected.GetComponent<PlayerController>().title == "Astrid") { characterPrefab = Instantiate(astridPrefab, portraitBackground.GetComponent<RectTransform>().anchoredPosition, Quaternion.identity, attackPreviewSprites.transform); }
        //TODO: include more prefabs for characters

        GameObject enemyPrefab;
        if (enemy.GetComponent<EnemyController>().title == "Soldier") { enemyPrefab = Instantiate(soldierPrefab, enemyPortraitBackground.GetComponent<RectTransform>().anchoredPosition, Quaternion.identity, attackPreviewSprites.transform); }
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
        selectorXPos = true;
        selectorYPos = true;

        //Disable vs and confirm button
        confirmButton.SetActive(false);
        vsText.SetActive(false);
        enabled = false;

    }

}
