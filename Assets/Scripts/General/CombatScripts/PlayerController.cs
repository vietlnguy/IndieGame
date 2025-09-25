using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public int offset = 0;
    public BattleController battleController;
    public int hp;
    public int maxHp;
    public int mana;
    public int maxMana;
    public int attack;
    public int defense;
    public int specialDefense;
    public int skill;
    public int speed;
    public int attackRange;
    public int moveRange;
    public int relationship;
    public bool owned;
    public string title;
    public Equipment weaponEquiped;
    public Equipment armorEquiped;
    public Equipment accessoryEquiped;
    public List<Attack> knownAttacks;
    public SaveManager saveManager;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        battleController = GameObject.Find("BattleController").GetComponent<BattleController>();
        saveManager = FindFirstObjectByType<SaveManager>();
        populateCharacterData();
    }
    void Start()
    {

    }
    void Update()
    {
    }
    void LateUpdate()
    {
        // Multiply by -100 to invert Y (lower on screen = higher order)
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100) + offset;
    }
    void OnMouseEnter()
    {
        if (battleController.introFinished && !battleController.disabledCharacters.Contains(gameObject))
        {
            spriteRenderer.color = Color.yellow;
        }
    }
    void OnMouseExit()
    {
        if (battleController.introFinished && !battleController.disabledCharacters.Contains(gameObject))
        {
            spriteRenderer.color = Color.white;     
        }

    }
    void OnMouseDown()
    {
        if (battleController.introFinished && !battleController.isPaused)
        {
            battleController.selectCharacter(gameObject);
        }

    }
    void populateCharacterData()
    {

        string temp = gameObject.name.Substring(0, gameObject.name.IndexOf("Prefab"));
        Character hero;
        if (temp == "MainCharacter")
        {
            hero = saveManager.loadedData.characters.Find(c => c.characterName == saveManager.loadedData.mainCharacterName);
        }
        else
        {
            hero = saveManager.loadedData.characters.Find(c => c.characterName == temp);
        }
        hp = hero.maxHp;
        maxHp = hero.maxHp;
        mana = hero.maxMana;
        maxMana = hero.maxMana;
        attack = hero.attack;
        defense = hero.defense;
        specialDefense = hero.specialDefense;
        skill = hero.skill;
        speed = hero.speed;
        attackRange = hero.attackRange;
        moveRange = hero.moveRange;
        relationship = hero.relationship;
        owned = hero.owned;
        title = hero.characterName;
        knownAttacks = hero.knownAttacks;
    }
}
