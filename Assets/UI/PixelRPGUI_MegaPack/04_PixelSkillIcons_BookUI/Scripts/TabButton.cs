using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Indigolay
{
    /// <summary>
    /// Individual tab button that works with TabButtonGroup
    /// </summary>
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Toggle))]
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("References")]
        [SerializeField] private TabButtonGroup tabGroup;

        [Header("Sprites")]
        [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite hoverSprite;
        [SerializeField] private Sprite selectedSprite;

        private Image image;
        private Toggle toggle;

        public TabButtonGroup TabGroup
        {
            get => tabGroup;
            set => tabGroup = value;
        }

        private void Awake()
        {
            image = GetComponent<Image>();
            toggle = GetComponent<Toggle>();

            if (toggle != null)
            {
                toggle.onValueChanged.AddListener(OnToggleValueChanged);
            }
        }

        private void Start()
        {
            if (image != null && normalSprite != null)
            {
                image.sprite = normalSprite;
            }
        }

        private void OnDestroy()
        {
            if (toggle != null)
            {
                toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
            }
        }

        private void OnToggleValueChanged(bool isOn)
        {
            if (isOn && tabGroup != null)
            {
                tabGroup.OnTabSelected(this);
            }
        }

        /// <summary>
        /// Update visual state for selected
        /// </summary>
        public void SetSelected(bool selected)
        {
            if (image == null) return;

            if (toggle != null)
            {
                toggle.SetIsOnWithoutNotify(selected);
            }

            if (selected)
            {
                image.sprite = selectedSprite;
            }
            else
            {
                image.sprite = normalSprite;
            }
        }

        /// <summary>
        /// Update visual state for hover
        /// </summary>
        public void SetHover(bool hover)
        {
            if (image == null) return;
            if (tabGroup != null && tabGroup.IsSelected(this)) return;

            if (hover)
            {
                image.sprite = hoverSprite;
            }
            else
            {
                image.sprite = normalSprite;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (tabGroup != null)
            {
                tabGroup.OnTabEnter(this);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (tabGroup != null)
            {
                tabGroup.OnTabExit(this);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (tabGroup != null)
            {
                tabGroup.OnTabSelected(this);
            }
        }
    }
}
