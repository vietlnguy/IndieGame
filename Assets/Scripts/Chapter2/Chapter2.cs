using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class Chapter2 : MonoBehaviour
{

    //Prefabs
    public GameObject basicEnemyPrefab;
    public GameObject mainCharacterPrefab;
    public GameObject astridPrefab;
    public GameObject celestePrefab;
    public GameObject lucasPrefab;

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
    private int lucasKills = 0;
     
    //Objects
    private GameObject characters;
    private GameObject enemies; 
    private GameObject mainCharacterObject;
    private GameObject astridObject;
    public GameObject camera;
    public Image subquest1X;
    public Image subquest1Check;
    public Image subquest2X;
    public Image subquest2Check;

    //Script references
    public BattleController battleController;
    private SaveManager saveManager;
    public GameOver gameOverScript;
    public AttackPreview attackPreviewScript;
    private TilemapPathfinder pathfinder;
    private PlayerController lucasScript;
    private PlayerController celesteScript;
    private VictorySequence victorySequenceScript;
    
    //Audios
    public AudioSource typingAudio;
    public AudioSource dangerIntroAudio;
    public AudioSource doorOpenAudio;

    //Screens
    public Image blackScreen;
    public Image outroScreen;
    
    //Dialogue
    public GameObject smallDialogueTextBox;
    public TextMeshProUGUI smallDialogueNameBox;
    public GameObject largeDialogue;
    public GameObject largeDialogueTextBox;
    public TextMeshProUGUI largeDialogueNameBox;
    public List<CharacterDialogue> dialogues;
    public List<CharacterDialogue> dialogues2;
    public List<CharacterDialogue> dialogues3;
    public List<CharacterDialogue> dialogues4;

    public void Awake()
    {    

        AudioListener.volume = PlayerPrefs.GetFloat("volume", 0.5f);
        saveManager = FindFirstObjectByType<SaveManager>();
        characters = GameObject.Find("Characters");
        enemies = GameObject.Find("Enemies");
        pathfinder = FindFirstObjectByType<TilemapPathfinder>();
        victorySequenceScript = FindFirstObjectByType<VictorySequence>();

        dialogues = new List<CharacterDialogue>();
        dialogues.Add(new CharacterDialogue(saveManager.loadedData.mainCharacterName, new string[] {"It's been a long time since we've been back to Maplemire.", "We should try to load up on supplies, while we can.", "It's not much further til we get to the castle."}));
        dialogues.Add(new CharacterDialogue("Astrid", new string[] {"That sounds like a good idea.", "Doesn't town seem awfully quiet, though? Where is everybody?"}));
        dialogues.Add(new CharacterDialogue(saveManager.loadedData.mainCharacterName, new string[] {"You're right, something's off--", "Wait a second.", "Something is going on at the church over there."}));

        dialogues2 = new List<CharacterDialogue>();
        dialogues2.Add(new CharacterDialogue("Lucas", new string[] {"Back off, chump!", "We don't have any of those stinkin' relics!"}));
        dialogues2.Add(new CharacterDialogue("Celeste", new string[]{"Oh dear.. Please forgive my brother, sir. But he speaks true.", "Our goddess, Ilvera, forbids us to lie. We do not possess any relics."}));
        dialogues2.Add(new CharacterDialogue("Soldier", new string[] {"I understand that priestess, but I have orders.", "If you don't comply, my superiors--"}));
        
        dialogues3 = new List<CharacterDialogue>();
        dialogues3.Add(new CharacterDialogue("Lance", new string[] {"Well well well! Wha' do we got 'er?", "A couple of sky worshippers. What's wrong? Your \"goddess\" didn't tell you we were coming?"}));
        dialogues3.Add(new CharacterDialogue("Celeste", new string[]{"The goddess guides us all.", "May she lead you to her warmth and understanding..."}));
        dialogues3.Add(new CharacterDialogue("Lance", new string[] {"Pft. I don't need any phony goddess.", "The only thing I need is all of the church tithings!", "Now hand them over!"}));
        dialogues3.Add(new CharacterDialogue("Lucas", new string[] {"We already told you and your crooks, we don't have any relics!", "So get lost, jerk!"}));
        dialogues3.Add(new CharacterDialogue("Lance", new string[] {"Woo weeee! The mouth on this one! Tsk tsk, your god should've taught you better manners!", "Let's go boys! Turn this town upside down!", "By order of Lord Beesly, find every relic you can!"}));
        dialogues3.Add(new CharacterDialogue("Lucas", new string[] {"Come on, sis! We gotta make a break for it!"}));
        dialogues3.Add(new CharacterDialogue("Celeste", new string[]{"Goddess, protect us..."}));

        dialogues4 = new List<CharacterDialogue>();
        dialogues4.Add(new CharacterDialogue(saveManager.loadedData.mainCharacterName, new string[] {"Wait what-- Lord Beesly's orders?", "That can't be.."}));
        dialogues4.Add(new CharacterDialogue("Astrid", new string[] {saveManager.loadedData.mainCharacterName + "!", "Those people running away from the church, they look like they are in danger!", "We can worry about Lord Beesly later, right now they need our help!"}));
        dialogues4.Add(new CharacterDialogue(saveManager.loadedData.mainCharacterName, new string[] {"You're right. Let's go!"}));

        bool hasNewCharacters = saveManager.loadedData.characters.Exists(c => c.characterName == "Celeste" || c.characterName == "Lucas");

        if (!hasNewCharacters)
        {
            Character celeste = new Character("Celeste", 9, 11, 4, 7, 4, 6, 6, 5, 3, 4, false, true);
            celeste.knownAttacks.Add(new SupportMove("Heal", 3, "hp", 5, null, null, "Heal an ally. (Scales with INT)"));
            celeste.inventory.Add(new Item("Potion", 5, "hp", 10, "Restores 10 HP.", false, false, false));
            celeste.weaponEquiped = new Equipment("Basic", "weapon", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Completely ordinary.");
            celeste.armorEquiped = new Equipment("Cloth", "armor", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Completely ordinary."); 
            celeste.accessoryEquiped = new Equipment("Mana Band", "accessory", 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Stores mana! +2 max Mana.");
            celeste.subquests.Add(new Subquest("Celeste1", "Don't let Celeste take any damage.", "Ask about how she became a priestess."));
            //TODO: Add more celeste subquests
            
            saveManager.loadedData.characters.Add(celeste);

            Character lucas = new Character("Lucas", 11, 7, 5, 3, 5, 5, 6, 6, 1, 5, false, false);
            lucas.knownAttacks.Add(new Attack("Double Punch", "physical", 1.1f, 0, 95, 0, 0,  new List<string>(),"Punch in rapid succession."));
            lucas.inventory.Add(new Item("Potion", 5, "hp", 10, "Restores 10 HP.", false, false, false));
            lucas.weaponEquiped = new Equipment("Basic", "weapon", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Completely ordinary.");
            lucas.armorEquiped = new Equipment("Cloth", "armor", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Completely ordinary."); 
            lucas.accessoryEquiped = new Equipment("Gauntlets", "accessory", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Completely ordinary");
            lucas.subquests.Add(new Subquest("Lucas1", "Lucas slays at least 2 enemies.", "Ask about his relationship to Celeste."));
            //TODO: Add more lucas subquests

            saveManager.loadedData.characters.Add(lucas);

        }
        //Load characters
        foreach (Character character in saveManager.loadedData.characters)
        {
            if (character.characterName == saveManager.loadedData.mainCharacterName)
            {
                Instantiate(mainCharacterPrefab, new Vector3(-29f, -11f, 0f), Quaternion.identity, characters.transform);
            }
            else if (character.characterName == "Astrid")
            {
                Instantiate(astridPrefab, new Vector3(-30f, -13f, 0f), Quaternion.identity, characters.transform);
            }
            else if (character.characterName == "Celeste")
            {
                GameObject temp = Instantiate(celestePrefab, new Vector3(13f, -8f, 0f), Quaternion.identity, characters.transform);
                celesteScript = temp.GetComponent<PlayerController>();
            }
            else if (character.characterName == "Lucas")
            {
                GameObject temp = Instantiate(lucasPrefab, new Vector3(13f, -8f, 0f), Quaternion.identity, characters.transform);
                lucasScript = temp.GetComponent<PlayerController>();
            }
        }

        //Give victory sequence script a list of all subquests
        List<Subquest> quests = new List<Subquest>();
        quests.Add(lucasScript.subquests[0]);
        quests.Add(celesteScript.subquests[0]);
        victorySequenceScript.subquests = quests;

    }
    public void Start()
    {
        intro = StartCoroutine(Intro());
        VictorySubscribe();
    }
    public void Update()
    {
        //Can script reinforcements, mid combat dialogues, etc.
        //Battle controller should be abstract enough to apply to all chapters
        //Chapter specific script events happen here, and win/lose conditions
        
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

        else 
        {
            //Check subquests
            if (celesteScript.currentHp < celesteScript.maxHp) 
            {
                celesteScript.subquests[0].failed = true;
                subquest1X.color = new Color(1f, 1f, 1f, 1f);
            }

            //Win condition
            if (battleController.enemies.transform.childCount == 0 && enemiesSpawned && !victorySequenceStarted && !attackPreviewScript.coroutineRunning)
            {
                if (celesteScript.subquests[0].failed == false) {
                    celesteScript.subquests[0].completed = true;
                    subquest1Check.color = new Color(1f, 1f, 1f, 1f);
                }
                
                if (lucasScript.subquests[0].completed == false) {
                    lucasScript.subquests[0].failed = true;
                    subquest2X.color = new Color(1f, 1f, 1f, 1f);
                }

                //Start outro scene
                battleController.CancelEveryting();
                StartCoroutine(Helpers.FadeOutAudio(dangerIntroAudio, .5f));
                StartCoroutine(victorySequenceScript.Victory());
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

    }
    public void CreateEnemies()
    {
        BasicRangedEnemy(-14.5f, -4f, 0f);
        BasicRangedEnemy(-10f, -29.5f, 0f);
        BasicEnemy(-9.5f, -12f, 0f);
        BasicEnemy(14f, -23f, 0f);
        BasicEnemy(17.5f, -7.5f, 0f);
        BossEnemy(24f, -10.4f, 0f);
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
        enemy.attackRange = 1;
        enemy.moveRange = 4;

        enemy.roams = true;
        enemy.ranged = false;
        enemy.support = false;
        enemy.hybrid = false;

        enemy.knownAttacks.Add(new Attack("Bash", "physical", 1.0f, 1.0f, 90, 0, 0,  new List<string>(),"Bash the enemy with your weapon."));
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

        enemy.knownAttacks.Add(new Attack("Bow Shot", "physical", 1.0f, 1.0f, 90, 0, 0,  new List<string>(),"Shoot at arrow at the enemy."));
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
        enemy.knownAttacks.Add(new Attack("Bash", "physical", 1.1f, 1.0f, 90, 0, 0,  new List<string>(),"Bash the enemy with your weapon."));

    }
    private void HandleDeath(string name)
    {
        Debug.Log("Heard that " + name + " died!");
        if (name == "Astrid" || name == saveManager.loadedData.mainCharacterName || name == "Lucas" || name == "Celeste")
        {
            shouldLose = true;
        }

    }
    private void HandleEnemyDeath(GameObject[] list)
    {
        Debug.Log("Heard that " + list[0].GetComponent<EnemyController>().title + " was killed by " + list[1].GetComponent<PlayerController>().title);

        if (list[1].GetComponent<PlayerController>().title == "Lucas")
        {
            lucasKills++;

            if (lucasKills >= 2)
            {
                lucasScript.subquests[0].completed = true;
                subquest2Check.color = new Color(1f, 1f, 1f, 1f);
            }

        }

    }
    private IEnumerator Intro()
    {
        GameObject mainChar = GameObject.Find("MainCharacterPrefab(Clone)");
        GameObject astrid = GameObject.Find("AstridPrefab(Clone)");
        GameObject celeste = GameObject.Find("CelestePrefab(Clone)");
        GameObject lucas = GameObject.Find("LucasPrefab(Clone)");

        //Intro sequence
        if (saveManager.loadedData.introBattleOutro == "Intro") 
        {
            lucas.GetComponent<SpriteRenderer>().enabled = false;
            celeste.GetComponent<SpriteRenderer>().enabled = false;

            //Fade Out blackwhite screen
            yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));

            yield return new WaitForSeconds(2f);
            //Move characters on screen
            yield return StartCoroutine(pathfinder.FollowPath(mainChar, new Vector3(-18.5f, -11f, 0f)));
            yield return StartCoroutine(pathfinder.FollowPath(astrid, new Vector3(-20.5f, -12.7f, 0f)));

            //Small dialogue
            for (int index = 0; index < dialogues.Count; index++)
            {
                //Update name text
                smallDialogueNameBox.text = dialogues[index].name;

                //Fade in text box
                StartCoroutine(Helpers.MoveRectTransform(smallDialogueTextBox, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 10f), .25f));
                StartCoroutine(Helpers.FadeInCanvasGroup(smallDialogueTextBox.GetComponent<CanvasGroup>(), 0.25f));

                yield return new WaitForSeconds(.25f);
                //Type each line
                for (int index2 = 0; index2 < dialogues[index].lines.Length; index2++)
                {
                    nextLine = false;
                    typingCoroutine = StartCoroutine(TypeLine(dialogues[index].lines[index2], dialogues[index].name, typingAudio, smallDialogueTextBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>(), .05f));
                    lineToBeTyped = dialogues[index].lines[index2];

                    while (isTyping || !nextLine)
                    {
                        yield return new WaitForSeconds(.25f);
                    }
                    smallDialogueTextBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
                
                }

                //Fade out text box
                StartCoroutine(Helpers.MoveRectTransform(smallDialogueTextBox, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, -10f), .25f));
                StartCoroutine(Helpers.FadeOutCanvasGroup(smallDialogueTextBox.GetComponent<CanvasGroup>(), 0.25f));

                yield return new WaitForSeconds(0.25f);

            }
            typingCoroutine = null;

            //Pan camera to church
            yield return StartCoroutine(Helpers.MoveTransform(camera.transform, camera.transform.position, new Vector3(10.23f, -7.5f, -10f), 1.5f));
            
            //enter lucas, celeste, and soldier
            GameObject soldier = Instantiate(basicEnemyPrefab, new Vector3(13f, -8f, 0f), Quaternion.identity, enemies.transform);
            soldier.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(Helpers.FadeInAudio(dangerIntroAudio, 1.5f));
            doorOpenAudio.Play();
            StartCoroutine(Helpers.EnterCharacter(lucas.GetComponent<SpriteRenderer>(), 0.15f));
            yield return StartCoroutine(pathfinder.FollowPath(lucas, new Vector3(11f, -10f, 0f)));
            StartCoroutine(Helpers.EnterCharacter(celeste.GetComponent<SpriteRenderer>(), 0.15f));
            yield return StartCoroutine(pathfinder.FollowPath(celeste, new Vector3(11.55f, -12.3f, 0f)));
            StartCoroutine(Helpers.EnterCharacter(soldier.GetComponent<SpriteRenderer>(), 0.15f));
            yield return StartCoroutine(pathfinder.FollowPath(soldier, new Vector3(15f, -10.5f, 0f)));
            
            //small dialoue 2
            for (int index = 0; index < dialogues2.Count; index++)
            {
                //Update name text
                smallDialogueNameBox.text = dialogues2[index].name;

                //Fade in text box
                StartCoroutine(Helpers.MoveRectTransform(smallDialogueTextBox, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 10f), .25f));
                StartCoroutine(Helpers.FadeInCanvasGroup(smallDialogueTextBox.GetComponent<CanvasGroup>(), 0.25f));

                yield return new WaitForSeconds(.25f);
                //Type each line
                for (int index2 = 0; index2 < dialogues2[index].lines.Length; index2++)
                {
                    nextLine = false;
                    typingCoroutine = StartCoroutine(TypeLine(dialogues2[index].lines[index2], dialogues2[index].name, typingAudio, smallDialogueTextBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>(), .05f));
                    lineToBeTyped = dialogues2[index].lines[index2];

                    while (isTyping || !nextLine)
                    {
                        yield return new WaitForSeconds(.25f);
                    }
                    smallDialogueTextBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
                
                }

                //Fade out text box
                StartCoroutine(Helpers.MoveRectTransform(smallDialogueTextBox, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, -10f), .25f));
                StartCoroutine(Helpers.FadeOutCanvasGroup(smallDialogueTextBox.GetComponent<CanvasGroup>(), 0.25f));

                yield return new WaitForSeconds(0.25f);

            }
            typingCoroutine = null;

            //enter boss
            GameObject boss = Instantiate(basicEnemyPrefab, new Vector3(13f, -8f, 0f), Quaternion.identity, enemies.transform);
            boss.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(Helpers.EnterCharacter(boss.GetComponent<SpriteRenderer>(), 0.15f));
            yield return StartCoroutine(pathfinder.FollowPath(boss, new Vector3(15f, -12.5f, 0f)));

            //small dialoue 3
            for (int index = 0; index < dialogues3.Count; index++)
            {
                //Update name text
                smallDialogueNameBox.text = dialogues3[index].name;

                //Fade in text box
                StartCoroutine(Helpers.MoveRectTransform(smallDialogueTextBox, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 10f), .25f));
                StartCoroutine(Helpers.FadeInCanvasGroup(smallDialogueTextBox.GetComponent<CanvasGroup>(), 0.25f));

                yield return new WaitForSeconds(.25f);
                //Type each line
                for (int index2 = 0; index2 < dialogues3[index].lines.Length; index2++)
                {
                    nextLine = false;
                    typingCoroutine = StartCoroutine(TypeLine(dialogues3[index].lines[index2], dialogues3[index].name, typingAudio, smallDialogueTextBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>(), .05f));
                    lineToBeTyped = dialogues3[index].lines[index2];

                    while (isTyping || !nextLine)
                    {
                        yield return new WaitForSeconds(.25f);
                    }
                    smallDialogueTextBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
                
                }

                //Fade out text box
                StartCoroutine(Helpers.MoveRectTransform(smallDialogueTextBox, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, -10f), .25f));
                StartCoroutine(Helpers.FadeOutCanvasGroup(smallDialogueTextBox.GetComponent<CanvasGroup>(), 0.25f));

                yield return new WaitForSeconds(0.25f);

            }
            typingCoroutine = null;

            pathfinder.moveSpeed = 5f;
            StartCoroutine(pathfinder.FollowPath(lucas, new Vector3(-1.5f, -16.5f, 0f)));
            yield return StartCoroutine(pathfinder.FollowPath(celeste, new Vector3(1f, -18f, 0f)));

            //pan camera back
            yield return StartCoroutine(Helpers.MoveTransform(camera.transform, camera.transform.position, new Vector3(-6.55f, -7.5f, -10f), 1.5f));

            //small dialoue 3
            for (int index = 0; index < dialogues4.Count; index++)
            {
                //Update name text
                smallDialogueNameBox.text = dialogues4[index].name;

                //Fade in text box
                StartCoroutine(Helpers.MoveRectTransform(smallDialogueTextBox, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 10f), .25f));
                StartCoroutine(Helpers.FadeInCanvasGroup(smallDialogueTextBox.GetComponent<CanvasGroup>(), 0.25f));

                yield return new WaitForSeconds(.25f);
                //Type each line
                for (int index2 = 0; index2 < dialogues4[index].lines.Length; index2++)
                {
                    nextLine = false;
                    typingCoroutine = StartCoroutine(TypeLine(dialogues4[index].lines[index2], dialogues4[index].name, typingAudio, smallDialogueTextBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>(), .05f));
                    lineToBeTyped = dialogues4[index].lines[index2];

                    while (isTyping || !nextLine)
                    {
                        yield return new WaitForSeconds(.25f);
                    }
                    smallDialogueTextBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
                
                }

                //Fade out text box
                StartCoroutine(Helpers.MoveRectTransform(smallDialogueTextBox, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, -10f), .25f));
                StartCoroutine(Helpers.FadeOutCanvasGroup(smallDialogueTextBox.GetComponent<CanvasGroup>(), 0.25f));

                yield return new WaitForSeconds(0.25f);

            }
            typingCoroutine = null;

            //fade to black
            yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 0.5f));
            saveManager.loadedData.introBattleOutro = "Battle";
        }
        
        else
        {
            astrid.transform.position = new Vector3(-20.5f, -12.7f, 0f);
            mainChar.transform.position = new Vector3(-18.5f, -11f, 0f);
            lucas.transform.position =  new Vector3(-1.5f, -16.5f, 0f);
            celeste.transform.position = new Vector3(1f, -18f, 0f);
            StartCoroutine(Helpers.FadeInAudio(dangerIntroAudio, 1.5f));

        }

        //Spawn enemies
        foreach (Transform child in enemies.transform)
        {
            Destroy(child.gameObject);
        }
        CreateEnemies();

        yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));
        battleController.StartCombat();
        saveManager.OverwriteSave();
        intro = null;
    }
    public IEnumerator OutroHelper()
    {   
        saveManager.loadedData.introBattleOutro = "Outro";
        saveManager.OverwriteSave();
        blackScreen.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        outroScreen.enabled = true;
        yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));


        yield return new WaitForSeconds(1f);


        //yield return StartCoroutine(saveManager.SceneTransition(true));
        //saveManager.loadedData.currentChapter = "Chapter 3";
        //saveManager.loadedData.introBattleOutro = "Overworld";
        //saveManager.OverwriteSave();
        //SceneManager.LoadScene("Overworld");
    }

    //Should rarely change
    private void Outro()
    {
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
    private IEnumerator PlaySmallDialogue(List<CharacterDialogue> dialogues)
    {
        smallDialogueTextBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";

        //Small dialogue
        for (int index = 0; index < dialogues.Count; index++)
        {
            //Update name text
            smallDialogueNameBox.text = dialogues[index].name;

            Helpers.DisableAllSmallPortraits();

            GameObject temp = GameObject.Find(dialogues[index].name + "SmallPortrait");
            if (temp == null)
            {
                temp = GameObject.Find("MainCharacterSmallPortrait");
            }
            temp.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            
            //Fade in text box
            StartCoroutine(Helpers.MoveRectTransform(smallDialogueTextBox, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 10f), .25f));
            StartCoroutine(Helpers.FadeInCanvasGroup(smallDialogueTextBox.GetComponent<CanvasGroup>(), 0.25f));

            yield return new WaitForSeconds(.25f);
            //Type each line
            for (int index2 = 0; index2 < dialogues[index].lines.Length; index2++)
            {
                nextLine = false;
                typingCoroutine = StartCoroutine(TypeLine(dialogues[index].lines[index2], dialogues[index].name, typingAudio, smallDialogueTextBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>(), .05f));
                lineToBeTyped = dialogues[index].lines[index2];

                Coroutine blinking = null;
                while (isTyping || !nextLine)
                {
                    yield return new WaitForSeconds(.25f);
                    if (!isTyping && !nextLine && blinking == null)
                    {
                        blinking = StartCoroutine(Helpers.DialogueBlinker("small"));
                    }

                }
                try
                {
                    StopCoroutine(blinking);
                }
                catch
                {
                    
                }
                blinking = null;
                Helpers.DisableBlinker("small");
                smallDialogueTextBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
            
            }

            //Fade out text box
            StartCoroutine(Helpers.MoveRectTransform(smallDialogueTextBox, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition, smallDialogueTextBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, -10f), .25f));
            StartCoroutine(Helpers.FadeOutCanvasGroup(smallDialogueTextBox.GetComponent<CanvasGroup>(), 0.25f));

            yield return new WaitForSeconds(0.25f);

        }
    }
    private IEnumerator PlayLargeDialogue(List<CharacterDialogue> dialogues)
    {
        largeDialogueTextBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";

        //Small dialogue
        for (int index = 0; index < dialogues.Count; index++)
        {
            //Update name text
            largeDialogueNameBox.text = dialogues[index].name;

            //Grayout all large portraits
            StartCoroutine(Helpers.GrayAllLargePortraits());

            //Light talking portrait
            StartCoroutine(Helpers.HighlightLargePortrait(dialogues[index].name));

            //Fade in text box
            StartCoroutine(Helpers.MoveRectTransform(largeDialogueTextBox, largeDialogueTextBox.GetComponent<RectTransform>().anchoredPosition, largeDialogueTextBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 10f), .25f));
            StartCoroutine(Helpers.FadeInCanvasGroup(largeDialogueTextBox.GetComponent<CanvasGroup>(), 0.25f));

            yield return new WaitForSeconds(.25f);
            //Type each line
            for (int index2 = 0; index2 < dialogues[index].lines.Length; index2++)
            {
                nextLine = false;
                typingCoroutine = StartCoroutine(TypeLine(dialogues[index].lines[index2], dialogues[index].name, typingAudio, largeDialogueTextBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>(), .05f));
                lineToBeTyped = dialogues[index].lines[index2];

                Coroutine blinking = null;
                while (isTyping || !nextLine)
                {
                    yield return new WaitForSeconds(.25f);
                    if (!isTyping && !nextLine && blinking == null)
                    {
                        blinking = StartCoroutine(Helpers.DialogueBlinker("large"));
                    }

                }
                try
                {
                    StopCoroutine(blinking);
                }
                catch
                {
                    
                }
                blinking = null;
                Helpers.DisableBlinker("large");
                largeDialogueTextBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
            
            }

            //Fade out text box
            StartCoroutine(Helpers.MoveRectTransform(largeDialogueTextBox, largeDialogueTextBox.GetComponent<RectTransform>().anchoredPosition, largeDialogueTextBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, -10f), .25f));
            StartCoroutine(Helpers.FadeOutCanvasGroup(largeDialogueTextBox.GetComponent<CanvasGroup>(), 0.25f));

            yield return new WaitForSeconds(0.25f);

        }
    }

}