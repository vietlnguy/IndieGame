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
        subquests = new List<Subquest>();
        subquests.Add(new Subquest("Astrid lands the killing blow on the boss.", 0));
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
    public void updateQuest(int questNumber, bool completed)
    {
        //Astrid lands the killing blow on the boss
        if (questNumber == 0)
        {
            if (completed)
            {
                subquests[questNumber].completed = true;
                quest1ImageCheck.color = new Color(1, 1, 1, 1);
            }
            else
            {
                subquests[questNumber].failed = true;
                quest1ImageX.color = new Color(1, 1, 1, 1);
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