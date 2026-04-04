using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BattleController : MonoBehaviour
{
    public Camera worldCamera;
    public GameObject characterSelected;
    public GameObject assistableCharacterSelected;
    public GameObject enemySelected;
    public GameObject enemies;
    public GameObject characters;
    public GameObject pauseMenu;
    public GameObject saveMenu;
    public GameObject fightScreen;
    public GameObject settingsMenu;
    public GameObject exitGameMenu;
    public GameObject exitMainMenu;
    public GameObject enemyTarget = null;
    public TextMeshProUGUI currentTurnText;
    public List<GameObject> disabledCharacters;
    public List<GameObject> disabledEnemies;
    public TextMeshProUGUI fightScreenText;
    public AudioSource playerPhaseAudio;
    public AudioSource enemyPhaseAudio;
    public bool introFinished = false;
    public bool isPaused = false;
    public bool isEnemyTurn = false;
    private bool hoverableEnabled = false;
    public TilemapPathfinder pathfinder;
    private AttackPreview attackPreviewScript;
    private CharacterMenu characterMenuScript;
    private CharacterAssistMenu characterAssistMenuScript;
    private MoveRangeCircle moveRangeCircleScript;
    private AttackRangeCircle attackRangeCircleScript;
    private EffectiveAttackRangeCircle effectiveAttackRangeCircleScript;
    private InventoryMenu inventoryMenuScript;
    private CharacterInfoScreen characterInfoScript;
    private GameOver gameOverScript;
    private int currentTurn = 1;
    private Coroutine enemyFollowCoroutine;
    public AudioSource walkingAudio;
    public GameObject victoryAndSubquestBox;
    private float cameraSpeed = 10f;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public bool neutralParty;
    private bool isNeutralTurn = false;
    
    void Awake()
    {
        disabledCharacters = new List<GameObject>();
        disabledEnemies = new List<GameObject>();
        worldCamera = Camera.main;

    }
    void Start()
    {
        attackPreviewScript = GameObject.Find("AttackPreview").GetComponent<AttackPreview>();
        characterMenuScript = GameObject.Find("CharacterMenu").GetComponent<CharacterMenu>();
        characterAssistMenuScript = GameObject.Find("CharacterAssistMenu").GetComponent<CharacterAssistMenu>();
        moveRangeCircleScript = GameObject.Find("MoveRangeCircle").GetComponent<MoveRangeCircle>();
        attackRangeCircleScript = GameObject.Find("AttackRangeCircle").GetComponent<AttackRangeCircle>();
        effectiveAttackRangeCircleScript = GameObject.Find("EffectiveAttackRangeCircle").GetComponent<EffectiveAttackRangeCircle>();
        inventoryMenuScript = GameObject.Find("InventoryMenu").GetComponent<InventoryMenu>();
        characterInfoScript = GameObject.Find("CharacterInfoScreen").GetComponent<CharacterInfoScreen>();
        gameOverScript = GameObject.Find("GameOverScreen").GetComponent<GameOver>();
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
                settingsMenu.SetActive(false);
                exitGameMenu.SetActive(false);
                exitMainMenu.SetActive(false);
                isPaused = false;
            }
            else
            {
                pauseMenu.SetActive(true);
                isPaused = true;
            }
        }

        if (introFinished && !isPaused && !gameOverScript.active)
        {
            if (!hoverableEnabled)
            {
                EnableHover();
                hoverableEnabled = true;
            }

            if (!characterAssistMenuScript.active && !characterMenuScript.active && !attackPreviewScript.active && !inventoryMenuScript.active && !isEnemyTurn && !characterInfoScript.active)
            {
                if (characterSelected == null && enemySelected == null)
                {
                    MoveCamera();
                }

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
    void MoveCamera()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); // Cleaner than KeyCode.W/A/S/D
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(moveX, moveY, 0f).normalized;
        
        if (moveDirection.magnitude > 0) 
        {
            Vector3 targetPosition = worldCamera.transform.position + moveDirection * cameraSpeed * Time.deltaTime;
            
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

            worldCamera.transform.position = targetPosition;
        }
    }
    private void HandleGameLoop()
    {
        int ownedCharacters = 0;
        foreach (Transform t in characters.transform)
        {
            if (t.gameObject.GetComponent<PlayerController>().owned)
            {
                ownedCharacters++;
            }
        }

        //All characters are disabled and its not the enemies turn yet. Should start enemy turn
        if (disabledCharacters.Count == ownedCharacters && neutralParty && !isNeutralTurn)
        {
            characterSelected = null;
            isNeutralTurn = true;
            StartCoroutine(neutralTurn());
        }

        //All characters are disabled and its not the enemies turn yet. Should start enemy turn
        else if (disabledCharacters.Count == ownedCharacters && !isEnemyTurn)
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
        playerPhaseAudio.Play();
        UpdateTurnNumber();
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

        //Execute each enemy's turn
        foreach (Transform enemy in enemies.transform)
        {
            Vector3 safeDestination = enemy.position;
            safeDestination.x = Mathf.Clamp(safeDestination.x, minX, maxX);
            safeDestination.y = Mathf.Clamp(safeDestination.y, minY, maxY);
            safeDestination.z = -10f;
            yield return StartCoroutine(Helpers.MoveTransform(worldCamera.transform, worldCamera.transform.position, safeDestination, 1f));

            EnemyController enemyScript = enemy.gameObject.GetComponent<EnemyController>();
            pathfinder.calculateOccupiedTiles(characters, enemies);
            enemyScript.selectEnemy();
            enemyTarget = null;

            yield return new WaitForSeconds(1f);

            if (enemyScript.roams)
            {

                //Enemy attacks player characters
                if (!enemyScript.support)
                {             
                    //if no characters within the EffectiveAttackRange then should move towards closest enemy
                    if (effectiveAttackRangeCircleScript.enemiesInRange.Count <= 0) 
                    { 
                        enemyTarget = GetClosest(enemy.gameObject, characters, null);
                        yield return StartCoroutine(pathfinder.EnemyFollowPath(enemy.gameObject, enemyTarget.transform.position));
                        walkingAudio.Stop();
                        Debug.Log("No one in range to attack");
                    }

                    //Characters within EffectiveAttackRange. Should go down attack priority list to determine target.
                    else if (effectiveAttackRangeCircleScript.enemiesInRange.Count > 0)
                    {
                        //Go through each character and see if any can be killed
                        foreach (GameObject character in effectiveAttackRangeCircleScript.enemiesInRange)
                        {
                            PlayerController playerScript = character.GetComponent<PlayerController>();

                            foreach (AttackMoves attack in enemyScript.knownAttacks)
                            {
                                //Calculate damage
                                int[] damageArray = attackPreviewScript.calculateDamage(enemy.gameObject, character, attack);
                                
                                //If can kill
                                if (damageArray[0] >= playerScript.currentHp && attack.manaCost <= enemyScript.currentMana)
                                {
                                    enemyTarget = character;
                                    break;
                                }

                                if (enemyTarget != null) {break;}
                            }
                        }

                        //Go through each character and see if any can be ranged attacked
                        if (enemyTarget == null && enemyScript.ranged) 
                        {
                            foreach (GameObject character in effectiveAttackRangeCircleScript.enemiesInRange)
                            {
                                PlayerController playerScript = character.GetComponent<PlayerController>();
                                if (!playerScript.ranged)
                                {
                                    enemyTarget = character;
                                    break;
                                }
                            }
                        }

                        //Go through each character and see if any can be melee attacked
                        if (enemyTarget == null && !enemyScript.ranged)
                        {
                            foreach (GameObject character in effectiveAttackRangeCircleScript.enemiesInRange)
                            {
                                PlayerController playerScript = character.GetComponent<PlayerController>();
                                if (playerScript.ranged)
                                {
                                    enemyTarget = character;
                                    break;
                                }
                            }
                        }
                        
                        //Else attack the closest
                        if (enemyTarget == null)
                        {
                            enemyTarget = GetClosest(enemy.gameObject, characters, null);
                        }
                    
                        //When target is not already in range, then move towards the target
                        if (!attackRangeCircleScript.enemiesInRange.Contains(enemyTarget)) {
                              
                            //Ranged enemies should stop movement as soon as within range to attack
                            if (enemyScript.ranged) { attackRangeCircleScript.enemyIsRangedAndMoving = true; }
                            else { attackRangeCircleScript.enemyIsRangedAndMoving = false; }

                            yield return StartCoroutine(pathfinder.EnemyFollowPath(enemy.gameObject, enemyTarget.transform.position));
                            walkingAudio.Stop();
                        }

                        yield return new WaitForSeconds(1f);
                        //TODO: attack indicator?

                        AttackMoves attackMove = null;

                        //Try to get killing move (mana allowing)
                        foreach (AttackMoves attack in enemyScript.knownAttacks)
                        {
                            //Calculate damage
                            int[] damageArray = attackPreviewScript.calculateDamage(enemy.gameObject, enemyTarget, attack);
                            
                            //If can kill
                            if (damageArray[0] >= enemyTarget.GetComponent<PlayerController>().currentHp && attack.manaCost <= enemyScript.currentMana)
                            {
                                attackMove = attack;
                                break;
                            }
                        }

                        //Else get most damage move (mana allowing)
                        if (attackMove == null)
                        {
                            int highestDamage = attackPreviewScript.calculateDamage(enemy.gameObject, enemyTarget, enemyScript.knownAttacks[0])[0];
                            attackMove = enemyScript.knownAttacks[0];
                            foreach (AttackMoves attack in enemyScript.knownAttacks)
                            {
                                //Calculate damage
                                int[] damageArray = attackPreviewScript.calculateDamage(enemy.gameObject, enemyTarget, enemyScript.knownAttacks[0]);

                                if (damageArray[0] > highestDamage && attack.manaCost <= enemyScript.currentMana)
                                {
                                    attackMove = attack;
                                }
                            }
                        }

                        yield return StartCoroutine(attackPreviewScript.startEnemyAttackSequence(enemy.gameObject, enemyTarget, attackMove));

                    }
                    
                }

                //Enemy supports other enemies
                else if (enemyScript.support)
                {

                }

                //Enemy supports and attacks
                else if (enemyScript.hybrid)
                {
                    
                }
       
            }
            
            //Does roam, but still attacks those in Effective attack range. i.e. sentinels
            else
            {
                if (effectiveAttackRangeCircleScript.enemiesInRange.Count != 0)
                {
                    //Go through each character and see if any can be killed
                    foreach (GameObject character in effectiveAttackRangeCircleScript.enemiesInRange)
                    {
                        PlayerController playerScript = character.GetComponent<PlayerController>();

                        foreach (AttackMoves attack in enemyScript.knownAttacks)
                        {
                            //Calculate damage
                            int[] damageArray = attackPreviewScript.calculateDamage(enemy.gameObject, character, attack);
                            
                            //If can kill
                            if (damageArray[0] >= playerScript.currentHp && attack.manaCost <= enemyScript.currentMana)
                            {
                                enemyTarget = character;
                                break;
                            }

                            if (enemyTarget != null) {break;}
                        }
                    }

                    //Go through each character and see if any can be ranged attacked
                    if (enemyTarget == null && enemyScript.ranged) 
                    {
                        foreach (GameObject character in effectiveAttackRangeCircleScript.enemiesInRange)
                        {
                            PlayerController playerScript = character.GetComponent<PlayerController>();
                            if (!playerScript.ranged)
                            {
                                enemyTarget = character;
                                break;
                            }
                        }
                    }

                    //Go through each character and see if any can be melee attacked
                    if (enemyTarget == null && !enemyScript.ranged)
                    {
                        foreach (GameObject character in effectiveAttackRangeCircleScript.enemiesInRange)
                        {
                            PlayerController playerScript = character.GetComponent<PlayerController>();
                            if (playerScript.ranged)
                            {
                                enemyTarget = character;
                                break;
                            }
                        }
                    }
                    
                    //Else attack the closest
                    if (enemyTarget == null)
                    {
                        enemyTarget = GetClosest(enemy.gameObject, null, effectiveAttackRangeCircleScript.enemiesInRange);
                    }
                
                    if (!attackRangeCircleScript.enemiesInRange.Contains(enemyTarget)) {
                            
                        //Ranged enemies should stop movement as soon as within range to attack
                        if (enemyScript.ranged) { attackRangeCircleScript.enemyIsRangedAndMoving = true; }
                        else { attackRangeCircleScript.enemyIsRangedAndMoving = false; }

                        yield return StartCoroutine(pathfinder.EnemyFollowPath(enemy.gameObject, enemyTarget.transform.position));
                        walkingAudio.Stop();
                    }

                    yield return new WaitForSeconds(1f);
                    //TODO: attack indicator?

                    AttackMoves attackMove = null;

                    //Try to get killing move (mana allowing)
                    foreach (AttackMoves attack in enemyScript.knownAttacks)
                    {
                        //Calculate damage
                        int[] damageArray = attackPreviewScript.calculateDamage(enemy.gameObject, enemyTarget, attack);
                        
                        //If can kill
                        if (damageArray[0] >= enemyTarget.GetComponent<PlayerController>().currentHp && attack.manaCost <= enemyScript.currentMana)
                        {
                            attackMove = attack;
                            break;
                        }
                    }

                    //Else get most damage move (mana allowing)
                    if (attackMove == null)
                    {
                        int highestDamage = attackPreviewScript.calculateDamage(enemy.gameObject, enemyTarget, enemyScript.knownAttacks[0])[0];
                        attackMove = enemyScript.knownAttacks[0];
                        foreach (AttackMoves attack in enemyScript.knownAttacks)
                        {
                            //Calculate damage
                            int[] damageArray = attackPreviewScript.calculateDamage(enemy.gameObject, enemyTarget, enemyScript.knownAttacks[0]);

                            if (damageArray[0] > highestDamage && attack.manaCost <= enemyScript.currentMana)
                            {
                                attackMove = attack;
                            }
                        }
                    }
 
                    yield return StartCoroutine(attackPreviewScript.startEnemyAttackSequence(enemy.gameObject, enemyTarget, attackMove));
                }
            }

            disabledEnemies.Add(enemy.gameObject);
            enemyScript.deselectEnemy();
            enemy.gameObject.GetComponent<EnemyController>().graySpriteAndFreeze();
            yield return new WaitForSeconds(1.5f);
        }


    }
    private IEnumerator neutralTurn()
    {
        fightScreenText.text = "Neutral Phase";
        fightScreen.GetComponent<CanvasGroup>().alpha = 1f;
        yield return new WaitForSeconds(2.5f);
        fightScreen.GetComponent<CanvasGroup>().alpha = 0f;
        yield return new WaitForSeconds(2f);

        foreach (Transform character in characters.transform)
        {
            PlayerController characterScript = character.gameObject.GetComponent<PlayerController>();
            if (characterScript.owned)
            {
                Vector3 safeDestination = character.transform.position;
                safeDestination.x = Mathf.Clamp(safeDestination.x, minX, maxX);
                safeDestination.y = Mathf.Clamp(safeDestination.y, minY, maxY);
                safeDestination.z = -10f;
                yield return StartCoroutine(Helpers.MoveTransform(worldCamera.transform, worldCamera.transform.position, safeDestination, 0.5f));

                characterScript.selectCharacter();
                
                if (characterScript.roams)
                {

                    if (!characterScript.support)
                    {             
                        //if no characters within the EffectiveAttackRange then should move towards closest enemy
                        if (effectiveAttackRangeCircleScript.enemiesInRange.Count <= 0) 
                        { 
                            enemyTarget = GetClosest(character.gameObject, enemies, null);
                            yield return StartCoroutine(pathfinder.FollowPath(character.gameObject, enemyTarget.transform.position));
                            walkingAudio.Stop();
                            Debug.Log("No one in range to attack");
                        }

                        //Characters within EffectiveAttackRange. Should go down attack priority list to determine target.
                        else if (effectiveAttackRangeCircleScript.enemiesInRange.Count > 0)
                        {
                            //Go through each character and see if any can be killed
                            foreach (GameObject enemy in effectiveAttackRangeCircleScript.enemiesInRange)
                            {
                                PlayerController playerScript = character.GetComponent<PlayerController>();

                                foreach (AttackMoves attack in characterScript.knownAttacks)
                                {
                                    //Calculate damage
                                    int[] damageArray = attackPreviewScript.calculateDamage(character.gameObject, character, attack);
                                    
                                    //If can kill
                                    if (damageArray[0] >= playerScript.currentHp && attack.manaCost <= characterScript.currentMana)
                                    {
                                        enemyTarget = character;
                                        break;
                                    }

                                    if (enemyTarget != null) {break;}
                                }
                            }

                            //Go through each character and see if any can be ranged attacked
                            if (enemyTarget == null && characterScript.ranged) 
                            {
                                foreach (GameObject character in effectiveAttackRangeCircleScript.enemiesInRange)
                                {
                                    PlayerController playerScript = character.GetComponent<PlayerController>();
                                    if (!playerScript.ranged)
                                    {
                                        enemyTarget = character;
                                        break;
                                    }
                                }
                            }

                            //Go through each character and see if any can be melee attacked
                            if (enemyTarget == null && !characterScript.ranged)
                            {
                                foreach (GameObject character in effectiveAttackRangeCircleScript.enemiesInRange)
                                {
                                    PlayerController playerScript = character.GetComponent<PlayerController>();
                                    if (playerScript.ranged)
                                    {
                                        enemyTarget = character;
                                        break;
                                    }
                                }
                            }
                            
                            //Else attack the closest
                            if (enemyTarget == null)
                            {
                                enemyTarget = GetClosest(enemy.gameObject, characters, null);
                            }
                        
                            //When target is not already in range, then move towards the target
                            if (!attackRangeCircleScript.enemiesInRange.Contains(enemyTarget)) {
                                
                                //Ranged enemies should stop movement as soon as within range to attack
                                if (characterScript.ranged) { attackRangeCircleScript.enemyIsRangedAndMoving = true; }
                                else { attackRangeCircleScript.enemyIsRangedAndMoving = false; }

                                yield return StartCoroutine(pathfinder.EnemyFollowPath(enemy.gameObject, enemyTarget.transform.position));
                                walkingAudio.Stop();
                            }

                            yield return new WaitForSeconds(1f);
                            //TODO: attack indicator?

                            AttackMoves attackMove = null;

                            //Try to get killing move (mana allowing)
                            foreach (AttackMoves attack in characterScript.knownAttacks)
                            {
                                //Calculate damage
                                int[] damageArray = attackPreviewScript.calculateDamage(enemy.gameObject, enemyTarget, attack);
                                
                                //If can kill
                                if (damageArray[0] >= enemyTarget.GetComponent<PlayerController>().currentHp && attack.manaCost <= characterScript.currentMana)
                                {
                                    attackMove = attack;
                                    break;
                                }
                            }

                            //Else get most damage move (mana allowing)
                            if (attackMove == null)
                            {
                                int highestDamage = attackPreviewScript.calculateDamage(enemy.gameObject, enemyTarget, characterScript.knownAttacks[0])[0];
                                attackMove = characterScript.knownAttacks[0];
                                foreach (AttackMoves attack in characterScript.knownAttacks)
                                {
                                    //Calculate damage
                                    int[] damageArray = attackPreviewScript.calculateDamage(enemy.gameObject, enemyTarget, characterScript.knownAttacks[0]);

                                    if (damageArray[0] > highestDamage && attack.manaCost <= characterScript.currentMana)
                                    {
                                        attackMove = attack;
                                    }
                                }
                            }

                            yield return StartCoroutine(attackPreviewScript.startEnemyAttackSequence(enemy.gameObject, enemyTarget, attackMove));

                        }
                        
                    }

                    //Enemy supports other enemies
                    else if (characterScript.support)
                    {

                    }

                    //Enemy supports and attacks
                    else if (characterScript.hybrid)
                    {
                        
                    }
        
                }
                
                //Does roam, but still attacks those in Effective attack range. i.e. sentinels
                else
                {
                    if (effectiveAttackRangeCircleScript.enemiesInRange.Count != 0)
                    {
                        //Go through each character and see if any can be killed
                        foreach (GameObject character in effectiveAttackRangeCircleScript.enemiesInRange)
                        {
                            PlayerController playerScript = character.GetComponent<PlayerController>();

                            foreach (AttackMoves attack in characterScript.knownAttacks)
                            {
                                //Calculate damage
                                int[] damageArray = attackPreviewScript.calculateDamage(enemy.gameObject, character, attack);
                                
                                //If can kill
                                if (damageArray[0] >= playerScript.currentHp && attack.manaCost <= characterScript.currentMana)
                                {
                                    enemyTarget = character;
                                    break;
                                }

                                if (enemyTarget != null) {break;}
                            }
                        }

                        //Go through each character and see if any can be ranged attacked
                        if (enemyTarget == null && characterScript.ranged) 
                        {
                            foreach (GameObject character in effectiveAttackRangeCircleScript.enemiesInRange)
                            {
                                PlayerController playerScript = character.GetComponent<PlayerController>();
                                if (!playerScript.ranged)
                                {
                                    enemyTarget = character;
                                    break;
                                }
                            }
                        }

                        //Go through each character and see if any can be melee attacked
                        if (enemyTarget == null && !characterScript.ranged)
                        {
                            foreach (GameObject character in effectiveAttackRangeCircleScript.enemiesInRange)
                            {
                                PlayerController playerScript = character.GetComponent<PlayerController>();
                                if (playerScript.ranged)
                                {
                                    enemyTarget = character;
                                    break;
                                }
                            }
                        }
                        
                        //Else attack the closest
                        if (enemyTarget == null)
                        {
                            enemyTarget = GetClosest(enemy.gameObject, null, effectiveAttackRangeCircleScript.enemiesInRange);
                        }
                    
                        if (!attackRangeCircleScript.enemiesInRange.Contains(enemyTarget)) {
                                
                            //Ranged enemies should stop movement as soon as within range to attack
                            if (characterScript.ranged) { attackRangeCircleScript.enemyIsRangedAndMoving = true; }
                            else { attackRangeCircleScript.enemyIsRangedAndMoving = false; }

                            yield return StartCoroutine(pathfinder.EnemyFollowPath(enemy.gameObject, enemyTarget.transform.position));
                            walkingAudio.Stop();
                        }

                        yield return new WaitForSeconds(1f);
                        //TODO: attack indicator?

                        AttackMoves attackMove = null;

                        //Try to get killing move (mana allowing)
                        foreach (AttackMoves attack in characterScript.knownAttacks)
                        {
                            //Calculate damage
                            int[] damageArray = attackPreviewScript.calculateDamage(enemy.gameObject, enemyTarget, attack);
                            
                            //If can kill
                            if (damageArray[0] >= enemyTarget.GetComponent<PlayerController>().currentHp && attack.manaCost <= characterScript.currentMana)
                            {
                                attackMove = attack;
                                break;
                            }
                        }

                        //Else get most damage move (mana allowing)
                        if (attackMove == null)
                        {
                            int highestDamage = attackPreviewScript.calculateDamage(enemy.gameObject, enemyTarget, characterScript.knownAttacks[0])[0];
                            attackMove = characterScript.knownAttacks[0];
                            foreach (AttackMoves attack in characterScript.knownAttacks)
                            {
                                //Calculate damage
                                int[] damageArray = attackPreviewScript.calculateDamage(enemy.gameObject, enemyTarget, characterScript.knownAttacks[0]);

                                if (damageArray[0] > highestDamage && attack.manaCost <= characterScript.currentMana)
                                {
                                    attackMove = attack;
                                }
                            }
                        }
    
                        yield return StartCoroutine(attackPreviewScript.startEnemyAttackSequence(enemy.gameObject, enemyTarget, attackMove));
                    }
                }

                disabledEnemies.Add(enemy.gameObject);
                characterScript.deselectEnemy();
                enemy.gameObject.GetComponent<EnemyController>().graySpriteAndFreeze();
                yield return new WaitForSeconds(1.5f);



            }
        }
    }
    private GameObject GetClosest(GameObject target, GameObject objects, List<GameObject> listOfObjects)
    {
        GameObject closest = null;
        float shortestDistance = Mathf.Infinity;
        
        if (objects != null)
        {
            foreach (Transform character in objects.transform)
            {
                if (character.gameObject == null || character.gameObject == target) continue;

                float distance = Vector3.Distance(target.transform.position, character.gameObject.transform.position);

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closest = character.gameObject;
                }
            }     
        }
        else
        {
            foreach (GameObject character in listOfObjects)
            {
                if (character == null || character == target) continue;

                float distance = Vector3.Distance(target.transform.position, character.transform.position);

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closest = character;
                }
            }
        }

        return closest;
    }
    public void CancelEveryting()
    {
        StopAllCoroutines();
    }
    private void UpdateTurnNumber()
    {
        currentTurn++;
        currentTurnText.text = currentTurn.ToString();
    }
    private void EnableHover()
    {
        foreach (Transform character in characters.transform)
        {
            character.gameObject.GetComponent<PlayerController>().hoverable = true;
        }
    }
    public void StartCombat()
    {
        introFinished = true;
        victoryAndSubquestBox.SetActive(true);
        foreach (Transform enemy in enemies.transform)
        {
            disabledEnemies.Add(enemy.gameObject);
        }
        isEnemyTurn = true;

    }
}
