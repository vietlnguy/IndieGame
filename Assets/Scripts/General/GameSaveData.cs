using System.Collections.Generic;

[System.Serializable]
public class GameSaveData
{
    public string currentScene;
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
    public int skill;
    public int speed;
    public int attackRange;
    public int moveRange;
    public int relationship;

    public Equipment weaponEquiped;
    public Equipment armorEquiped;
    public Equipment accessoryEquiped;

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