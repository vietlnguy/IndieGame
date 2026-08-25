using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Indigolay
{
    public class SkillIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerClickHandler
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private Image disableImage;

        [SerializeField] private GameObject selectObject;

        [SerializeField] private UnityEvent OnClickEvent;

        public string IconSpriteName => iconImage.sprite.name;


        private void OnEnable()
        {
            GameEventManager.AddListener<bool>(GameEvent.OnSkillIconToggleChanged, OnSkillIconToggleChanged);
        }

        private void OnDisable()
        {
            GameEventManager.RemoveListener<bool>(GameEvent.OnSkillIconToggleChanged, OnSkillIconToggleChanged);
        }

        public void SetSprite(Sprite sprite)
        {
            iconImage.sprite = sprite;
        }

        public void SetDisableSprite(Sprite sprite)
        {
            disableImage.sprite = sprite;
        }

        public void ShowEnable(bool show)
        {
            iconImage.gameObject.SetActive(show);
            disableImage.gameObject.SetActive(!show);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            selectObject.SetActive(true);
            GameEventManager.TriggerEvent<SkillIcon>(GameEvent.OnSkillIconChanged, this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            selectObject.SetActive(false);
            GameEventManager.TriggerEvent(GameEvent.OnSkillClosed);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickEvent?.Invoke();
        }

        private void OnSkillIconToggleChanged(bool isOn)
        {
            ShowEnable(isOn);
        }
    }
} 
