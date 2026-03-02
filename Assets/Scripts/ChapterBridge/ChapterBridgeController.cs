using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class ChapterBridgeController : MonoBehaviour
{
    public TextMeshProUGUI chapterTextBox;
    public CanvasGroup chapterTextBoxCanvas;
    public TextMeshProUGUI descriptionTextBox;
    private SaveManager scm;
    public AudioSource backgroundAudio;

    void Awake()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume", 0.5f);
        scm = GameObject.Find("SaveManager").GetComponent<SaveManager>();
    }

    void Start()
    {
        StartCoroutine(Intro());
    }
    private IEnumerator Intro()
    {
        //Play intro
        yield return new WaitForSeconds(2f);
        backgroundAudio.Play();
        chapterTextBox.text = scm.loadedData.currentChapter + ":";
        yield return StartCoroutine(FadeIn(chapterTextBoxCanvas, 1f));
        string temp = GetChapterName();
        yield return StartCoroutine(TypeLine(descriptionTextBox, temp));
        yield return new WaitForSeconds(2f);

        //Transition to next scene
        yield return StartCoroutine(scm.SceneTransition(false));
        if (scm.loadedData.introBattleOutro == "Overworld")
        {
            SceneManager.LoadScene("Overworld");
        }
        else if (scm.loadedData.currentChapter == "Chapter 1")
        {
            SceneManager.LoadScene(scm.loadedData.currentChapter);
        }
        else
        {
            scm.loadedData.introBattleOutro = "Camp";
            scm.OverwriteSave();
            SceneManager.LoadScene("Camp");
        }

    }
    private IEnumerator TypeLine(TextMeshProUGUI textComponent, string s) {

        float textSpeed = .05f;

        foreach (char c in s) {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

    }
    private IEnumerator FadeIn(CanvasGroup group, float duration)
    {       
        float startAlpha = 0f;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(startAlpha, 1f, time / duration);
            yield return null;
        }
        group.alpha = 1f;
        yield return null;
    }
    private string GetChapterName()
    {
        try {
            string s = "error";

            if (scm.loadedData.currentChapter == "Chapter 1")
            {
                s = "An Unexpected Adventure";
            }
            else if (scm.loadedData.currentChapter == "Chapter 2")
            {
                s = "The Blue-Haired Twins";
            }

            return s;
        }

        catch
        {
            return "error";
        }
    }
}