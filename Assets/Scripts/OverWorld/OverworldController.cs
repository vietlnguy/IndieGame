using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class OverworldController : MonoBehaviour
{
    public SaveManager scm;
    public Image blackScreen;
    public GameObject background;
    public GameObject cameraObj;
    public TextMeshProUGUI textBox;
    public CanvasGroup textBoxCanvasGroup;
    private List<Dialogue> dialogues;
    public AudioSource chapter2BackgroundAudio;

    void Awake()
    {
        scm = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        dialogues = new List<Dialogue>();
    }

    void Start()
    {
        if (scm.loadedData.currentChapter == "Chapter2")
        {
            dialogues.Add(new Dialogue(0, new string[] {"The continent of Fessa.", "Long after the disappearance of the Tah'Lo, modern-day man claimed the land.", "Divided into states and appointed a Lord, each state operates independently.", "With their only obligation being loyalty to Reiss, King of Fessa."}));

            StartCoroutine("Chapter2");
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
                yield return StartCoroutine(Helpers.FadeInCanvasGroup(textBoxCanvasGroup, 2f));
                yield return new WaitForSeconds(2f);
                yield return StartCoroutine(Helpers.FadeOutCanvasGroup(textBoxCanvasGroup, 2f));

            }
        }
        

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