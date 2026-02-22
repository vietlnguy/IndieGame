using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class UIGeneral : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    public Image blackScreen;
    public GameObject saveMenu;
    public SaveManager scm;
    public GameObject EnterNameUI;
    public TMP_InputField inputBox;
    public GameObject highlightOverlay;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        try {
            highlightOverlay.SetActive(true);
            highlightOverlay.GetComponent<RectTransform>().anchoredPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
        }
        catch
        { 
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {   
        highlightOverlay.SetActive(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        string tag = gameObject.tag;

        if (tag == "continue")
        {
            saveMenu.SetActive(true);
        }
        else if (tag == "new game")
        {
            EnterNameUI.SetActive(true);
        }
        else if (tag == "settings")
        {

        }
        else if (tag == "exit")
        {

        }
        else if (tag == "exit_enter_name")
        {
            inputBox.text = "";
            EnterNameUI.SetActive(false);
        }

    }
    private IEnumerator FadeScreen(Image obj, float duration){
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        float startAlpha = 0f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

}
