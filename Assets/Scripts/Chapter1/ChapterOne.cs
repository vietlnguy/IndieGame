using UnityEngine;

public class ChapterOne : MonoBehaviour {

    public GameObject basicEnemyPrefab;
    public GameObject enemies;
    public BattleController battleController;
    private bool enemiesSpawned = false;
    private bool shouldLose = false;
    private bool gameOver = false;

    public void Awake()
    {
        enemies = GameObject.Find("Enemies");

    }
    public void Update()
    {
        //Can script reinforcements, mid combat dialogues, etc.
        //Battle controller should be abstract enough to apply to all chapters
        //Chapter specific script events happen here, and win/lose conditions
        
        //Win condition
        if (battleController.enemies.transform.childCount == 0 && enemiesSpawned)
        {
            //Start outro scene
            Debug.Log("Win");
            enemiesSpawned = false; //remove later
        }
        
        //Lose condition
        if (shouldLose && !gameOver)
        {
           Debug.Log("Game Over");
           gameOver = true;
        }

        //Subquests?
    }
    public void CreateEnemies()
    {
        BasicEnemy(-21f, -11.25f, 0f);
        BasicEnemy(-9f, -14.24f, 0f);
        BasicEnemy(7.7f, -3.84f, 0f);
        BossEnemy(10.5f, -6.36f, 0f);
        enemiesSpawned = true;
        CharacterDeathSubscribe();

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

        enemy.knownAttacks.Add(new Attack("Bash", "physical", 1.0f, 1.0f, 90, 0, 0, "Bash the enemy with your weapon."));
    }
    public void BossEnemy(float x, float y, float z)
    {
        GameObject temp = Instantiate(basicEnemyPrefab, new Vector3(x, y, z), Quaternion.identity, enemies.transform);
        EnemyController enemy = temp.GetComponent<EnemyController>();
        enemy.title = "Soldier";

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
        if (name == "Astrid")
        {
            shouldLose = true;
        }
    }
}