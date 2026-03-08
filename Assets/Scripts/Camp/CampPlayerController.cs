using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
public class CampPlayerController : MonoBehaviour
{
    
    private SaveManager scm;
    public int currentHp;
    public int maxHp;
    public int currentMana;
    public int maxMana;
    public int attack;
    public int intelligence;
    public int defense;
    public int resistance;
    public int skill;
    public int speed;
    public int attackRange;
    public int moveRange;
    public int baseMaxHp;
    public int baseMaxMana;
    public int baseAttack;
    public int baseIntelligence;
    public int baseDefense;
    public int baseResistance;
    public int baseSkill;
    public int baseSpeed;
    public int baseAttackRange;
    public int baseMoveRange;
    public int totalAttackMod;
    public int totalIntelligenceMod;
    public int totalDefenseMod;
    public int totalResistanceMod;
    public int totalSkillMod;
    public int totalSpeedMod;
    public float totalAttackMult;
    public float totalIntelligenceMult;
    public float totalDefenseMult;
    public float totalResistanceMult;
    public float totalSkillMult;
    public float totalSpeedMult;
    public float totalAttackRangeMult;
    public float totalMoveRangeMult;
    public float totalHpMod;
    public float totalHpMult;
    public float totalManaMod;
    public float totalManaMult;
    public List<Subquest> subquests;
    public bool owned;
    public string title;
    public bool ranged;
    public List<string> statuses;
    public Equipment weaponEquiped;
    public Equipment armorEquiped;
    public Equipment accessoryEquiped;
    public List<AttackMoves> knownAttacks;
    public List<Item> inventory;
    public string deathDialogue;
    private CampMoveCircle campMoveCircleScript;
    private CampAssistMenu campAssistMenuScript;
    private SpriteRenderer spriteRenderer;
    void Awake()
    {
        scm = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        campMoveCircleScript = FindFirstObjectByType<CampMoveCircle>();
        campAssistMenuScript = FindFirstObjectByType<CampAssistMenu>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        populateCharacterData();
    }
    void Update()
    {
        if (campMoveCircleScript.alliesInRange.Contains(gameObject)) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int layerMask = LayerMask.GetMask("Characters"); // ignore AttackRange layer
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // Click logic
                if (Input.GetMouseButtonDown(0))
                {
                    OnClick();
                }
            }
        }
    }
    private void populateCharacterData()
    {

        string temp = gameObject.name.Substring(0, gameObject.name.IndexOf("Prefab"));
        Character savedCharacter;
        if (temp == "MainCharacter")
        {
            savedCharacter = scm.loadedData.characters.Find(c => c.characterName == scm.loadedData.mainCharacterName);
        }
        else
        {
            savedCharacter = scm.loadedData.characters.Find(c => c.characterName == temp);
        } 
        baseMaxHp = savedCharacter.baseMaxHp;
        baseMaxMana = savedCharacter.baseMaxMana;
        baseAttack = savedCharacter.baseAttack;
        baseIntelligence = savedCharacter.baseIntelligence;
        baseDefense = savedCharacter.baseDefense;
        baseResistance = savedCharacter.baseResistance;
        baseSkill = savedCharacter.baseSkill;
        baseSpeed = savedCharacter.baseSpeed;
        baseAttackRange = savedCharacter.baseAttackRange;
        baseMoveRange = savedCharacter.baseMoveRange;
        owned = savedCharacter.owned;
        ranged = savedCharacter.ranged;
        title = savedCharacter.characterName;
        knownAttacks = savedCharacter.knownAttacks;
        inventory = savedCharacter.inventory;
        weaponEquiped = savedCharacter.weaponEquiped;
        armorEquiped = savedCharacter.armorEquiped;
        accessoryEquiped = savedCharacter.accessoryEquiped;
        subquests = savedCharacter.subquests;
        
        if (savedCharacter.characterName == "Astrid")
        {
            deathDialogue = "Ah-- I suppose this is it. Stay safe everyone...";
        }
        else if (savedCharacter.characterName == "Amara")
        {
            deathDialogue = "Guh- I guess my luck finally ran out...";
        }
        else if (savedCharacter.characterName == "Celeste")
        {
            deathDialogue = "Ah- goddess... I come to you.. Protect Luc--";
        }
        else if (savedCharacter.characterName == "Gerard")
        {
            deathDialogue = "Urgh- no. I cannot fall here. I must protect the princess...";
        }
        else if (savedCharacter.characterName == "Ivy")
        {
            deathDialogue = "Ah- protect... forest.. please.";
        }
        else if (savedCharacter.characterName == "Katherine")
        {
            deathDialogue = "Ach- Not yet.. I can still fight.. I am strong..";   
        }
        else if (savedCharacter.characterName == "Lucas")
        {
            deathDialogue = "Guh- Bummer... Looks like I won't be a legend...";
        }
        else if (savedCharacter.characterName == "Penelope")
        {
            deathDialogue = "Oh- that's my blood... and I was just getting to the good part...";   
        }
        else if (savedCharacter.characterName == "Vanessa")
        {
            deathDialogue = "Ah- my time is up. I knew you'd come for me one day, Lady Death...";
        }
        else
        {
            deathDialogue = "Ack- I think this is it for me.. I'm sorry, everyone... ";
        }


        calculateStats();
    }
    public void calculateStats()
    {   
        Equipment tempWeapon;
        Equipment tempArmor;
        Equipment tempAccessory;

        //When no equipment then use default
        if (weaponEquiped == null)
        {
            tempWeapon = new Equipment("temp", "weapon", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "temp");
        }
        else
        {
            tempWeapon = weaponEquiped;
        }       
        if (armorEquiped == null)
        {
            tempArmor = new Equipment("temp", "weapon", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "temp");
        }
        else
        {
            tempArmor = armorEquiped;
        }
        if (accessoryEquiped == null)
        {            
            tempAccessory = new Equipment("temp", "weapon", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "temp");
        }
        else
        {
            tempAccessory = accessoryEquiped;
        }
        
        totalAttackMod = tempWeapon.attackMod + tempArmor.attackMod + tempAccessory.attackMod;
        totalIntelligenceMod = tempWeapon.intelligenceMod + tempArmor.intelligenceMod + tempAccessory.intelligenceMod;
        totalDefenseMod = tempWeapon.defenseMod + tempArmor.defenseMod + tempAccessory.defenseMod;
        totalResistanceMod = tempWeapon.resistanceMod + tempArmor.resistanceMod + tempAccessory.resistanceMod;
        totalSkillMod = tempWeapon.skillMod + tempArmor.skillMod + tempAccessory.skillMod;
        totalSpeedMod = tempWeapon.speedMod + tempArmor.speedMod + tempAccessory.speedMod;
        totalHpMod = tempWeapon.hpMod + tempArmor.hpMod + tempAccessory.hpMod;
        totalManaMod = tempWeapon.manaMod + tempArmor.manaMod + tempAccessory.manaMod;
        
        totalAttackMult = 1 + tempWeapon.attackMult + tempArmor.attackMult + tempAccessory.attackMult;
        totalIntelligenceMult = 1 + tempWeapon.intelligenceMult + tempArmor.intelligenceMult + tempAccessory.intelligenceMult;
        totalDefenseMult = 1 + tempWeapon.defenseMult + tempArmor.defenseMult + tempAccessory.defenseMult;
        totalResistanceMult = 1 + tempWeapon.resistanceMult + tempArmor.resistanceMult + tempAccessory.resistanceMult;
        totalSkillMult = 1 + tempWeapon.skillMult + tempArmor.skillMult + tempAccessory.skillMult;
        totalSpeedMult = 1 +tempWeapon.speedMult + tempArmor.speedMult + tempAccessory.speedMult;
        totalAttackRangeMult = 1 + tempWeapon.attackRangeMult + tempArmor.attackRangeMult + tempAccessory.attackRangeMult;
        totalMoveRangeMult = 1 + tempWeapon.moveRangeMult + tempArmor.moveRangeMult + tempAccessory.moveRangeMult;
        totalHpMult = 1 + tempWeapon.hpMult + tempArmor.hpMult + tempAccessory.hpMult;
        totalManaMult = 1 + tempWeapon.manaMult + tempArmor.manaMult + tempAccessory.manaMult;

        maxHp = Mathf.RoundToInt((float)(baseMaxHp + totalHpMod) * totalHpMult);
        currentHp = maxHp;
        maxMana = Mathf.RoundToInt((float)(baseMaxMana + totalManaMod) * totalManaMult);
        currentMana = maxMana;
        attack = Mathf.RoundToInt((float)(baseAttack + totalAttackMod) * totalAttackMult);
        intelligence = Mathf.RoundToInt((float)(baseIntelligence + totalIntelligenceMod) * totalIntelligenceMult);
        defense = Mathf.RoundToInt((float)(baseDefense + totalDefenseMod) * totalDefenseMult);
        resistance = Mathf.RoundToInt((float)(baseResistance + totalResistanceMod) * totalResistanceMult);
        skill = Mathf.RoundToInt((float)(baseSkill + totalSkillMod) * totalSkillMult);
        speed = Mathf.RoundToInt((float)(baseSpeed + totalSpeedMod) * totalSpeedMult);
        moveRange = Mathf.RoundToInt((float)baseMoveRange* totalMoveRangeMult);
        attackRange = Mathf.RoundToInt((float)baseAttackRange * totalAttackRangeMult);
    }
    public void highlightAssistable()
    {
        spriteRenderer.color = Color.green;
    }
    public void unhighlight()
    {
        spriteRenderer.color = Color.white;
    }
    private void OnClick()
    {
       campAssistMenuScript.enableCharacterAssistMenu(gameObject);
    }
}