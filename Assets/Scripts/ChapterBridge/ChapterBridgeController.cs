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
        yield return new WaitForSeconds(2f);
        
        backgroundAudio.Play();
        string temp = GetAndUpdateChapter();
        chapterTextBox.text = "Chapter " + temp + ":";
        yield return StartCoroutine(FadeIn(chapterTextBoxCanvas, 1f));

        temp = GetChapterName();
        yield return StartCoroutine(TypeLine(descriptionTextBox, temp));

        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(scm.SceneTransition());
        SceneManager.LoadScene(scm.loadedData.currentChapter);

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
    private string GetAndUpdateChapter()
    {
        try {
            int returnInt = int.Parse(scm.loadedData.currentChapter);
            returnInt++;
            scm.loadedData.currentChapter = "Chapter" + returnInt.ToString();
            return returnInt.ToString();
        }
        catch
        {
            //currentChapter is prologue so return chapter 1;
            scm.loadedData.currentChapter = "Chapter1";
            return "1";
        }
    }
    private string GetChapterName()
    {
        try {
            string s = "error";

            if (scm.loadedData.currentChapter == "Chapter1")
            {
                s = "An Unexpected Adventure";
            }

            return s;
        }

        catch
        {
            return "error";
        }
    }
}