using UnityEngine;

namespace Indigolay
{
    [CreateAssetMenu(fileName = "DataSO", menuName = "ScriptableObjects/DataSO", order = 1)]
    public class DataSO : ScriptableObject
    {
        public Sprite textSprite;
        public Sprite iconSprite;
        public string description;

        public Sprite[] mainIconList;
        public Sprite[] disableIconList;
    }
}
