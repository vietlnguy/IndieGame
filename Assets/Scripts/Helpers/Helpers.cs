using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public static class Helpers
{
    public static IEnumerator FadeOutAudio(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0, time / duration);
            yield return null;
        }

        source.volume = 0;
        source.Stop();
    }
    public static IEnumerator FadeInAudio(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        source.Play();
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0.5f, time / duration);
            yield return null;
        }

        source.volume = 0.5f;
    }
    public static IEnumerator FadeOutCanvasGroup(CanvasGroup group, float duration)
    {
        float startAlpha = group.alpha;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            group.alpha = Mathf.Lerp(startAlpha, 0f, t);
            yield return null;
        }

        group.alpha = 0f;
    }
    public static IEnumerator FadeInCanvasGroup(CanvasGroup group, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            group.alpha = t;
            yield return null;
        }
        group.alpha = 1f;

    }
    public static IEnumerator FadeOutImageAlpha(Image image, float duration)
    {
        float elapsed = 0f;
        Color startColor = image.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);

            image.color = new Color(
                startColor.r,
                startColor.g,
                startColor.b,
                alpha
            );

            yield return null;
        }

        // Ensure fully transparent at end
        image.color = new Color(
            startColor.r,
            startColor.g,
            startColor.b,
            0f
        );
    }
    public static IEnumerator FadeInImageAlpha(Image image, float duration)
    {
        float elapsed = 0f;
        Color startColor = image.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Lerp(0f, 1f, elapsed / duration);

            image.color = new Color(
                startColor.r,
                startColor.g,
                startColor.b,
                alpha
            );

            yield return null;
        }

        // Ensure fully transparent at end
        image.color = new Color(
            startColor.r,
            startColor.g,
            startColor.b,
            1f
        );
    }
    public static IEnumerator MoveRectTransform(GameObject obj, Vector2 startPos, Vector2 targetPos, float duration)
    {
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            rectTransform.anchoredPosition = 
                Vector2.Lerp(startPos, targetPos, t);

            yield return null;
        }

        // Snap exactly to target at the end
        rectTransform.anchoredPosition = targetPos;
    }
    public static IEnumerator MoveTransform(Transform obj, Vector3 startPos, Vector3 targetPos, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            obj.position = Vector3.Lerp(startPos, targetPos, t);

            yield return null;
        }

        // Snap exactly to target at the end
        obj.position = targetPos;
    }


}