using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Astrid1 : MonoBehaviour
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
        dialogues.Add(new CharacterDialogue("Astrid", new string[] {"Do you have a minute to talk, " + mcn + "?"}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"Of course, is something the matter?"}));
        dialogues.Add(new CharacterDialogue("Astrid", new string[] {"Well...", "I was thinking about these bracelets.", "Back on the farm, I felt a rush of energy when that soldier attacked me.", "But now I don't feel anything at all."}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"Hmm.", "It is ancient technology..", "Maybe it automatically responds to danger?"}));
        dialogues.Add(new CharacterDialogue("Astrid", new string[] {"I'm not sure.", "And what if we're in danger and I need to use the power again?", "I'm worried I won't be able to access it."}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"That is a good point..", "Well why don't we test out some theories?", "I'll pretend to attack you and then you try to push me away!"}));
        dialogues.Add(new CharacterDialogue("Astrid", new string[] {"Uh-- that doesn't seem very scientific."}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"Come on, what's there to lose?", "We might learn a thing or two."}));
        dialogues.Add(new CharacterDialogue("Astrid", new string[] {"Alright, I guess..", "Just don't do anything reckless!"}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"I won't!", "Okay, ready?", "1...", "2...", "3!"}));
        //Charge at astrid, pins against tree

        dialogues.Add(new CharacterDialogue("Astrid", new string[] {"Oof--"}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"HAND OVER ALL OF YOUR MONEY!! OR MY KNIFE WILL BE THE LAST THING YOU SEE."}));
        dialogues.Add(new CharacterDialogue("Astrid", new string[] {"Hrrmphhhh!"}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"HAHAHA!", "IS THAT ALL THE STRENGTH YOU CAN MUSTER UP???"}));
        dialogues.Add(new CharacterDialogue("Astrid", new string[] {"Hrrrrrrahhh!!!"}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"HOW CUTE!", "MAYBE AFTER I TAKE ALL OF YOUR MONEY, I'LL TAKE A PEEK UNDER THAT SKIRT TOO!!"}));
        dialogues.Add(new CharacterDialogue("Astrid", new string[] {mcn + "!", "Why would you say that??"}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"Stay focused, Astrid! This could happen in real life you need to be prepared!",}));

        //turns Astrid around against tree or the wagon or something
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"HAR HAR HAR. YOU'RE CUTE ASS BELONGS TO ME.",}));
        dialogues.Add(new CharacterDialogue("Astrid", new string[] {"You dumbass, I know what you're doing!!"}));

        //hits it from behind lol
        dialogues.Add(new CharacterDialogue("Astrid", new string[] {"Mmphhh...", "How is this-- *ahhh* supposed to unlock the power of the bracelets?"}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"Shhhh.", "This is all a part of the training.."}));
        dialogues.Add(new CharacterDialogue("Astrid", new string[] {"Ungh... that feels good", "Right there... don't stop...", }));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"Hey!", "What do you mean \"don't stop\"? This is training, you're supposed to push me away!", "Tsk tsk tsk.",}));
        dialogues.Add(new CharacterDialogue("Astrid", new string[] {"Sorry sweetie, I guess this bandit can fuck me better. Ungg..."}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"HA. HA. Very funny..."}));
        dialogues.Add(new CharacterDialogue("Astrid", new string[] {"Mmmmm... I'm about to cum..", "Keep going, don't stop..."}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"Ahhh fuck, me too...", "It feel's too good!"}));

        //cums lol
        dialogues.Add(new CharacterDialogue("Astrid", new string[] {"Ahhhh...", "So much for figuring out how to use the bracelets..."}));
        dialogues.Add(new CharacterDialogue(mcn, new string[] {"What do you mean? I learned a ton!", "For instance, the bracelets do not react to sex!"}));
        dialogues.Add(new CharacterDialogue("Astrid", new string[] {"Woooooooow, at this rate we'll have the bracelets mastered in no time..."}));


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

        public CharacterDialogue(string name ,string[] lines)
        {
            this.lines = lines;
            this.name = name;
        }
    }
}