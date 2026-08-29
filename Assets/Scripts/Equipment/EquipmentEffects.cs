using System.Collections.Generic;
using UnityEngine;

public static class EquipmentEffects
{
    public static void DamageDealt(GameObject attacker, GameObject target, int damageDealt, bool wasCrit)
    {
        PlayerController wearer = attacker.GetComponent<PlayerController>();
        if (wearer == null)
        {
            return;
        }
        int total = 0;
        foreach (SpecialEffectSO effect in EquippedEffects(wearer))
        {
            total += effect.OnDamageDealt(wearer, target, damageDealt, wasCrit);
        }
        if (total == 0)
        {
            return;
        }
        ApplyDamage(target, total);
        Debug.Log("Equipment effect: +" + total + " bonus damage to " + target.name);
    }

    public static void TakeHit(GameObject defender, GameObject attacker, int damageTaken, bool wasCrit)
    {
        PlayerController wearer = defender.GetComponent<PlayerController>();
        if (wearer == null)
        {
            return;
        }
        int total = 0;
        foreach (SpecialEffectSO effect in EquippedEffects(wearer))
        {
            total += effect.OnTakeHit(wearer, attacker, damageTaken, wasCrit);
        }
        if (total == 0)
        {
            return;
        }
        ApplyDamage(attacker, total);
        Debug.Log("Equipment effect: " + total + " damage reflected to " + attacker.name);
    }

    private static IEnumerable<SpecialEffectSO> EquippedEffects(PlayerController wearer)
    {
        Equipment[] slots = { wearer.weaponEquiped, wearer.armorEquiped, wearer.accessoryEquiped };
        foreach (Equipment slot in slots)
        {
            if (slot == null || string.IsNullOrEmpty(slot.name))
            {
                continue;
            }
            EquipmentSO equipmentSO = EquipmentManager.Get(slot.name);
            if (equipmentSO == null || equipmentSO.specialEffects == null)
            {
                continue;
            }
            foreach (SpecialEffectSO effect in equipmentSO.specialEffects)
            {
                if (effect != null)
                {
                    yield return effect;
                }
            }
        }
    }

    private static void ApplyDamage(GameObject person, int amount)
    {
        EnemyController enemy = person.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.currentHp = enemy.currentHp - amount;
            return;
        }
        PlayerController player = person.GetComponent<PlayerController>();
        if (player != null)
        {
            player.currentHp = player.currentHp - amount;
        }
    }
}
