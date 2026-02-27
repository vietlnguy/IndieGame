using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class VictorySequence : MonoBehaviour
{
    public AudioSource victoryAudio;
    public AudioSource xAudio;
    public AudioSource checkAudio0;
    public AudioSource checkAudio1;
    public AudioSource checkAudio2;
    private List<AudioSource> checkAudios;
    public List<Subquest> subquests;
    public GameObject victoryText;
    public GameObject subquestsBox;
    public GameObject subquestsBoxContent;
    public GameObject subquestEntryPrefab;
    public GameObject retryButton;
    public GameObject continueButton;
    
    void Awake()
    {
        checkAudios = new List<AudioSource>();
        checkAudios.Add(checkAudio0);
        checkAudios.Add(checkAudio1);
        checkAudios.Add(checkAudio2);
        disableHoverable();
    }
    public IEnumerator Victory()
    {   
        //Fade out background music
        StartCoroutine(Helpers.FadeOutAudio(GameObject.Find("CombatBackgroundAudio").GetComponent<AudioSource>(), 0.5f));
        
        //Show victory
        victoryText.SetActive(true);
        victoryAudio.Play();
        yield return new WaitForSeconds(2.5f);

        //Move Victory text up
        StartCoroutine(MoveBox());

        //Populate subquests content
        foreach (Subquest subquest in subquests)
        {
            //Set description
            GameObject temp = Instantiate(subquestEntryPrefab, subquestsBoxContent.transform, false);
            temp.GetComponent<TextMeshProUGUI>().text = subquest.description;

            //Set quest number
            Transform temp2 = temp.transform.Find("Number");
            temp2.gameObject.GetComponent<TextMeshProUGUI>().text = (subquest.questNumber + 1).ToString() + ".";
        }

        //Scale up subquests box
        StartCoroutine(ScaleIn());

        yield return new WaitForSeconds(2f);

        //Show check or X for each Subquest
        int index = 0;
        foreach (Transform child in subquestsBoxContent.transform)
        {
            //Failed
            if (!subquests[index].completed || subquests[index].failed)
            {
                child.transform.Find("X").GetComponent<Image>().enabled = true;
                xAudio.Play();
            }

            //Succeeded
            else if (subquests[index].completed && !subquests[index].failed)
            {
                child.transform.Find("Check").GetComponent<Image>().enabled = true;
                checkAudios[index].Play();
            }

            yield return new WaitForSeconds(.5f);
            index++;
        }

        yield return new WaitForSeconds(1f);
        //Show retry and continue buttons
        retryButton.SetActive(true);
        continueButton.SetActive(true);

    }
    private IEnumerator MoveBox()
    {
        RectTransform rectTransform = victoryText.GetComponent<RectTransform>();
        Vector2 startPos = rectTransform.anchoredPosition;
        Vector2 targetPosition = new Vector2(0f, 135f);
        float elapsed = 0f;
        float duration = 0.5f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPosition, t);

            yield return null;
        }
        rectTransform.anchoredPosition = targetPosition;
  
    }
    private IEnumerator ScaleIn()
    {
        RectTransform rectTransform = subquestsBox.GetComponent<RectTransform>();
        
        float elapsed = 0f;
        float duration = 0.5f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);

            yield return null;
        }

        rectTransform.localScale = Vector3.one;
    }
    private void disableHoverable()
    {
        GameObject characters = GameObject.Find("Characters");
        foreach (Transform child in characters.transform)
        {
            child.GetComponent<PlayerController>().hoverable = false;
        }

        GameObject enemies = GameObject.Find("Enemies");
        foreach (Transform child in enemies.transform)
        {
            child.GetComponent<EnemyController>().hoverable = false;
        }
    }
    private void enableHoverable()
    {
        GameObject characters = GameObject.Find("Characters");
        foreach (Transform child in characters.transform)
        {
            child.GetComponent<PlayerController>().hoverable = true;
        }

        GameObject enemies = GameObject.Find("Enemies");
        foreach (Transform child in enemies.transform)
        {
            child.GetComponent<EnemyController>().hoverable = true;
        }
    }
}