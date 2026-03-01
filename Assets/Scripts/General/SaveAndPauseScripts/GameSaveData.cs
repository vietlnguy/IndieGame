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
    public List<Equipment> ownedEquipment;

    public GameSaveData()
    {
        characters = new List<Character>();
        ownedEquipment = new List<Equipment>();
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
    public int baseMoveRange;
    public int relationship;
    public bool owned;
    public bool ranged;
    public List<Item> inventory;

    [SerializeReference]
    public List<AttackMoves> knownAttacks = new();
    public Equipment weaponEquiped;
    public Equipment armorEquiped;
    public Equipment accessoryEquiped;

    public Character(string characterName, int maxHp, int maxMana, int attack, int intelligence, int defense, int resistance, int skill, int speed, int attackRange, int moveRange, bool owned, bool ranged)
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
        relationship = 0;
        knownAttacks = new List<AttackMoves>();
        inventory = new List<Item>();

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

    public Attack(string name, string damageType, float attackMult, float intMult, int baseAccuracy, int baseCrit, int manaCost, string description) : base(name, manaCost, description)
    {
        this.damageType = damageType; //physical or magical. Determines whether resisted by DEF or RES
        this.attackMult = attackMult;
        this.intMult = intMult;
        this.baseAccuracy = baseAccuracy; //base accuracy of the attack
        this.baseCrit = baseCrit; //base critical chance

    }
}

[System.Serializable]
public class SupportMove : AttackMoves
{
    public string restoresHpOrMana;
    public int restorationAmount;
    public List<string> buffs;
    public List<string> cures;
    
    public SupportMove(string name, int manaCost, string restoresHpOrMana, int restorationAmount, List<string> buffs, List<string> cures, string description) : base(name, manaCost, description)
    {
        this.restoresHpOrMana = restoresHpOrMana;
        this.restorationAmount = restorationAmount;
        this.buffs = buffs;
        this.cures = cures;
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
public class Subquest
{
    public string description;
    public int questNumber;
    public bool completed;
    public bool failed;

    public Subquest(string description, int questNumber)
    {
        this.description = description;
        this.questNumber = questNumber;
        this.completed = false;
        this.failed = false;
    }
}
public class CharacterDialogue
{
    public string[] lines;
    public string title;
    public Sprite sprite;
    public bool pauseExecutionAfter;
    public float dialoguePitch;
    public bool autoPlay;

}