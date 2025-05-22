using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Important: use TMPro, not UnityEngine.UI

public class DialogueSystem : MonoBehaviour
{
    
    public TextMeshProUGUI textComponent;
    private string[] lines;
    public float textSpeed;
    private int linesIndex = 0;
    private int dialoguesIndex = 0;
    public Image targetImage;
    public Sprite mainCharacterSprite;
    public Sprite penelopeSprite;
    public Sprite amaraSprite;
    public Sprite npcSprite;
    public Sprite gerardSprite;
    private bool startedDialogue = false;
    public TextMeshProUGUI title;
    public GameObject canvas;
    private RectTransform rectTransform;
    private CanvasGroup group;
    private float time = .15f;
    public GameObject controller;
    private Controller controllerScript;
    private CharacterDialogue[] dialogues;
    public AudioClip soundClip;
    private AudioSource audioSource;
    void Start()
    {
        rectTransform = canvas.GetComponent<RectTransform>();
        group = canvas.GetComponent<CanvasGroup>();
        controllerScript = controller.GetComponent<Controller>();
        audioSource = GetComponent<AudioSource>();
        
        dialogues = new CharacterDialogue[] {
            new CharacterDialogue(false, mainCharacterSprite, "Liam", new string[] {"Phew we made it... I thought we were done for."}),
            new CharacterDialogue(false, penelopeSprite, "Penelope", new string[] {"What a beautiful oasis! Nice to finally get out of that wretched castle..","Maybe I'll go lounge out by the pond and get a nice tan!"}),
            new CharacterDialogue(false, mainCharacterSprite, "Liam", new string[] {"We should resupply while we have the chance. We're almost completely out of water.", "Maybe we can find a vendor and if we're lucky maybe an armory too-- ","Wait a minute, do you hear that? Something is going on over there..."}),
            new CharacterDialogue(true, amaraSprite, "???", new string[] {"Now now boys, there's no need to be angry.","It was an honest mistake-- I assure you, your money is right here!"}),
            new CharacterDialogue(false, npcSprite, "NPC", new string[] {"Cut the crap! We know you've been scamming people!", "You better hand over the money plus interest!"}),
            new CharacterDialogue(false, amaraSprite,"???", new string[] {"Looks like the gig is up...","* WAM *"}),
            new CharacterDialogue(false, npcSprite,"NPC", new string[] {"ACK-- my nose!","That bitch! Get her boys! I don't care if you have to tear down every house! We're getting our money back."}),
            new CharacterDialogue(true, penelopeSprite,"Penelope", new string[] {"Whoa! Those ruffians are terrorizing the townsfolk!","And right when I was about to relax * ugh * The nerve! We have to stop them!"}),
            new CharacterDialogue(false, gerardSprite,"Gerard", new string[] {"Your Highness, we barely escaped capital forces.","Perhaps it would be wise if we avoid gathering any more unwanted attention."}),
            new CharacterDialogue(false, penelopeSprite,"Penelope", new string[] {"Are you suggesting we just leave them?? Unacceptable!","Please Liam, we have to help them! What's the point of fighting if we don't help those in need??"}),
            new CharacterDialogue(false, mainCharacterSprite,"Liam", new string[] {"...","Fine. We can help them, but stay behind me. I don't want anyone else getting hurt."}),
            new CharacterDialogue(false, penelopeSprite,"Penelope", new string[] {"Yay! Thank you!"})
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
                if (dialogues[dialoguesIndex].title == "???") {
                    controllerScript.dialogueFinished = true;
                }
                if (dialogues[dialoguesIndex].title == "Penelope") {
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
        targetImage.sprite = sprite;
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
