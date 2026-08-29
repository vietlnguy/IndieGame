using UnityEngine;

[CreateAssetMenu(fileName = "NewFireEffect", menuName = "IndieGame/Special Effects/Fire")]
public class FireEffectSO : SpecialEffectSO
{
    public int bonusDamage;

    public override int OnDamageDealt(PlayerController wearer, GameObject target, int damageDealt, bool wasCrit)
    {
        return bonusDamage;
    }
}
