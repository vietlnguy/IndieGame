using UnityEngine;

public abstract class SpecialEffectSO : ScriptableObject
{
    public virtual int OnDamageDealt(PlayerController wearer, GameObject target, int damageDealt, bool wasCrit)
    {
        return 0;
    }

    public virtual int OnTakeHit(PlayerController wearer, GameObject attacker, int damageTaken, bool wasCrit)
    {
        return 0;
    }
}
