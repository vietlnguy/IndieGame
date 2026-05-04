using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public string currentChapter;
    public string introBattleOutro;
    public string mainCharacterName;
    public List<Character> characters;
    public List<Equipment> supplyEquipment;
    public List<Item> supplyItems;

    public GameSaveData()
    {
        characters = new List<Character>();
        supplyEquipment = new List<Equipment>();
        supplyItems = new List<Item>();
    }
}

[System.Serializable]
public class Character
{
    public string characterName;
    public int baseMaxHp;
    public int baseMaxMana;
    public int baseAttack;
    public int baseIntelligence;
    public int baseDefense;
    public int baseResistance;
    public int baseSkill;
    public int baseSpeed;
    public int baseAttackRange;
    public float baseMoveRange;
    public List<Subquest> subquests;
    public bool owned;
    public bool ranged;
    public bool roams;
    public bool support;
    public List<Item> inventory;

    [SerializeReference]
    public List<AttackMoves> knownAttacks = new();
    public Equipment weaponEquiped;
    public Equipment armorEquiped;
    public Equipment accessoryEquiped;


    public Character(string characterName, int maxHp, int maxMana, int attack, int intelligence, int defense, int resistance, int skill, int speed, int attackRange, float moveRange, bool owned, bool ranged)
    {
        this.characterName = characterName;
        this.baseMaxHp = maxHp;
        this.baseMaxMana = maxMana;
        this.baseAttack = attack;
        this.baseIntelligence = intelligence;
        this.baseDefense = defense;
        this.baseResistance = resistance;
        this.baseSkill = skill;
        this.baseSpeed = speed;
        this.baseAttackRange = attackRange;
        this.baseMoveRange = moveRange;
        this.ranged = ranged;
        this.owned = owned;
        knownAttacks = new List<AttackMoves>();
        inventory = new List<Item>();
        this.subquests = new List<Subquest>();

        if (!owned)
        {
            if (characterName == "Celeste")
            {
                this.support = true;
                this.roams = true;
            }
            else if (characterName == "Lucas")
            {
                this.support = false;
                this.roams = true;
            }
        }

    }

}

[System.Serializable]
public class Equipment
{
    public string name;
    public string type;
    public int hpMod;
    public int manaMod;
    public int attackMod;
    public int intelligenceMod;
    public int defenseMod;
    public int resistanceMod;
    public int skillMod;
    public int speedMod;
    public float hpMult;
    public float manaMult;
    public float attackMult;
    public float intelligenceMult;
    public float defenseMult;
    public float resistanceMult;
    public float skillMult;
    public float speedMult;
    public float attackRangeMult;
    public float moveRangeMult;
    public string description;

    public Equipment(string name, string type, int hpMod, int manaMod, int attackMod, int intelligenceMod, int defenseMod, int resistanceMod, int skillMod, int speedMod, float hpMult, float manaMult, float attackMult, float intelligenceMult, float defenseMult, float resistanceMult, float skillMult, float speedMult, float attackRangeMult, float moveRangeMult, string description)
    {
        this.name = name;
        this.type = type;

        this.hpMod = hpMod;
        this.manaMod = manaMod;
        this.attackMod = attackMod;
        this.intelligenceMod = intelligenceMod;
        this.defenseMod = defenseMod;
        this.resistanceMod = resistanceMod;
        this.skillMod = skillMod;
        this.speedMod = speedMod;

        this.hpMult = hpMult;
        this.manaMult = manaMult;
        this.attackMult = attackMult;
        this.intelligenceMult = intelligenceMult;
        this.defenseMult = defenseMult;
        this.resistanceMult = resistanceMult;
        this.skillMult = skillMult;
        this.speedMult = speedMult;
        this.attackRangeMult = attackRangeMult;
        this.moveRangeMult = moveRangeMult;
        
        this.description = description;
    }
    public Equipment(Equipment equipment)
    {
        this.name = equipment.name;
        this.type = equipment.type;

        this.hpMod = equipment.hpMod;
        this.manaMod = equipment.manaMod;
        this.attackMod = equipment.attackMod;
        this.intelligenceMod = equipment.intelligenceMod;
        this.defenseMod = equipment.defenseMod;
        this.resistanceMod = equipment.resistanceMod;
        this.skillMod = equipment.skillMod;
        this.speedMod = equipment.speedMod;

        this.hpMult = equipment.hpMult;
        this.manaMult = equipment.manaMult;
        this.attackMult = equipment.attackMult;
        this.intelligenceMult = equipment.intelligenceMult;
        this.defenseMult = equipment.defenseMult;
        this.resistanceMult = equipment.resistanceMult;
        this.skillMult = equipment.skillMult;
        this.speedMult = equipment.speedMult;
        this.attackRangeMult = equipment.attackRangeMult;
        this.moveRangeMult = equipment.moveRangeMult;
        
        this.description = equipment.description;
    }
}

[System.Serializable]
public class AttackMoves
{
    public string name;
    public int manaCost;
    public string description;

    public AttackMoves(string name, int manaCost, string description)
    {
        this.name = name;
        this.manaCost = manaCost;
        this.description = description;
    }
}

