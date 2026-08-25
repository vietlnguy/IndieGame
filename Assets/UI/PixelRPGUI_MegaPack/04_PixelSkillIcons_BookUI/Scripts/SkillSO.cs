using UnityEngine;

namespace Indigolay
{
    [CreateAssetMenu(fileName = "SkillSO", menuName = "ScriptableObjects/SkillSO", order = 1)]
    public class SkillSO : ScriptableObject
    {
        public Sprite icon;
        public int starRating;
        public string skillName;
        public string description;
    }
}