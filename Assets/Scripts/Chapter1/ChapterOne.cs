using UnityEngine;
using System.Collections;

[DefaultExecutionOrder(-1)]
public class ChapterOne : MonoBehaviour {

    public GameObject basicEnemyPrefab;
    private GameObject enemies;
    public BattleController battleController;
    private bool enemiesSpawned = false;
    private bool shouldLose = false;
    private bool gameOver = false;
    private bool victorySequenceStarted = false;
    private SaveManager saveManager;
    public GameOver gameOverScript;
    public AttackPreview attackPreviewScript;
    public SubquestsBox subquestsBoxScript;
    public VictorySequence victorySequence;
    public GameObject mainCharacterPrefab;
    public GameObject astridPrefab;
    private GameObject characters;
    private PlayerController astridScript;
    private TilemapPathfinder pathfinder;
    
    
    public Image whiteScreen;
    public AudioSource fluteAudio;

    public void Awake()
    {    
    
        saveManager = FindFirstObjectByType<SaveManager>();
        pathfinder = FindFirstObjectByType<TilemapPathfinder>();
        characters = GameObject.Find("Characters");
        enemies = GameObject.Find("Enemies");

        foreach (Character character in saveManager.loadedData.characters)
        {
            if (character.characterName == saveManager.loadedData.mainCharacterName)
            {
                Instantiate(mainCharacterPrefab, new Vector3(-20f, -9.35f), Quaternion.identity, characters.transform);
            }
            else if (character.characterName == "Astrid")
            {
                GameObject temp = Instantiate(astridPrefab, new Vector3(-4.45f, -11.65f), Quaternion.identity, characters.transform);
                astridScript = GameObject.Find("AstridPrefab(Clone)").GetComponent<PlayerController();

            }
        }

        victorySequence = FindFirstObjectByType<VictorySequence>();
        victorySequence.subquests.Add(astridScript.subquests[0]);

        //intro = StartCoroutine(Intro());
    }
    public void Update()
    {
        //Intro typing control
        if (intro != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isTyping)
                {
                    StopCoroutine(typingCoroutine);
                    typingCoroutine = null;
                    smallDialogueTextBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = lineToBeTyped;
                    typingAudio.Stop();
                    isTyping = false;
                }
                else
                {
                    nextLine = true;
                }
            }
        }

        //Win condition
        if (battleController.enemies.transform.childCount == 0 && enemiesSpawned && !victorySequenceStarted && !attackPreviewScript.coroutineRunning)
        {
            //Start outro scene
            battleController.CancelEveryting();
            StartCoroutine(victorySequence.Victory());
            enemiesSpawned = false; //remove later
            victorySequenceStarted = true;
        }
        
        //Lose condition
        if (shouldLose && !gameOver && !attackPreviewScript.coroutineRunning)
        {
            battleController.CancelEveryting();
            gameOver = true;
            StartCoroutine(gameOverScript.GameOverSequence());

        }

    }
    public void CreateEnemies()
    {
        BasicRangedEnemy(-21f, -11.25f, 0f);
        BasicEnemy(-9f, -14.24f, 0f);
        BasicEnemy(2.57f, -10.15f, 0f);
        BossEnemy(10.5f, -6.36f, 0f);
        enemiesSpawned = true;
        CharacterDeathSubscribe(); 
        EnemyDeathSubscribe();

    }
    public void BasicEnemy(float x, float y, float z)
    {
        GameObject temp = Instantiate(basicEnemyPrefab, new Vector3(x, y, z), Quaternion.identity, enemies.transform);
        EnemyController enemy = temp.GetComponent<EnemyController>();
        enemy.title = "Soldier";

        enemy.maxHp = 11;
        enemy.currentHp = enemy.maxHp;
        enemy.maxMana = 8;
        enemy.currentMana = enemy.maxMana;
        enemy.attack = 7;
        enemy.defense = 4;
        enemy.resistance = 3;
        enemy.intelligence = 4;
        enemy.skill = 5;
        enemy.speed = 4;
        enemy.attackRange = 1.2f;
        enemy.moveRange = 4;

        enemy.roams = true;
        enemy.ranged = false;
        enemy.support = false;
        enemy.hybrid = false;

        enemy.knownAttacks.Add(new Attack("Bash", "physical", 1.0f, 1.0f, 90, 0, 0, "Bash the enemy with your weapon."));
    }
    public void BasicRangedEnemy(float x, float y, float z)
    {
        GameObject temp = Instantiate(basicEnemyPrefab, new Vector3(x, y, z), Quaternion.identity, enemies.transform);
        EnemyController enemy = temp.GetComponent<EnemyController>();
        enemy.title = "Soldier";

        enemy.maxHp = 11;
        enemy.currentHp = enemy.maxHp;
        enemy.maxMana = 8;
        enemy.currentMana = enemy.maxMana;
        enemy.attack = 7;
        enemy.defense = 4;
        enemy.resistance = 3;
        enemy.intelligence = 4;
        enemy.skill = 5;
        enemy.speed = 4;
        enemy.attackRange = 3;
        enemy.moveRange = 4;

        enemy.roams = true;
        enemy.ranged = true;
        enemy.support = false;
        enemy.hybrid = false;

        enemy.knownAttacks.Add(new Attack("Bow Shot", "physical", 1.0f, 1.0f, 90, 0, 0, "Shoot at arrow at the enemy."));
    }
    public void BossEnemy(float x, float y, float z)
    {
        GameObject temp = Instantiate(basicEnemyPrefab, new Vector3(x, y, z), Quaternion.identity, enemies.transform);
        EnemyController enemy = temp.GetComponent<EnemyController>();
        enemy.title = "Hegseth";

        enemy.maxHp = 14;
        enemy.currentHp = enemy.maxHp;
        enemy.maxMana = 9;
        enemy.currentMana = enemy.maxMana;
        enemy.attack = 8;
        enemy.defense = 5;
        enemy.resistance = 3;
        enemy.intelligence = 4;
        enemy.skill = 6;
        enemy.speed = 4;
        enemy.attackRange = 1;
        enemy.moveRange = 4;

        enemy.roams = false;
        enemy.ranged = false;
        enemy.boss = true;
        enemy.deathDialogue = "Gah-- I must fall back. You will regret this. King Reiss WILL have your relic...";
        enemy.knownAttacks.Add(new Attack("Bash", "physical", 1.1f, 1.0f, 90, 0, 0, "Bash the enemy with your weapon."));

    }
    private void CharacterDeathSubscribe()
    {
        PlayerController.OnCharacterDied += HandleDeath;
    }
    private void CharacterDeathUnsubscribe()
    {
        PlayerController.OnCharacterDied -= HandleDeath;
    }
    private void HandleDeath(string name)
    {
        Debug.Log("Heard that " + name + " died!");
        if (name == "Astrid" || name == saveManager.loadedData.mainCharacterName)
        {
            shouldLose = true;
        }

    }
    private void EnemyDeathSubscribe()
    {
        EnemyController.OnEnemyDied += HandleEnemyDeath;
    }
    private void EnemyDeathUnsubscribe()
    {
        EnemyController.OnEnemyDied -= HandleEnemyDeath;
    }
    private void HandleEnemyDeath(GameObject[] list)
    {
        Debug.Log("Heard that " + list[0].GetComponent<EnemyController>().title + " was killed by " + list[1].GetComponent<PlayerController>().title);

        //Subquest 1: Astrid lands killing blow on boss
        if (list[0].GetComponent<EnemyController>().boss && list[1].GetComponent<PlayerController>().title != "Astrid")
        {
            astridScript.subquests[0].failed = true;
        }
        else if (list[0].GetComponent<EnemyController>().boss && list[1].GetComponent<PlayerController>().title == "Astrid")
        {
            astridScript.subquests[0].completed = true;
        }

    }
    private IEnumerator Intro()
    {

        if (saveManager.loadedData.introBattleOutro == "Intro")
        {
            //Overworld movement and dialogue
            yield return StartCoroutine(Helpers.FadeOutImageAlpha(whiteScreen, 1f));
            yield return new WaitForSeconds(1f);
            StartCoroutine(Helpers.FadeInAudio(fluteAudio));
            yield return StartCoroutine(pathfinder.FollowPath(mainChar, new Vector3(-6.57f, -11.28f, 0f)));
            yield return new WaitForSeconds(.5f);
            doorAudio.Play();
            yield return StartCoroutine(characterDisappear(mainChar));
            DialogueController.GetComponent<SmallDialogue>().NextDialogue();
            while (!dialogueFinished)
            {
                yield return new WaitForSeconds(1f);
            }

            //scene
            yield return StartCoroutine(FadeScreen(blackScreen, 1f));
            sexScreen.GetComponent<CanvasGroup>().alpha = 1f;
            yield return StartCoroutine(UndoFade(blackScreen, 1f));
            DialogueController.GetComponent<SmallDialogue>().NextDialogue();
            while (!secondDialogueFinished)
            {
                yield return new WaitForSeconds(1f);
            }

            //In house dialogue
            yield return StartCoroutine(FadeScreen(blackScreen, 2f));
            sexScreen.GetComponent<CanvasGroup>().alpha = 0f;
            houseScreen.GetComponent<CanvasGroup>().alpha = 1f;
            yield return StartCoroutine(UndoFade(blackScreen, 1f));
            DialogueController.GetComponent<FullBodyDialogue>().NextDialogue();
            while (!thirdDialogueFinished)
            {
                yield return new WaitForSeconds(1f);
            }

            //Overworld artifact scene and dialogue
            yield return StartCoroutine(FadeScreen(blackScreen, 2f));
            houseScreen.GetComponent<CanvasGroup>().alpha = 0f;
            disableCharacterImages();
            yield return StartCoroutine(UndoFade(blackScreen, 2f));
            artifactShineAudio.Play();
            yield return new WaitForSeconds(4f);
            DialogueController.GetComponent<SmallDialogue>().NextDialogue();
            while (!fourthDialogueFinished)
            {
                yield return new WaitForSeconds(1f);
            }

            //In house dialogue 2
            yield return StartCoroutine(FadeScreen(blackScreen, 2f));
            houseScreen.GetComponent<CanvasGroup>().alpha = 1f;
            enableCharacterImages();
            yield return StartCoroutine(UndoFade(blackScreen, 1f));
            DialogueController.GetComponent<FullBodyDialogue>().NextDialogue();
            while (!fifthDialogueFinished)
            {
                yield return new WaitForSeconds(1f);
            }
            
            //Overworld
            yield return StartCoroutine(FadeScreen(blackScreen, 2f));
            houseScreen.GetComponent<CanvasGroup>().alpha = 0f;
            chapterOneScript.CreateEnemies();
            disableCharacterImages();
            yield return StartCoroutine(UndoFade(blackScreen, 2f));
            doorAudio.Play();
            yield return StartCoroutine(characterAppear(mainChar));
            yield return new WaitForSeconds(.5f);
            yield return StartCoroutine(pathfinder.FollowPath(mainChar, new Vector3(-7.53f, -12f, 0f)));
            astrid.transform.position = new Vector3(-6.57f, -11.28f, 0f);
            astrid.GetComponent<SpriteRenderer>().enabled = true;
            yield return StartCoroutine(characterAppear(astrid));
            yield return StartCoroutine(pathfinder.FollowPath(astrid, new Vector3(-5.66f, -12f, 0f)));
            yield return new WaitForSeconds(.5f);

            saveManager.loadedData.introBattleOutro = "Battle";
            saveManager.OverwriteSave();

            fightScreen.GetComponent<CanvasGroup>().alpha = 1f;
            swordClash.Play();
            forestBattleTheme.Play();
            yield return new WaitForSeconds(1.5f);
            fightScreen.GetComponent<CanvasGroup>().alpha = 0f;

            
            DialogueController.GetComponent<SmallDialogue>().NextDialogue();   
            while (!sixthDialogueFinished)
            {
                yield return new WaitForSeconds(1f);
            }
        }
        else if (saveManager.loadedData.introBattleOutro == "Battle")
        {

        }
        enableVictorySubquestBoxes();
        yield return new WaitForSeconds(2f);
        battleController.StartCombat();
        tutorialScript.EnableTutorial();
    }
    private IEnumerator TypeLine(string line, string speaker, AudioSource audioSource, TextMeshProUGUI textBox, float textSpeed) {
        if (speaker == "Astrid")
        {
            audioSource.pitch = 1.2f;
            textBox.color = new Color(1f, .75f, .79f, 1f);
        }
        else
        {
            audioSource.pitch = 1.0f;
            textBox.color = Color.white;
        }
        isTyping = true;
        audioSource.Play();
        foreach (char c in line.ToCharArray()) {
            textBox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        audioSource.Stop();
        isTyping = false;
    }
    public struct CharacterDialogue {
        public string[] lines;
        public string name;
        public GameObject characterImage;
        public CharacterDialogue(GameObject characterImage, string name,string[] lines)
        {
            this.lines = lines;
            this.name = name;
            this.characterImage = characterImage;
        }
    }
}