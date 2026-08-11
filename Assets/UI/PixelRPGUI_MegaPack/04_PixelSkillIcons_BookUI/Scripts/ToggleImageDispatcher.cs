using UnityEngine;
using UnityEngine.UI;

namespace Indigolay
{
    /// <summary>
    /// Dispatches different images based on Toggle's isOn state
    /// </summary>
    [RequireComponent(typeof(Toggle))]
    public class ToggleImageDispatcher : MonoBehaviour
    {
        [Header("Target Image")]
        [SerializeField] private Image targetImage;

        [Header("Sprites")]
        [SerializeField] private Sprite onSprite;
        [SerializeField] private Sprite offSprite;

        private Toggle toggle;

        private void Awake()
        {
            toggle = GetComponent<Toggle>();
        }

        private void OnEnable()
        {
            if (toggle != null)
            {
                toggle.onValueChanged.AddListener(OnToggleValueChanged);
                // Set initial state
                OnToggleValueChanged(toggle.isOn);
            }
        }

        private void OnDisable()
        {
            if (toggle != null)
            {
                toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
            }
        }

        private void OnToggleValueChanged(bool isOn)
        {
            if (targetImage == null) return;

            targetImage.sprite = isOn ? onSprite : offSprite;
        }

        /// <summary>
        /// Manually set the image based on state
        /// </summary>
        public void SetImage(bool isOn)
        {
            OnToggleValueChanged(isOn);
        }
    }
}
