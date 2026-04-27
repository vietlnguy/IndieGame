using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Celeste1 : MonoBehaviour
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

        dialogues.Add(new CharacterDialogue(mcn, new string[] {"Hello, priestess. Are you busy?"}));
        dialogues.Add(new CharacterDialogue("Celeste", new string[] {"Ah- " + mcn + "... how can I help you?"}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"I was just wondering about our conversation from earlier..", "Can anyone truly be redeemed in Ilvera's light?"}));
        dialogues.Add(new CharacterDialogue("Celeste", new string[] {"Yes.", "It does not matter how vile, how wicked, how ..lost.. one may have been.", "If you dedicate your life to good, the goddess will accept you into her embrace."}));
        dialogues.Add(new CharacterDialogue(mcn, new string[]{"I've done more than I'm proud of.", "I was young and reckless.", "Stole from the innocent, hurt the ones I loved, and killed a few undeserving..", "Moving to the countryside was my way of starting over."}));
        dialogues.Add(new CharacterDialogue("Celeste", new string[] {"You are not the first to confess their sins to me...", "The road to redemption is long, but you have taken the first step...", "I am no stranger to redemption...", "I too have walked the path from darkness into Ilvera's light..."}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"Oh, I always forget that priests have lives before being ordained.", "If I may ask, what was your life like before you became a priestess?", "I bet you still spent your time giving to the needy!"}));
        dialogues.Add(new CharacterDialogue("Celeste", new string[] {"Aha erm-- something like that..."}));

        //flashback lol
        dialogues.Add(new CharacterDialogue("Strong Man 1", new string[] {"Mmm yeah, you like the feel of my big dick??"}))
        dialogues.Add(new CharacterDialogue("Celeste", "Mmm fuckkk yess, it feels so good "))


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