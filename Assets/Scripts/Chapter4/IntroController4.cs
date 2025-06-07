using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class IntroController4: MonoBehaviour
{

    public GameObject mainChar;
    public GameObject astrid;
    public GameObject celeste;
    public GameObject lucas;
    public GameObject gerard;
    public GameObject katherine;
    public GameObject penelope;
    public GameObject amara;
    public GameObject textBoxController;
    public TilemapPathfinder pathfinder;
    public float moveSpeed = 6f;
    public bool dialogueFinished = false;
    public bool secondDialogueFinished = false;
    public bool thirdDialogueFinished = false;
    public bool fourthDialogueFinished = false;
    public ArrowToggle arrow;
    public AudioSource footstepAudio;
    public GameObject blackScreen;
    public GameObject enemies;
    public GameObject fightScreen;
    public GameObject characters;
    public AudioSource swordClash;
    public AudioSource timpani;
    public MainPlayerController mpc;
    void Start() {
        StartCoroutine(introSequence());
    }
    void Update() {
 
    }
    private IEnumerator MoveToPosition(GameObject obj, float x, float y, float speed = 5f) {
        yield return new WaitForSeconds(2f);
        Vector3 targetPos = new Vector3(x, y, 0);
        
        Animator animator = obj.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }

    while (Vector3.Distance(obj.transform.position, targetPos) > 0.01f)
    {
        obj.transform.position = Vector3.MoveTowards(
            obj.transform.position,
            targetPos,
            speed * Time.deltaTime
        );
        yield return null;
    }

        if (animator != null)
        {
            animator.SetBool("isWalking", false); // Assume there's an "Idle" animation to return to
        }
    }
    private IEnumerator moveCamera(float x, float y, float z)
    {   
        Vector3 destination = new Vector3(x, y, z);
        Transform cam = GameObject.Find("Main Camera").GetComponent<Transform>();
        float duration = 1.5f;
        Vector3 startPos = cam.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cam.position = Vector3.Lerp(startPos, destination, elapsed / duration);
            yield return null;
        }

        cam.position = destination; // Ensure final position is exact

    }
    private IEnumerator MoveAllCharacters() {

        StartCoroutine(MoveToPosition(astrid, -18f, -6.5f));
        StartCoroutine(MoveToPosition(mainChar, -17f, -4.5f));
        StartCoroutine(MoveToPosition(celeste, -19f, -3f));
        StartCoroutine(MoveToPosition(lucas, -22f, -4.7f));
        StartCoroutine(MoveToPosition(katherine, -20f, -4.6f));
        StartCoroutine(MoveToPosition(gerard, -21f, -7.4f));
        yield return new WaitForSeconds(2f);
        footstepAudio.Play();
        yield return StartCoroutine(MoveToPosition(penelope, -23f, -6.8f));
        footstepAudio.Stop();

    }
    private IEnumerator FollowPath(GameObject character, Vector3 targetPos) {
        List<Vector3> path = pathfinder.GetWorldPath(character.transform.position, targetPos);
        footstepAudio.Play();
        foreach (Vector3 waypoint in path)
        {
            while (Vector3.Distance(character.transform.position, waypoint) > 0.05f)
            {
                character.transform.position = Vector3.MoveTowards(character.transform.position, waypoint, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
        footstepAudio.Stop();
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
    private IEnumerator FadeToBlack(GameObject obj){
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        float duration = 1f;
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
    private IEnumerator UndoFadeToBlack(GameObject obj){
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        float duration = 1f;
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
    private IEnumerator introSequence() {
        yield return StartCoroutine(MoveAllCharacters());
        yield return new WaitForSeconds(1f);
        textBoxController.GetComponent<DialogueSystem>().NextDialogue();
        while (!dialogueFinished) {
            yield return new WaitForSeconds(1f);
        }
        yield return StartCoroutine(moveCamera(-1f, -12f, -10f));
        yield return StartCoroutine(arrow.Blink());
        textBoxController.GetComponent<DialogueSystem>().NextDialogue();
        while (!secondDialogueFinished) {
            yield return new WaitForSeconds(1f);
        }
        arrow.ToggleOff();
        yield return StartCoroutine(characterAppear(amara));
        yield return StartCoroutine(FollowPath(amara, new Vector3(-0.1f, -1.6f, 0f)));
        yield return StartCoroutine(characterDisappear(amara));
        textBoxController.GetComponent<DialogueSystem>().NextDialogue();
        while (!thirdDialogueFinished) {
            yield return new WaitForSeconds(1f);
        }
        yield return StartCoroutine(FadeToBlack(blackScreen));
        GameObject.Find("Main Camera").GetComponent<Transform>().position = new Vector3(11f, -7f, -10f);
        GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize = 13f;
        renderEnemies(enemies);
        yield return StartCoroutine(UndoFadeToBlack(blackScreen));
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(FadeToBlack(blackScreen));
        GameObject.Find("Main Camera").GetComponent<Transform>().position = new Vector3(-6f, -5f, -10f);
        GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize = 11f;
        yield return StartCoroutine(UndoFadeToBlack(blackScreen));
        textBoxController.GetComponent<DialogueSystem>().NextDialogue();
        while (!fourthDialogueFinished) {
            yield return new WaitForSeconds(1f);
        }
        fightScreen.GetComponent<CanvasGroup>().alpha = 1f;
        swordClash.Play();
        timpani.Play();
        yield return new WaitForSeconds(3f);
        fightScreen.GetComponent<CanvasGroup>().alpha = 0f;
        mpc.introFinished = true;
        enableAllColliders();
    }
    
    
}




