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
    public GameObject hegsethObj;
    public GameObject soldierObj;
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
    public AudioSource doorKnockAudio;
    public AudioSource hegsethThemeAudio;
    public GameObject namePlate;
    public AudioSource backgroundMusic;
    public AudioSource rummagingAudio;
    void Start()
    {
        rectTransform = canvas.GetComponent<RectTransform>();
        group = canvas.GetComponent<CanvasGroup>();
        controllerScript = controller.GetComponent<IntroController>();
        audioSource = GetComponent<AudioSource>();
        
        dialogues = new CharacterDialogue[] {
            //Enter characters
            new CharacterDialogue(1,false, "Astrid", new string[] {"Hehe, feeling better?"}),
            new CharacterDialogue(0,false, "Liam", new string[] {"I feel like a new man!", "Like I've got the strength to plow ten fields!"}),
            new CharacterDialogue(0,false, "Astrid", new string[] {"As long as you make time to plow mine."}),
            new CharacterDialogue(0,false, "Liam", new string[] {"Of course!", "Truth be told... I was worried about moving to the countryside.", "Away from all of our friends and family, starting over.", "With all of that news about ancient Tah'Lo artifacts, the world seems to be spinning faster and faster.", "A quiet life with you is all I need."}),
            new CharacterDialogue(0,false, "Astrid", new string[] {"I couldn't agree more.", "I am curious, though. They say the ancient Tah'Lo people were incredibly advanced.", "I wonder what kind of amazing things they could do..."}),
            new CharacterDialogue(0,false, "Liam", new string[] {"Beats me.", "As long as it doesn't involve me, they can have all of the artifacts they want."}),

            //Door Knock
            new CharacterDialogue(2,false, "Liam", new string[] {"Huh? Someone's at the door?", "All the way out here?"}),

            //Answer the Door
            new CharacterDialogue(3,false, "Hegseth", new string[] {"Hello. Apologies for the intrusion.", "My name is Hegseth. I am a member of the Kings council.", "What a wonderful home you have."}),
            new CharacterDialogue(0,false, "Liam", new string[] {"Thank you.", "Is there something I can help you with?"}),
            new CharacterDialogue(0,false, "Hegseth", new string[] {"Yes, indeed.", "As I am sure you are aware, our great King Reiss (long may he reign), has declared that all Tah'Lo artifacts be relinquished to local authorities.", "It is for the safety and prosperity of the people that our king has decreed it."}),
            new CharacterDialogue(0,false, "Liam", new string[] {"That's fine, but we don't have any Tah'Lo artifacts.", "I couldn't even tell you if I saw one."}),
            new CharacterDialogue(0,false, "Hegseth", new string[] {"I see.", "Well then, you wouldn't mind if we inspected the area?", "It is common for Tah'Lo artifacts to go unnoticed by ...commoners."}),
            new CharacterDialogue(0,false, "Liam", new string[] {"Is that necessary? This is just a farm.", "The only thing you will find is wheat and pig crap."}),
            new CharacterDialogue(0,false, "Hegseth", new string[] {"I assure you it will not take long.", "We would hate to send word to the king that some folk have been uncooperative."}),
            new CharacterDialogue(0,false, "Liam", new string[] {"..."}),
            new CharacterDialogue(0,false, "Hegseth", new string[] {"Good.", "Now then...", "Men!"}),

            //Enter soldiers
            new CharacterDialogue(4,false, "Soldier", new string[] {"Yes, sir!"}),
            new CharacterDialogue(0,false, "Hegseth", new string[] {"Search the area for any Tah'Lo artifacts!", "Be thorough, I will not tolerate any mistakes."}),
            new CharacterDialogue(0,false, "Soldier", new string[] {"Yes, sir!"}),     


            //Fade to black, Look around, reappear dialogue
            new CharacterDialogue(5,false, "Hegseth", new string[] {"Did you find anything?"}),
            new CharacterDialogue(0,false, "Soldier", new string[] {"Nothing, sir. Checked every nook and cranny."}),
            new CharacterDialogue(0,false, "Hegseth", new string[] {"Hmm. Perhaps you are telling the truth."}),
            new CharacterDialogue(0,false, "Liam", new string[] {"I told you.", "Now if you're done, I'd like you to leave. I have work to do."}),
            new CharacterDialogue(0,false, "Hegseth", new string[] {"Of course! Of course!", "There's just one last thing...", "The lady's bracelets.", "Hand them over."}),
            new CharacterDialogue(0,false, "Astrid", new string[] {"What? My bracelets?", "These aren't Tah'Lo artifacts. These are just ordinary bracelets."}),
            new CharacterDialogue(0,false, "Hegseth", new string[] {"That will be for the king to decide."}),
            new CharacterDialogue(0,false, "Astrid", new string[] {"What? No!", "There must be a mistake.", "These were a gift from my mother! I can't give them away!"}),
            new CharacterDialogue(0,false, "Hegseth", new string[] {"Such a shame.. But your mother will be proud of you for being a devout citizen of the kingdom.", "Now let me take those off your hands--"}),

            //Move Hegseth toward 
            new CharacterDialogue(6,false, "Liam", new string[] {"Back off.", "Take another step towards her and you will regret it."}),


            //Move Hegseth back
            new CharacterDialogue(7,false, "Hegseth", new string[] {"Oh ho ho. I would think very carefully about what you are doing, boy.", "Defying me is defying the king."}),

            new CharacterDialogue(0,false, "Liam", new string[] {"Leave. I won't ask you again."}),
            new CharacterDialogue(0,false, "Hegseth", new string[] {"Very well.", "Remember that you chose this...", "Men! Kill her and take the bracelet!"}),
            new CharacterDialogue(0,false, "Soldier", new string[] {"Yes, sir!"}),

            //Move soldiers
            new CharacterDialogue(8,false, "Astrid", new string[] {"Ah!! Liam!"}),
            new CharacterDialogue(0,false, "Liam", new string[] {"Astrid!!!"}),

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
                    StartCoroutine(NextLine());
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
        StartCoroutine(FadeBoxInAndStartDialogue(dialogues[dialoguesIndex].title));
    }
    public IEnumerator NextLine() {
        if (linesIndex < lines.Length - 1) {
            linesIndex++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
            
        }
        else {
            //No more dialogue left
            if (dialoguesIndex >= dialogues.Length) {
                yield return StartCoroutine(FadeOutAndMove());
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
                yield return StartCoroutine(FadeOutAndMove());
                textComponent.text = string.Empty;
                linesIndex = 0;
            }
            //Start next character's dialogue
            else {
                textComponent.text = string.Empty;
                linesIndex = 0;
                yield return StartCoroutine(FadeOutAndMove());
                startedDialogue = false;
                dialoguesIndex++;
                NextDialogue(); 
            }
        }
    }
    IEnumerator TypeLine() {
        audioSource.pitch = dialogues[dialoguesIndex].dialoguePitch;
        audioSource.PlayOneShot(soundClip);
        foreach (char c in lines[linesIndex].ToCharArray()) {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        audioSource.Stop();
    }
    private IEnumerator MoveCharacter(GameObject obj, float x, float y)
    {
        float duration = 0.2f;
        Vector2 startPos = obj.GetComponent<RectTransform>().anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            obj.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPos, new Vector2(x, y), t);
            elapsedTime += Time.deltaTime; 
            yield return null;
        }      
        obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

    }
    IEnumerator FadeBoxInAndStartDialogue(string name) {
        //One off events i.e. move character, stop sound etc.
        if (dialogues[dialoguesIndex].action == 1)
        {
            StartCoroutine(MoveCharacter(mainCharacterObj, -362.3f, -164));
            yield return StartCoroutine(MoveCharacter(astridObj, 440.69f, -196f));
            yield return new WaitForSeconds(.5f);
        }
        if (dialogues[dialoguesIndex].action == 2)
        {
            backgroundMusic.Pause();
            doorKnockAudio.Play();
            yield return new WaitForSeconds(3f);
        }
        if (dialogues[dialoguesIndex].action == 3)
        {
            yield return StartCoroutine(FadeOutCharacter(mainCharacterObj));
            yield return new WaitForSeconds(1f);
            mainCharacterObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(269f, -164f);
            Vector3 currentScale = mainCharacterObj.GetComponent<RectTransform>().localScale;
            currentScale.x *= -1f;
            mainCharacterObj.GetComponent<RectTransform>().localScale = currentScale;
            hegsethObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-379f, -208f);
            yield return StartCoroutine(MoveCharacter(astridObj, 629f, -196f));
            yield return StartCoroutine(UndoFadeOutCharacter(mainCharacterObj));
            yield return StartCoroutine(UndoFadeOutCharacter(hegsethObj));
            hegsethThemeAudio.Play();
        }
        if (dialogues[dialoguesIndex].action == 4)
        {
           yield return StartCoroutine(MoveCharacter(soldierObj,-612f, -131f));
        }
        if (dialogues[dialoguesIndex].action == 5)
        {
            yield return StartCoroutine(FadeOutCharacter(soldierObj));
            rummagingAudio.Play();
            yield return new WaitForSeconds(9f);
            yield return StartCoroutine(UndoFadeOutCharacter(soldierObj));
        }
        if (dialogues[dialoguesIndex].action == 6)
        {
            yield return StartCoroutine(MoveCharacter(hegsethObj, 135f, -208f));
            yield return StartCoroutine(MoveCharacter(mainCharacterObj, 240f, -164f));
            yield return new WaitForSeconds(1f);
        }
        if (dialogues[dialoguesIndex].action == 7)
        {
            yield return StartCoroutine(MoveCharacter(hegsethObj, -379f, -208f));
        }
        if (dialogues[dialoguesIndex].action == 8)
        {
            yield return StartCoroutine(MoveCharacter(soldierObj, 424.25f, -131f));
        }

        //Move nameplate
        if (name == "Liam")
        {
            namePlate.GetComponent<RectTransform>().anchoredPosition = new Vector2(mainCharacterObj.GetComponent<RectTransform>().anchoredPosition.x, mainCharacterObj.GetComponent<RectTransform>().anchoredPosition.y - 152.8f);
        }
        else if (name == "Astrid")
        {
            namePlate.GetComponent<RectTransform>().anchoredPosition = new Vector2(astridObj.GetComponent<RectTransform>().anchoredPosition.x, astridObj.GetComponent<RectTransform>().anchoredPosition.y - 120f);
        }
        else if (name == "Hegseth")
        {
            namePlate.GetComponent<RectTransform>().anchoredPosition = new Vector2(hegsethObj.GetComponent<RectTransform>().anchoredPosition.x, hegsethObj.GetComponent<RectTransform>().anchoredPosition.y - 114f);
        }
        else if (name == "Soldier")
        {
            namePlate.GetComponent<RectTransform>().anchoredPosition = new Vector2(soldierObj.GetComponent<RectTransform>().anchoredPosition.x, soldierObj.GetComponent<RectTransform>().anchoredPosition.y - 186.63f);
        } 

        title.text = name;
        Vector2 targetPos = new Vector2(0f, 0f);
        Vector2 startPos = new Vector2(0f, -10f);
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
    IEnumerator FadeOutCharacter(GameObject character)
    {
        float duration = .5f;
        Color endColor = new Color(0f, 0f, 0f, 0f);
        float elapsedTime = 0f; 

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            character.GetComponent<Image>().color = Color.Lerp(new Color(1f, 1f, 1f, 1f), endColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

            character.GetComponent<Image>().color = endColor;
    }
    IEnumerator UndoFadeOutCharacter(GameObject character)
    {
        float duration = .5f;
        Color endColor = new Color(1f, 1f, 1f, 1f);
        float elapsedTime = 0f; 

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            character.GetComponent<Image>().color = Color.Lerp(new Color(0f, 0f, 0f, 0f), endColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

            character.GetComponent<Image>().color = endColor;
    }
    private struct CharacterDialogue
    {
        public string[] lines;
        public string title;
        public bool pauseExecutionBefore;
        public float dialoguePitch;
        public int action;
        public CharacterDialogue(int action, bool pauseExecutionBefore, string title, string[] lines)
        {
            this.action = action;
            this.lines = lines;
            this.title = title;
            this.pauseExecutionBefore = pauseExecutionBefore;


            if (title == "Liam")
            {
                this.dialoguePitch = 0.75f;
            }
            else if (title == "Hegseth")
            {
                this.dialoguePitch = 0.7f;
            }
            else
            {
                this.dialoguePitch = 1f;
            }
        }
    }



}
