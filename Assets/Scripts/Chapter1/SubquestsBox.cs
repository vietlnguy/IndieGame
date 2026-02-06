using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class SubquestsBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    private RectTransform rect;
    private Vector2 startPos;
    private Vector2 endPos;
    public Image quest1Image;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        startPos = rect.anchoredPosition;
        endPos = new Vector2(489, 291);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopCoroutine(MoveDown());
        StopCoroutine(MoveUp());
        StartCoroutine(MoveDown());

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(MoveDown());
        StopCoroutine(MoveUp());
        StartCoroutine(MoveUp());

    }
    public void updateQuest(int questNumber, bool completed)
    {
        if (questNumber == 1)
        {
            if (completed)
            {
                quest1Image.color = new Color(1, 1, 1, 1);
            }
        }
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