using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class ConfirmMainMenuGameButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    private SaveManager scm;

    void Awake()
    {
        image = GetComponent<Image>();
        scm = GameObject.Find("SaveManager").GetComponent<SaveManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = new Color(.65f, .65f, .65f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = new Color(1f, 1f, 1f);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        image.color = new Color(1f, 1f, 1f);
        StartCoroutine(Helper());
    }

    private IEnumerator Helper()
    {
        yield return StartCoroutine(scm.SceneTransition(false));
        SceneManager.LoadScene("MainMenu");
    }

}
