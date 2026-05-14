using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

    private SaveManager saveManager;
    public CanvasGroup gameOverCanvasGroup;
    public GameObject gameOverRetryButton;
    public GameObject gameOverMainMenuButton;
    public AudioSource gameOverAudio;
    public bool active = false;

    public void Awake()
    {
        saveManager = FindAnyObjectByType<SaveManager>();
    }
    public void Update()
    {
    }

    public IEnumerator GameOverSequence()
    {
        active = true;
        yield return new WaitForSeconds(.5f);
        //Fade out all audios
        AudioSource[] sources = FindObjectsByType<AudioSource>();
        foreach (AudioSource source in sources)
        {
            StartCoroutine(FadeOut(source));
        }
        yield return new WaitForSeconds(1.5f);
        gameOverAudio.volume = 0.5f;
        gameOverAudio.Play();

        
        //Fade in the game over screen
        gameOverCanvasGroup.alpha = 0f;
        gameOverCanvasGroup.interactable = true;
        gameOverCanvasGroup.blocksRaycasts = true;
        float elapsed = 0f;
        float time = 1.5f;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / time);
            gameOverCanvasGroup.alpha = t;
            yield return null;
        }
        gameOverCanvasGroup.alpha = 1f;

        yield return new WaitForSeconds(1f);

        gameOverMainMenuButton.SetActive(true);
        gameOverRetryButton.SetActive(true);

    }
    private IEnumerator FadeOut(AudioSource source)
    {
        float startVolume = source.volume;
        float duration = 0.5f;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0, time / duration);
            yield return null;
        }

        source.volume = 0;
        source.Stop();
    }
}