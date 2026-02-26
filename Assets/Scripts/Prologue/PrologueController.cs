using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class PrologueController : MonoBehaviour
{

    public AudioSource backgroundAudio;
    public List<string> dialogues;
    public CanvasGroup textBoxGroup;
    public TextMeshProUGUI textBoxText;
    public CanvasGroup backgroundImage;
    public GameObject cameraObject;
    private SaveManager scm;
    private bool isPaused = false;
    public GameObject pauseMenu;
    public GameObject saveMenu;
    public GameObject settingsMenu;
    public GameObject exitGameMenu;
    public GameObject exitMainMenu;
    private bool pressedOnce = false;
    public GameObject skipTextBox;
    private Coroutine coroutineRunning;

    void Awake(){
        AudioListener.volume = PlayerPrefs.GetFloat("volume", 0.5f);
        scm = GameObject.Find("SaveManager").GetComponent<SaveManager>();
    }
    void Start()
    {
        dialogues.Add("Before the age of man...");
        dialogues.Add("... there were only beings of nature.");
        dialogues.Add("Spirits, nymphs, and early mankind living in perfect harmony.");
        dialogues.Add("For a thousand years they grew, sharing knowledge and wisdom.");
        dialogues.Add("... and pleasure.");
        dialogues.Add("Eventually, these beings became indistinguishable from one another, and thus adopted a new name.. ");
        dialogues.Add("The Tah'Lo.");
        dialogues.Add("An extension of nature and technology, the Tah'Lo were masters of the weave.");
        dialogues.Add("Bestowing magic into artifacts and each other alike, the Tah'Lo enjoyed an age of prosperity.");
        dialogues.Add("But dark times loomed ahead...");
        dialogues.Add("For some, it was not enough.");
        dialogues.Add("The lure of power was all it took to topple the mighty civilization.");
        dialogues.Add("Millenia have since passed, and tales of the Tah'Lo have turned into legend.");
        dialogues.Add("But once again the power of the Tah'Lo threaten the stablity of the realm.");
        dialogues.Add("On a small farm in the countryside, an unlikely adventure begins.");

        StartCoroutine(Intro());
    }
    void Update()
    {
        //On ESC press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                pauseMenu.SetActive(false);
                saveMenu.SetActive(false);
                settingsMenu.SetActive(false);
                exitGameMenu.SetActive(false);
                exitMainMenu.SetActive(false);
                isPaused = false;
            }
            else
            {
                pauseMenu.SetActive(true);
                isPaused = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (pressedOnce && coroutineRunning == null)
            {
                coroutineRunning = StartCoroutine(GoNextScene());
            }
            else 
            { 
                pressedOnce = true;
                EnableSkip();
            }
        }
    }
    private IEnumerator Intro() {
        //Fade in Audio
        backgroundAudio.Play();
        StartCoroutine(FadeInAudio(backgroundAudio));

        yield return new WaitForSeconds(2f);
        StartCoroutine(FadeIn(backgroundImage, 2f));
        StartCoroutine(PanCamera(cameraObject.transform));

        yield return new WaitForSeconds(1f);

        foreach (string s in dialogues)
        {
            textBoxText.text = s;
            yield return FadeIn(textBoxGroup, 1.5f);
            yield return new WaitForSeconds(2f);
            yield return FadeOut(textBoxGroup, 1.5f);
            yield return new WaitForSeconds(0.5f);
        }

        yield return StartCoroutine(scm.SceneTransition());
        SceneManager.LoadScene("ChapterBridge");

        yield return null;
    }
    private IEnumerator FadeOut(CanvasGroup group, float duration)
    {       
        float startAlpha = 1f;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(startAlpha, 0f, time / duration);
            yield return null;
        }
        group.alpha = 0f;
        yield return null;
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
    private IEnumerator FadeInAudio(AudioSource source)
    {
        float duration = 0.5f;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(0f, 0.35f, time / duration);
            yield return null;
        }

        source.volume = 0.35f;

    }
    private IEnumerator FadeOutAudio(AudioSource source)
    {
        float startVolume = source.volume;
        float duration = 1.5f;
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
    private IEnumerator PanCamera(Transform transform)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(0, -146f, -10f);
        float duration = 88f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
    }
    private void EnableSkip()
    {
        skipTextBox.SetActive(true);
    }
    private IEnumerator GoNextScene()
    {
        yield return StartCoroutine(scm.SceneTransition());
        SceneManager.LoadScene("ChapterBridge");
    }
}