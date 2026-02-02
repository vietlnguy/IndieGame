using System.Collections.Generic;
using System.Collections;

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
    public List<Item> inventory;
    public List<Attack> knownAttacks;
    public Equipment weaponEquiped;
    public Equipment armorEquiped;
    public Equipment accessoryEquiped;

    public Character(string characterName, int maxHp, int maxMana, int attack, int intelligence, int defense, int resistance, int skill, int speed, int attackRange, int moveRange, bool owned)
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
        this.owned = owned;
        relationship = 0;
        knownAttacks = new List<Attack>();
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
public class Attack
{
    public string name;
    public string damageType;
    public int baseDamage;
    public int accuracy;
    public int baseCrit;
    public string description;
    public int manaCost;
    public bool isSupportingMove;

    public Attack(string name, string damageType, int baseDamage, int accuracy, int baseCrit, int manaCost, bool isSupportingMove, string description)
    {
        this.name = name;
        this.damageType = damageType;
        this.baseDamage = baseDamage;
        this.accuracy = accuracy;
        this.baseCrit = baseCrit;
        this.manaCost = manaCost;
        this.isSupportingMove = isSupportingMove;
        this.description = description;

        //TODO: How to handle secondary effects?

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
