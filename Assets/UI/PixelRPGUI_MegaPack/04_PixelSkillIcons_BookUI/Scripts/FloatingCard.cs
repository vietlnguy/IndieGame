using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Indigolay
{
    public class FloatingCard : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private Image[] starImages;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        [SerializeField] private Sprite emptyStarImage;
        [SerializeField] private Sprite fullStarImage;

        public void UpdateUI(SkillSO skill)
        {
            if (skill == null) return;

            // Set icon
            if (iconImage != null)
            {
                iconImage.sprite = skill.icon;
            }

            // Set skill name
            if (nameText != null)
            {
                nameText.text = skill.skillName;
            }

            // Set description
            if (descriptionText != null)
            {
                descriptionText.text = skill.description;
            }

            // Set star rating
            if (starImages != null && starImages.Length > 0)
            {
                for (int i = 0; i < starImages.Length; i++)
                {
                    if (starImages[i] != null)
                    {
                        // Fill stars based on rating (1-3)
                        starImages[i].sprite = i < skill.starRating ? fullStarImage : emptyStarImage;
                    }
                }
            }
        }
    }
}
