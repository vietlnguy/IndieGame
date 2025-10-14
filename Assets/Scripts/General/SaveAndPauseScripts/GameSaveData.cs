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
    public int maxHp;
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
    public List<Item> inventory;
    public List<Attack> knownAttacks;
    public Equipment weaponEquiped;
    public Equipment armorEquiped;
    public Equipment accessoryEquiped;

    public Character(string characterName, int maxHp, int maxMana, int attack, int defense, int specialDefense, int skill, int speed, int attackRange, int moveRange, bool owned)
    {
        this.characterName = characterName;
        this.maxHp = maxHp;
        this.maxMana = maxMana;
        this.attack = attack;
        this.defense = defense;
        this.specialDefense = specialDefense;
        this.skill = skill;
        this.speed = speed;
        this.attackRange = attackRange;
        this.moveRange = moveRange;
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
    public int hpMod;
    public int manaMod;
    public int attackMod;
    public int defenseMod;
    public int skillMod;
    public int speedMod;
    public int attackRangeMod;
    public int moveRangeMod;

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
}
