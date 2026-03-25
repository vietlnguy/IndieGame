using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class Chapter2 : MonoBehaviour
{
    public GameObject camera;
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
    public GameObject victorySequence;
    public GameObject mainCharacterPrefab;
    public GameObject astridPrefab;
    private GameObject characters;
    private TilemapPathfinder pathfinder;
    private Coroutine intro;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool nextLine = false;
    private string lineToBeTyped = "";
    public AudioSource typingAudio;
    public GameObject smallDialogueTextBox;
    public TextMeshProUGUI smallDialogueNameBox;
    public GameObject mainCharacterImage;
    public GameObject astridImage;
    public GameObject lucasImage;
    public GameObject celesteImage;
    public GameObject soldierImage;
    public List<CharacterDialogue> dialogues;
    public List<CharacterDialogue> dialogues2;

    public void Awake()
    {    
        saveManager = FindFirstObjectByType<SaveManager>();
        characters = GameObject.Find("Characters");
        enemies = GameObject.Find("Enemies");
        pathfinder = FindFirstObjectByType<TilemapPathfinder>();

        dialogues = new List<CharacterDialogue>();
        dialogues.Add(new CharacterDialogue(mainCharacterImage, saveManager.loadedData.mainCharacterName, new string[] {"It's been a long time since we've been back to Maplemire.", "We should try to load up on supplies, while we can.", "It's not much further til we get to the castle."}));
        dialogues.Add(new CharacterDialogue(astridImage, "Astrid", new string[] {"That sounds like a good idea.", "Doesn't town seem awfully quiet, though? Where is everybody?"}));
        dialogues.Add(new CharacterDialogue(mainCharacterImage, saveManager.loadedData.mainCharacterName, new string[] {"You're right, something's off--", "Wait a second.", "Something is going on at the church over there."}));

        dialogues2 = new List<CharacterDialogue>();
        dialogues2.Add(new CharacterDialogue(lucasImage, "Lucas", new string[] {"Back off, chump!", "We don't have any of those stinkin' relics!"}));
        dialogues2.Add(new CharacterDialogue(celesteImage, "Celeste", new string[]{"Oh dear. Please forgive my brother, sir. But he speaks true.", "Our goddess, Ilvera, forbids us to lie. We do not possess any relics."}));
        dialogues2.Add(new CharacterDialogue(soldierImage, "Soldier", new string[] {"I understand that priestess, but I have orders. "}));
        
        
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
        }

    }
    public void Start()
    {
        intro = StartCoroutine(Intro());
    }
    public void Update()
    {
        //Can script reinforcements, mid combat dialogues, etc.
        //Battle controller should be abstract enough to apply to all chapters
        //Chapter specific script events happen here, and win/lose conditions
        
        //Intro control
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
            victorySequence.SetActive(true);
            StartCoroutine(victorySequence.GetComponent<VictorySequence>().Victory());
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
        enemy.attackRange = 1;
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
            //Quest failure
            subquestsBoxScript.updateQuest("Astrid1", false, list[1]);
        }
        else if (list[0].GetComponent<EnemyController>().boss && list[1].GetComponent<PlayerController>().title == "Astrid")
        {
            //Quest success
            subquestsBoxScript.updateQuest("Astrid1", true, list[1]);

        }

    }
    private IEnumerator Intro()
    {
        GameObject mainChar = GameObject.Find("MainCharacterPrefab(Clone)");
        GameObject astrid = GameObject.Find("AstridPrefab(Clone)");

        //Fade Out blackwhite screen

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
        
        //blink arrow

        //small dialoue
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


        //pan camera back

        //small dialogue


        yield return null;
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