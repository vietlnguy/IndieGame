using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class RecruitDialogue : MonoBehaviour
{
    private BattleController battleController;
    private SaveManager saveManager;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool nextLine = false;
    public bool active = false;
    private string lineToBeTyped = "";
    public AudioSource typingAudio;
    public GameObject smallDialogueTextBox;
    public TextMeshProUGUI smallDialogueNameBox;
    public GameObject mainCharacterImage;
    public GameObject astridImage;
    public GameObject lucasImage;
    public GameObject celesteImage;


    void Awake()
    {
        saveManager = FindAnyObjectByType<SaveManager>();
        battleController = FindAnyObjectByType<BattleController>();
    }

    void Update()
    {
        if (active && Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
                smallDialogueTextBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = lineToBeTyped;
                typingAudio.Stop();
                isTyping = false;
            }
            else
            {
                nextLine = true;
            }
        }
    }
    public void Recruit(GameObject character)
    {
        PlayerController characterScript = character.GetComponent<PlayerController>();
        if (characterScript.title == "Lucas" || characterScript.title == "Celeste")
        {
            StartCoroutine(Chapter2Recruit(characterScript.title));
        }
        
        //Add more character checks here
    }
    private IEnumerator Chapter2Recruit(string name)
    {
        active = true;
        Helpers.DisableCharacterHoverAndClick();
        Helpers.DisableEnemyHoverAndClick();
        List<CharacterDialogue> dialogues = new List<CharacterDialogue>();
        
        if (name == "Lucas")
        {
            dialogues.Add(new CharacterDialogue(mainCharacterImage, saveManager.loadedData.mainCharacterName, new string[] {"Hey! Over here!"}));
            dialogues.Add(new CharacterDialogue(lucasImage, "Lucas", new string[] {"Who are you??", "Are you one of those chumps here to take our stuff???"}));
            dialogues.Add(new CharacterDialogue(mainCharacterImage, saveManager.loadedData.mainCharacterName, new string[] {"No, I'm not with them!", "My name is " + saveManager.loadedData.mainCharacterName + ". Come with me, we can fight them together!"}));
            dialogues.Add(new CharacterDialogue(lucasImage, "Lucas", new string[] {"Hmph! We don't need any help! We'll be fine on our own."}));
            dialogues.Add(new CharacterDialogue(mainCharacterImage, saveManager.loadedData.mainCharacterName, new string[] {"Don't be a fool, kid! Look around!", "Just let me help you, and when we're through this I'll explain everything!"}));
            dialogues.Add(new CharacterDialogue(lucasImage, "Lucas", new string[] {"Grr, you're probably right. I'll let my sis know, but no funny business!", "One hand on her and I'll knock your lights out!"}));
            dialogues.Add(new CharacterDialogue(mainCharacterImage, saveManager.loadedData.mainCharacterName, new string[] {"Okay, deal! Now let's go!"}));
        }
        else if (name == "Celeste")
        {
            dialogues.Add(new CharacterDialogue(mainCharacterImage, saveManager.loadedData.mainCharacterName, new string[] {"Priestess! Over here!"}));
            dialogues.Add(new CharacterDialogue(celesteImage, "Celeste", new string[] {"Ah- I apologize, sir... The next church service will have to be rescheduled.."}));
            dialogues.Add(new CharacterDialogue(mainCharacterImage, saveManager.loadedData.mainCharacterName, new string[] {"What?? No. I am here to help.", "I saw those thugs chase you off!"}));
            dialogues.Add(new CharacterDialogue(celesteImage, "Celeste", new string[] {"Ah- we are saved.. Ilvera's blessing upon you..", "I will let my brother know.", "The goddess forbids me to harm another, but allow me to tend to your wounds.."}));
            dialogues.Add(new CharacterDialogue(mainCharacterImage, saveManager.loadedData.mainCharacterName, new string[] {"Okay, stay behind me!"}));
        }

        yield return StartCoroutine(GoThroughDialogue(dialogues));

        UpdateOwnership(name);
        Helpers.EnableCharacterHoverAndClick();
        Helpers.EnableEnemyHoverAndClick();
        battleController.characterSelected.GetComponent<PlayerController>().endTurn();
        battleController.neutralParty = false;
    }
    private void UpdateOwnership(string name)
    {
        if (name == "Lucas" || name == "Celeste")
        {
            foreach (Transform character in battleController.characters.transform)
            {
                if (character.gameObject.GetComponent<PlayerController>().title == "Lucas" || character.gameObject.GetComponent<PlayerController>().title == "Celeste")
                {
                    character.gameObject.GetComponent<PlayerController>().owned = true;
                }
            }
        }
    }
    private IEnumerator GoThroughDialogue(List<CharacterDialogue> dialogues)
    {
        //Small dialogue
        for (int index = 0; index < dialogues.Count; index++)
        {
            //Update name text
            smallDialogueNameBox.text = dialogues[index].name;

            //Fade in text box
            StartCoroutine(Helpers.MoveRectTransform(smallDialogueTextBox, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 10f), .25f));
            StartCoroutine(Helpers.FadeInCanvasGroup(smallDialogueTextBox.GetComponent<CanvasGroup>(), 0.25f));

            yield return new WaitForSeconds(.25f);
            
            //Type each line
            for (int index2 = 0; index2 < dialogues[index].lines.Length; index2++)
            {
                nextLine = false;
                typingCoroutine = StartCoroutine(TypeLine(dialogues[index].lines[index2], dialogues[index].name, typingAudio, smallDialogueTextBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>(), .05f));
                lineToBeTyped = dialogues[index].lines[index2];

                while (isTyping || !nextLine)
                {
                    yield return new WaitForSeconds(.25f);
                }
                smallDialogueTextBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
            
            }

            //Fade out text box
            StartCoroutine(Helpers.MoveRectTransform(smallDialogueTextBox, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, -10f), .25f));
            StartCoroutine(Helpers.FadeOutCanvasGroup(smallDialogueTextBox.GetComponent<CanvasGroup>(), 0.25f));

            yield return new WaitForSeconds(0.25f);

        }
        typingCoroutine = null;
    }
    private IEnumerator TypeLine(string line, string speaker, AudioSource audioSource, TextMeshProUGUI textBox, float textSpeed) {
        if (speaker == "Astrid")
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
        public GameObject characterImage;
        public CharacterDialogue(GameObject characterImage, string name,string[] lines)
        {
            this.lines = lines;
            this.name = name;
            this.characterImage = characterImage;
        }
    }
}