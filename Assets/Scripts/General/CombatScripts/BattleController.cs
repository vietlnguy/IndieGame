using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BattleController : MonoBehaviour
{

    public GameObject characterSelected;
    public GameObject assistableCharacterSelected;
    public GameObject enemySelected;
    public GameObject enemies;
    public GameObject characters;
    public List<GameObject> disabledCharacters;
    public List<GameObject> disabledEnemies;
    public bool introFinished = true;
    public GameObject fightScreen;
    public TextMeshProUGUI fightScreenText;
    public AudioSource playerPhaseAudio;
    public AudioSource enemyPhaseAudio;
    public GameObject pauseMenu;
    public GameObject saveMenu;
    public bool isPaused = false;
    public AttackPreview attackPreviewScript;
    public CharacterMenu characterMenuScript;
    public CharacterAssistMenu characterAssistMenuScript;
    public Camera worldCamera;
    public TilemapPathfinder pathfinder;
    public MoveRangeCircle moveRangeCircleScript;
    public AttackRangeCircle attackRangeCircleScript;
    public InventoryMenu inventoryMenuScript;
    public CharacterInfoScreen characterInfoScript;
    public bool isEnemyTurn = false;
    public int currentTurn = 0;
    void Awake()
    {
        disabledCharacters = new List<GameObject>();
        disabledEnemies = new List<GameObject>();
        worldCamera = Camera.main;

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
            if (!characterAssistMenuScript.active && !characterMenuScript.active && !attackPreviewScript.active && !inventoryMenuScript.active && !isEnemyTurn && !characterInfoScript.active)
            {
                if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Q))
                {
                    if (characterSelected)
                    {
                        characterSelected.GetComponent<PlayerController>().deselectCharacter();
                    }
                    else if (enemySelected)
                    {
                        enemySelected.GetComponent<EnemyController>().deselectEnemy();
                    }
                }
            }
            HandleGameLoop();
        }

    }
    private void HandleGameLoop()
    {
        //All characters are disabled and its not the enemies turn yet. Should start enemy turn
        if (disabledCharacters.Count == characters.transform.childCount && !isEnemyTurn)
        {
            isEnemyTurn = true;
            characterSelected = null;
            StartCoroutine(enemyTurn());
        }

        //Start player phase
        else if (disabledEnemies.Count == enemies.transform.childCount && isEnemyTurn)
        {
            isEnemyTurn = false;
            foreach (Transform enemy in enemies.transform)
            {
                enemy.gameObject.GetComponent<EnemyController>().unhighlight();
            }
            foreach (Transform character in characters.transform)
            {
                character.gameObject.GetComponent<PlayerController>().unhighlight();
            }
            disabledEnemies.Clear();
            disabledCharacters.Clear();
            StartCoroutine(playerTurn());
        }
    }
    private IEnumerator playerTurn()
    {
        currentTurn++;
        playerPhaseAudio.Play();
        fightScreenText.text = "Player Phase";
        fightScreen.GetComponent<CanvasGroup>().alpha = 1f;
        yield return new WaitForSeconds(2.5f);
        fightScreen.GetComponent<CanvasGroup>().alpha = 0f;
        
    }
    private IEnumerator enemyTurn()
    {
        enemyPhaseAudio.Play();
        fightScreenText.text = "Enemy Phase";
        fightScreen.GetComponent<CanvasGroup>().alpha = 1f;
        yield return new WaitForSeconds(2.5f);
        fightScreen.GetComponent<CanvasGroup>().alpha = 0f;

        yield return new WaitForSeconds(2f);

        foreach (Transform enemy in enemies.transform)
        {
            pathfinder.calculateOccupiedTiles(characters, enemies);

            enemy.GetComponent<EnemyController>().selectEnemy();

            yield return new WaitForSeconds(2f);

            if (enemy.gameObject.GetComponent<EnemyController>().roams)
            {
                GameObject target = GetClosest(enemy.gameObject, characters);
                yield return StartCoroutine(pathfinder.EnemyFollowPath(enemy.gameObject, target.transform.position));
            }

            disabledEnemies.Add(enemy.gameObject);
            enemy.gameObject.GetComponent<EnemyController>().graySpriteAndFreeze();
        }

        attackRangeCircleScript.disableAttackRange();
        moveRangeCircleScript.disableMoveRange();

        yield return null;

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
