using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Indigolay
{
    public class MyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            StopAllCoroutines();
            StartCoroutine(RunPointerEnter());
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StopAllCoroutines();
            StartCoroutine(RunPointerExit());
        }

        public void OnPointerMove(PointerEventData eventData)
        {
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            StopAllCoroutines();
            StartCoroutine(RunPointerDown());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            StopAllCoroutines();
            StartCoroutine(RunPointerUp());
        }

        IEnumerator RunPointerExit()
        {
            Vector3 startScale = transform.localScale;
            Vector3 targetScale = Vector3.one;
            float duration = 0.2f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                float easedT = EasingFunction.EaseOutQuad(0f, 1f, t);
                transform.localScale = Vector3.Lerp(startScale, targetScale, easedT);
                yield return null;
            }

            transform.localScale = targetScale;
        }

        IEnumerator RunPointerEnter()
        {
            Vector3 startScale = transform.localScale;
            Vector3 targetScale = Vector3.one * 1.1f;
            float duration = 0.2f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                float easedT = EasingFunction.EaseOutQuad(0f, 1f, t);
                transform.localScale = Vector3.Lerp(startScale, targetScale, easedT);
                yield return null;
            }

            transform.localScale = targetScale;
        }

        IEnumerator RunPointerUp()
        {
            Vector3 startScale = transform.localScale;
            Vector3 targetScale = Vector3.one * 1.1f;
            float duration = 0.4f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                float easedT = EasingFunction.EaseOutElastic(0f, 1f, t);
                transform.localScale = Vector3.Lerp(startScale, targetScale, easedT);
                yield return null;
            }

            transform.localScale = targetScale;
        }

        IEnumerator RunPointerDown()
        {
            Vector3 startScale = transform.localScale;
            Vector3 targetScale = Vector3.one * 0.95f;
            float duration = 0.1f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                float easedT = EasingFunction.EaseOutQuad(0f, 1f, t);
                transform.localScale = Vector3.Lerp(startScale, targetScale, easedT);
                yield return null;
            }

            transform.localScale = targetScale;
        }
    }
}
