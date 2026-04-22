using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    public Tutorial tutorialScript;
    public VictorySequence victorySequence;
    public GameObject mainCharacterPrefab;
    public GameObject astridPrefab;
    private GameObject mainCharacterObject;
    private GameObject astridObject;
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
    public GameObject largeDialogue;
    public GameObject largeDialogueTextBox;
    public TextMeshProUGUI largeDialogueNameBox;
    public Image blackScreen;
    public Image whiteScreen;
    public Image sexScreen;
    public Image houseScreen;
    public Image outroScreen;
    public AudioSource fluteAudio;
    public AudioSource doorAudio;
    public AudioSource doorKnockAudio;
    public AudioSource hegsethThemeAudio;
    public AudioSource rummagingAudio;
    public AudioSource shineAudio;
    public AudioSource knockbackAudio;
    public AudioSource battleMusicAudio;
    public GameObject mainCharacterSmallPortrait;
    public GameObject mainCharacterLargePortrait;
    public GameObject astridSmallPortrait;
    public GameObject astridLargePortrait;
    public GameObject hegsethLargePortrait;
    public GameObject soldierSmallPortrait;
    public GameObject soldierLargePortrait;
    public GameObject smallDialogueBlinker;
    public GameObject largeDialogueBlinker;
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
    private List<CharacterDialogue> dialogues14;
    private List<CharacterDialogue> dialogues15;
    private List<CharacterDialogue> dialogues16;

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
                mainCharacterObject = Instantiate(mainCharacterPrefab, new Vector3(-20f, -9.35f), Quaternion.identity, characters.transform);
            }
            else if (character.characterName == "Astrid")
            {
                astridObject = Instantiate(astridPrefab, new Vector3(-6.58f, -10.55f), Quaternion.identity, characters.transform);
                StartCoroutine(Helpers.FadeSpriteToBlack(astridObject));
                astridScript = GameObject.Find("AstridPrefab(Clone)").GetComponent<PlayerController>();

            }
        }

        victorySequence = FindFirstObjectByType<VictorySequence>();
        victorySequence.subquests.Add(astridScript.subquests[0]);
        VictorySubscribe();

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
        dialogues3.Add(new CharacterDialogue(astridLargePortrait, "Astrid", new string[] {"Hehe, feeling better?"}));
        dialogues3.Add(new CharacterDialogue(mainCharacterLargePortrait, saveManager.loadedData.mainCharacterName, new string[] {"I feel like a new man!", "Like I've got the strength to plow ten fields!"}));
        dialogues3.Add(new CharacterDialogue(astridLargePortrait, "Astrid", new string[] {"As long as you make time to plow mine."}));
        dialogues3.Add(new CharacterDialogue(mainCharacterLargePortrait, saveManager.loadedData.mainCharacterName, new string[] {"Of course!", "Truth be told... I was worried about moving to the countryside.", "Away from all of our friends and family, starting over.", "With all of that news about ancient Tah'Lo artifacts, the world seems to be spinning faster and faster.", "A quiet life with you is all I need."}));
        dialogues3.Add(new CharacterDialogue(astridLargePortrait, "Astrid", new string[] {"I couldn't agree more.", "I am curious, though. They say the ancient Tah'Lo people were incredibly advanced.", "I wonder what kind of amazing things they could do..."}));
        dialogues3.Add(new CharacterDialogue(mainCharacterLargePortrait, saveManager.loadedData.mainCharacterName, new string[] {"Beats me.", "As long as it doesn't involve me, they can have all of the artifacts they want."}));
        
        dialogues4 = new List<CharacterDialogue>();
        dialogues4.Add(new CharacterDialogue(mainCharacterLargePortrait, saveManager.loadedData.mainCharacterName, new string[] {"Huh? Someone's at the door?", "All the way out here?"}));
        
        dialogues5 = new List<CharacterDialogue>(); 
        dialogues5.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"Hello. Apologies for the intrusion.", "My name is Hegseth. I am a member of the Kings council.", "What a wonderful home you have."}));
        dialogues5.Add(new CharacterDialogue(mainCharacterLargePortrait, saveManager.loadedData.mainCharacterName, new string[] {"Thank you.", "Is there something I can help you with?"}));
        dialogues5.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"Yes, indeed.", "As I am sure you are aware, our great King Reiss (long may he reign), has declared that all Tah'Lo artifacts be relinquished to local authorities.", "It is for the safety and prosperity of the people that our king has decreed it."}));
        dialogues5.Add(new CharacterDialogue(mainCharacterLargePortrait, saveManager.loadedData.mainCharacterName, new string[] {"That's fine, but we don't have any Tah'Lo artifacts.", "I couldn't even tell you if I saw one."}));
        dialogues5.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"I see.", "Well then, you wouldn't mind if we inspected the area?", "It is typical for Tah'Lo artifacts to go unnoticed by ...commoners."}));
        dialogues5.Add(new CharacterDialogue(mainCharacterLargePortrait, saveManager.loadedData.mainCharacterName, new string[] {"Is that necessary? There is nothing here but wheat and pig crap."}));
        dialogues5.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"I assure you it will not take long.", "We would hate to send word to the king that some folk have been uncooperative..."}));
        dialogues5.Add(new CharacterDialogue(mainCharacterLargePortrait, saveManager.loadedData.mainCharacterName, new string[] {"..."}));
        dialogues5.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"Good.", "Now then...", "Men!"}));
           
        dialogues6 = new List<CharacterDialogue>();        
        dialogues6.Add(new CharacterDialogue(soldierLargePortrait, "Soldier", new string[] {"Yes, sir!"}));
        dialogues6.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"Search the area for any Tah'Lo artifacts.", "Be thorough, I will not tolerate any mistakes."}));
        dialogues6.Add(new CharacterDialogue(soldierLargePortrait, "Soldier", new string[] {"Yes, sir!"}));     
        
        dialogues7 = new List<CharacterDialogue>(); 
        dialogues7.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"Did you find anything?"}));
        dialogues7.Add(new CharacterDialogue(soldierLargePortrait, "Soldier", new string[] {"Nothing, sir. Checked every nook and cranny."}));
        dialogues7.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"Hmm. Perhaps you are telling the truth."}));
        dialogues7.Add(new CharacterDialogue(mainCharacterLargePortrait, saveManager.loadedData.mainCharacterName, new string[] {"Now if you're done, I'd like you to leave. I have work to do."}));
        dialogues7.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"Of course, of course!", "There's just one last thing...", "The lady's bracelets.", "Hand them over."}));
        dialogues7.Add(new CharacterDialogue(astridLargePortrait, "Astrid", new string[] {"What? My bracelets?", "These aren't Tah'Lo artifacts. These are just ordinary bracelets."}));
        dialogues7.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"That will be for the king to decide."}));
        dialogues7.Add(new CharacterDialogue(astridLargePortrait, "Astrid", new string[] {"What? No!", "There must be a mistake.", "These were a gift from my mother! I can't give them away!"}));
        dialogues7.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"Such a shame.. But your mother will be proud of you for being a devout citizen of the kingdom.", "Now let me take those off your hands--"}));
        
        dialogues8 = new List<CharacterDialogue>();
        dialogues8.Add(new CharacterDialogue(mainCharacterLargePortrait, saveManager.loadedData.mainCharacterName, new string[] {"Stop right there. That's enough.", "We told you we don't have any relics.", "I am going to have to ask you and the men you brought to leave."}));
            
        dialogues9 = new List<CharacterDialogue>(); 
        dialogues9.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"I would think very carefully about what you are doing, boy.", "To defy me, is to defy your king."}));
        dialogues9.Add(new CharacterDialogue(mainCharacterLargePortrait, saveManager.loadedData.mainCharacterName, new string[] {"I won't ask you again."}));
        dialogues9.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"Very well. You leave me no choice then.", "Soldier! Grab the woman and take the bracelet! Kill them both if you have to."}));
        dialogues9.Add(new CharacterDialogue(soldierLargePortrait, "Soldier", new string[] {"Yes, sir!"}));
        
        dialogues10 = new List<CharacterDialogue>();
        dialogues10.Add(new CharacterDialogue(astridLargePortrait, "Astrid", new string[] {"Ah- " + saveManager.loadedData.mainCharacterName + "!"}));
        dialogues10.Add(new CharacterDialogue(mainCharacterLargePortrait, saveManager.loadedData.mainCharacterName, new string[] {"Astrid!!!"}));
        
        dialogues11 = new List<CharacterDialogue>();
        dialogues11.Add(new CharacterDialogue(soldierSmallPortrait, "Soldier", new string[] {"What the--"}));
        dialogues11.Add(new CharacterDialogue(soldierSmallPortrait, "Soldier", new string[] {"Ack!"}));

        dialogues12 = new List<CharacterDialogue>();
        dialogues12.Add(new CharacterDialogue(soldierLargePortrait, "Soldier", new string[] {"Oof--"}));
        dialogues12.Add(new CharacterDialogue(mainCharacterLargePortrait, saveManager.loadedData.mainCharacterName, new string[] {"What the...", "Astrid, what's happening?"}));
        dialogues12.Add(new CharacterDialogue(astridLargePortrait, "Astrid", new string[] {"I don't know. I just felt a surge of power coming from my bracelets!"}));
        dialogues12.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"What are you doing, soldier?? Kill her!"}));
        dialogues12.Add(new CharacterDialogue(soldierLargePortrait, "Soldier", new string[] {"Uh...", "Yes, sir!"}));

        dialogues13 = new List<CharacterDialogue>();
        dialogues13.Add(new CharacterDialogue(soldierLargePortrait, "Soldier", new string[] {"Urgh..."}));
        dialogues13.Add(new CharacterDialogue(hegsethLargePortrait, "Hegseth", new string[] {"Useless...", "Retreat! We'll regroup outside and take the relic by force!"}));

        dialogues14 = new List<CharacterDialogue>();
        dialogues14.Add(new CharacterDialogue(mainCharacterLargePortrait, saveManager.loadedData.mainCharacterName, new string[] {"Argh. That doesn't sound good.", "Astrid, are you okay?"}));
        dialogues14.Add(new CharacterDialogue(astridLargePortrait, "Astrid", new string[] {"*huff huff*", "Yes, I'm okay.", "I just need to catch my breath then I'll be ready to fight."}));
        dialogues14.Add(new CharacterDialogue(mainCharacterLargePortrait, saveManager.loadedData.mainCharacterName, new string[] {"No, you should stay here. I can handle this."}));
        dialogues14.Add(new CharacterDialogue(astridLargePortrait, "Astrid", new string[] {"No way.", "I am just as much a fighter as you.", "We will protect our home together."}));
        dialogues14.Add(new CharacterDialogue(mainCharacterLargePortrait, saveManager.loadedData.mainCharacterName, new string[] {"Okay, stay near me.", "Let's kill the bastard."}));

        dialogues15 = new List<CharacterDialogue>();
        dialogues15.Add(new CharacterDialogue(mainCharacterSmallPortrait, saveManager.loadedData.mainCharacterName, new string[] {"It's been a while since we've been in battle.", "Let's take this slowly.", "I'll charge the enemy, you support me from behind."}));
        dialogues15.Add(new CharacterDialogue(astridSmallPortrait, "Astrid", new string[] {"Let's do this!"}));

        //Outro
        dialogues16 = new List<CharacterDialogue>();
        dialogues16.Add(new CharacterDialogue(mainCharacterLargePortrait, saveManager.loadedData.mainCharacterName, new string[] {"Phew... I don't know the last time I swung a sword.", "Or killed a man..", "It's done now, we're safe."}));
        dialogues16.Add(new CharacterDialogue(astridLargePortrait, "Astrid", new string[]{"..."}));
        dialogues16.Add(new CharacterDialogue(mainCharacterLargePortrait, saveManager.loadedData.mainCharacterName, new string[] {"What's wrong? Are you okay?"}));
        dialogues16.Add(new CharacterDialogue(astridLargePortrait, "Astrid", new string[] {"What are we going to do now? We can't stay here.", "As long as I have these bracelets, we'll never be safe.","Maybe I should've just handed them over."}));
        dialogues16.Add(new CharacterDialogue(mainCharacterLargePortrait, saveManager.loadedData.mainCharacterName, new string[] {"Absolutely not.", "We're going to speak with Lord Beesly, in town. I've known him to be an honorable man.", "He will speak to the royal envoy and fix this."}));
        dialogues16.Add(new CharacterDialogue(astridLargePortrait, "Astrid", new string[] {"*sigh* And just as we were getting settled in..."}));


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
            yield return StartCoroutine(pathfinder.FollowPath(mainCharacterObject, new Vector3(-6.57f, -11.28f, 0f)));
            yield return new WaitForSeconds(.5f);
            doorAudio.Play();

            yield return StartCoroutine(Helpers.FadeSpriteToBlack(mainCharacterObject));
            yield return StartCoroutine(PlaySmallDialogue(dialogues));
            typingCoroutine = null;

            //NSFW scene
            yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1f));
            sexScreen.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));
            yield return StartCoroutine(PlaySmallDialogue(dialogues2));
            typingCoroutine = null;

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
            yield return StartCoroutine(PlayLargeDialogue(dialogues3));
            typingCoroutine = null;

            //Door knock
            fluteAudio.Stop();
            doorKnockAudio.Play();
            yield return new WaitForSeconds(1.5f);
            yield return StartCoroutine(PlayLargeDialogue(dialogues4));
            typingCoroutine = null;

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
            yield return StartCoroutine(PlayLargeDialogue(dialogues5));
            typingCoroutine = null;

            //Enter soldier
            soldierLargePortrait.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            soldierLargePortrait.GetComponent<RectTransform>().anchoredPosition = new Vector2(-366f, -208f);
            yield return StartCoroutine(Helpers.UndoFadeToBlackTransparent(soldierLargePortrait, .5f));
            yield return StartCoroutine(PlayLargeDialogue(dialogues6));
            typingCoroutine = null;

            //Exit soldier and search
            yield return StartCoroutine(Helpers.FadeToBlackTransparent(soldierLargePortrait, 0.5f));
            rummagingAudio.Play();
            yield return new WaitForSeconds(7f);
            yield return StartCoroutine(Helpers.UndoFadeToBlackTransparent(soldierLargePortrait, 0.5f));
            yield return StartCoroutine(PlayLargeDialogue(dialogues7));
            typingCoroutine = null;

            //Move hegseth towards astrid
            yield return StartCoroutine(Helpers.MoveRectTransform(hegsethLargePortrait, hegsethLargePortrait.GetComponent<RectTransform>().anchoredPosition, new Vector2(hegsethLargePortrait.GetComponent<RectTransform>().anchoredPosition.x + 100f, hegsethLargePortrait.GetComponent<RectTransform>().anchoredPosition.y), 0.5f));
            yield return StartCoroutine(Helpers.MoveRectTransform(mainCharacterLargePortrait, mainCharacterLargePortrait.GetComponent<RectTransform>().anchoredPosition, new Vector2(mainCharacterLargePortrait.GetComponent<RectTransform>().anchoredPosition.x - 25f, mainCharacterLargePortrait.GetComponent<RectTransform>().anchoredPosition.y), 0.5f));
            yield return StartCoroutine(PlayLargeDialogue(dialogues8));
            typingCoroutine = null;

            //Move hegseth back
            yield return StartCoroutine(Helpers.MoveRectTransform(hegsethLargePortrait, hegsethLargePortrait.GetComponent<RectTransform>().anchoredPosition, new Vector2(hegsethLargePortrait.GetComponent<RectTransform>().anchoredPosition.x - 75f, hegsethLargePortrait.GetComponent<RectTransform>().anchoredPosition.y), 0.5f));
            yield return StartCoroutine(PlayLargeDialogue(dialogues9));
            typingCoroutine = null;

            //Move soldier towards astrid
            yield return StartCoroutine(Helpers.MoveRectTransform(soldierLargePortrait, soldierLargePortrait.GetComponent<RectTransform>().anchoredPosition, new Vector2(181f, soldierLargePortrait.GetComponent<RectTransform>().anchoredPosition.y), 0.25f));
            yield return StartCoroutine(PlayLargeDialogue(dialogues10));
            typingCoroutine = null;

            //Fade screen to black and play shine audio
            StartCoroutine(Helpers.FadeOutAudio(hegsethThemeAudio, 1f));
            yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1.5f));
            houseScreen.enabled = false;
            largeDialogue.SetActive(false);
            yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1.5f));
            shineAudio.Play();
            yield return new WaitForSeconds(1.5f);
            yield return StartCoroutine(PlaySmallDialogue(dialogues11));
            typingCoroutine = null;

            //Back to house and push away soldier
            yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1.5f));
            houseScreen.enabled = true;
            largeDialogue.SetActive(true);
            yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1.5f));
            knockbackAudio.Play();
            yield return StartCoroutine(Helpers.MoveRectTransform(soldierLargePortrait, soldierLargePortrait.GetComponent<RectTransform>().anchoredPosition, new Vector2(-366f, -208f), 0.25f));
            yield return StartCoroutine(PlayLargeDialogue(dialogues12));
            typingCoroutine = null;

            //Move soldier towards astrid
            yield return StartCoroutine(Helpers.MoveRectTransform(soldierLargePortrait, soldierLargePortrait.GetComponent<RectTransform>().anchoredPosition, new Vector2(181f, soldierLargePortrait.GetComponent<RectTransform>().anchoredPosition.y), 0.25f));
            knockbackAudio.Play();
            yield return StartCoroutine(Helpers.MoveRectTransform(soldierLargePortrait, soldierLargePortrait.GetComponent<RectTransform>().anchoredPosition, new Vector2(-366f, -208f), 0.25f));
            yield return StartCoroutine(PlayLargeDialogue(dialogues13));
            typingCoroutine = null;

            //Exit hegseth and soldier
            StartCoroutine(Helpers.FadeToBlackTransparent(hegsethLargePortrait, 0.5f));
            yield return StartCoroutine(Helpers.FadeToBlackTransparent(soldierLargePortrait, 0.5f));
            yield return StartCoroutine(PlayLargeDialogue(dialogues14));
            soldierLargePortrait.SetActive(false);
            hegsethLargePortrait.SetActive(false);
            typingCoroutine = null;

            //Exit house scene
            yield return StartCoroutine(Helpers.FadeInImageAlpha(whiteScreen, 0.5f));
            largeDialogue.SetActive(false);
        }

        saveManager.OverwriteSave();

        //Spawn all enemies
        CreateEnemies();

        mainCharacterObject.transform.position = new Vector3(-6.57f, -11.28f, 0f);

        //Fade out white screen
        yield return StartCoroutine(Helpers.FadeOutImageAlpha(whiteScreen, 0.5f));

        //Move characters to starting positions
        yield return StartCoroutine(Helpers.UndoFadeSpriteToBlack(mainCharacterObject));
        yield return StartCoroutine(pathfinder.FollowPath(mainCharacterObject, new Vector3(-7.5f, -13f, 0f)));
        yield return new WaitForSeconds(1f);

        astridObject.transform.position = new Vector3(-6.57f, -11.28f, 0f);
        yield return StartCoroutine(Helpers.UndoFadeSpriteToBlack(astridObject));
        yield return StartCoroutine(pathfinder.FollowPath(astridObject, new Vector3(-5.66f, -13f, 0f)));

        //Small dialogue
        yield return StartCoroutine(PlaySmallDialogue(dialogues15));
        typingCoroutine = null;

        //EnableTutorial
        battleMusicAudio.Play();
        yield return new WaitForSeconds(2f);
        battleController.StartCombat();
        yield return new WaitForSeconds(2.5f);
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
        smallDialogueTextBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";

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

                Coroutine blinking = null;
                while (isTyping || !nextLine)
                {
                    yield return new WaitForSeconds(.25f);
                    if (!isTyping && !nextLine && blinking == null)
                    {
                        blinking = StartCoroutine(DialogueBlinker(smallDialogueBlinker));
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
                smallDialogueBlinker.SetActive(false);
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
            StartCoroutine(GrayAllPortraits());

            //Light talking portrait
            StartCoroutine(HighlightPortrait(dialogues[index].characterImage));

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
                        blinking = StartCoroutine(DialogueBlinker(largeDialogueBlinker));
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
                largeDialogueBlinker.SetActive(false);
                largeDialogueTextBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
            
            }

            //Fade out text box
            StartCoroutine(Helpers.MoveRectTransform(largeDialogueTextBox, largeDialogueTextBox.GetComponent<RectTransform>().anchoredPosition, largeDialogueTextBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, -10f), .25f));
            StartCoroutine(Helpers.FadeOutCanvasGroup(largeDialogueTextBox.GetComponent<CanvasGroup>(), 0.25f));

            yield return new WaitForSeconds(0.25f);

        }
    }
    private IEnumerator DialogueBlinker(GameObject blinker)
    {
        while (true)
        {
            blinker.SetActive(true);
            yield return new WaitForSeconds(.75f);
            blinker.SetActive(false);
            yield return new WaitForSeconds(.75f);
        }
    }
    private void DisableAllPortraits()
    {
        mainCharacterSmallPortrait.SetActive(false);
        astridSmallPortrait.SetActive(false);
        soldierSmallPortrait.SetActive(false);
    }
    private IEnumerator GrayAllPortraits()
    {
        float duration = 0.2f;
        float elapsed = 0f;
        Color endColor = new Color(0.35f, 0.35f, 0.35f, 1f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            // Calculate the percentage of completion (0 to 1)
            float t = elapsed / duration;
            
            // Apply the interpolated color
            mainCharacterLargePortrait.GetComponent<Image>().color = Color.Lerp(mainCharacterLargePortrait.GetComponent<Image>().color, endColor, t);
            astridLargePortrait.GetComponent<Image>().color = Color.Lerp(astridLargePortrait.GetComponent<Image>().color, endColor, t);
            hegsethLargePortrait.GetComponent<Image>().color = Color.Lerp(hegsethLargePortrait.GetComponent<Image>().color, endColor, t);
            soldierLargePortrait.GetComponent<Image>().color = Color.Lerp(soldierLargePortrait.GetComponent<Image>().color, endColor, t);

            // Wait until the next frame
            yield return null;
        }

        // Ensure we land exactly on the target color
        mainCharacterLargePortrait.GetComponent<Image>().color = endColor;
        astridLargePortrait.GetComponent<Image>().color = endColor;
        hegsethLargePortrait.GetComponent<Image>().color = endColor;
        soldierLargePortrait.GetComponent<Image>().color = endColor;

        //Send to back
        mainCharacterLargePortrait.GetComponent<Image>().color = endColor;
        astridLargePortrait.GetComponent<Image>().color = endColor;
        hegsethLargePortrait.GetComponent<Image>().color = endColor;
        soldierLargePortrait.GetComponent<Image>().color = endColor;
    }
    private IEnumerator HighlightPortrait(GameObject character)
    {
        float duration = .2f;
        float elapsed = 0f;
        Color startColor = character.GetComponent<Image>().color;
        Color endColor = Color.white;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            // Calculate the percentage of completion (0 to 1)
            float t = elapsed / duration;
            
            // Apply the interpolated color
            character.GetComponent<Image>().color = Color.Lerp(startColor, endColor, t);

            // Wait until the next frame
            yield return null;
        }

        // Ensure we land exactly on the target color
        character.GetComponent<Image>().color = endColor;
        character.transform.SetSiblingIndex(largeDialogue.transform.childCount - 2);
    }
    private void VictorySubscribe()
    {
        VictoryContinueButton.OnStartOutro += Outro;
    }
    private void Outro()
    {
        intro = StartCoroutine(OutroHelper());
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

        yield return StartCoroutine(PlayLargeDialogue(dialogues16));
        typingCoroutine = null;

        yield return StartCoroutine(saveManager.SceneTransition(true));
        saveManager.loadedData.currentChapter = "Chapter 2";
        saveManager.loadedData.introBattleOutro = "Overworld";
        saveManager.OverwriteSave();
        SceneManager.LoadScene("Overworld");
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