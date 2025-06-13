using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Important: use TMPro, not UnityEngine.UI

public class FullBodyDialogue : MonoBehaviour
{
    
    public TextMeshProUGUI textComponent;
    private string[] lines;
    private float textSpeed = .02f;
    private int linesIndex = 0;
    private int dialoguesIndex = 0;
    public GameObject mainCharacterObj;
    public GameObject astridObj;
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
            new CharacterDialogue(true, false, "Astrid", new string[] {"Hehe, feeling better?"}),
            new CharacterDialogue(false, false, "Liam", new string[] {"I feel like a new man!", "Like I've got the strength to plow ten fields!"}),
            new CharacterDialogue(false, false, "Astrid", new string[] {"I hope I'm one of them!"}),
            new CharacterDialogue(false, false, "Liam", new string[] {"Of course! *wink*", "Honestly though... I couldn't have done this without you.", "Having you here gives me the strength to perservere."}),
           
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
        if (dialogues[dialoguesIndex].moveCharactersIn)
        {
           StartCoroutine(EnterCharacters());
        }
        StartCoroutine(FadeBoxInAndStartDialogue(dialogues[dialoguesIndex].title));
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
    IEnumerator TypeLine() {
        audioSource.pitch = dialogues[dialoguesIndex - 1].dialoguePitch;
        audioSource.PlayOneShot(soundClip);
        foreach (char c in lines[linesIndex].ToCharArray()) {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        audioSource.Stop();
    }
    private IEnumerator EnterCharacters()
    {
        float duration = 0.2f;
        Vector2 mainCharStartPosition = mainCharacterObj.GetComponent<RectTransform>().anchoredPosition;
        Vector2 astridStartPosition = astridObj.GetComponent<RectTransform>().anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the interpolation factor (0 to 1)
            float t = elapsedTime / duration;

            // Optional: Ease the movement (e.g., SmoothStep for smoother start/end)
            // t = t * t * (3f - 2f * t); // SmoothStep easing function
            // t = Mathf.Sin(t * Mathf.PI * 0.5f); // EaseOutSine

            // Interpolate the position
            mainCharacterObj.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(mainCharStartPosition, new Vector2(-362.3f, -208f), t);
            astridObj.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(astridStartPosition, new Vector2(440.69f, -196f), t);
            elapsedTime += Time.deltaTime; // Increment elapsed time by the time since last frame
            yield return null; // Wait for the next frame
        }

        // Ensure the image reaches the exact target position at the end        
        mainCharacterObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-362.3f, -208f);
        astridObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(440.69f, -196f);

    }
    IEnumerator FadeBoxInAndStartDialogue(string name) {
        yield return new WaitForSeconds(.2f);
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
    
    private struct CharacterDialogue
    {
        public string[] lines;
        public string title;
        public bool pauseExecutionBefore;
        public float dialoguePitch;
        public bool moveCharactersIn;

        public CharacterDialogue(bool moveCharactersIn, bool pauseExecutionBefore, string title, string[] lines)
        {
            this.lines = lines;
            this.title = title;
            this.pauseExecutionBefore = pauseExecutionBefore;
            this.moveCharactersIn = moveCharactersIn;

            if (title == "Liam")
            {
                this.dialoguePitch = 0.75f;
            }
            else if (title == "Penelope")
            {
                this.dialoguePitch = 0.95f;
            }
            else if (title == "???")
            {
                this.dialoguePitch = 0.9f;
            }
            else if (title == "Gerard")
            {
                this.dialoguePitch = 0.7f;
            }
            else if (title == "NPC")
            {
                this.dialoguePitch = 0.8f;
            }
            else
            {
                this.dialoguePitch = 1f;
            }
        }
    }



}
