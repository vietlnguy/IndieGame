using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class GameOverRetryButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    private SaveManager saveManager;
    private AudioSource confirmationAudio;
    void Awake()
    {
        image = GetComponent<Image>();
        saveManager = FindFirstObjectByType<SaveManager>();
        confirmationAudio = GetComponent<AudioSource>();
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
        StartCoroutine(RestartScene());
    }

    private IEnumerator RestartScene()
    {   
        confirmationAudio.Play();
        yield return new WaitForSeconds(.25f);
        yield return StartCoroutine(saveManager.SceneTransition(false));
        SceneManager.LoadScene(saveManager.loadedData.currentChapter);
    }

}
