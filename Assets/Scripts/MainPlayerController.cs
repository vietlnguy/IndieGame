using UnityEngine;
using System.Collections.Generic;


public class MainPlayerController : MonoBehaviour
{
    private GameObject moveRange;
    private SpriteRenderer moveRangeSR;
    private EdgeCollider2D moveRangeCollider;
    private Camera mainCam;
    private PlayerController reporter;
    private Vector3 originalPosition;
    private GameObject characterSelected;
    private float moveSpeed = 8.0f;
    private GameObject endTurnObj;
    private bool endTurnUIActive = false;
    private List<GameObject> disabledCharacters;

    private int partySize;

    void Awake() 
    {
        disabledCharacters = new List<GameObject>();

        //TODO: partySize should read from the database
        partySize = 4;
    }
    void Start()
    {

        moveRange = GameObject.Find("MoveRange");
        moveRangeSR = moveRange.GetComponent<SpriteRenderer>();
        moveRangeCollider = moveRange.GetComponent<EdgeCollider2D>();
        mainCam = Camera.main;
        endTurnObj = GameObject.Find("End_Turn");
        
    }
    void Update()
    {
        HandleGameLoop();
        HandleMovement();
        HandleSelection();

    }
    private void HandleGameLoop() {
        if (disabledCharacters.Count == partySize) {
            //Play animation for enemy phase
            //Enemy turn

            foreach (var obj in disabledCharacters) {
                characterSelected = obj;
                characterSelected.GetComponent<Animator>().SetBool("isFrozen", false);
                characterSelected.GetComponent<Animator>().SetBool("isWalking", false);
                unhighlightSprite();
            }
            characterSelected = null;
            disabledCharacters.Clear();
        }
    }
    private void HandleSelection() {
        
        //On left click, check character select
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mouseWorld);

            //EndTurnUI is active and clicked YES/NO
            if (hit != null && endTurnUIActive && (hit.gameObject.tag == "end_turn_yes" || hit.gameObject.tag == "end_turn_no")) {
                if (hit.gameObject.tag == "end_turn_yes") {
                    endTurn();
                }
                else {
                    disableAttackRange();
                    disableMoveRange();
                    unhighlightSprite();
                    unlockOtherCharacterMovement();
                    disableEndTurnUI();
                    characterSelected.GetComponent<Animator>().SetBool("isWalking", false);
                    characterSelected.transform.position = originalPosition;
                    characterSelected = null;
                }
            }

            //EndTurnUI is active and clicked something else
            else if (endTurnUIActive) {
                StartCoroutine(FlashCoroutine());
            }  

            //No character selected yet and character isn't disabled
            else if (hit != null && hit.gameObject.tag == "character" && characterSelected == null && !disabledCharacters.Contains(hit.gameObject)) {
                characterSelected = GameObject.Find(hit.gameObject.name);
                originalPosition = characterSelected.transform.position;
                highLightSprite();
                lockOtherCharacterMovement();
                enableMoveRange();
                enableAttackRange();
            }
            
            //Same character is selected again
            else if (hit != null && hit.gameObject.tag == "character" && characterSelected != null && hit.gameObject.name == characterSelected.name){
                enableEndTurnUI();
            }

            //Existing character selection but clicked a different character that is not disabled
            else if (hit != null && hit.gameObject.tag == "character" && characterSelected != null && hit.gameObject.name != characterSelected.name && !disabledCharacters.Contains(hit.gameObject)){
                disableAttackRange();
                disableMoveRange();
                unhighlightSprite();
                unlockOtherCharacterMovement();
                disableEndTurnUI();
                characterSelected.transform.position = originalPosition;
                characterSelected = GameObject.Find(hit.gameObject.name);
                originalPosition = characterSelected.transform.position;
                lockOtherCharacterMovement();
                enableMoveRange();
                enableAttackRange();

            }

            /*
            //Existing character selection and clicked enemy or interactable
            else if (hit != null && hit.gameObject == enemyGameObj && isSelected){
                - show combat UI
                - start combat
            }
            */
            
