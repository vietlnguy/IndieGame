using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Controller: MonoBehaviour
{
    public GameObject pathfinderObj;
    public GameObject dialogueCanvas;
    public GameObject mainChar;
    public GameObject astrid;
    public GameObject celeste;
    public GameObject lucas;
    public GameObject gerard;
    public GameObject katherine;
    public GameObject penelope;
    public GameObject amara;
    public GameObject textBoxController;
    private float moveSpeed = 3f;
    private bool moveCameraRoutineRunning = false;
    private bool charactersMovingRoutineRunning = true;
    private bool startDialogueRoutineRunning = false;
    private bool arrowRoutineRunning = false;
    public bool dialogueFinished = false;
    private bool moveCameraFinished = false;
    public bool arrowFinished = false;
    public bool secondDialogueFinished = false;
    private bool secondDialogueRoutineRunning = false;
    private bool enemiesAppearFinished = false;
    private bool enemiesAppearRoutineRunning = false;
    private bool moveAmaraFinished = false;
    private bool moveAmaraRoutine = false;
    public GameObject arrow;

    void Start() {
        //Enter characters
        StartCoroutine(MoveAllCharacters());
    }

    void Update() {

        //Start intro dialogue
        if (!charactersMovingRoutineRunning && !startDialogueRoutineRunning && !dialogueFinished) {
            StartCoroutine(delayThenStartDialogue());
            startDialogueRoutineRunning = true;
        }

        //move camera to house
        else if (dialogueFinished && !moveCameraRoutineRunning && !moveCameraFinished) {
            StartCoroutine(moveCamera(-1f, -12f, -10f));
            moveCameraRoutineRunning = true;
        }

        //arrow indicator
        else if (moveCameraFinished && !arrowRoutineRunning && !arrowFinished) {
            arrow.GetComponent<ArrowToggle>().StartBlinking();
            arrowRoutineRunning = true;
        }

        else if (arrowFinished && !secondDialogueRoutineRunning && !secondDialogueFinished) {
            textBoxController.GetComponent<DialogueSystem>().NextDialogue();
            secondDialogueRoutineRunning = true;
        }
        else if (secondDialogueFinished && !moveAmaraRoutine && !moveAmaraFinished) {
            StartCoroutine(FollowPath(amara));
            moveAmaraRoutine = true;
        }
        //bring up amara dialogue
        //amara appears in front of house
        //run amara to +

        //blinker.StartBlinking();
        
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
    private IEnumerator delayThenStartDialogue() {
        yield return new WaitForSeconds(1f);
        textBoxController.GetComponent<DialogueSystem>().NextDialogue();
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
        moveCameraFinished = true;
    }
    private IEnumerator MoveAllCharacters() {
        StartCoroutine(MoveToPosition(astrid, -18f, -6.5f));
        StartCoroutine(MoveToPosition(mainChar, -17f, -4.5f));
        StartCoroutine(MoveToPosition(celeste, -19f, -3f));
        StartCoroutine(MoveToPosition(lucas, -22f, -4.7f));
        StartCoroutine(MoveToPosition(katherine, -20f, -4.6f));
        StartCoroutine(MoveToPosition(gerard, -21f, -7.4f));
        yield return StartCoroutine(MoveToPosition(penelope, -23f, -6.8f));
        charactersMovingRoutineRunning = false;
    }
    public IEnumerator FollowPath(GameObject character)
    {
        yield return new WaitForSeconds(2f);
        List<Vector3> path = pathfinderObj.GetComponent<TilemapPathfinder>().GetWorldPath(character.transform.position, new Vector3(-.3f, -.76f, 0f));
        foreach (Vector3 waypoint in path)
        {
            while (Vector3.Distance(character.transform.position, waypoint) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, waypoint, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
        moveAmaraFinished = true;
        moveAmaraRoutine = false;
    }


}




