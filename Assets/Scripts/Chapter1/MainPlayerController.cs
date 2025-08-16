using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MainPlayerController : MonoBehaviour
{
    public GameObject moveRange;
    private Vector3 originalPosition;
    private GameObject characterSelected;
    private GameObject enemySelected;
    public float moveSpeed = 8.0f;
    public GameObject endTurnObj;
    private bool endTurnUIActive = false;
    private List<GameObject> disabledCharacters;
    private List<GameObject> disabledEnemies;
    private bool allowMovement = false;
    private int partySize;
    public bool introFinished = true;
    public GameObject fightScreen;
    public TextMeshProUGUI fightScreenText;
    public AudioSource playerPhaseAudio;
    public AudioSource enemyPhaseAudio;
    public bool liamDefeated = false;
    public bool astridDefeated = false;
    public GameObject enemies;
    private int enemiesRemaining = 4;
    public GameObject pauseMenu;
    public bool isPaused = false;
    void Awake()
    {
        disabledCharacters = new List<GameObject>();
        disabledEnemies = new List<GameObject>();

        //TODO: partySize should read from the database
        partySize = 2;
    }
    void Start()
    {
    }
    void Update()
    {
        //On ESC press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                pauseMenu.SetActive(false);
                isPaused = false;
            }
            else
            {
                pauseMenu.SetActive(true);
                isPaused = true;
            }
        }
        if (introFinished && !isPaused) {
            HandleGameLoop();
            HandleMovement();
            HandleSelection();
        }
    }
    private void HandleGameLoop() {

        //Win condition
        if (enemiesRemaining == 0)
        {
            //start outro
        }

        //Lose condition
        else if (liamDefeated || astridDefeated)
        {
            //death dialogue
            //retry menu
            //restart fight
        }

        //Start enemy phase
        else if (disabledCharacters.Count == partySize)
        {
            foreach (var obj in disabledCharacters)
            {
                characterSelected = obj;
                characterSelected.GetComponent<Animator>().SetBool("isFrozen", false);
                characterSelected.GetComponent<Animator>().SetBool("isWalking", false);
                unhighlightSprite(characterSelected);
            }
            characterSelected = null;
            disabledCharacters.Clear();

            StartCoroutine(phaseTransition("Enemy"));
            StartCoroutine(enemyTurn());
        }

        //Start player phase
        else if (disabledEnemies.Count == enemiesRemaining)
        {

            disabledEnemies.Clear();
            StartCoroutine(phaseTransition("Player"));
        }
    }
    private void HandleSelection() {
        
        //On left click, check character select
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mouseWorld);

            //If endTurnUI is active then cannot click anything else
            if (endTurnUIActive)
            {
                if (hit == null)
                {
                    StartCoroutine(FlashCoroutine());
                }
                else if (hit.gameObject.tag == "end_turn_yes")
                {
                    endTurn();
                }
                else if (hit.gameObject.tag == "end_turn_no")
                {
                    disableAttackRange(characterSelected);
                    disableMoveRange();
                    unhighlightSprite(characterSelected);
                    unlockOtherCharacterMovement();
                    disableEndTurnUI();
                    characterSelected.GetComponent<Animator>().SetBool("isWalking", false);
                    characterSelected.transform.position = originalPosition;
                    characterSelected = null;
                }
                else
                {
                    StartCoroutine(FlashCoroutine());
                }
            }

            else
            {
                //Clicked a character/enemy
                if (hit != null && (hit.gameObject.tag == "character" || hit.gameObject.tag == "enemy"))
                {
                    //Clicked a player character
                    if (hit.gameObject.tag == "character")
                    {
                        if (enemySelected != null)
                        {
                            unhighlightSprite(enemySelected);
                            disableAttackRange(enemySelected);
                            enemySelected = null;
                        }
                        //No character selected yet
                        if (characterSelected == null)
                        {
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
                        else if (hit.gameObject == characterSelected)
                        {
                            enableEndTurnUI();
                        }
                        //Character selected, but clicked a different character
                        else if (hit.gameObject != characterSelected)
                        {
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
                    else if (hit.gameObject.tag == "enemy")
                    {
                        //Character already selected
                        if (characterSelected)
                        {
                            List<GameObject> enemiesInRange = transform.Find(characterSelected.name + "/AttackRange").GetComponent<AttackRange>().enemiesInRange;
                            if (!enemiesInRange.Contains(hit.gameObject))
                            {
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
                            else
                            {
                                Debug.Log("can attack");
                            }
                        }
                        //Enemy already selected
                        else if (enemySelected)
                        {
                            allowMovement = false;
                            unhighlightSprite(enemySelected);
                            disableAttackRange(enemySelected);
                            enemySelected = hit.gameObject;
                            highLightSprite(enemySelected);
                            enableAttackRange(enemySelected);
                            showCharacterInfo(enemySelected);
                        }
                        //Neither character or enemy selected
                        else
                        {
                            allowMovement = false;
                            enemySelected = hit.gameObject;
                            highLightSprite(enemySelected);
                            enableAttackRange(enemySelected);
                            showCharacterInfo(enemySelected);
                        }
                    }
                }
                //Clicked an interactable
                else if (hit != null && hit.gameObject.tag == "interactable")
                {

                }
                //Didn't click character, enemy, or interactable
                else
                {
                    //Character previously selected but hasn't moved
                    if (characterSelected != null && characterSelected.transform.position == originalPosition)
                    {
                        characterSelected.transform.position = originalPosition;
                        disableMoveRange();
                        disableAttackRange(characterSelected);
                        unlockOtherCharacterMovement();
                        unhighlightSprite(characterSelected);
                        characterSelected = null;
                    }
                    //Character was selected and has moved
                    else if (characterSelected != null && characterSelected.transform.position != originalPosition)
                    {
                        enableEndTurnUI();
                    }
                    //Enemy previously selected
                    else if (enemySelected != null)
                    {
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
        if (introFinished) {
            //Highlight enemy and enemy isn't disabled
            if (child.tag == "enemy" && !disabledEnemies.Contains(child))
            {
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                sr.color = new Color32(255, 50, 50, 255);
            }

            //Highlight character on mouse enter if they are not disabled
            else if (child.tag == "character" && !disabledCharacters.Contains(child))
            {
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                sr.color = new Color32(100, 100, 255, 255);
            }
        }
    }
    public void OnChildMouseExit(GameObject child) {
        if (introFinished) {
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
        moveRange.GetComponent<SpriteRenderer>().enabled = true;
        moveRange.GetComponent<EdgeCollider2D>().enabled = true;

    }
    private void disableMoveRange() {
        moveRange.transform.position = characterSelected.transform.position;
        moveRange.GetComponent<SpriteRenderer>().enabled = false;
        moveRange.GetComponent<EdgeCollider2D>().enabled = false;

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
        Debug.Log("flash called");
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
    private void freezePositions(string s)
    {
        if (s == "Enemies")
        {
            for (int i = 0; i < enemies.transform.childCount; i++)
            {
                Transform childTransform = enemies.transform.GetChild(i);
                GameObject childGameObject = childTransform.gameObject;
                childGameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            }
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform childTransform = transform.GetChild(i);
                GameObject childGameObject = childTransform.gameObject;
                childGameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                childGameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform childTransform = transform.GetChild(i);
                GameObject childGameObject = childTransform.gameObject;
                childGameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            }
            for (int i = 0; i < enemies.transform.childCount; i++)
            {
                Transform childTransform = transform.GetChild(i);
                GameObject childGameObject = childTransform.gameObject;
                childGameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                childGameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }
    private IEnumerator phaseTransition(string s)
    {
        if (s == "Player")
        {
            playerPhaseAudio.Play();
            fightScreenText.text = "Player Phase";
            fightScreen.GetComponent<CanvasGroup>().alpha = 1f;
            yield return new WaitForSeconds(2.5f);
            fightScreen.GetComponent<CanvasGroup>().alpha = 0f;
            freezePositions("Enemies");
        }
        else
        {
            enemyPhaseAudio.Play();
            fightScreenText.text = "Enemy Phase";
            fightScreen.GetComponent<CanvasGroup>().alpha = 1f;
            yield return new WaitForSeconds(2.5f);
            fightScreen.GetComponent<CanvasGroup>().alpha = 0f;
            freezePositions("Characters");
        }
        yield return null;
    }
    private IEnumerator enemyTurn()
    {
        yield return new WaitForSeconds(4f); //remove later
        for (int i = 0; i < enemies.transform.childCount; i++)
        {
            Transform childTransform = enemies.transform.GetChild(i);
            GameObject childGameObject = childTransform.gameObject;

            //TODO: move and attack

            disabledEnemies.Add(childGameObject);
        }

        yield return null;

    }
    private struct GameInfo
    {
        public int totalEnemies;

    }




}
