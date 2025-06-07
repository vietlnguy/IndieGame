using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class IntroController: MonoBehaviour
{

    public GameObject mainChar;
    public GameObject astrid;
    public GameObject textBoxController;
    public float moveSpeed = 4f;
    public bool dialogueFinished = false;
    public bool secondDialogueFinished = false;
    public ArrowToggle arrow;
    public GameObject whiteScreen;
    public GameObject blackScreen;
    public GameObject sexScreen;
    public GameObject enemies;
    public GameObject fightScreen;
    public GameObject characters;
    public AudioSource swordClash;
    public AudioSource timpani;
    public MainPlayerController mpc;
    public TilemapPathfinder pathfinder;
    public AudioSource footstepAudio;
    public AudioSource doorAudio;
    void Start() {
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
    private IEnumerator introSequence()
    {
        yield return StartCoroutine(UndoFade(whiteScreen, 1f));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FollowPath(mainChar, new Vector3(-6.57f, -11.28f, 0f)));
        yield return new WaitForSeconds(.5f);
        doorAudio.Play();
        yield return StartCoroutine(characterDisappear(mainChar));
        textBoxController.GetComponent<DialogueSystem>().NextDialogue();
        while (!dialogueFinished)
        {
            yield return new WaitForSeconds(1f);
        }
        yield return StartCoroutine(FadeScreen(blackScreen, 1f));
        yield return StartCoroutine(FadeScreen(sexScreen, 1f));
        textBoxController.GetComponent<DialogueSystem>().NextDialogue();
        while (!secondDialogueFinished)
        {
            yield return new WaitForSeconds(1f);
        }
    }
    
    
}




