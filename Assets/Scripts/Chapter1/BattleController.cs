using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BattleController : MonoBehaviour
{
    public GameObject moveRange;
    public AttackRange attackRange;
    public Vector3 originalPosition;
    public GameObject characterSelected;
    private GameObject enemySelected;
    public float moveSpeed = 6.0f;
    public GameObject endTurnObj;
    private bool endTurnUIActive = false;
    public List<GameObject> disabledCharacters;
    private List<GameObject> disabledEnemies;
    public List<GameObject> enemiesInRange;
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
    public GameObject saveMenu;
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
                saveMenu.SetActive(false);
                isPaused = false;
            }
            else
            {
                pauseMenu.SetActive(true);
                isPaused = true;
            }
        }

        if (introFinished && !isPaused)
        {
            if (characterSelected)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    DeselectCharacter();
                    disableEndTurnUI();
                }
                else if (!endTurnUIActive && !disabledCharacters.Contains(characterSelected))
                {
                    HandleMovement();
                }
            }

            HandleGameLoop();
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
    public void selectEnemy(GameObject enemy)
    {
        if (characterSelected)
        {
            if (enemiesInRange.Contains(enemy))
            {
                Debug.Log("can attack");
            }

        }
    }
    public void selectCharacter(GameObject character)
    {
        if (disabledCharacters.Contains(character))
        {
            //should only bring up character info
        }
        //Same character clicked again.
        else if (characterSelected == character)
        {
            enableEndTurnUI();
            return;
        }
        //check if another character is already selected
        else if (characterSelected != null && characterSelected != character)
        {
            DeselectCharacter();
        }

        else
        {
            characterSelected = character;
            originalPosition = character.transform.position;
            characterSelected.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            characterSelected.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            enableMoveRange();
            attackRange.enableAttackRange(character);
        }

    }
    public void DeselectCharacter()
    {
        //reset position and freeze
        characterSelected.transform.position = originalPosition;
        characterSelected.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        attackRange.disableAttackRange();
        disableMoveRange();
        characterSelected = null;
        
    }
    private void HandleMovement()
    {
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
    private void enableEndTurnUI()
    {
        endTurnObj.SetActive(true);
        if (characterSelected.transform.position.y > -4.5f)
        {
            endTurnObj.transform.position = new Vector3(characterSelected.transform.position.x - 0.3f, characterSelected.transform.position.y - 3.2f, characterSelected.transform.position.z);
            endTurnUIActive = true;
        }
        else
        {
            endTurnObj.transform.position = new Vector3(characterSelected.transform.position.x - 0.3f, characterSelected.transform.position.y + 2.6f, characterSelected.transform.position.z);
            endTurnUIActive = true;
        }
        
    }
    public void disableEndTurnUI() {
        endTurnObj.SetActive(false);
        endTurnUIActive = false;
    }
    private void enableMoveRange() {

        moveRange.SetActive(true);
        float moveRangeScale = characterSelected.GetComponent<PlayerController>().moveRange;
        moveRange.transform.localScale = new Vector3(moveRangeScale, moveRangeScale, moveRangeScale);
        moveRange.transform.position = characterSelected.transform.position;

    }
    private void disableMoveRange() {
        moveRange.SetActive(false);
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
    private void graySpriteAndFreeze()
    {
        SpriteRenderer sr = characterSelected.GetComponent<SpriteRenderer>();
        sr.color = Color.gray;
        characterSelected.GetComponent<Animator>().SetBool("isFrozen", true);
        characterSelected.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

    }
    public void endCharacterTurn() {
        disableMoveRange();
        attackRange.disableAttackRange();
        graySpriteAndFreeze();
        disableEndTurnUI();             
        characterSelected.GetComponent<Animator>().SetBool("isFrozen", true);
        disabledCharacters.Add(characterSelected);
        characterSelected = null;
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
        }
        else
        {
            enemyPhaseAudio.Play();
            fightScreenText.text = "Enemy Phase";
            fightScreen.GetComponent<CanvasGroup>().alpha = 1f;
            yield return new WaitForSeconds(2.5f);
            fightScreen.GetComponent<CanvasGroup>().alpha = 0f;
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




}
