using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class GameOverRetryButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    public SaveManager saveManager;
    void Awake()
    {
        image = GetComponent<Image>();
        saveManager = FindFirstObjectByType<SaveManager>();
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
        StartCoroutine(RestartScene());
    }

    private IEnumerator RestartScene()
    {
        yield return StartCoroutine(saveManager.SceneTransition());
        SceneManager.LoadScene(saveManager.loadedData.currentChapter);
    }

}
