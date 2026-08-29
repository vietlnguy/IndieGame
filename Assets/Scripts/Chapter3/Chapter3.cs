using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Chapter3 : MonoBehaviour
{

    //Prefabs
    public GameObject basicEnemyPrefab;
    public GameObject mainCharacterPrefab;
    public GameObject astridPrefab;
    public GameObject celestePrefab;
    public GameObject lucasPrefab;
    public GameObject katherinePrefab;
    public GameObject gerardPrefab;
    public GameObject penelopePrefab;

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
    public GameObject camera;
    public GameObject victoryAndSubquestBox;


    //Script references
    public BattleController battleController;
    private SaveManager saveManager;
    public GameOver gameOverScript;
    public AttackPreview attackPreviewScript;
    private TilemapPathfinder pathfinder;
    private PlayerController penelopeScript;
    private PlayerController gerardScript;
    private PlayerController katherineScript;
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
    public GameObject mainCharacterLargePortrait;
    public GameObject astridLargePortrait;
    public GameObject lucasLargePortrait;
    public GameObject celesteLargePortrait;

    private List<CharacterDialogue> dialogues;
    private List<CharacterDialogue> dialogues2;
    private List<CharacterDialogue> dialogues3;
    private List<CharacterDialogue> dialogues4;
    private List<CharacterDialogue> dialogues5;
    private List<CharacterDialogue> dialogues6;
    private List<CharacterDialogue> outroDialogue1;

    public void Awake()
    {    

        AudioListener.volume = PlayerPrefs.GetFloat("volume", 0.5f);
        saveManager = FindAnyObjectByType<SaveManager>();
        characters = GameObject.Find("Characters");
        enemies = GameObject.Find("Enemies");
        pathfinder = FindAnyObjectByType<TilemapPathfinder>();
        victorySequenceScript = FindAnyObjectByType<VictorySequence>();

        dialogues = new List<CharacterDialogue>();
        dialogues.Add(new CharacterDialogue("Lord Beesly", new string[] {"Report, Commander."}));
        dialogues.Add(new CharacterDialogue("Soldier", new string[] {"My lord. The mercenaries we sent into town were unsuccessful in finding any relics."}));
        dialogues.Add(new CharacterDialogue("Lord Beesly", new string[] {"Tch. We're running out of time, Commander.", "Double the men and search again.", "Do whatever it takes to find the relics."}));
        dialogues.Add(new CharacterDialogue("Soldier", new string[] {"As you wish, my lord."}));

        dialogues2 = new List<CharacterDialogue>();
        dialogues2.Add(new CharacterDialogue("???", new string[] {"Lord Beesly."}));
        dialogues2.Add(new CharacterDialogue("Lord Beesly", new string[] {"Gah-", "What are you doing here??"}));
        dialogues2.Add(new CharacterDialogue("???", new string[] {"We had a deal, Lord Beesly."}));
        dialogues2.Add(new CharacterDialogue("Lord Beesly", new string[] {"I'm trying. I just need a little more time. I swear it."}));
        dialogues2.Add(new CharacterDialogue("???", new string[] {"If you do not produce any relics soon, you will have more to worry about than your precious castle.", "Speaking of which, your daughter approaches. She's quite lovely now isn't she?"}));
        dialogues2.Add(new CharacterDialogue("Lord Beesly", new string[] {"You wretch."}));
        dialogues2.Add(new CharacterDialogue("???", new string[] {"Tick tock, Lord Beesly."}));

        dialogues3 = new List<CharacterDialogue>();
        dialogues3.Add(new CharacterDialogue("Penelope", new string[] {"Father!", "I demand to know what is going on!", "What are these reports I am hearing of thugs ransacking the village??"}));
        dialogues3.Add(new CharacterDialogue("Lord Beesly", new string[] {"Penelope... please.", "You don't understand what is at stake.", "What I do, I do to protect us."}));
        dialogues3.Add(new CharacterDialogue("Penelope", new string[] {"It doesn't justify terrorizing these people.", "What is happening to you, Father?", "The man that raised me would never sacrifice his humanity for some stupid relics!"}));
        dialogues3.Add(new CharacterDialogue("Lord Beesly", new string[] {"..."}));

        dialogues4 = new List<CharacterDialogue>();
        dialogues4.Add(new CharacterDialogue("Soldier", new string[] {"My lord, sorry to inturrupt.", "There is a band of people that are requesting council.", "They say they have information about the relics."}));
        dialogues4.Add(new CharacterDialogue("Lord Beesly", new string[] {"Send them in, at once!"}));

        dialogues5 = new List<CharacterDialogue>();
        dialogues5.Add(new CharacterDialogue(saveManager.loadedData.mainCharacterName, new string[] {"Beesly!"}));
        dialogues5.Add(new CharacterDialogue("Lord Beesly", new string[] {saveManager.loadedData.mainCharacterName + "?", "What are you doing here?"}));
        dialogues5.Add(new CharacterDialogue(saveManager.loadedData.mainCharacterName, new string[] {"I'd rather be retired on my farm, but it seems like fate has other plans.", "Would you care to explain why imperial soldiers and thugs are ambushing citizens?"}));
        dialogues5.Add(new CharacterDialogue("Penelope", new string[] {"I want to know as well."}));
        dialogues5.Add(new CharacterDialogue("Lord Beesly", new string[] {"Sigh...", "I guess you deserve to know...", "The relics are more than just historical artifacts.", "They possess tremendous power.", "Rumors have spread of people doing unimaginable feats.", "Moving boulders and leaping mountains.", "King Reiss has charged me and the other state governors with collecting any and all relics."}));
        dialogues5.Add(new CharacterDialogue("Astrid", new string[] {"That would explain why those imperial soldiers arrived at our farm.", "And the tremendous power I felt."}));
        dialogues5.Add(new CharacterDialogue("Lord Beesly", new string[] {"You possess a relic??", "Please, give it here! You don't understand what is at stake!"}));
        dialogues5.Add(new CharacterDialogue("Astrid", new string[] {"But--"}));
        dialogues5.Add(new CharacterDialogue(saveManager.loadedData.mainCharacterName, new string[] {"No.", "Beesly, it doesn't matter what the King has threatened you with.", "This is not the way. Let us help you."}));
        dialogues5.Add(new CharacterDialogue("Lord Beesly", new string[] {"No! For your own sake, give me the relic.", "NOW!"}));
        dialogues5.Add(new CharacterDialogue("Penelope", new string[] {"Father, please!"}));
        dialogues5.Add(new CharacterDialogue("Lord Beesly", new string[] {"Stay back, Penelope!", "Guards, seize them!", "Do not let them leave with that relic!"}));

        dialogues6 = new List<CharacterDialogue>();
        dialogues6.Add(new CharacterDialogue(saveManager.loadedData.mainCharacterName, new string[] {"Shit..."}));
        dialogues6.Add(new CharacterDialogue("Lucas", new string[] {"Nice going. What's the plan, now?"}));
        dialogues6.Add(new CharacterDialogue(saveManager.loadedData.mainCharacterName, new string[] {"Escape!"}));
        dialogues6.Add(new CharacterDialogue("Penelope", new string[] {"Follow me! I know the way!", "Katherine! Gerard! With me, we're leaving!"}));
        dialogues6.Add(new CharacterDialogue("Gerard", new string[] {"Princess, but the Lord--"}));
        dialogues6.Add(new CharacterDialogue("Katherine", new string[] {"Penny..."}));
        dialogues6.Add(new CharacterDialogue("Penelope", new string[] {"You're both sworn to me. Help me fix this madness!"}));


        outroDialogue1 = new List<CharacterDialogue>();
        outroDialogue1.Add(new CharacterDialogue(saveManager.loadedData.mainCharacterName, new string[] {"Looks like we managed to scare them off for now.", "Is everybody alright?"}));

        bool hasNewCharacters = saveManager.loadedData.characters.Exists(c => c.characterName == "Penelope" || c.characterName == "Gerard" || c.characterName == "Katherine");

        if (!hasNewCharacters)
        {
            Character penelope = new Character("Penelope", 9, 11, 4, 7, 4, 6, 6, 5, 3, 4, false, true);
            penelope.knownAttacks.Add(new SupportMove("Heal", 3, "hp", 5, null, null, "Heal an ally. (Scales with INT)"));
            penelope.inventory.Add(new Item("Potion", 5, "hp", 10, "Restores 10 HP.", false, false, false));
            penelope.weaponEquiped = EquipmentManager.Create("Basic");
            penelope.armorEquiped = EquipmentManager.Create("Cloth");
            penelope.accessoryEquiped = EquipmentManager.Create("Mana Band");
            penelope.subquests.Add(new Subquest("Penelope1", "Don't let Penelope take any damage.", "Ask to learn more about the goddess."));
            penelope.subquests.Add(new Subquest("Penelope2", "Placeholder.", "Placeholder description."));
            penelope.subquests.Add(new Subquest("Penelope3", "Placeholder", "Placeholder description."));

            
            saveManager.loadedData.characters.Add(penelope);

            Character gerard = new Character("Gerard", 11, 7, 5, 3, 5, 5, 6, 6, 1, 5, false, false);
            gerard.knownAttacks.Add(new Attack("Rapid Punch", "physical", 1.0f, 1.0f, 95, 0, 0, new List<Debuff>(), "Strike the enemy with a quick punch.")); 
            gerard.inventory.Add(new Item("Potion", 5, "hp", 10, "Restores 10 HP.", false, false, false));
            gerard.weaponEquiped = EquipmentManager.Create("Basic");
            gerard.armorEquiped = EquipmentManager.Create("Cloth");
            gerard.accessoryEquiped = EquipmentManager.Create("Gauntlets");
            gerard.subquests.Add(new Subquest("Gerard1", "Lucas slays at least 2 enemies.", "Ask about his relationship to Celeste."));
            gerard.subquests.Add(new Subquest("Gerard2", "Placeholder.", "Placeholder description."));
            gerard.subquests.Add(new Subquest("Gerard3", "Placeholder.", "Placeholder description."));

            saveManager.loadedData.characters.Add(gerard);

            Character katherine = new Character("Katherine", 11, 7, 5, 3, 5, 5, 6, 6, 1, 5, false, false);
            katherine.knownAttacks.Add(new Attack("Rapid Punch", "physical", 1.0f, 1.0f, 95, 0, 0, new List<Debuff>(), "Strike the enemy with a quick punch.")); 
            katherine.inventory.Add(new Item("Potion", 5, "hp", 10, "Restores 10 HP.", false, false, false));
            katherine.weaponEquiped = EquipmentManager.Create("Basic");
            katherine.armorEquiped = EquipmentManager.Create("Cloth");
            katherine.accessoryEquiped = EquipmentManager.Create("Gauntlets");
            katherine.subquests.Add(new Subquest("Katherine1", "Lucas slays at least 2 enemies.", "Ask about his relationship to Celeste."));
            katherine.subquests.Add(new Subquest("Katherine2", "Placeholder.", "Placeholder description."));
            katherine.subquests.Add(new Subquest("Katherine3", "Placeholder.", "Placeholder description."));
            //TODO: Add more lucas subquests

            saveManager.loadedData.characters.Add(katherine);

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
            }
            else if (character.characterName == "Lucas")
            {
                GameObject temp = Instantiate(lucasPrefab, new Vector3(13f, -8f, 0f), Quaternion.identity, characters.transform);
            }
            else if (character.characterName == "Penelope")
            {
                GameObject temp = Instantiate(penelopePrefab, new Vector3(13f, -8f, 0f), Quaternion.identity, characters.transform);
                penelopeScript = temp.GetComponent<PlayerController>();
            }
            else if (character.characterName == "Gerard")
            {
                GameObject temp = Instantiate(gerardPrefab, new Vector3(13f, -8f, 0f), Quaternion.identity, characters.transform);
                gerardScript = temp.GetComponent<PlayerController>();
            }
            else if (character.characterName == "Katherine")
            {
                GameObject temp = Instantiate(katherinePrefab, new Vector3(13f, -8f, 0f), Quaternion.identity, characters.transform);
                katherineScript = temp.GetComponent<PlayerController>();
            }
        
        }

        //Give victory sequence script a list of all subquests
        List<Subquest> quests = new List<Subquest>();
        quests.Add(penelopeScript.subquests[0]);
        quests.Add(gerardScript.subquests[0]);
        quests.Add(katherineScript.subquests[0]);
        victorySequenceScript.subquests = quests;

    }
    public void Start()
    {
        if (saveManager.loadedData.introBattleOutro == "Intro")
        {
            intro = StartCoroutine(Intro());
        }
        else if (saveManager.loadedData.introBattleOutro == "Battle")
        {
            intro = StartCoroutine(Battle());
        }
        else if (saveManager.loadedData.introBattleOutro == "Outro")
        {
            Outro();
        }
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
                    largeDialogueTextBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = lineToBeTyped;
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
            //if (celesteScript.currentHp < celesteScript.maxHp) 
            //{
            //    celesteScript.subquests[0].failed = true;
            //    subquest1X.color = new Color(1f, 1f, 1f, 1f);
            //}

            //Win condition
            if (battleController.enemies.transform.childCount == 0 && enemiesSpawned && !victorySequenceStarted && !attackPreviewScript.coroutineRunning)
            {
                //if (celesteScript.subquests[0].failed == false) {
                //    celesteScript.subquests[0].completed = true;
                //    subquest1Check.color = new Color(1f, 1f, 1f, 1f);
                //}
                //
                //if (lucasScript.subquests[0].completed == false) {
                //    lucasScript.subquests[0].failed = true;
                //    subquest2X.color = new Color(1f, 1f, 1f, 1f);
                //}

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

        enemy.roams = true;
        enemy.ranged = true;
        enemy.support = false;
        enemy.hybrid = false;

        enemy.knownAttacks.Add(new Attack("Bow Shot", "physical", 1.0f, 1.0f, 90, 0, 0,  new List<Debuff>(),"Shoot at arrow at the enemy."));
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
        enemy.knownAttacks.Add(new Attack("Bash", "physical", 1.1f, 1.0f, 90, 0, 0,  new List<Debuff>(),"Bash the enemy with your weapon."));

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



    }
    private IEnumerator Intro()
    {
        GameObject mainChar = GameObject.Find("MainCharacterPrefab(Clone)");
        GameObject astrid = GameObject.Find("AstridPrefab(Clone)");
        GameObject celeste = GameObject.Find("CelestePrefab(Clone)");
        GameObject lucas = GameObject.Find("LucasPrefab(Clone)");
        GameObject penelope = GameObject.Find("PenelopePrefab(Clone)");
        GameObject gerard = GameObject.Find("GerardPrefab(Clone)");
        GameObject katherine = GameObject.Find("KatherinePrefab(Clone)");
        
        //Intro sequence
        if (saveManager.loadedData.introBattleOutro == "Intro") 
        {
            //Fade Out blackwhite screen
            yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));

            yield return new WaitForSeconds(2f);

            //Move characters to front gate
            yield return StartCoroutine(pathfinder.FollowPath(mainChar, new Vector3(-18.5f, -11f, 0f)));
            yield return StartCoroutine(pathfinder.FollowPath(astrid, new Vector3(-20.5f, -12.7f, 0f)));
            yield return StartCoroutine(pathfinder.FollowPath(lucas, new Vector3(-18.5f, -11f, 0f)));
            yield return StartCoroutine(pathfinder.FollowPath(celeste, new Vector3(-20.5f, -12.7f, 0f)));

            //Small dialogue
            yield return StartCoroutine(PlaySmallDialogue(dialogues));
            typingCoroutine = null;

            //Dialogue about entering the castle

            //Fade to black then reappear inside castle throne room

            //Mysterious figure appears confronting Lord Beesly

            //Dialogue about collecting relics etc

            //Mysterious figure disappears

            //Enter Penelope, Gerard, and Katherine

            //Dialogue about confrontation

            //Soldier enters to introduce party

            //Party enters

            //Dialogue

            //Party escape



            lucas.GetComponent<SpriteRenderer>().enabled = false;
            celeste.GetComponent<SpriteRenderer>().enabled = false;

            //Fade Out blackwhite screen
            yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));

            yield return new WaitForSeconds(2f);
            //Move characters on screen
            yield return StartCoroutine(pathfinder.FollowPath(mainChar, new Vector3(-18.5f, -11f, 0f)));
            yield return StartCoroutine(pathfinder.FollowPath(astrid, new Vector3(-20.5f, -12.7f, 0f)));

            //Small dialogue
            yield return StartCoroutine(PlaySmallDialogue(dialogues));
            typingCoroutine = null;

            //Pan camera to church
            yield return StartCoroutine(Helpers.CameraMoveTransform(camera.transform, camera.transform.position, new Vector3(10.23f, -7.5f, -10f), 1.5f));
            
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
            yield return StartCoroutine(PlaySmallDialogue(dialogues2));
            typingCoroutine = null;

            //enter boss
            GameObject boss = Instantiate(basicEnemyPrefab, new Vector3(13f, -8f, 0f), Quaternion.identity, enemies.transform);
            boss.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(Helpers.EnterCharacter(boss.GetComponent<SpriteRenderer>(), 0.15f));
            yield return StartCoroutine(pathfinder.FollowPath(boss, new Vector3(15f, -12.5f, 0f)));

            //small dialoue 3
            yield return StartCoroutine(PlaySmallDialogue(dialogues3));
            typingCoroutine = null;

            pathfinder.moveSpeed = 5f;
            StartCoroutine(pathfinder.FollowPath(lucas, new Vector3(-1.5f, -16.5f, 0f)));
            yield return StartCoroutine(pathfinder.FollowPath(celeste, new Vector3(1f, -18f, 0f)));

            //pan camera back
            yield return StartCoroutine(Helpers.CameraMoveTransform(camera.transform, camera.transform.position, new Vector3(-6.55f, -7.5f, -10f), 1.5f));

            //small dialoue 3
            yield return StartCoroutine(PlaySmallDialogue(dialogues4));
            typingCoroutine = null;

            //fade to black
            yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 0.5f));
            saveManager.loadedData.introBattleOutro = "Battle";
        }
        
        yield return StartCoroutine(Battle());
    }
    private IEnumerator Battle()
    {
        GameObject mainChar = GameObject.Find("MainCharacterPrefab(Clone)");
        GameObject astrid = GameObject.Find("AstridPrefab(Clone)");
        GameObject celeste = GameObject.Find("CelestePrefab(Clone)");
        GameObject lucas = GameObject.Find("LucasPrefab(Clone)");
        GameObject penelope = GameObject.Find("PenelopePrefab(Clone)");
        GameObject gerard = GameObject.Find("GerardPrefab(Clone)");
        GameObject katherine = GameObject.Find("KatherinePrefab(Clone)");

        astrid.transform.position = new Vector3(-20.5f, -12.7f, 0f);
        mainChar.transform.position = new Vector3(-18.5f, -11f, 0f);
        lucas.transform.position =  new Vector3(-1.5f, -16.5f, 0f);
        celeste.transform.position = new Vector3(1f, -18f, 0f);
        StartCoroutine(Helpers.FadeInAudio(dangerIntroAudio, 1.5f));

        
        //Spawn enemies
        foreach (Transform child in enemies.transform)
        {
            Destroy(child.gameObject);
        }
        CreateEnemies();

        yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));
        battleController.StartCombat();
        saveManager.loadedData.introBattleOutro = "Battle";
        saveManager.OverwriteSave();
        VictorySubscribe();
        intro = null;
    }
    public IEnumerator OutroHelper()
    {   

        saveManager.loadedData.introBattleOutro = "Outro";
        saveManager.OverwriteSave();
        
        yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1.5f));
        outroScreen.enabled = true;
        yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1.5f));
        mainCharacterLargePortrait.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        astridLargePortrait.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        lucasLargePortrait.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        celesteLargePortrait.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        mainCharacterLargePortrait.GetComponent<RectTransform>().anchoredPosition = new Vector2(-318f, mainCharacterLargePortrait.GetComponent<RectTransform>().anchoredPosition.y);
        astridLargePortrait.GetComponent<RectTransform>().anchoredPosition = new Vector2(-194f, astridLargePortrait.GetComponent<RectTransform>().anchoredPosition.y);
        lucasLargePortrait.GetComponent<RectTransform>().anchoredPosition = new Vector2(206f, lucasLargePortrait.GetComponent<RectTransform>().anchoredPosition.y);
        celesteLargePortrait.GetComponent<RectTransform>().anchoredPosition = new Vector2(382f, celesteLargePortrait.GetComponent<RectTransform>().anchoredPosition.y);

        //Enter characters
        StartCoroutine(Helpers.UndoFadeToBlackTransparent(mainCharacterLargePortrait, 0.5f));
        yield return StartCoroutine(Helpers.UndoFadeToBlackTransparent(astridLargePortrait, 0.5f));
        StartCoroutine(Helpers.UndoFadeToBlackTransparent(lucasLargePortrait, 0.5f));
        yield return StartCoroutine(Helpers.UndoFadeToBlackTransparent(celesteLargePortrait, 0.5f));

        yield return StartCoroutine(PlayLargeDialogue(outroDialogue1));
        typingCoroutine = null;

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(saveManager.SceneTransition(true));
        saveManager.loadedData.currentChapter = "Chapter 3";
        saveManager.loadedData.introBattleOutro = "Overworld";
        saveManager.OverwriteSave();
        SceneManager.LoadScene("Overworld");
    }

    //Should rarely change
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