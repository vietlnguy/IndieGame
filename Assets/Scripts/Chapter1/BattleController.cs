using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BattleController : MonoBehaviour
{
    public AttackRange attackRange;
    public Vector3 originalPosition;
    public GameObject characterSelected;
    public GameObject enemySelected;
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
    public GameObject characters;
    private int enemiesRemaining = 4;
    public GameObject pauseMenu;
    public GameObject saveMenu;
    public bool isPaused = false;
    public AttackPreview attackPreviewScript;
    public AudioSource walkingAudio;
    public Camera worldCamera;
    public RectTransform characterToolTipCanvasRect;
    public GameObject characterToolTip;
    public TextMeshProUGUI characterToolTipHp;
    public TextMeshProUGUI characterToolTipMaxHp;
    public TextMeshProUGUI characterToolTipMana;
    public TextMeshProUGUI characterToolTipMaxMana;
    public TilemapPathfinder pathfinder;
    public GameObject moveRangeCircle;

    void Awake()
    {
        disabledCharacters = new List<GameObject>();
        disabledEnemies = new List<GameObject>();
        worldCamera = Camera.main;

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
                    if (attackPreviewScript.active)
                    {
                        StartCoroutine(attackPreviewScript.disablePreview());
                    }
                    else
                    {
                        DeselectCharacter();
                        disableEndTurnUI();
                    }

                }
                else if (!endTurnUIActive && !disabledCharacters.Contains(characterSelected) && !attackPreviewScript.active)
                {
                    HandleMovement();
                }
            }
            if (enemySelected && !attackPreviewScript.active)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    disableMoveRange();
                    attackRange.disableAttackRange();
                }
            }

            HandleGameLoop();
        }


    }
    private void HandleGameLoop()
    {

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
        enemySelected = enemy;
        //Starting an Attack
        if (characterSelected)
        {
            if (enemiesInRange.Contains(enemy) && !attackPreviewScript.active)
            {
                StartCoroutine(attackPreviewScript.enablePreview(enemy));
            }

            //reset character and show enemy range
            else if (!enemiesInRange.Contains(enemy) && !attackPreviewScript.active)
            {
                DeselectCharacter();
                enableEnemyMoveRange();
                attackRange.enableAttackRange(enemy);
            }
        }
        else
        {
            //show enemy range
            enableEnemyMoveRange();
            attackRange.enableAttackRange(enemy);
        }
    }
    public void selectCharacter(GameObject character)
    {
        //Character is disabled
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
        //if another character is already selected
        else if (characterSelected != null && characterSelected != character)
        {
            DeselectCharacter();
            characterSelected = character;
            originalPosition = character.transform.position;
            characterSelected.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            characterSelected.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            enableMoveRange();
            attackRange.enableAttackRange(character);
        }
        //Select character
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

        if (Input.GetKey(KeyCode.W)) { direction.y += 1; if (!walkingAudio.isPlaying) { walkingAudio.Play(); } }
        if (Input.GetKey(KeyCode.S))
        {
            direction.y -= 1;
            if (!walkingAudio.isPlaying)
            {
                walkingAudio.Play();
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.x -= 1;
            if (!walkingAudio.isPlaying)
            {
                walkingAudio.Play();
            }
            Vector3 localScale = characterSelected.transform.localScale;
            localScale.x = -Mathf.Abs(localScale.x);
            characterSelected.transform.localScale = localScale;

        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x += 1;
            if (!walkingAudio.isPlaying)
            {
                walkingAudio.Play();
            }
            Vector3 localScale = characterSelected.transform.localScale;
            localScale.x = Mathf.Abs(localScale.x);
            characterSelected.transform.localScale = localScale;
        }

        if (Input.GetKeyUp(KeyCode.W)) { walkingAudio.Stop(); }
        if (Input.GetKeyUp(KeyCode.S)) { walkingAudio.Stop(); }
        if (Input.GetKeyUp(KeyCode.A)) { walkingAudio.Stop(); }
        if (Input.GetKeyUp(KeyCode.D)) { walkingAudio.Stop(); }

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
    public void disableEndTurnUI()
    {
        endTurnObj.SetActive(false);
        endTurnUIActive = false;
    }
    private void enableMoveRange()
    {
        moveRangeCircle.SetActive(true);
        moveRangeCircle.transform.position = characterSelected.transform.position;
        moveRangeCircle.GetComponent<MoveRangeCircle>().DrawFilledCircle(characterSelected.GetComponent<PlayerController>().moveRange);
    }
    private void disableMoveRange()
    {
        moveRangeCircle.SetActive(false);
    }
    private void enableEnemyMoveRange()
    {

        moveRangeCircle.SetActive(true);
        moveRangeCircle.transform.position = enemySelected.transform.position;
        moveRangeCircle.GetComponent<MoveRangeCircle>().DrawEnemyFilledCircle(enemySelected.GetComponent<EnemyController>().moveRange);

    }
    private System.Collections.IEnumerator FlashCoroutine()
    {
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
    public void endCharacterTurn()
    {
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
        yield return new WaitForSeconds(2f);
        
        foreach (Transform enemy in enemies.transform)
        {
            selectEnemy(enemy.gameObject);

            yield return new WaitForSeconds(2f);
            //If enemy roams
            if (enemy.gameObject.GetComponent<EnemyController>().roams)
            {
                GameObject target = GetClosest(enemy.gameObject, characters);
                yield return StartCoroutine(pathfinder.EnemyFollowPath(enemy.gameObject, target.transform.position));
            }

            disabledEnemies.Add(enemy.gameObject);
            yield return new WaitForSeconds(1.5f);
        }

        yield return null;

    }
    public void enableCharacterToolTip(GameObject character)
    {
        if (!attackPreviewScript.active)
        {
            characterToolTip.SetActive(true);
            try
            {
                characterToolTipHp.text = character.GetComponent<PlayerController>().hp.ToString();
                characterToolTipMaxHp.text = character.GetComponent<PlayerController>().maxHp.ToString();
                characterToolTipMana.text = character.GetComponent<PlayerController>().mana.ToString();
                characterToolTipMaxMana.text = character.GetComponent<PlayerController>().maxMana.ToString();
            }
            catch
            {
                characterToolTipHp.text = character.GetComponent<EnemyController>().hp.ToString();
                characterToolTipMaxHp.text = character.GetComponent<EnemyController>().maxHp.ToString();
                characterToolTipMana.text = character.GetComponent<EnemyController>().mana.ToString();
                characterToolTipMaxMana.text = character.GetComponent<EnemyController>().maxMana.ToString();
            }
            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(worldCamera, character.transform.position);

            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(characterToolTipCanvasRect, screenPos, worldCamera, out localPos);

            characterToolTip.GetComponent<RectTransform>().localPosition = localPos + new Vector2(-90f, 200f);
        }
    }
    public void disableCharacterToolTip()
    {
        characterToolTip.SetActive(false);
    }
    private GameObject GetClosest(GameObject target, GameObject objects)
    {
        GameObject closest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Transform character in objects.transform)
        {
            if (character.gameObject == null) continue;

            float distance = Vector3.Distance(target.transform.position, character.gameObject.transform.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closest = character.gameObject;
            }
        }

        return closest;
    }
}
