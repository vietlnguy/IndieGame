using UnityEngine;

[CreateAssetMenu(fileName = "NewThornsEffect", menuName = "IndieGame/Special Effects/Thorns")]
public class ThornsEffectSO : SpecialEffectSO
{
    public int reflectDamage;

    public override int OnTakeHit(PlayerController wearer, GameObject attacker, int damageTaken, bool wasCrit)
    {
        return reflectDamage;
    }
}
