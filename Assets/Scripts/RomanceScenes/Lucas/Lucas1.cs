using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lucas1 : MonoBehaviour
{
    private SaveManager scm;
    private List<CharacterDialogue> dialogues;
    public GameObject nameBox;
    public GameObject textBox;
    private bool active = false;
    public bool nextLine = false;
    private bool isTyping = false;
    public Coroutine typingCoroutine;
    private string lineToBeTyped = "";
    public AudioSource typingAudio;
    private string mcn = "";
    
    void Awake()
    {
        scm = FindFirstObjectByType<SaveManager>();
        mcn = scm.loadedData.mainCharacterName;
        dialogues = new List<CharacterDialogue>();

        dialogues.Add(new CharacterDialogue("Lucas", new string[] {"Hiya!", "Hng!", "Hah!"}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"Nice, jab."}));
        dialogues.Add(new CharacterDialogue("Lucas", new string[] {"AHH--", "Who's there?!"}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"It's just me.", "Sorry for startling you."}));
        dialogues.Add(new CharacterDialogue("Lucas", new string[] {"You didn't startle me!", "I was just pretending to be suprised so that you'd let your guard down."}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"Of course.", "You know, I've done a bit of martial training in my life.", "I could show you a thing or two if you'd like?"}));
        dialogues.Add(new CharacterDialogue("Lucas", new string[] {"I don't need your help to train!", "Me and sis have always survived just fine on our own."}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"I can see that.", "Celeste is lucky to have such a protective brother.", "She seems to appreciate all the things you do for her."}));
        dialogues.Add(new CharacterDialogue("Lucas", new string[] {"Wait... she said that?", "...I was always worried I wasn't doing enough..."}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"Can I ask you something?", "How long have you and Celeste been alone?", "What happened to your parents?"}));
        dialogues.Add(new CharacterDialogue("Lucas", new string[] {"...", "Mama and Papa died when we were young.", "It was a typical day like any other.", "Mama and Papa went to town each week to get food and supplies.", "Except this time, on their way back they were ambushed...", "By a couple of nobody bandits."}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"I'm sorry for your loss."}));
        dialogues.Add(new CharacterDialogue("Lucas", new string[] {"After that, I vowed to become stronger to protect Celeste.", "To never let anything like that ever happen again to our family."}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"That is very brave of you.", "You and I are not very different.", "I lost a dear friend of mine not long ago.", "His name was William.", "I was reckless and selfish. It took a long time to forgive myself."}));
        dialogues.Add(new CharacterDialogue("Lucas", new string[] {"...", "Actually.. can you show me some of those moves you were talking about?", "Let's get stronger together!", "For Mama and Papa and Celeste!", "And for your friend!"}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"Let's do it!", "Hiya!!!"}));



    }
    void Start()
    {
        StartCoroutine(Sequence());
    }
    void Update()
    {
        if (active)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                if (isTyping)
                {
                    StopCoroutine(typingCoroutine);
                    typingCoroutine = null;
                    textBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = lineToBeTyped;
                    typingAudio.Stop();
                    isTyping = false;
                }
                else
                {
                    nextLine = true;
                }
            }
        }
    }
    private IEnumerator Sequence()
    {
        //Go through each dialogue
        active = true;
        for (int index = 0; index < dialogues.Count; index++)
        {
            //Update name text
            nameBox.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = dialogues[index].name;

            //Fade in text box
            StartCoroutine(Helpers.MoveRectTransform(textBox, textBox.GetComponent<RectTransform>().anchoredPosition, textBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 10f), .25f));
            StartCoroutine(Helpers.FadeInCanvasGroup(textBox.GetComponent<CanvasGroup>(), 0.25f));

            yield return new WaitForSeconds(.25f);
            //Type each line
            for (int index2 = 0; index2 < dialogues[index].lines.Length; index2++)
            {
                nextLine = false;
                typingCoroutine = StartCoroutine(TypeLine(dialogues[index].lines[index2], dialogues[index].name, typingAudio, textBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>(), .05f));
                lineToBeTyped = dialogues[index].lines[index2];

                while (isTyping || !nextLine)
                {
                    yield return new WaitForSeconds(.25f);
                }
                textBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
            
            }

            //Fade out text box
            StartCoroutine(Helpers.MoveRectTransform(textBox, textBox.GetComponent<RectTransform>().anchoredPosition, textBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, -10f), .25f));
            StartCoroutine(Helpers.FadeOutCanvasGroup(textBox.GetComponent<CanvasGroup>(), 0.25f));

            yield return new WaitForSeconds(0.25f);

        }
 
        EndCutscene();
    }
    private void EndCutscene()
    {
        FindFirstObjectByType<CampDialogue>().Resume();
    }
    private IEnumerator TypeLine(string line, string speaker, AudioSource audioSource, TextMeshProUGUI textBox, float textSpeed) {
        if (speaker == "Celeste")
        {
            audioSource.pitch = 1.2f;
            textBox.color = new Color(1f, .75f, .79f, 1f);
        }
        else
        {
            audioSource.pitch = 1.0f;
            textBox.color = Color.white;
        }
        isTyping = true;
        audioSource.Play();
        foreach (char c in line.ToCharArray()) {
            textBox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        audioSource.Stop();
        isTyping = false;
    }
    public struct CharacterDialogue {
        public string[] lines;
        public string name;

        public CharacterDialogue(string name ,string[] lines)
        {
            this.lines = lines;
            this.name = name;
        }
    }
}