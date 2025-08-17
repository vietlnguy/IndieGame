using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class IntroController: MonoBehaviour
{

    public GameObject mainChar;
    public GameObject astrid;
    public GameObject DialogueController;
    public GameObject fullBodyCharacterParent;
    public float moveSpeed = 4f;
    public bool dialogueFinished = false;
    public bool secondDialogueFinished = false;
    public bool thirdDialogueFinished = false;
    public bool fourthDialogueFinished = false;
    public bool fifthDialogueFinished = false;
    public bool sixthDialogueFinished = false;
    public ArrowToggle arrow;
    public GameObject whiteScreen;
    public GameObject blackScreen;
    public GameObject sexScreen;
    public GameObject houseScreen;
    public GameObject enemies;
    public GameObject fightScreen;
    public TextMeshProUGUI fightScreenText;
    public GameObject characters;
    public AudioSource swordClash;
    public AudioSource timpani;
    public MainPlayerController mpc;
    public TilemapPathfinder pathfinder;
    public AudioSource footstepAudio;
    public AudioSource doorAudio;
    public AudioSource artifactShineAudio;
    public AudioSource forestBattleTheme;
    public AudioSource playerPhaseAudio;
    public MainPlayerController mainPlayerController;
    public SaveManager saveManager;

    void Awake()
    {
        saveManager = FindFirstObjectByType<SaveManager>();
    }
    void Start()
    {
        saveManager.introBattleOutro = "intro";
        StartCoroutine(introSequence());  
    }
    void Update() {
 
    }
    private IEnumerator characterAppear(GameObject character) {
        SpriteRenderer spriteRenderer = character.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        float duration = .15f;
        Color startColor = Color.black;
        Color endColor = Color.white;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            spriteRenderer.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        spriteRenderer.color = endColor;
        yield return new WaitForSeconds(1f);
    }
    private IEnumerator characterDisappear(GameObject character) {
        SpriteRenderer spriteRenderer = character.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        float duration = .15f;
        Color startColor = Color.white;
        Color endColor = Color.black;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            spriteRenderer.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        spriteRenderer.color = endColor;
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(1f);
    }
    private IEnumerator FadeScreen(GameObject obj, float duration){
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        float startAlpha = 0f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
    private IEnumerator UndoFade(GameObject obj, float duration){
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        float startAlpha = 1f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
    private void renderEnemies(GameObject enemies) {
        foreach (Transform child in enemies.transform)
        {
            child.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    private void enableAllColliders() {
        foreach (Transform child in enemies.transform)
        {
            child.gameObject.GetComponent<Rigidbody2D>().simulated = true;
        }
        foreach (Transform child in mpc.transform)
        {
            child.gameObject.GetComponent<Rigidbody2D>().simulated = true;
        }
    }
    private IEnumerator FollowPath(GameObject character, Vector3 targetPos) {
        List<Vector3> path = pathfinder.GetWorldPath(character.transform.position, targetPos);
        footstepAudio.Play();
        foreach (Vector3 waypoint in path)
        {
            while (Vector3.Distance(character.transform.position, waypoint) > 0.1f)
            {
                character.transform.position = Vector3.MoveTowards(character.transform.position, waypoint, moveSpeed * Time.deltaTime);
                yield return null;
            }

        }
        footstepAudio.Stop();
    }
    private void disableCharacterImages()
    {
        for (int i = 0; i < fullBodyCharacterParent.transform.childCount; i++)
        {
            Transform childTransform = fullBodyCharacterParent.transform.GetChild(i);
            GameObject childGameObject = childTransform.gameObject;
            Image childImage = childGameObject.GetComponent<Image>();
            if (childImage != null)
            {
                childImage.enabled = false;
            }
        }
    }
    private void enableEnemySprites()
    {
        for (int i = 0; i < enemies.transform.childCount; i++)
        {
            Transform childTransform = enemies.transform.GetChild(i);
            GameObject childGameObject = childTransform.gameObject;
            SpriteRenderer child = childGameObject.GetComponent<SpriteRenderer>();
            child.enabled = true;
        }
    }
    private void enableCharacterImages()
    {
        for (int i = 0; i < fullBodyCharacterParent.transform.childCount; i++)
        {
            Transform childTransform = fullBodyCharacterParent.transform.GetChild(i);
            GameObject childGameObject = childTransform.gameObject;
            Image childImage = childGameObject.GetComponent<Image>();
            if (childImage != null)
            {
                childImage.enabled = true;
            }
        }
    }
    private IEnumerator introSequence()
    {
        
        //Overworld movement and dialogue
        yield return StartCoroutine(UndoFade(whiteScreen, 1f));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FollowPath(mainChar, new Vector3(-6.57f, -11.28f, 0f)));
        yield return new WaitForSeconds(.5f);
        doorAudio.Play();
        yield return StartCoroutine(characterDisappear(mainChar));
        DialogueController.GetComponent<SmallDialogue>().NextDialogue();
        while (!dialogueFinished)
        {
            yield return new WaitForSeconds(1f);
        }

        //scene
        yield return StartCoroutine(FadeScreen(blackScreen, 1f));
        sexScreen.GetComponent<CanvasGroup>().alpha = 1f;
        yield return StartCoroutine(UndoFade(blackScreen, 1f));
        DialogueController.GetComponent<SmallDialogue>().NextDialogue();
        while (!secondDialogueFinished)
        {
            yield return new WaitForSeconds(1f);
        }

        //In house dialogue
        yield return StartCoroutine(FadeScreen(blackScreen, 2f));
        sexScreen.GetComponent<CanvasGroup>().alpha = 0f;
        houseScreen.GetComponent<CanvasGroup>().alpha = 1f;
        yield return StartCoroutine(UndoFade(blackScreen, 1f));
        DialogueController.GetComponent<FullBodyDialogue>().NextDialogue();
        while (!thirdDialogueFinished)
        {
            yield return new WaitForSeconds(1f);
        }

        //Overworld artifact scene and dialogue
        yield return StartCoroutine(FadeScreen(blackScreen, 2f));
        houseScreen.GetComponent<CanvasGroup>().alpha = 0f;
        disableCharacterImages();
        yield return StartCoroutine(UndoFade(blackScreen, 2f));
        artifactShineAudio.Play();
        yield return new WaitForSeconds(4f);
        DialogueController.GetComponent<SmallDialogue>().NextDialogue();
        while (!fourthDialogueFinished)
        {
            yield return new WaitForSeconds(1f);
        }

        //In house dialogue 2
        yield return StartCoroutine(FadeScreen(blackScreen, 2f));
        houseScreen.GetComponent<CanvasGroup>().alpha = 1f;
        enableCharacterImages();
        yield return StartCoroutine(UndoFade(blackScreen, 1f));
        DialogueController.GetComponent<FullBodyDialogue>().NextDialogue();
        while (!fifthDialogueFinished)
        {
            yield return new WaitForSeconds(1f);
        }
        
        //Overworld
        yield return StartCoroutine(FadeScreen(blackScreen, 2f));
        houseScreen.GetComponent<CanvasGroup>().alpha = 0f;
        enableEnemySprites();
        disableCharacterImages();
        yield return StartCoroutine(UndoFade(blackScreen, 2f));
        doorAudio.Play();
        yield return StartCoroutine(characterAppear(mainChar));
        yield return new WaitForSeconds(.5f);
        yield return StartCoroutine(FollowPath(mainChar, new Vector3(-7.53f, -12f, 0f)));
        astrid.transform.position = new Vector3(-6.57f, -11.28f, 0f);
        astrid.GetComponent<SpriteRenderer>().enabled = true;
        yield return StartCoroutine(characterAppear(astrid));
        yield return StartCoroutine(FollowPath(astrid, new Vector3(-5.66f, -12f, 0f)));
        yield return new WaitForSeconds(.5f);

        fightScreen.GetComponent<CanvasGroup>().alpha = 1f;
        swordClash.Play();
        forestBattleTheme.Play();
        yield return new WaitForSeconds(1.5f);
        fightScreen.GetComponent<CanvasGroup>().alpha = 0f;

        
        DialogueController.GetComponent<SmallDialogue>().NextDialogue();   
        while (!sixthDialogueFinished)
        {
            yield return new WaitForSeconds(1f);
        }
        

        //TODO: Tutorial steps

        yield return new WaitForSeconds(2f);
        playerPhaseAudio.Play();
        fightScreenText.text = "Player Phase";
        fightScreen.GetComponent<CanvasGroup>().alpha = 1f;
        yield return new WaitForSeconds(2.5f);
        fightScreen.GetComponent<CanvasGroup>().alpha = 0f;

        mainPlayerController.introFinished = true;
        saveManager.introBattleOutro = "battle";
    }
    
}




