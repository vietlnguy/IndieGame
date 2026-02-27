using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    public static IEnumerator FadeOutCanvasGroup(CanvasGroup group, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            group.alpha = t;
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
}