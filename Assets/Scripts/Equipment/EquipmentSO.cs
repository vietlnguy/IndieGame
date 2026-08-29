using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType { Weapon, Armor, Accessory }

[CreateAssetMenu(fileName = "NewEquipment", menuName = "IndieGame/Equipment")]
public class EquipmentSO : ScriptableObject
{
    public EquipmentType type;
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
    [TextArea] public string description;
    public List<SpecialEffectSO> specialEffects;

    public Equipment ToEquipment()
    {
        return new Equipment(name, TypeString(), hpMod, manaMod, attackMod, intelligenceMod, defenseMod, resistanceMod, skillMod, speedMod, hpMult, manaMult, attackMult, intelligenceMult, defenseMult, resistanceMult, skillMult, speedMult, attackRangeMult, moveRangeMult, description);
    }

    public string TypeString()
    {
        switch (type)
        {
            case EquipmentType.Weapon: return "weapon";
            case EquipmentType.Armor: return "armor";
            default: return "accessory";
        }
    }
}
