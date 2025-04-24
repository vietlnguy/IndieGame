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
    private GameObject enemySelected;
    private float moveSpeed = 8.0f;
    private GameObject endTurnObj;
    private bool endTurnUIActive = false;
    private List<GameObject> disabledCharacters;
    private List<GameObject> disabledEnemies;
    private bool allowMovement = false;
    private int partySize;

    void Awake() 
    {
        disabledCharacters = new List<GameObject>();
        disabledEnemies = new List<GameObject>();

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
                unhighlightSprite(characterSelected);
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

            //If endTurnUI is active then cannot click anything else
            if (endTurnUIActive) {
                //Didn't click yes or no
                if (hit == null || hit.gameObject.tag != "end_turn_yes" || hit.gameObject.tag != "end_turn_yes") {
                    StartCoroutine(FlashCoroutine());             
                }
                //End turn if yes
                else if (hit.gameObject.tag == "end_turn_yes") {
                    endTurn();
                }
                //Reset character if no
                else if (hit.gameObject.tag == "end_turn_no") {
                    disableAttackRange(characterSelected);
                    disableMoveRange();
                    unhighlightSprite(characterSelected);
                    unlockOtherCharacterMovement();
                    disableEndTurnUI();
                    characterSelected.GetComponent<Animator>().SetBool("isWalking", false);
                    characterSelected.transform.position = originalPosition;
                    characterSelected = null;
                }
            }
            
            //Clicked something else
            else {
                //Clicked a character/enemy
                if (hit != null && (hit.gameObject.tag == "character" || hit.gameObject.tag == "enemy")) {
                    //Clicked a player character
                    if (hit.gameObject.tag == "character") {
                        if (enemySelected != null) {
                            unhighlightSprite(enemySelected);
                            disableAttackRange(enemySelected);
                            enemySelected = null;
                        }
                        //No character selected yet
                        if (characterSelected == null) {
                            characterSelected = hit.gameObject;
                            originalPosition = characterSelected.transform.position;
                            highLightSprite(characterSelected);
                            lockOtherCharacterMovement();
                            enableMoveRange();
                            enableAttackRange(characterSelected);
                            showCharacterInfo(characterSelected);
                            if (!disabledCharacters.Contains(characterSelected)) { allowMovement = true; }
                            else { allowMovement = false; }
                        }
                        //Character selected already and clicked again
                        else if (hit.gameObject == characterSelected) {
                            enableEndTurnUI();
                        }
                        //Character selected, but clicked a different character
                        else if (hit.gameObject != characterSelected) {
                            disableAttackRange(characterSelected);
                            disableMoveRange();
                            unhighlightSprite(characterSelected);
                            unlockOtherCharacterMovement();
                            characterSelected.transform.position = originalPosition;
                            characterSelected = hit.gameObject;
                            originalPosition = characterSelected.transform.position;
                            lockOtherCharacterMovement();
                            enableMoveRange();
                            enableAttackRange(characterSelected);
                            showCharacterInfo(characterSelected);
                            if (!disabledCharacters.Contains(characterSelected)) { allowMovement = true; }
                            else { allowMovement = false; }
                        }
                    }
                    //Clicked an enemy
                    else if (hit.gameObject.tag == "enemy") {
                        //Character already selected
                        if (characterSelected) {
                            List<GameObject> enemiesInRange = transform.Find(characterSelected.name + "/AttackRange").GetComponent<AttackRange>().enemiesInRange;
                            if (!enemiesInRange.Contains(hit.gameObject)) {
                                characterSelected.transform.position = originalPosition;    
                                disableMoveRange();
                                disableAttackRange(characterSelected);               
                                unlockOtherCharacterMovement();
                                unhighlightSprite(characterSelected);
                                characterSelected = null;
                                enemySelected = hit.gameObject;
                                enableAttackRange(enemySelected);
                                highLightSprite(enemySelected);
                                showCharacterInfo(enemySelected);
                            }
                            else {
                                Debug.Log("can attack");
                            }                    
                        }
                        //Enemy already selected
                        else if (enemySelected) {
                            allowMovement = false;
                            unhighlightSprite(enemySelected);
                            disableAttackRange(enemySelected);
                            enemySelected = hit.gameObject;
                            highLightSprite(enemySelected);
                            enableAttackRange(enemySelected);
                            showCharacterInfo(enemySelected);    
                        }
                        //Neither character or enemy selected
                        else {
                            allowMovement = false;
                            enemySelected = hit.gameObject;
                            highLightSprite(enemySelected);
                            enableAttackRange(enemySelected);
                            showCharacterInfo(enemySelected);
                        }
                    }
                }
                //Clicked an interactable
                else if (hit != null && hit.gameObject.tag == "interactable") {

                }
                //Didn't click character, enemy, or interactable
                else {
                    //Character previously selected but hasn't moved
                    if (characterSelected != null && characterSelected.transform.position == originalPosition) {
                        characterSelected.transform.position = originalPosition;    
                        disableMoveRange();
                        disableAttackRange(characterSelected);               
                        unlockOtherCharacterMovement();
                        unhighlightSprite(characterSelected);
                        characterSelected = null;
                    }
                    //Character was selected and has moved
                    else if (characterSelected != null && characterSelected.transform.position != originalPosition) {
                        enableEndTurnUI();
                    }
                    //Enemy previously selected
                    else if (enemySelected != null) {
                        unhighlightSprite(enemySelected);
                        disableAttackRange(enemySelected);
                        enemySelected = null;
                    }
                    
                }
            }
        }
        //On right click, if a character was selected then reset the selected character's position, untoggle attack range, move range and UI
        else if (Input.GetMouseButtonDown(1)) {
            if (characterSelected != null) {
                unhighlightSprite(characterSelected);
                disableAttackRange(characterSelected);
                disableMoveRange();
                unlockOtherCharacterMovement();
                disableEndTurnUI();
                characterSelected.transform.position = originalPosition;
                characterSelected.GetComponent<Animator>().SetBool("isWalking", false);
                characterSelected = null;
            }
            else if (enemySelected != null) {
                disableAttackRange(enemySelected);  
                unhighlightSprite(enemySelected);
                enemySelected = null;
            }
        }

    }
    private void HandleMovement()
    {
        if (characterSelected == null || endTurnUIActive || !allowMovement) {
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
        //Highlight enemy and enemy isn't disabled
        if (child.tag == "enemy" && !disabledEnemies.Contains(child)) {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            sr.color = new Color32(255,50,50,255);
        }

        //Highlight character on mouse enter if they are not disabled
        else if (child.tag == "character" && !disabledCharacters.Contains(child)) {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            sr.color = new Color32(100, 100, 255, 255);
        }
    }
    public void OnChildMouseExit(GameObject child) {
        //Unhighlight character if they're not disabled and not selected
        if (child.tag == "enemy" && !disabledEnemies.Contains(child)) {
            if (enemySelected == null) {
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                sr.color = Color.white;
            }
            else if (enemySelected != null && enemySelected != child) {
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                sr.color = Color.white;
            } 
        }

        else if (child.tag == "character" && !disabledCharacters.Contains(child)) {
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
    private void enableAttackRange(GameObject obj) {
        //TODO attackRange.transform.scale. Scale with character stats.
        SpriteRenderer attackRangeSR = obj.transform.Find("AttackRange").GetComponent<SpriteRenderer>();
        attackRangeSR.enabled = true;
    }
    private void disableAttackRange(GameObject obj) {
        //TODO attackRange.transform.scale. Scale with character stats.
        SpriteRenderer attackRangeSR = obj.transform.Find("AttackRange").GetComponent<SpriteRenderer>();
        attackRangeSR.enabled = false;
    }
    private void highLightSprite(GameObject obj) {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (obj.tag == "enemy") {
            sr.color = new Color32(255,50,50,255);
        }
        else {
            sr.color = new Color32(100, 100, 255, 255);
        }
    }
    private void unhighlightSprite(GameObject obj) {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
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
        disableAttackRange(characterSelected);
        disableMoveRange();
        graySpriteAndFreeze();
        disableEndTurnUI();
        unlockOtherCharacterMovement();                
        characterSelected.GetComponent<Animator>().SetBool("isFrozen", true);
        disabledCharacters.Add(characterSelected);
        characterSelected = null;
    }
    private void showCharacterInfo(GameObject obj) {
        Debug.Log("show charcter info");
    }






}
