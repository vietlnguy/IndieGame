using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SubquestsBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    private RectTransform rect;
    private Vector2 startPos;
    private Vector2 endPos;
    public Image quest1ImageCheck;
    public Image quest1ImageX;
    Coroutine moveDownCoroutine;
    Coroutine moveUpCoroutine;
    public List<Subquest> subquests;
    public VictorySequence victorySequenceScript;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        startPos = rect.anchoredPosition;
        endPos = new Vector2(489, 291);

    }
    void Start()
    {   
        //Remember: Everytime I add a subquest here, I should add an exact copy subquest to PlayerController.
        subquests = new List<Subquest>();
        subquests.Add(new Subquest("Astrid1", "Astrid lands the killing blow on the boss.", "Discuss the power of the bracelets."));
        victorySequenceScript.subquests = subquests;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (moveUpCoroutine != null) {
            StopCoroutine(moveUpCoroutine);
        }
        moveDownCoroutine = StartCoroutine(MoveDown());

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (moveUpCoroutine != null) {
            StopCoroutine(moveUpCoroutine);
        }
        moveUpCoroutine = StartCoroutine(MoveUp());

    }
    public void updateQuest(string identifier, bool completed, GameObject characterToUpdate)
    {
        //Update this script's subquests
        foreach (Subquest s in subquests)
        {
            if (s.sceneToLoad == identifier)
            {
                if (completed)
                {
                    s.completed = true;
                    quest1ImageCheck.color = new Color(1, 1, 1, 1);
                }
                else
                {
                    s.failed = true;
                    quest1ImageX.color = new Color(1, 1, 1, 1);
                }
            }
        }

        //Update the characters subquests
        foreach (Subquest s in characterToUpdate.GetComponent<PlayerController>().subquests)
        {
            if (s.sceneToLoad == identifier)
            {
                if (completed)
                {
                    s.completed = true;
                }
                else
                {
                    s.failed = true;
                }
            }
        }

        victorySequenceScript.subquests = subquests;

    }
    private IEnumerator MoveDown()
    {
        Vector2 currentPos = rect.anchoredPosition;
        float duration = .5f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            rect.anchoredPosition = Vector2.Lerp(currentPos, endPos, t / duration);
            yield return null;
        }

        rect.anchoredPosition = endPos;
    }
    private IEnumerator MoveUp()
    {
        Vector2 currentPos = rect.anchoredPosition;
        float duration = .5f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            rect.anchoredPosition = Vector2.Lerp(currentPos, startPos, t / duration);
            yield return null;
        }

        rect.anchoredPosition = startPos;
    }

}