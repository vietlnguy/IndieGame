using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class CampDialogue : MonoBehaviour
{
    private SaveManager scm;
    public Image blackScreen;
    public TextMeshProUGUI nameBox;
    public TextMeshProUGUI textBox;
    public GameObject everdellScenery;
    public GameObject astridImage;
    private bool active = false;
    public bool nextLine = false;
    private bool isTyping = false;
    private bool astridTalkedToAlready = false;
    public List<CharacterDialogue> dialogues;
    public Coroutine typingCoroutine;
    private string lineToBeTyped = "";
    public AudioSource typingAudio;


    void Awake()
    {
        scm = FindFirstObjectByType<SaveManager>();
        dialogues = new List<CharacterDialogue>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (active)
        {
            if (Input.GetMouseButtonDown(0))
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

    public IEnumerator EnableDialogueWindow(GameObject character)
    {
        DetermineDialogue();
        yield return new StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1f));

        CampPlayerController characterScript = character.GetComponent<CampPlayerController>();
        nameBox.text = characterScript.title;

        //Enable the right character image
        if (characterScript.title == "Astrid")
        {
            astridImage.SetActive(true);
        }
        else if (characterScript.title == "Amara")
        {
            
        }

        //Enable the right background
        if (scm.loadedData.currentChapter == "Chapter 2")
        {
            everdellScenery.SetActive();
        }

        yield return new StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));

        //Go through each dialogue
        active = true;
        for (int index = 0; index < dialogues.Count; index++)
        {
            //Update name text
            textBoxNameBox.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = dialogues[index].name;

            //Update nameBox position
            textBoxNameBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(dialogues[index].characterImage.GetComponent<RectTransform>().anchoredPosition.x, textBoxNameBox.GetComponent<RectTransform>().anchoredPosition.y);

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
    
    }
    private void DetermineDialogue(GameObject character)
    {
        dialogues.Clear();
        CampPlayerController characterScript = character.GetComponent<CampPlayerController>();
        

        if (characterScript.title == "Astrid")
        {
            if (scm.loadedData.currentChapter == "Chapter 2")
            {
                if (!astridTalkedToAlready)
                {
                    dialogues.Add(new CharacterDialogue(astridImage, characterScript.title, new string[] {"Even though we had to leave our home, it's nice to be out in the countryside again.", "I wonder how long we'll be gone.",  "Ooo when we get to Maplemire do you think we can get turkey pies??", "It's been so long since I've had one."}));
                }
                else
                {
                    dialogues.Add(new CharacterDialogue(astridImage, characterScript.title, new string[] {"Hi dear, need something?"}));
                }
            
            }
            else if (scm.loadedData.currentChapter == "Chapter 3")
            {
                
            }
        }
    }
    public IEnumerator TypeLine(string line, string speaker, AudioSource audioSource, TextMeshProUGUI textBox, float textSpeed) {
        if (speaker == "Astrid")
        {
            audioSource.pitch = 1.2f;
        }
        else
        {
            audioSource.pitch = 1.0f;
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
        public GameObject characterImage;
        public CharacterDialogue(GameObject characterImage, string name,string[] lines)
        {
            this.lines = lines;
            this.name = name;
            this.characterImage = characterImage;
        }
    }

}