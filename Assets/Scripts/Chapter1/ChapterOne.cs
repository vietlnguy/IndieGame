using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

[DefaultExecutionOrder(-1)]
public class ChapterOne : MonoBehaviour {

    private SaveManager saveManager;
    
    //Prefabs
    public GameObject basicEnemyPrefab;
    public GameObject mainCharacterPrefab;
    public GameObject astridPrefab;

    //Bools & Trackers
    private bool enemiesSpawned = false;
    private bool shouldLose = false;
    private bool gameOver = false;
    private bool victorySequenceStarted = false;
    private bool isTyping = false;
    private bool nextLine = false;
    private Coroutine intro;
    private Coroutine typingCoroutine;
    private string lineToBeTyped = "";

    //Objects
    private GameObject characters;
    private GameObject enemies; 
    private GameObject mainCharacterObject;
    private GameObject astridObject;
    public GameObject victoryAndSubquestBox;

    //Audios
    public AudioSource typingAudio;
    public AudioSource fluteAudio;
    public AudioSource doorAudio;
    public AudioSource doorKnockAudio;
    public AudioSource hegsethThemeAudio;
    public AudioSource rummagingAudio;
    public AudioSource shineAudio;
    public AudioSource knockbackAudio;
    public AudioSource battleMusicAudio;

    //Script references
    public BattleController battleController;
    public GameOver gameOverScript;
    public AttackPreview attackPreviewScript;
    public Tutorial tutorialScript;
    public VictorySequence victorySequence;
    private PlayerController astridScript;
    private TilemapPathfinder pathfinder;
    private DialogueController dialogueControllerScript;

    //Screens
    public Image blackScreen;
    public Image whiteScreen;
    public Image sexScreen;
    public Image houseScreen;
    public Image outroScreen;
   
    public GameObject mainCharacterLargePortrait;
    public GameObject astridLargePortrait;
    public GameObject hegsethLargePortrait;
    public GameObject soldierLargePortrait;

    //Subquests
    public TextMeshProUGUI astridSubquestText;

    public void Awake()
    {    
    
        saveManager = FindAnyObjectByType<SaveManager>();
        pathfinder = FindAnyObjectByType<TilemapPathfinder>();
        characters = GameObject.Find("Characters");
        enemies = GameObject.Find("Enemies");
        dialogueControllerScript = FindAnyObjectByType<DialogueController>();

        foreach (Character character in saveManager.loadedData.characters)
        {
            if (character.characterName == saveManager.loadedData.mainCharacterName)
            {
                mainCharacterObject = Instantiate(mainCharacterPrefab, new Vector3(-19f, -7.9f, 0f), Quaternion.identity, characters.transform);
            }
            else if (character.characterName == "Astrid")
            {
                astridObject = Instantiate(astridPrefab, new Vector3(-9.2f, -11.75f), Quaternion.identity, characters.transform);
                StartCoroutine(Helpers.FadeSpriteToBlack(astridObject));
                astridScript = GameObject.Find("AstridPrefab(Clone)").GetComponent<PlayerController>();

            }
        }

        victorySequence = FindAnyObjectByType<VictorySequence>();
        victorySequence.subquests.Add(astridScript.subquests[0]);
        VictorySubscribe();

    }
    public void Start()
    {
        if (saveManager.loadedData.introBattleOutro == "Outro")
        {
            Outro();
        }
        else
        {
            intro = StartCoroutine(Intro());
        }
    }
    public void Update()
    {
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
        BasicRangedEnemy(-19f, -9.75f, 0f);
        BasicEnemy(-11.3f, -14.1f, 0f);
        BasicEnemy(1.95f, -6.17f, 0f);
        BossEnemy(14.5f, -13.8f, 0f);
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
        enemy.unmodifiedMoveRange = 4;

        enemy.roams = true;
        enemy.ranged = false;
        enemy.support = false;
        enemy.hybrid = false;

        //enemy.knownAttacks.Add(new Attack("Triumphant Shout", "physical", 1.5f, 1.0f, 100, 0, 0, new List<Debuff>(){new Debuff("Taunted", 100, 1)}, "Taunts the enemy. Forced to attack closest."));
        enemy.knownAttacks.Add(new Attack("Bash", "physical", 1.0f, 1.0f, 90, 0, 0,  new List<Debuff>(),"Bash the enemy with your weapon."));
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
        enemy.unmodifiedMoveRange = 4;

        enemy.roams = true;
        enemy.ranged = true;
        enemy.support = false;
        enemy.hybrid = false;

        enemy.knownAttacks.Add(new Attack("Bow Shot", "physical", 1.0f, 1.0f, 90, 0, 0, new List<Debuff>(), "Shoot at arrow at the enemy."));
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
        enemy.unmodifiedMoveRange = 4;

        enemy.roams = false;
        enemy.ranged = false;
        enemy.boss = true;
        enemy.deathDialogue = "Gah-- I must fall back. You will regret this. King Reiss WILL have your relic...";
        enemy.knownAttacks.Add(new Attack("Bash", "physical", 1.1f, 1.0f, 90, 0, 0, new List<Debuff>(), "Bash the enemy with your weapon."));

    }
    private IEnumerator Intro()
    {
        //REMOVE AFTER TESTING
        //saveManager.loadedData.introBattleOutro = "Battle";
        
        if (saveManager.loadedData.introBattleOutro == "Intro")
        {

            //Overworld movement and dialogue
            yield return StartCoroutine(Helpers.FadeOutImageAlpha(whiteScreen, 1f));
            yield return new WaitForSeconds(1f);
            StartCoroutine(Helpers.FadeInAudio(fluteAudio, 1f));
            yield return StartCoroutine(pathfinder.FollowPath(mainCharacterObject, new Vector3(-9.2f, -11.68f, 0f)));
            yield return new WaitForSeconds(.5f);
            doorAudio.Play();
            yield return StartCoroutine(Helpers.FadeSpriteToBlack(mainCharacterObject));
            yield return Helpers.PlayDialogueAndWait(dialogueControllerScript, false);
            yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1f));

