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
        scm = FindAnyObjectByType<SaveManager>();
        mcn = scm.loadedData.mainCharacterName;
        dialogues = new List<CharacterDialogue>();

        dialogues.Add(new CharacterDialogue(mcn, new string[] {"Hello, priestess. Are you busy?"}));
        dialogues.Add(new CharacterDialogue("Celeste", new string[] {"Ah- " + mcn + "... how can I help you?"}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"I was just wondering about our conversation from earlier..", "Can anyone truly be redeemed in Ilvera's light?"}));
        dialogues.Add(new CharacterDialogue("Celeste", new string[] {"Yes.", "It does not matter how vile, how wicked, how ..lost.. one may have been.", "If you dedicate your life to good, the goddess will accept you into her embrace."}));
        dialogues.Add(new CharacterDialogue(mcn, new string[]{"I've done more than I'm proud of.", "I was young and reckless.", "Stole from the innocent, hurt the ones I loved, and killed a few undeserving..", "Moving to the countryside was my way of starting over."}));
        dialogues.Add(new CharacterDialogue("Celeste", new string[] {"You are not the first to confess their sins to me...", "The road to redemption is long, but you have taken the first step toward Ilvera's light..."}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"Truthfully, I don't know much about Ilvera...", "Please show me more."}));
        dialogues.Add(new CharacterDialogue("Celeste", new string[] {"Gladly...", "Please meet me by the river..."}));

        //River scene
        dialogues.Add(new CharacterDialogue("Celeste", new string[] {"Please kneel and close your eyes...", "Ilvera is not just the goddess of the sky..", "She is the goddess of freedom.."}));
        dialogues.Add(new CharacterDialogue("Celeste", new string[] {"Look to the sky and feel the breeze on your face.", "Feel the warmth of the sun against your skin.", "Give yourself to her..."}));

        //Main character nudged between celeste breasts
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"Oh--"}));
        dialogues.Add(new CharacterDialogue("Celeste", new string[] {"Listen all around you...", "Listen to the sound of the trees in the wind.", "She is here...", "She is everywhere..."}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"I can feel her..."}));

        //Over the pants squeezearoo
        dialogues.Add(new CharacterDialogue("Celeste", new string[] {"Good...", "Take a deep breath, let her essence fill your soul..",}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"Ungh--"}));
        dialogues.Add(new CharacterDialogue("Celeste", new string[] {"Forget all your worries...", "Forget all of your sins of the past...", "Give yourself to Ilvera..."}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"Mmm-- yes..."}));
        dialogues.Add(new CharacterDialogue("Celeste", new string[] {"Feel all of the tension inside of you...", "And when you are ready...", "...release."}));
        
        //Bust
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"Ahhh---"}));

        dialogues.Add(new CharacterDialogue("Celeste", new string[] {"Very good...", "The goddess watches over you now.."}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"I feel like a weight has been lifted off my shoulders...", "Thank you, priestess.", "I look forward to learning more about Ilvera."}));
        dialogues.Add(new CharacterDialogue("Celeste", new string[] {"Likewise.", "May she protect us til then..."}));


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
        FindAnyObjectByType<CampDialogue>().Resume();
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