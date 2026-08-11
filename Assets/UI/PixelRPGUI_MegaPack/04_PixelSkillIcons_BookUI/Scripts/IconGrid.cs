using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Indigolay
{
    public class IconGrid : MonoBehaviour
    {
        [SerializeField] private SkillIcon skillIconPrefab;
        [SerializeField] private List<Sprite> sprites;

        private IEnumerator Start()
        {
            yield return null;

            Sprite[] spriteArray = sprites.ToArray();

            for (int i = 0; i < spriteArray.Length; i++)
            {
                SkillIcon icon = Instantiate(skillIconPrefab, transform);
                icon.SetSprite(spriteArray[i]);
            }
        }
    }
}
