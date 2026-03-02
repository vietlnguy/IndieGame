using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class OverworldController : MonoBehaviour
{
    private SaveManager scm;
    public Image blackScreen;
    public GameObject cameraObj;
    public TextMeshProUGUI textBox;
    public CanvasGroup textBoxCanvasGroup;
    private List<Dialogue> dialogues;
    public AudioSource chapter2BackgroundAudio;
    public Image seledoBackground;
    public Image cilyBackground;
    public Image brunthBackground;
    public Image everdellBackground;
    public GameObject mainCharacter;
    public GameObject pauseMenu;
    public GameObject saveMenu;
    public GameObject settingsMenu;
    public GameObject exitGameMenu;
    public GameObject exitMainMenu;
    private bool isPaused = false;
    void Awake()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume", 0.5f);
        scm = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        dialogues = new List<Dialogue>();
    }

    void Start()
    {
        if (scm.loadedData.currentChapter == "Chapter 2")
        {
            dialogues.Add(new Dialogue(0, new string[] {"The continent of Fessa.", "Long after the disappearance of the Tah'Lo, modern-day man claimed the land.", "Divided into states and appointed a Lord, each state operates independently.", "With their only obligation being loyalty to Reiss, King of Fessa."}));
            dialogues.Add(new Dialogue(1, new string[] {"Seledo, the Sand State.", "Desolate and remote, treasure hunters scour the land in search of riches.", "Ruled by Lady Nilah, nicknamed The Oasis Queen."}));
            dialogues.Add(new Dialogue(2, new string[] {"Cily, the Merchant State.", "Bustling with trade, you can find all manner of luxury and sin.", "Overseen by Lord Featherington."}));
            dialogues.Add(new Dialogue(3, new string[] {"Brunth, the Mountain State.", "Forged in the mines, these hardy people value strength above all.", "Led by the stoic, Lord Gareth."}));
            dialogues.Add(new Dialogue(4, new string[] {"Everdell, the Farmland State.", "Lush with forests and greenery, the land is rife with tranquility.", "Ruled by Lord Beesly, the Kind."}));
            dialogues.Add(new Dialogue(5, new string[] {"After trouble with the Royal Guard, our hero heads to the state capital of Everdell for answers..."}));
            
            StartCoroutine("Chapter2");
        }
    }

    void Update()
    {
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
    }

    private IEnumerator Chapter2()
    {
        StartCoroutine(Helpers.FadeInAudio(chapter2BackgroundAudio, 1f));
        yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));
        StartCoroutine(Helpers.MoveTransform(cameraObj.transform, cameraObj.transform.position, new Vector3(0f, 7f, -10f), 5f));

        foreach (Dialogue dialogue in dialogues)
        {
            for (int index = 0; index < dialogue.lines.Length; index++)
            {
                textBox.text = dialogue.lines[index];
                yield return StartCoroutine(Helpers.FadeInCanvasGroup(textBoxCanvasGroup, 1.5f));
                yield return new WaitForSeconds(1.5f);
                yield return StartCoroutine(Helpers.FadeOutCanvasGroup(textBoxCanvasGroup, 1.5f));

            }

            //Events happen after the dialogue
            if (dialogue.eventNumber == 0)
            {
                yield return StartCoroutine(Helpers.FadeInImageAlpha(seledoBackground, .25f, .5f));
                yield return StartCoroutine(Helpers.FadeOutImageAlpha(seledoBackground, .5f));
                yield return StartCoroutine(Helpers.FadeInImageAlpha(seledoBackground, .25f, .5f));
                yield return StartCoroutine(Helpers.FadeOutImageAlpha(seledoBackground, .5f));
                yield return StartCoroutine(Helpers.FadeInImageAlpha(seledoBackground, .25f, .5f));
            }
            else if (dialogue.eventNumber == 1)
            {
                yield return StartCoroutine(Helpers.FadeInImageAlpha(cilyBackground, .25f, .5f));
                yield return StartCoroutine(Helpers.FadeOutImageAlpha(cilyBackground, .5f));
                yield return StartCoroutine(Helpers.FadeInImageAlpha(cilyBackground, .25f, .5f));
                yield return StartCoroutine(Helpers.FadeOutImageAlpha(cilyBackground, .5f));
                yield return StartCoroutine(Helpers.FadeInImageAlpha(cilyBackground, .25f, .5f));
            }
            else if (dialogue.eventNumber == 2)
            {
                yield return StartCoroutine(Helpers.FadeInImageAlpha(brunthBackground, .25f, .5f));
                yield return StartCoroutine(Helpers.FadeOutImageAlpha(brunthBackground, .5f));
                yield return StartCoroutine(Helpers.FadeInImageAlpha(brunthBackground, .25f, .5f));
                yield return StartCoroutine(Helpers.FadeOutImageAlpha(brunthBackground, .5f));
                yield return StartCoroutine(Helpers.FadeInImageAlpha(brunthBackground, .25f, .5f));
            }
            else if (dialogue.eventNumber == 3)
            {
                yield return StartCoroutine(Helpers.FadeInImageAlpha(everdellBackground, .25f, .5f));
                yield return StartCoroutine(Helpers.FadeOutImageAlpha(everdellBackground, .5f));
                yield return StartCoroutine(Helpers.FadeInImageAlpha(everdellBackground, .25f, .5f));
                yield return StartCoroutine(Helpers.FadeOutImageAlpha(everdellBackground, .5f));
                yield return StartCoroutine(Helpers.FadeInImageAlpha(everdellBackground, .25f, .5f));
            }
            else if (dialogue.eventNumber == 4)
            {
                StartCoroutine(Helpers.FadeOutImageAlpha(everdellBackground, .5f));
                StartCoroutine(Helpers.FadeOutImageAlpha(cilyBackground, .5f));
                StartCoroutine(Helpers.FadeOutImageAlpha(brunthBackground, .5f));
                yield return StartCoroutine(Helpers.FadeOutImageAlpha(seledoBackground, .5f));

                StartCoroutine(Helpers.MoveTransform(cameraObj.transform, cameraObj.transform.position, new Vector3(34f, 42.5f, -10f), 2.5f));
                StartCoroutine(Helpers.ScaleCameraSize(cameraObj.GetComponent<Camera>(), 69, 4f));

                StartCoroutine(Helpers.ChangeImageColor(mainCharacter.GetComponent<Image>(), new Color(1f, 1f, 1f, 1f), 3f));
                StartCoroutine(Helpers.MoveRectTransform(mainCharacter, mainCharacter.GetComponent<RectTransform>().anchoredPosition, new Vector2(37.38f, 46.8f), 5f));
            }
            else if (dialogue.eventNumber == 5)
            {
                yield return new WaitForSeconds(2f);
                StartCoroutine(Helpers.ChangeImageColor(mainCharacter.GetComponent<Image>(), new Color(0f, 0f, 0f, 0f), 1f));
                yield return new WaitForSeconds(1f);
            }
        }
        

        scm.loadedData.introBattleOutro = "Camp";
        yield return StartCoroutine(scm.SceneTransition(true));
        SceneManager.LoadScene("ChapterBridge");

    }

    public struct Dialogue {
    public string[] lines;
    public int eventNumber;
        public Dialogue(int eventNumber, string[] lines)
        {
            this.lines = lines;
            this.eventNumber = eventNumber;
        }
    }
}