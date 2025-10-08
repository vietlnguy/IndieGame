using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BattleController : MonoBehaviour
{

    public GameObject characterSelected;
    public GameObject enemySelected;
    public List<GameObject> disabledCharacters;
    private List<GameObject> disabledEnemies;
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
    public CharacterMenu characterMenuScript;
    public Camera worldCamera;
    public TilemapPathfinder pathfinder;
    public MoveRangeCircle moveRangeCircleScript;
    public bool isEnemyTurn = false;
    public bool characterMenuActive = false;
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
            if (Input.GetMouseButtonDown(1))
            {
                if (characterMenuScript.active)
                {
                    characterMenuScript.disableCharacterMenu();
                    characterSelected.GetComponent<PlayerController>().movementEnabled = true;
                }
                else if (characterSelected)
                {
                    characterSelected.GetComponent<PlayerController>().deselectCharacter();
                }
            }
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
    //public void endCharacterTurn()
    //{
    //    moveRangeCircleScript.disableMoveRange();

    //    characterSelected.GetComponent<PlayerController>().graySpriteAndFreeze();
    //    disableEndTurnUI();
    //    characterSelected.GetComponent<Animator>().SetBool("isFrozen", true);
    //    disabledCharacters.Add(characterSelected);
    //    characterSelected = null;
    //}
    private IEnumerator phaseTransition(string s)
    {
        if (s == "Player")
        {
            isEnemyTurn = false;
            playerPhaseAudio.Play();
            fightScreenText.text = "Player Phase";
            fightScreen.GetComponent<CanvasGroup>().alpha = 1f;
            yield return new WaitForSeconds(2.5f);
            fightScreen.GetComponent<CanvasGroup>().alpha = 0f;
        }
        else
        {
            isEnemyTurn = true;
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
            pathfinder.calculateOccupiedTiles(characters, enemies);
            
            yield return new WaitForSeconds(2f);

            if (enemy.gameObject.GetComponent<EnemyController>().roams)
            {
                GameObject target = GetClosest(enemy.gameObject, characters);
                yield return StartCoroutine(pathfinder.EnemyFollowPath(enemy.gameObject, target.transform.position));
            }

            disabledEnemies.Add(enemy.gameObject);
        }


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
