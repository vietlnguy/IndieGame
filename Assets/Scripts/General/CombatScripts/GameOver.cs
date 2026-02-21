using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

    public SaveManager saveManager;
    public CanvasGroup gameOverCanvasGroup;
    public GameObject gameOverRetryButton;
    public GameObject gameOverMainMenuButton;
    public AudioSource gameOverAudio;
    public AudioSource backgroundAudio;
    public AudioSource backgroundMusicAudio;
    public bool active = false;

    public void Awake()
    {
        saveManager = FindFirstObjectByType<SaveManager>();
    }
    public void Update()
    {
    }

    public IEnumerator GameOverSequence()
    {
        active = true;
        yield return new WaitForSeconds(.5f);
        gameOverAudio.Play();
        backgroundAudio.Stop();
        backgroundMusicAudio.Stop();
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
}