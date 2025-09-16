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
    void Awake()
    {
        scm = FindFirstObjectByType<SaveManager>();
    }

    void Start()
    {
    }

    void Update()
    {
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
                    Debug.Log("error or empty");
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
        if (battleController.characterSelected.GetComponent<PlayerController>().title == scm.loadedData.mainCharacterName) { characterPrefab = Instantiate(mainCharacterPrefab, portraitBackground.transform.position, Quaternion.identity, attackPreviewSprites.transform); }
        else if (battleController.characterSelected.GetComponent<PlayerController>().title == "Astrid") { characterPrefab = Instantiate(astridPrefab, portraitBackground.transform.position, Quaternion.identity, attackPreviewSprites.transform); }
        //TODO: include more prefabs for characters

        GameObject enemyPrefab;
        if (enemy.GetComponent<EnemyController>().title == "Soldier") { enemyPrefab = Instantiate(soldierPrefab, enemyPortraitBackground.transform.position, Quaternion.identity, attackPreviewSprites.transform); }
        //TODO: include more prefabs for enemies

        //Enable buttons
        confirmButton.SetActive(true);
        vsText.SetActive(true);
        enabled = true;
        
    }
    public IEnumerator disablePreview()
    {
        foreach (Transform child in attackPreviewSprites.transform)
        {
            Destroy(child.gameObject);
        }
        attackWooshAudio.Play();
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

        confirmButton.SetActive(false);
        vsText.SetActive(false);
        enabled = false;

    }

}
