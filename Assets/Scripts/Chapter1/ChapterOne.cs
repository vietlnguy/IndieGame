using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

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
    public Tutorial tutorialScript;
    public VictorySequence victorySequence;
    public GameObject mainCharacterPrefab;
    public GameObject astridPrefab;
    public GameObject mainChar;
    private GameObject characters;
    private PlayerController astridScript;
    private TilemapPathfinder pathfinder;
    private Coroutine intro;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool nextLine = false;
    private string lineToBeTyped = "";
    public AudioSource typingAudio;
    public GameObject smallDialogueTextBox;
    public TextMeshProUGUI smallDialogueNameBox;
    public Image blackScreen;
    public Image whiteScreen;
    public Image sexScreen;
    public Image houseScreen;
    public AudioSource fluteAudio;
    public AudioSource doorAudio;
    public GameObject mainCharacterSmallPortrait;
    public GameObject astridSmallPortrait;
    public GameObject hegsethLargePortrait;
    public GameObject soldierSmallPortrait;

    private List<CharacterDialogue> dialogues;
    private List<CharacterDialogue> dialogues2;
    private List<CharacterDialogue> dialogues3;
    private List<CharacterDialogue> dialogues4;
    private List<CharacterDialogue> dialogues5;
    private List<CharacterDialogue> dialogues6;
    private List<CharacterDialogue> dialogues7;
    private List<CharacterDialogue> dialogues8;
    private List<CharacterDialogue> dialogues9;
    private List<CharacterDialogue> dialogues10;
    private List<CharacterDialogue> dialogues11;
    private List<CharacterDialogue> dialogues12;
    private List<CharacterDialogue> dialogues13;
    

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
                mainChar = Instantiate(mainCharacterPrefab, new Vector3(-20f, -9.35f), Quaternion.identity, characters.transform);
            }
            else if (character.characterName == "Astrid")
            {
                GameObject temp = Instantiate(astridPrefab, new Vector3(-4.45f, -11.65f), Quaternion.identity, characters.transform);
                astridScript = GameObject.Find("AstridPrefab(Clone)").GetComponent<PlayerController>();

            }
        }

        victorySequence = FindFirstObjectByType<VictorySequence>();
        victorySequence.subquests.Add(astridScript.subquests[0]);

        dialogues = new List<CharacterDialogue>();
        dialogues.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"Honey, I'm back!"}));
        dialogues.Add(new CharacterDialogue(astridSmallPortrait, "Astrid", new string[] {"Oh! Back so soon?", "I've hardly started dinner!"}));
        dialogues.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"I had an early start this morning.", "Need an extra hand?"}));
        dialogues.Add(new CharacterDialogue(astridSmallPortrait, "Astrid", new string[] {"No thank you, dear.", "You've already done so much around the farm, I can handle dinner.", "You should relax!"}));
        dialogues.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"It's hard to relax when there is still so much to do...", "I'm the one that said we should move out here.", "I'm starting to wonder if that was a dumb decision..."}));
        dialogues.Add(new CharacterDialogue(astridSmallPortrait, "Astrid", new string[] {"I've never regretted a second being here.", "It doesn't matter where we are, as long as we're together, I'm happy."}));
        dialogues.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"I don't deserve you."}));
        dialogues.Add(new CharacterDialogue(astridSmallPortrait, "Astrid", new string[] {"Hehe, that's what they all say.", "Now why don't I help you relax a little more?", "You must be so tense. Let me give you a hand."}));
        dialogues.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"Actually, I don't feel as sore as-- uughh--"}));

        dialogues2 = new List<CharacterDialogue>();
        dialogues2.Add(new CharacterDialogue(astridSmallPortrait, "Astrid", new string[] {"Hehe, maybe you are just a dumb farmer.", "Working out in the sun all day must've turned your brain to mush."}));
        dialogues2.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"Haah... maybe you're right.. Ahh...", "It doesn't help that you're draining all of the blood from my head now, too."}));
        dialogues2.Add(new CharacterDialogue(astridSmallPortrait, "Astrid", new string[] {"It's okay, it's looks like it's going to a different head, hehe.", "Looks like I better give it some special attention."}));
        dialogues2.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"Ungh..."}));
        dialogues2.Add(new CharacterDialogue(astridSmallPortrait, "Astrid", new string[] {"Mwgh, mwgh, mwgh..."}));
        dialogues2.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"Ahh-- it feels too good!", "I don't think I can hold it in any longer!"}));
        dialogues2.Add(new CharacterDialogue(astridSmallPortrait, "Astrid", new string[] {"Mmmmm--"}));

        dialogues3 = new List<CharacterDialogue>();
        dialogues3.Add(new CharacterDialogue(astridSmallPortrait, "Astrid", new string[] {"Hehe, feeling better?"}));
        dialogues3.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"I feel like a new man!", "Like I've got the strength to plow ten fields!"}));
        dialogues3.Add(new CharacterDialogue(astridSmallPortrait, "Astrid", new string[] {"As long as you make time to plow mine."}));
        dialogues3.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"Of course!", "Truth be told... I was worried about moving to the countryside.", "Away from all of our friends and family, starting over.", "With all of that news about ancient Tah'Lo artifacts, the world seems to be spinning faster and faster.", "A quiet life with you is all I need."}));
        dialogues3.Add(new CharacterDialogue(astridSmallPortrait, "Astrid", new string[] {"I couldn't agree more.", "I am curious, though. They say the ancient Tah'Lo people were incredibly advanced.", "I wonder what kind of amazing things they could do..."}));
        dialogues3.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"Beats me.", "As long as it doesn't involve me, they can have all of the artifacts they want."}));
        
        dialogues4 = new List<CharacterDialogue>();
        dialogues4.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"Huh? Someone's at the door?", "All the way out here?"}));
        
        dialogues5 = new List<CharacterDialogue>(); 
        dialogues5.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"Hello. Apologies for the intrusion.", "My name is Hegseth. I am a member of the Kings council.", "What a wonderful home you have."}));
        dialogues5.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"Thank you.", "Is there something I can help you with?"}));
        dialogues5.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"Yes, indeed.", "As I am sure you are aware, our great King Reiss (long may he reign), has declared that all Tah'Lo artifacts be relinquished to local authorities.", "It is for the safety and prosperity of the people that our king has decreed it."}));
        dialogues5.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"That's fine, but we don't have any Tah'Lo artifacts.", "I couldn't even tell you if I saw one."}));
        dialogues5.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"I see.", "Well then, you wouldn't mind if we inspected the area?", "It is typical for Tah'Lo artifacts to go unnoticed by ...commoners."}));
        dialogues5.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"Is that necessary? There is nothing here but wheat and pig crap."}));
        dialogues5.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"I assure you it will not take long.", "We would hate to send word to the king that some folk have been uncooperative..."}));
        dialogues5.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"..."}));
        dialogues5.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"Good.", "Now then...", "Men!"}));
           
        dialogues6 = new List<CharacterDialogue>();        
        dialogues6.Add(new CharacterDialogue(soldierSmallPortrait, "Soldier", new string[] {"Yes, sir!"}));
        dialogues6.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"Search the area for any Tah'Lo artifacts.", "Be thorough, I will not tolerate any mistakes."}));
        dialogues6.Add(new CharacterDialogue(soldierSmallPortrait, "Soldier", new string[] {"Yes, sir!"}));     
        
        dialogues7 = new List<CharacterDialogue>(); 
        dialogues7.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"Did you find anything?"}));
        dialogues7.Add(new CharacterDialogue(soldierSmallPortrait, "Soldier", new string[] {"Nothing, sir. Checked every nook and cranny."}));
        dialogues7.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"Hmm. Perhaps you are telling the truth."}));
        dialogues7.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"I told you.", "Now if you're done, I'd like you to leave. I have work to do."}));
        dialogues7.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"Of course, of course!", "There's just one last thing...", "The lady's bracelets.", "Hand them over."}));
        dialogues7.Add(new CharacterDialogue(astridSmallPortrait, "Astrid", new string[] {"What? My bracelets?", "These aren't Tah'Lo artifacts. These are just ordinary bracelets."}));
        dialogues7.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"That will be for the king to decide."}));
        dialogues7.Add(new CharacterDialogue(astridSmallPortrait, "Astrid", new string[] {"What? No!", "There must be a mistake.", "These were a gift from my mother! I can't give them away!"}));
        dialogues7.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"Such a shame.. But your mother will be proud of you for being a devout citizen of the kingdom.", "Now let me take those off your hands--"}));
        
        dialogues8 = new List<CharacterDialogue>();
        dialogues8.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"Take another step towards her and you will regret it."}));
            
        dialogues9 = new List<CharacterDialogue>(); 
        dialogues9.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"Oh ho ho.", "I would think very carefully about what you are doing, boy.", "Defying me is defying the king."}));
        dialogues9.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"Leave. I won't ask you again."}));
        dialogues9.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"Very well.", "Remember that you chose this...", "Men! Kill her and take the bracelet!"}));
        dialogues9.Add(new CharacterDialogue(soldierSmallPortrait, "Soldier", new string[] {"Yes, sir!"}));
        
        dialogues10 = new List<CharacterDialogue>();
        dialogues10.Add(new CharacterDialogue(astridSmallPortrait, "Astrid", new string[] {"Ah! Liam!"}));
        dialogues10.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"Astrid!!!"}));
        
        dialogues11 = new List<CharacterDialogue>();
        dialogues11.Add(new CharacterDialogue(soldierSmallPortrait, "Soldier", new string[] {"What the--"}));
        dialogues11.Add(new CharacterDialogue(soldierSmallPortrait, "Soldier", new string[] {"Ack!"}));

        dialogues12 = new List<CharacterDialogue>();
        dialogues12.Add(new CharacterDialogue(soldierSmallPortrait, "Soldier", new string[] {"Ack!"}));
        dialogues12.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"Astrid?", "What's happening?"}));
        dialogues12.Add(new CharacterDialogue(astridSmallPortrait, "Astrid", new string[] {"I don't know. I just felt a surge of power coming from my bracelets!"}));
        dialogues12.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"What are you doing, soldier?? Kill her!"}));
        dialogues12.Add(new CharacterDialogue(soldierSmallPortrait, "Soldier", new string[] {"Uh...", "Yes, sir!"}));
        dialogues12.Add(new CharacterDialogue(soldierSmallPortrait, "Soldier", new string[] {"Urgh..."}));
        dialogues12.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"Useless, peasant..", "Retreat! Gather the men outside! We'll take the artifact by sheer force!"}));
        dialogues12.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"Argh. That doesn't sound good.", "Astrid, are you okay?"}));
        dialogues12.Add(new CharacterDialogue(astridSmallPortrait, "Astrid", new string[] {"*huff huff*", "Yes, I'm okay.", "I just need to catch my breath then I'll be ready to fight."}));
        dialogues12.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"No, you should stay here. I can handle this."}));
        dialogues12.Add(new CharacterDialogue(astridSmallPortrait, "Astrid", new string[] {"No way.", "I am just as much a fighter as you.", "We will protect our home together."}));
        dialogues12.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"Okay, stay near me.", "Let's kill the bastard."}));

        dialogues13 = new List<CharacterDialogue>();
        dialogues13.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"It's been a while since we've been in battle.", "Let's take this slowly.", "I'll charge the enemy, you support me from behind."}));
        dialogues13.Add(new CharacterDialogue(astridSmallPortrait, "Astrid", new string[] {"Let's do this!"}));

    }
    public void Start()
    {
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
            StartCoroutine(Helpers.FadeInAudio(fluteAudio, 1f));
            yield return StartCoroutine(pathfinder.FollowPath(mainChar, new Vector3(-6.57f, -11.28f, 0f)));
            yield return new WaitForSeconds(.5f);
            doorAudio.Play();
            yield return StartCoroutine(Helpers.FadeSpriteToBlack(mainChar));
            yield return StartCoroutine(PlaySmallDialogue(dialogues));
            typingCoroutine = null;

            //NSFW scene
            yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1f));
            sexScreen.GetComponent<CanvasGroup>().alpha = 1f;
            yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));
            yield return StartCoroutine(PlaySmallDialogue(dialogues2));
            typingCoroutine = null;

            //In house dialogue
            yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1f));
            sexScreen.GetComponent<CanvasGroup>().alpha = 0f;
            houseScreen.GetComponent<CanvasGroup>().alpha = 1f;
            yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));

            //yield return StartCoroutine(PlaySmallDialogue(dialogues2));
            //typingCoroutine = null;

    /*
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
        
        */
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
    private IEnumerator PlaySmallDialogue(List<CharacterDialogue> dialogues)
    {
        //Small dialogue
        for (int index = 0; index < dialogues.Count; index++)
        {
            //Update name text
            smallDialogueNameBox.text = dialogues[index].name;

            DisableAllPortraits();
            dialogues[index].characterImage.SetActive(true);

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
    }
    private void DisableAllPortraits() {
        mainCharacterSmallPortrait.SetActive(false);
        astridSmallPortrait.SetActive(false);
        soldierSmallPortrait.SetActive(false);
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