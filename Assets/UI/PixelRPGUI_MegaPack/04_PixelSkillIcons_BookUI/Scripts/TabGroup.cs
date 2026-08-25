using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Indigolay
{
    public class TabGroup : MonoBehaviour
    {
        const float animationInterval = 0.1f;

        [SerializeField] private Transform grid;
        [SerializeField] private Image textImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private DataSO dataSO;
        [SerializeField] private SkillIcon skillIconPrefab;

        private List<SkillIcon> skillIconList = new List<SkillIcon>();

        private void OnEnable()
        {
            textImage.sprite = dataSO.textSprite;
            iconImage.sprite = dataSO.iconSprite;
            descriptionText.text = dataSO.description;

            if (skillIconList.Count == 0)
            {
                for (int i = 0; i < dataSO.mainIconList.LongLength; i++)
                {
                    var skillIcon = Instantiate(skillIconPrefab, grid);
                    skillIcon.gameObject.SetActive(true);
                    skillIcon.SetSprite(dataSO.mainIconList[i]);
                    skillIcon.SetDisableSprite(dataSO.disableIconList[i]);
                    skillIconList.Add(skillIcon);
                }
            }
        }

        public void ShowEnabled(bool enabled)
        {
            for (int i = 0; i < skillIconList.Count; i++)
            {
                skillIconList[i].ShowEnable(enabled);
            }
        }
    }
}
