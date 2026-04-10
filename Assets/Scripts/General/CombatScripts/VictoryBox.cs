using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class VictoryBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    private RectTransform rect;
    private Vector2 startPos;
    public Vector2 endPos;
    private Coroutine moveDownCoroutine;
    private Coroutine moveUpCoroutine;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        startPos = rect.anchoredPosition;
        //endPos = new Vector2(235, 291);
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