            //NSFW scene
            yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1f));
            sexScreen.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));
            yield return Helpers.PlayDialogueAndWait(dialogueControllerScript, true);

            //In house dialogue
            yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1f));
            sexScreen.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            houseScreen.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));

            //Move characters in frame
            Helpers.FlipRectTransformXScale(astridLargePortrait);
            mainCharacterLargePortrait.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            astridLargePortrait.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            mainCharacterLargePortrait.GetComponent<RectTransform>().anchoredPosition = new Vector2(-225f, -164f);
            astridLargePortrait.GetComponent<RectTransform>().anchoredPosition = new Vector2(330f, -196f);
            StartCoroutine(Helpers.UndoFadeToBlackTransparent(mainCharacterLargePortrait, 0.5f));
            yield return StartCoroutine(Helpers.UndoFadeToBlackTransparent(astridLargePortrait, 0.5f));
            yield return new WaitForSeconds(.5f);
            yield return Helpers.PlayDialogueAndWait(dialogueControllerScript, true);

            //Door knock
            fluteAudio.Stop();
            doorKnockAudio.Play();
            yield return new WaitForSeconds(1.5f);
            yield return Helpers.PlayDialogueAndWait(dialogueControllerScript, true);

            //Move main char
            yield return StartCoroutine(Helpers.FadeToBlackTransparent(mainCharacterLargePortrait, .5f));
            yield return new WaitForSeconds(1f);
            mainCharacterLargePortrait.GetComponent<RectTransform>().anchoredPosition = new Vector2(122f, -164f);
            Helpers.FlipRectTransformXScale(mainCharacterLargePortrait);
            yield return StartCoroutine(Helpers.UndoFadeToBlackTransparent(mainCharacterLargePortrait, .5f));

            //Enter hegseth
            hegsethLargePortrait.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            hegsethLargePortrait.GetComponent<RectTransform>().anchoredPosition = new Vector2(-202f, -208f);
            yield return StartCoroutine(Helpers.UndoFadeToBlackTransparent(hegsethLargePortrait, .5f));
            hegsethThemeAudio.Play();
            yield return Helpers.PlayDialogueAndWait(dialogueControllerScript, true);


            //Enter soldier
            soldierLargePortrait.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            soldierLargePortrait.GetComponent<RectTransform>().anchoredPosition = new Vector2(-366f, -208f);
            yield return StartCoroutine(Helpers.UndoFadeToBlackTransparent(soldierLargePortrait, .5f));
            yield return Helpers.PlayDialogueAndWait(dialogueControllerScript, true);


            //Exit soldier and search
            yield return StartCoroutine(Helpers.FadeToBlackTransparent(soldierLargePortrait, 0.5f));
            rummagingAudio.Play();
            yield return new WaitForSeconds(7f);
            yield return StartCoroutine(Helpers.UndoFadeToBlackTransparent(soldierLargePortrait, 0.5f));
            yield return Helpers.PlayDialogueAndWait(dialogueControllerScript, true);

            //Move hegseth towards astrid
            yield return StartCoroutine(Helpers.MoveRectTransform(hegsethLargePortrait, hegsethLargePortrait.GetComponent<RectTransform>().anchoredPosition, new Vector2(hegsethLargePortrait.GetComponent<RectTransform>().anchoredPosition.x + 100f, hegsethLargePortrait.GetComponent<RectTransform>().anchoredPosition.y), 0.5f));
            yield return StartCoroutine(Helpers.MoveRectTransform(mainCharacterLargePortrait, mainCharacterLargePortrait.GetComponent<RectTransform>().anchoredPosition, new Vector2(mainCharacterLargePortrait.GetComponent<RectTransform>().anchoredPosition.x - 25f, mainCharacterLargePortrait.GetComponent<RectTransform>().anchoredPosition.y), 0.5f));
            yield return Helpers.PlayDialogueAndWait(dialogueControllerScript, true);

            //Move hegseth back
            yield return StartCoroutine(Helpers.MoveRectTransform(hegsethLargePortrait, hegsethLargePortrait.GetComponent<RectTransform>().anchoredPosition, new Vector2(hegsethLargePortrait.GetComponent<RectTransform>().anchoredPosition.x - 75f, hegsethLargePortrait.GetComponent<RectTransform>().anchoredPosition.y), 0.5f));
            yield return Helpers.PlayDialogueAndWait(dialogueControllerScript, true);

            //Move soldier towards astrid
            yield return StartCoroutine(Helpers.MoveRectTransform(soldierLargePortrait, soldierLargePortrait.GetComponent<RectTransform>().anchoredPosition, new Vector2(181f, soldierLargePortrait.GetComponent<RectTransform>().anchoredPosition.y), 0.25f));
            yield return Helpers.PlayDialogueAndWait(dialogueControllerScript, true);

            //Fade screen to black and play shine audio
            StartCoroutine(Helpers.FadeOutAudio(hegsethThemeAudio, 1f));
            yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1.5f));
            houseScreen.enabled = false;
            dialogueControllerScript.HideLargePortraits();
            yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1.5f));
            shineAudio.Play();
            yield return new WaitForSeconds(1.5f);
            yield return Helpers.PlayDialogueAndWait(dialogueControllerScript, false);

            //Back to house and push away soldier
            yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1.5f));
            houseScreen.enabled = true;
            dialogueControllerScript.ShowLargePortraits();
            yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1.5f));
            knockbackAudio.Play();
            yield return StartCoroutine(Helpers.MoveRectTransform(soldierLargePortrait, soldierLargePortrait.GetComponent<RectTransform>().anchoredPosition, new Vector2(-366f, -208f), 0.25f));
            yield return Helpers.PlayDialogueAndWait(dialogueControllerScript, true);

            //Move soldier towards astrid
            yield return StartCoroutine(Helpers.MoveRectTransform(soldierLargePortrait, soldierLargePortrait.GetComponent<RectTransform>().anchoredPosition, new Vector2(181f, soldierLargePortrait.GetComponent<RectTransform>().anchoredPosition.y), 0.25f));
            knockbackAudio.Play();
            yield return StartCoroutine(Helpers.MoveRectTransform(soldierLargePortrait, soldierLargePortrait.GetComponent<RectTransform>().anchoredPosition, new Vector2(-366f, -208f), 0.25f));
            yield return Helpers.PlayDialogueAndWait(dialogueControllerScript, true);

            //Exit hegseth and soldier
            StartCoroutine(Helpers.FadeToBlackTransparent(hegsethLargePortrait, 0.5f));
            yield return StartCoroutine(Helpers.FadeToBlackTransparent(soldierLargePortrait, 0.5f));
            hegsethLargePortrait.GetComponent<Image>().enabled = false;
            soldierLargePortrait.GetComponent<Image>().enabled = false;
            yield return Helpers.PlayDialogueAndWait(dialogueControllerScript, true);

            //Exit house scene
            yield return StartCoroutine(Helpers.FadeInImageAlpha(whiteScreen, 2f));
            houseScreen.enabled = false;
            dialogueControllerScript.HideLargePortraits();
        }

        saveManager.loadedData.introBattleOutro = "Battle";
        saveManager.OverwriteSave();

        //Spawn all enemies
        CreateEnemies();

        mainCharacterObject.transform.position = new Vector3(-9.2f, -11.5f, 0f);

        //Fade out white screen
        yield return StartCoroutine(Helpers.FadeOutImageAlpha(whiteScreen, 0.5f));

        //Move characters to starting positions
        yield return StartCoroutine(Helpers.UndoFadeSpriteToBlack(mainCharacterObject));
        yield return StartCoroutine(pathfinder.FollowPath(mainCharacterObject, new Vector3(-10.75f, -12.5f, 0f)));
        yield return new WaitForSeconds(1f);

        astridObject.transform.position = new Vector3(-9.2f, -11.5f, 0f);
        yield return StartCoroutine(Helpers.UndoFadeSpriteToBlack(astridObject));
        yield return StartCoroutine(pathfinder.FollowPath(astridObject, new Vector3(-8.15f, -12.5f, 0f)));

        //Small dialogue
        yield return Helpers.PlayDialogueAndWait(dialogueControllerScript, false);

        //EnableTutorial
        battleMusicAudio.Play();
        yield return new WaitForSeconds(2f);
        battleController.StartCombat();
        yield return new WaitForSeconds(2.5f);
        tutorialScript.EnableTutorial();


        
    }
    public IEnumerator OutroHelper()
    {   
        saveManager.loadedData.introBattleOutro = "Outro";
        saveManager.OverwriteSave();
        blackScreen.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        whiteScreen.enabled = false;
        outroScreen.enabled = true;
        yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));

        Helpers.FlipRectTransformXScale(astridLargePortrait);
        astridLargePortrait.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        astridLargePortrait.GetComponent<RectTransform>().anchoredPosition = new Vector2(270f, -130f);
        mainCharacterLargePortrait.GetComponent<RectTransform>().anchoredPosition = new Vector2(-213f, -109f);      
        mainCharacterLargePortrait.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        yield return StartCoroutine(Helpers.UndoFadeToBlackTransparent(mainCharacterLargePortrait, 0.5f));
        yield return StartCoroutine(Helpers.UndoFadeToBlackTransparent(astridLargePortrait, 0.5f));

        yield return new WaitForSeconds(1f);

        yield return Helpers.PlayDialogueAndWait(dialogueControllerScript, true);

        yield return StartCoroutine(saveManager.SceneTransition(true));
        saveManager.loadedData.currentChapter = "Chapter 2";
        saveManager.loadedData.introBattleOutro = "Overworld";
        saveManager.OverwriteSave();
        SceneManager.LoadScene("Overworld");
    }
    private void HandleDeath(string name)
    {
        Debug.Log("Heard that " + name + " died!");
        if (name == "Astrid" || name == saveManager.loadedData.mainCharacterName)
        {
            shouldLose = true;
        }

    }
    private void HandleEnemyDeath(GameObject[] list)
    {
        Debug.Log("Heard that " + list[0].GetComponent<EnemyController>().title + " was killed by " + list[1].GetComponent<PlayerController>().title);

        //Subquest 1: Astrid lands killing blow on boss
        if (list[0].GetComponent<EnemyController>().boss && list[1].GetComponent<PlayerController>().title != "Astrid")
        {
            astridScript.subquests[0].failed = true;
            Helpers.FailSubquestText(astridSubquestText);
        }
        else if (list[0].GetComponent<EnemyController>().boss && list[1].GetComponent<PlayerController>().title == "Astrid")
        {
            astridScript.subquests[0].completed = true;
            Helpers.SucceedSubquestText(astridSubquestText);

        }

    }
    
    private void Outro()
    {
        victoryAndSubquestBox.SetActive(false);
        intro = StartCoroutine(OutroHelper());
    }
    private void VictorySubscribe()
    {
        VictoryContinueButton.OnStartOutro += Outro;
    }
    private void EnemyDeathSubscribe()
    {
        EnemyController.OnEnemyDied += HandleEnemyDeath;
    }
    private void EnemyDeathUnsubscribe()
    {
        EnemyController.OnEnemyDied -= HandleEnemyDeath;
    }
    private void CharacterDeathSubscribe()
    {
        PlayerController.OnCharacterDied += HandleDeath;
    }
    private void CharacterDeathUnsubscribe()
    {
        PlayerController.OnCharacterDied -= HandleDeath;
    }

}