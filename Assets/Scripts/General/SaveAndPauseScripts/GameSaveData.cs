using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class GameSaveData
{
    public string currentChapter;
    public string introBattleOutro;
    public string mainCharacterName;
    public List<CharacterData> characters;
    public List<Equipment> ownedEquipment;

    public GameSaveData()
    {
        characters = new List<CharacterData>();
        ownedEquipment = new List<Equipment>();
    }
}

[System.Serializable]
public class CharacterData
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
    public List<Attack> knownAttacks;
    public Equipment weaponEquiped;
    public Equipment armorEquiped;
    public Equipment accessoryEquiped;

    public CharacterData(string characterName, int maxHp, int attack, int defense, int specialDefense, int skill, int speed, int attackRange, int moveRange)
    {
        this.characterName = characterName;
        this.maxHp = maxHp;
        this.attack = attack;
        this.defense = defense;
        this.specialDefense = specialDefense;
        this.skill = skill;
        this.speed = speed;
        this.attackRange = attackRange;
        this.moveRange = moveRange;
        relationship = 0;
        knownAttacks = new List<Attack>();

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

    public Attack(string name, string damageType, int baseDamage, int accuracy)
    {
        this.name = name;
        this.damageType = damageType;
        this.baseDamage = baseDamage;
        this.accuracy = accuracy;
    }
}