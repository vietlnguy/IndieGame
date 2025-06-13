using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Important: use TMPro, not UnityEngine.UI

public class SmallDialogue : MonoBehaviour
{
    
    public TextMeshProUGUI textComponent;
    private string[] lines;
    private float textSpeed = .02f;
    private int linesIndex = 0;
    private int dialoguesIndex = 0;
    public Image portraitImage;
    public Sprite mainCharacterSprite;
    public Sprite astridSprite;
    private bool startedDialogue = false;
    public TextMeshProUGUI title;
    public GameObject canvas;
    private RectTransform rectTransform;
    private CanvasGroup group;
    private float time = .15f;
    public GameObject controller;
    private IntroController controllerScript;
    private CharacterDialogue[] dialogues;
    public AudioClip soundClip;
    private AudioSource audioSource;
    void Start()
    {
        rectTransform = canvas.GetComponent<RectTransform>();
        group = canvas.GetComponent<CanvasGroup>();
        controllerScript = controller.GetComponent<IntroController>();
        audioSource = GetComponent<AudioSource>();
        
        dialogues = new CharacterDialogue[] {
            //SmallDialogue
            new CharacterDialogue(false, mainCharacterSprite, "Liam", new string[] {"Honey, I'm back!"}),
            new CharacterDialogue(false, astridSprite, "Astrid", new string[] {"Oh! Back so soon?", "I've hardly started dinner!"}),
            new CharacterDialogue(false, mainCharacterSprite, "Liam", new string[] {"I had an early start this morning.", "Need an extra hand?"}),
            new CharacterDialogue(false, astridSprite, "Astrid", new string[] {"No thank you, dear.", "You've already done so much around the farm, I can handle dinner.", "You should relax!"}),
            new CharacterDialogue(false, mainCharacterSprite, "Liam", new string[] {"It's hard to relax when there is still so much to do...", "I'm the one that said we should move out here.", "I'm starting to wonder if that was a dumb decision..."}),
            new CharacterDialogue(false, astridSprite, "Astrid", new string[] {"I've never regretted a second being here.", "It doesn't matter where we are, as long as we're together, I'm happy."}),
            new CharacterDialogue(false, mainCharacterSprite, "Liam", new string[] {"I don't deserve you."}),
            new CharacterDialogue(false, astridSprite, "Astrid", new string[] {"Hehe, that's what they all say.", "Now why don't I help you relax a little more?", "You must be so tense."}),
            new CharacterDialogue(false, mainCharacterSprite, "Liam", new string[] {"Actually, I don't feel as sore as-- uughh--"}),
            new CharacterDialogue(true, astridSprite, "Astrid", new string[] {"Hehe, maybe you are just a dumb farmer.", "Working out in the sun all day must've turned your brain to mush."}),
            new CharacterDialogue(false, mainCharacterSprite, "Liam", new string[] {"Haah... maybe you're right.. Ahh...", "It doesn't help that you're draining all of the blood from my head now, too."}),
            new CharacterDialogue(false, astridSprite, "Astrid", new string[] {"It's okay, it's looks like it's going to a different head, hehe.", "Looks like I better give it some special attention."}),
            new CharacterDialogue(false, mainCharacterSprite, "Liam", new string[] {"Ooo... that's good... just like that..."}),
            new CharacterDialogue(false, astridSprite, "Astrid", new string[] {"Mwgh, mwgh, mwgh..."}),
            new CharacterDialogue(false, mainCharacterSprite, "Liam", new string[] {"Uughh--", "It feels too good!", "I can't hold it in any longer!"}),
            new CharacterDialogue(false, astridSprite, "Astrid", new string[] {"Mhhmmmm--"}),

            //FullBodyDialogue
            new CharacterDialogue(true, astridSprite, "Astrid", new string[] {"Hehe, feeling better?"}),
            new CharacterDialogue(false, mainCharacterSprite, "Liam", new string[] {"I feel like a new man!", "Like I've got the strength to plow ten fields!"}),
            new CharacterDialogue(false, astridSprite, "Astrid", new string[] {"I hope I'm one of them!"}),
            new CharacterDialogue(false, mainCharacterSprite, "Liam", new string[] {"Of course! *wink*", "Honestly though... I couldn't have done this without you.", "Having you here gives me the strength to perservere."}),
           

        };

        textComponent.text = string.Empty;
    }
    void Update()
    {
        if (startedDialogue) {
            if (Input.GetMouseButtonDown(0)) // Left click
            {
                if (textComponent.text == lines[linesIndex])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    textComponent.text = lines[linesIndex];
                    audioSource.Stop();
                }
            }
        }
    }
    public void NextDialogue() {
        lines = dialogues[dialoguesIndex].lines;
        StartCoroutine(FadeBoxInAndStartDialogue(dialogues[dialoguesIndex].sprite, dialogues[dialoguesIndex].title));
        dialoguesIndex++;
    }
    public void NextLine() {
        if (linesIndex < lines.Length - 1) {
            linesIndex++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
            
        }
        else {
            //No more dialogue left
            if (dialoguesIndex >= dialogues.Length) {
                StartCoroutine(FadeOutAndMove());
                startedDialogue = false;
            }
            else if (dialogues[dialoguesIndex].pauseExecutionBefore){
                if (!controllerScript.dialogueFinished) {
                    controllerScript.dialogueFinished = true;
                }
                else if (controllerScript.dialogueFinished && !controllerScript.secondDialogueFinished) {
                    controllerScript.secondDialogueFinished = true;
                }
                startedDialogue = false;
                StartCoroutine(FadeOutAndMove());
                textComponent.text = string.Empty;
                linesIndex = 0;
            }
            //Start next character's dialogue
            else {
                textComponent.text = string.Empty;
                linesIndex = 0;
                StartCoroutine(FadeOutAndMove());
                startedDialogue = false;
                NextDialogue(); 
            }
        }
    }
    IEnumerator FadeBoxInAndStartDialogue(Sprite sprite, string name) {
        yield return new WaitForSeconds(.25f);
        portraitImage.sprite = sprite;
        title.text = name;
        Vector2 targetPos = new Vector2(0f, 0f);
        Vector2 startPos = new Vector2(0f, -10f); // Off-screen bottom
        rectTransform.anchoredPosition = startPos;
        group.alpha = 0f;

        float elapsed = 0f;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / time);
            group.alpha = t;
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // Ensure final values are set
        group.alpha = 1f;
        rectTransform.anchoredPosition = targetPos;
        startedDialogue = true;
        StartCoroutine(TypeLine());

    }
    IEnumerator FadeOutAndMove() {
        Vector2 targetPos = new Vector2(0f, -10f);
        Vector2 startPos = new Vector2(0f, 0f);

        float elapsed = 0f;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / time);
            group.alpha = group.alpha = Mathf.Lerp(1, 0f, elapsed / time);
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // Ensure final values are set
        group.alpha = 0f;
        rectTransform.anchoredPosition = targetPos;
    }
    IEnumerator TypeLine() {
        audioSource.pitch = dialogues[dialoguesIndex - 1].dialoguePitch;
        audioSource.PlayOneShot(soundClip);
        foreach (char c in lines[linesIndex].ToCharArray()) {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        audioSource.Stop();
    }
    private struct CharacterDialogue {
        public string[] lines;
        public string title;
        public Sprite sprite;
        public bool pauseExecutionBefore;
        public float dialoguePitch;

        public CharacterDialogue(bool pauseExecutionBefore, Sprite sprite, string title, string[] lines) {
            this.lines = lines;
            this.title = title;
            this.sprite = sprite;
            this.pauseExecutionBefore = pauseExecutionBefore;

            if (title == "Liam") {
                this.dialoguePitch = 0.75f;
            }
            else if (title == "Penelope") {
                this.dialoguePitch = 0.95f;
            }
            else if (title == "???") {
                this.dialoguePitch = 0.9f;
            }
            else if (title == "Gerard") {
                this.dialoguePitch = 0.7f;
            }
            else if (title == "NPC") {
                this.dialoguePitch = 0.8f;
            }
            else {
                this.dialoguePitch = 1f;
            }
        }
    }



}
