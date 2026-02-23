using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    public CanvasGroup creditsCanvasGroup;
    public GameObject creditsCanvas;
    public GameObject mainMenuCanvas;
    public AudioSource mainMenuThemeAudio;
    public GameObject mainMenuParentGroup;

    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume", 0.5f);
        StartCoroutine(GameIntro());
    }

    void Update()
    {
        
    }

    private IEnumerator GameIntro()
    {
        yield return new WaitForSeconds(1.5f);

        //Fade in and fade out intro
        float startAlpha = 0f;
        float time = 0f;
        float duration = 2f;
        while (time < duration)
        {
            time += Time.deltaTime;
            creditsCanvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, time / duration);
            yield return null;
        }
        creditsCanvasGroup.alpha = 1f;
        yield return new WaitForSeconds(2f);
        startAlpha = 1f;
        time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            creditsCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, time / duration);
            yield return null;
        }
        creditsCanvasGroup.alpha = 0f;

        //Fade in main menu
        mainMenuCanvas.SetActive(true);
        creditsCanvas.SetActive(false);
        CanvasGroup mainMenuCanvasGroup = mainMenuParentGroup.GetComponent<CanvasGroup>();
        RectTransform mainMenuCanvasRect = mainMenuParentGroup.GetComponent<RectTransform>();
        Vector2 targetPos = new Vector2(0f, 0f);
        Vector2 startPos = new Vector2(0f, -10f);
        mainMenuThemeAudio.Play();
        startAlpha = 0f;
        time = 0f;
        duration = 1f;
        while (time < duration)
        {
            time += Time.deltaTime;
            mainMenuCanvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, time / duration);
            mainMenuCanvasRect.anchoredPosition = Vector2.Lerp(startPos, targetPos, time);
            yield return null;
        }
        mainMenuCanvasGroup.alpha = 1f;

    }

}