            //Existing character selection, clicked anything else
            else {
                //Character was selected but hasn't moved then can click away
                if (characterSelected != null && characterSelected.transform.position == originalPosition) {
                    characterSelected.transform.position = originalPosition;    
                    disableMoveRange();
                    disableAttackRange();               
                    unlockOtherCharacterMovement();
                    unhighlightSprite();
                    characterSelected = null;
                }
            
                //Character was selected and has moved
                if (characterSelected != null && characterSelected.transform.position != originalPosition) {
                    enableEndTurnUI();
                }
             
            }
        }

        //On right click, if a character was selected then reset the selected character's position, untoggle attack range, move range and UI
        else if (Input.GetMouseButtonDown(1)) {
            if (characterSelected != null) {
                unhighlightSprite();
                disableAttackRange();
                disableMoveRange();
                unlockOtherCharacterMovement();
                disableEndTurnUI();
                characterSelected.transform.position = originalPosition;
                characterSelected = null;

            }
        }

    }
    private void HandleMovement()
    {
        if (characterSelected == null || endTurnUIActive) {
            return;
        }

        else {
            Vector3 direction = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) direction.y += 1;
            if (Input.GetKey(KeyCode.S)) direction.y -= 1;
            if (Input.GetKey(KeyCode.A)) direction.x -= 1;
            if (Input.GetKey(KeyCode.D)) direction.x += 1;

            if (direction != Vector3.zero)
            {
                characterSelected.transform.position += direction.normalized * moveSpeed * Time.deltaTime;
                characterSelected.GetComponent<Animator>().SetBool("isWalking", true);
            }
            else
            {
                characterSelected.GetComponent<Animator>().SetBool("isWalking", false);
            }
        }
    }
    private void lockOtherCharacterMovement() {
        foreach (Transform obj in this.transform) {
            if (obj.name != characterSelected.name) {
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }
    private void unlockOtherCharacterMovement() {
        foreach (Transform obj in transform) {
            if (obj.name != characterSelected.name) {
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
                rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            }
        }
    }
    public void OnChildMouseEnter(GameObject child) {
        //Highlight character on mouse enter if they are not disabled
        if (!disabledCharacters.Contains(child)) {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            sr.color = Color.yellow;
        }
    }
    public void OnChildMouseExit(GameObject child) {
        //Unhighlight character if they're not disabled and not selected
        if (!disabledCharacters.Contains(child)) {
            if (characterSelected == null) {
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                sr.color = Color.white;
            }
            else if (characterSelected != null && characterSelected != child) {
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                sr.color = Color.white;
            } 
        }
    }
    private void enableEndTurnUI() {
        if (characterSelected.transform.position.y > -4.5f) {
            endTurnObj.transform.position = new Vector3(characterSelected.transform.position.x - 0.3f, characterSelected.transform.position.y - 3.2f, characterSelected.transform.position.z);  
            endTurnUIActive = true;
        }
        else {        
            endTurnObj.transform.position = new Vector3(characterSelected.transform.position.x- 0.3f, characterSelected.transform.position.y + 2.6f, characterSelected.transform.position.z);  
            endTurnUIActive = true; 
        }

    }
    private void disableEndTurnUI() {
        endTurnObj.transform.position = new Vector3(characterSelected.transform.position.x + 100f, characterSelected.transform.position.y + 100f, characterSelected.transform.position.z);
        endTurnUIActive = false;
    }
    private void enableMoveRange() {
        moveRange.transform.position = characterSelected.transform.position;
        //TODO moveRange.transform.scale. Scale with character stats.
        moveRangeSR.enabled = true;
        moveRangeCollider.enabled = true;

    }
    private void disableMoveRange() {
        moveRange.transform.position = characterSelected.transform.position;
        moveRangeSR.enabled = false;
        moveRangeCollider.enabled = false;

    }
    private void enableAttackRange() {
        //TODO attackRange.transform.scale. Scale with character stats.
        SpriteRenderer attackRangeSR = characterSelected.transform.Find("AttackRange").GetComponent<SpriteRenderer>();
        attackRangeSR.enabled = true;
    }
    private void disableAttackRange() {
        //TODO attackRange.transform.scale. Scale with character stats.
        SpriteRenderer attackRangeSR = characterSelected.transform.Find("AttackRange").GetComponent<SpriteRenderer>();
        attackRangeSR.enabled = false;
    }
    private void highLightSprite() {
        SpriteRenderer sr = characterSelected.GetComponent<SpriteRenderer>();
        sr.color = Color.yellow;
    }
    private void unhighlightSprite() {
        SpriteRenderer sr = characterSelected.GetComponent<SpriteRenderer>();
        sr.color = Color.white;   
    }
    private System.Collections.IEnumerator FlashCoroutine() {
        SpriteRenderer endTurnSR = endTurnObj.GetComponent<SpriteRenderer>();
        float flashDuration = 0.1f;
        int flashCount = 3;
        float transparentAlpha = 0.35f;

        for (int i = 0; i < flashCount; i++)
        {
            // Fade out (transparent)
            endTurnSR.color = new Color(1, 1, 1, transparentAlpha);
            yield return new WaitForSeconds(flashDuration);

            // Fade in (opaque)
            endTurnSR.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(flashDuration);
        }
    }
    private void graySpriteAndFreeze() {
        SpriteRenderer sr = characterSelected.GetComponent<SpriteRenderer>();
        sr.color = Color.gray;                
        characterSelected.GetComponent<Animator>().SetBool("isFrozen", true);
    }
    private void endTurn() {
        disableAttackRange();
        disableMoveRange();
        graySpriteAndFreeze();
        disableEndTurnUI();
        unlockOtherCharacterMovement();                
        characterSelected.GetComponent<Animator>().SetBool("isFrozen", true);
        disabledCharacters.Add(characterSelected);
        characterSelected = null;
    }
}
