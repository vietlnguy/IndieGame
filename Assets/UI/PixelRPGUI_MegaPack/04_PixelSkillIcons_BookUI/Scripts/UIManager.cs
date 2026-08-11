using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Indigolay
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup showcaseCanvasGroup;
        [SerializeField] private FloatingCard floatingCard;
        [SerializeField] private CanvasGroup floatingCardCanvasGroup;
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform floatingCardRect;
        [SerializeField] private Toggle enableSkillIConToggle;
        [SerializeField] private Toggle enableTooltipToggle;

        [SerializeField] private float fadeDuration = 0.2f;
        [SerializeField] private float positionMoveSpeed = 0.15f; // Lerp interpolation speed (0-1)

        private Coroutine fadeCoroutine;

        private System.Collections.Generic.Dictionary<string, SkillSO> skillCache = new System.Collections.Generic.Dictionary<string, SkillSO>();

        private readonly string[] skillFolderPaths = new string[]
        {
            "ScriptableObjects/01_Warrior_Berserker",
            "ScriptableObjects/02_Mage_Sorcerer",
            "ScriptableObjects/03_Archer_Assassin",
            "ScriptableObjects/04_Priest_Paladin"
        };


        private void OnEnable()
        {
            GameEventManager.AddListener<SkillIcon>(GameEvent.OnSkillIconChanged, OnSkillIconChanged);
            GameEventManager.AddListener(GameEvent.OnSkillClosed, OnSkillClosed);

            enableSkillIConToggle.onValueChanged.AddListener(OnSkillIconToggleChaned);
            enableTooltipToggle.onValueChanged.AddListener(OnTooltipToggleChanged);
        }


        private void OnDisable()
        {
            GameEventManager.RemoveListener<SkillIcon>(GameEvent.OnSkillIconChanged, OnSkillIconChanged);
            GameEventManager.RemoveListener(GameEvent.OnSkillClosed, OnSkillClosed);

            enableSkillIConToggle.onValueChanged.RemoveListener(OnSkillIconToggleChaned);
            enableTooltipToggle.onValueChanged.RemoveListener(OnTooltipToggleChanged);
        }

        private void OnSkillIconToggleChaned(bool isOn)
        {
            GameEventManager.TriggerEvent<bool>(GameEvent.OnSkillIconToggleChanged, isOn);
        }

        private void OnTooltipToggleChanged(bool isOn)
        {
            if (!isOn)
            {
                FadeOut();
            }

            GameEventManager.TriggerEvent<bool>(GameEvent.OnTooltipToggleChanged, isOn);
        }

        private void Update()
        {
            if (showcaseCanvasGroup == null || showcaseCanvasGroup.alpha < 1f) return;

            // Update floating card position if it's active
            if (floatingCard != null && floatingCard.gameObject.activeSelf)
            {
                PositionFloatingCard();
            }
        }

        private IEnumerator Start()
        {
            showcaseCanvasGroup.gameObject.SetActive(true);
            showcaseCanvasGroup.alpha = 0;

            yield return new WaitForSeconds(2f);

            float timer = 1;
            while (timer > 0)
            {
                float alpha = Mathf.Clamp01(1 - timer);

                // Apply alpha to showcaseCanvasGroup
                showcaseCanvasGroup.alpha = alpha;

                timer -= Time.deltaTime;
                yield return null;
            }

            // Set final alpha
            showcaseCanvasGroup.alpha = 1;
        }


        private void OnSkillIconChanged(SkillIcon icon)
        {

            if (showcaseCanvasGroup == null || showcaseCanvasGroup.alpha < 1f) return;

            if (icon == null) return;

            if (!enableTooltipToggle.isOn) return;

            // Extract skill name from sprite name
            // Example: UI_SkillIcon_WR_01_RustySword -> RustySword
            string spriteName = icon.IconSpriteName;
            string skillName = ExtractSkillName(spriteName);

            // Find matching SkillSO
            SkillSO skill = FindSkillByName(skillName);
            if (skill == null)
            {
                FadeOut();
                return;
            }

            // Update floating card UI
            floatingCard.UpdateUI(skill);

            // Fade in the card
            FadeIn();
        }

        private string ExtractSkillName(string spriteName)
        {
            // Pattern: UI_SkillIcon_XX_##_SkillName
            string[] parts = spriteName.Split('_');
            if (parts.Length >= 5)
            {
                // Join everything after the 4th underscore
                return string.Join("_", parts.Skip(4));
            }
            return spriteName;
        }

        private SkillSO FindSkillByName(string skillName)
        {
            // Check cache first
            if (skillCache.TryGetValue(skillName, out SkillSO cachedSkill))
            {
                return cachedSkill;
            }

            // Try to load from each folder
            foreach (string folderPath in skillFolderPaths)
            {
                string fullPath = $"{folderPath}/{skillName}";
                SkillSO skill = Resources.Load<SkillSO>(fullPath);

                if (skill != null)
                {
                    // Cache for next time
                    skillCache[skillName] = skill;
                    return skill;
                }
            }

            return null;
        }

        private void PositionFloatingCard()
        {
            if (floatingCardRect == null || canvas == null)
            {
                Debug.LogWarning("FloatingCardRect or Canvas is not assigned!");
                return;
            }

            // Get mouse position in screen space
            Vector2 mousePos = Mouse.current.position.ReadValue();

            // Convert to canvas space
            Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                mousePos,
                cam,
                out Vector2 localPoint
            );

            // Get floating card size
            Vector2 cardSize = floatingCardRect.sizeDelta;

            // Get canvas rect
            RectTransform canvasRect = canvas.transform as RectTransform;
            Vector2 canvasSize = canvasRect.sizeDelta;

            // Determine pivot based on mouse position
            // If mouse is in bottom half, use bottom-left pivot (0, 0)
            // If mouse is in top half, use top-left pivot (0, 1)
            bool isBottomHalf = localPoint.y < 0;

            if (isBottomHalf)
            {
                // Bottom half - pivot at bottom-left, card goes up
                floatingCardRect.pivot = new Vector2(0, 0);
                float offsetX = 15f;
                float offsetY = 15f; // Offset upward

                Vector2 targetPos = localPoint + new Vector2(offsetX, offsetY);

                // Check right boundary
                if (targetPos.x + cardSize.x > canvasSize.x / 2)
                {
                    targetPos.x = localPoint.x - offsetX - cardSize.x;
                }

                // Check left boundary
                if (targetPos.x < -canvasSize.x / 2)
                {
                    targetPos.x = -canvasSize.x / 2;
                }

                // Check top boundary (card extends upward from pivot)
                if (targetPos.y + cardSize.y > canvasSize.y / 2)
                {
                    targetPos.y = canvasSize.y / 2 - cardSize.y;
                }

                // Check bottom boundary
                if (targetPos.y < -canvasSize.y / 2)
                {
                    targetPos.y = -canvasSize.y / 2;
                }

                // Smoothly move to target position
                floatingCardRect.localPosition = Vector2.Lerp(
                    floatingCardRect.localPosition,
                    targetPos,
                    positionMoveSpeed
                );
            }
            else
            {
                // Top half - pivot at top-left, card goes down
                floatingCardRect.pivot = new Vector2(0, 1);
                float offsetX = 15f;
                float offsetY = -15f; // Offset downward

                Vector2 targetPos = localPoint + new Vector2(offsetX, offsetY);

                // Check right boundary
                if (targetPos.x + cardSize.x > canvasSize.x / 2)
                {
                    targetPos.x = localPoint.x - offsetX - cardSize.x;
                }

                // Check left boundary
                if (targetPos.x < -canvasSize.x / 2)
                {
                    targetPos.x = -canvasSize.x / 2;
                }

                // Check top boundary
                if (targetPos.y > canvasSize.y / 2)
                {
                    targetPos.y = canvasSize.y / 2;
                }

                // Check bottom boundary (card extends downward from pivot)
                if (targetPos.y - cardSize.y < -canvasSize.y / 2)
                {
                    targetPos.y = -canvasSize.y / 2 + cardSize.y;
                }

                // Smoothly move to target position
                floatingCardRect.localPosition = Vector2.Lerp(
                    floatingCardRect.localPosition,
                    targetPos,
                    positionMoveSpeed
                );
            }
        }


        private void OnSkillClosed()
        {
            FadeOut();
        }

        private void FadeIn()
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            floatingCard.gameObject.SetActive(true);
            fadeCoroutine = StartCoroutine(FadeCoroutine(1f));
        }

        private void FadeOut()
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine = StartCoroutine(FadeCoroutine(0f));
        }

        private IEnumerator FadeCoroutine(float targetAlpha)
        {
            if (floatingCardCanvasGroup == null)
            {
                // Fallback if CanvasGroup is not assigned
                if (targetAlpha == 0f)
                {
                    floatingCard.gameObject.SetActive(false);
                }
                yield break;
            }

            float startAlpha = floatingCardCanvasGroup.alpha;
            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeDuration;
                floatingCardCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
                yield return null;
            }

            floatingCardCanvasGroup.alpha = targetAlpha;

            // Deactivate after fade out
            if (targetAlpha == 0f)
            {
                floatingCard.gameObject.SetActive(false);
            }
        }

    }
}