[System.Serializable]
public class Attack : AttackMoves 
{
    public string damageType;
    public float attackMult;
    public float intMult;
    public int baseAccuracy;
    public int baseCrit;
    public List<Debuff> debuffs;
    public List<Buff> buffs;

    public Attack(string name, string damageType, float attackMult, float intMult, int baseAccuracy, int baseCrit, int manaCost, string description) : base(name, manaCost, description)
    {
        this.damageType = damageType; //physical or magical. Determines whether resisted by DEF or RES
        this.attackMult = attackMult;
        this.intMult = intMult;
        this.baseAccuracy = baseAccuracy; //base accuracy of the attack
        this.baseCrit = baseCrit; //base critical chance
        this.debuffs = new List<Debuff>();
        this.buffs = new List<Buff>();
    }
    public Attack(string name, string damageType, float attackMult, float intMult, int baseAccuracy, int baseCrit, int manaCost, List<Debuff> debuffs,  string description) : base(name, manaCost, description)
    {
        this.damageType = damageType; //physical or magical. Determines whether resisted by DEF or RES
        this.attackMult = attackMult;
        this.intMult = intMult;
        this.baseAccuracy = baseAccuracy; //base accuracy of the attack
        this.baseCrit = baseCrit; //base critical chance
        this.debuffs = new List<Debuff>();
        this.buffs = new List<Buff>();
        this.debuffs = debuffs;
    }
    public Attack(string name, string damageType, float attackMult, float intMult, int baseAccuracy, int baseCrit, int manaCost, List<Buff> buffs,  string description) : base(name, manaCost, description)
    {
        this.damageType = damageType; //physical or magical. Determines whether resisted by DEF or RES
        this.attackMult = attackMult;
        this.intMult = intMult;
        this.baseAccuracy = baseAccuracy; //base accuracy of the attack
        this.baseCrit = baseCrit; //base critical chance
        this.debuffs = new List<Debuff>();
        this.buffs = new List<Buff>();
        this.buffs = buffs;
    }
}

[System.Serializable]
public class SupportMove : AttackMoves
{
    public string restoresHpOrMana;
    public int restorationAmount;
    public List<Buff> buffs;
    public List<string> cures;
    
    public SupportMove(string name, int manaCost, string restoresHpOrMana, int restorationAmount, List<Buff> buffs, List<string> cures, string description) : base(name, manaCost, description)
    {
        this.restoresHpOrMana = restoresHpOrMana;
        this.restorationAmount = restorationAmount;
        this.buffs = new List<Buff>();
        this.cures = new List<string>();
        this.buffs = buffs;
        this.cures = cures;
    }
    public SupportMove(string name, int manaCost, string restoresHpOrMana, int restorationAmount, string description) : base(name, manaCost, description)
    {
        this.restoresHpOrMana = restoresHpOrMana;
        this.restorationAmount = restorationAmount;
        this.buffs = new List<Buff>();
        this.cures = new List<string>();
    }
    public SupportMove(string name, int manaCost, string restoresHpOrMana, int restorationAmount, List<string> cures, string description) : base(name, manaCost, description)
    {
        this.restoresHpOrMana = restoresHpOrMana;
        this.restorationAmount = restorationAmount;
        this.buffs = new List<Buff>();
        this.cures = new List<string>();
        this.cures = cures;
    }
    public SupportMove(string name, int manaCost, string restoresHpOrMana, int restorationAmount, List<Buff> buffs, string description) : base(name, manaCost, description)
    {
        this.restoresHpOrMana = restoresHpOrMana;
        this.restorationAmount = restorationAmount;
        this.buffs = new List<Buff>();
        this.cures = new List<string>();
        this.buffs = buffs;

    }
}

[System.Serializable]
public class Item
{
    public string name;
    public int currentQuantity;
    public int maxQuantity;
    public string hpOrMana;
    public int restorationAmount;
    public string description;
    public bool curesBlind;
    public bool curesBleed;
    public bool curesRooted;

    //TODO: implement more status conditions?

    public Item(string name, int maxQuantity, string hpOrMana, int restorationAmount, string description, bool curesBlind, bool curesBleed, bool curesRooted)
    {
        this.name = name;
        this.maxQuantity = maxQuantity;
        this.currentQuantity = maxQuantity;
        this.hpOrMana = hpOrMana;
        this.restorationAmount = restorationAmount;
        this.description = description;
        this.curesBlind = curesBlind;
        this.curesBleed = curesBleed;
        this.curesRooted = curesRooted;

    }
    public Item(Item item)
    {
        this.name = item.name;
        this.maxQuantity = item.maxQuantity;
        this.currentQuantity = item.currentQuantity;
        this.hpOrMana = item.hpOrMana;
        this.restorationAmount = item.restorationAmount;
        this.description = item.description;
        this.curesBlind = item.curesBlind;
        this.curesBleed = item.curesBleed;
        this.curesRooted = item.curesRooted;
    }

}

[System.Serializable]
public class Subquest
{
    public string description;
    public string campDescription;
    public string sceneToLoad;
    public bool completed;
    public bool failed;
    public bool newAttackGained;
 
    public Subquest(string sceneToLoad, string description, string campDescription)
    {
        this.description = description;
        this.campDescription = campDescription;
        this.sceneToLoad = sceneToLoad;
        this.completed = false;
        this.failed = false;
        this.newAttackGained = false;
    }
}