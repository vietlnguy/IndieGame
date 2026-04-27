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
        float startAlpha = startColor.a;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Lerp(startAlpha, 0f, elapsed / duration);

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

            float alpha = Mathf.Lerp(startColor.a, 1f, elapsed / duration);

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
    public static IEnumerator FadeInImageAlpha(Image image, float targetAlpha, float duration)
    {
        float elapsed = 0f;
        Color startColor = image.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Lerp(0f, targetAlpha, elapsed / duration);

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
            targetAlpha
        );
    }
    public static IEnumerator FadeToBlackTransparent(GameObject obj, float duration)
    {
        Color endColor = new Color(0f, 0f, 0f, 0f);
        float elapsedTime = 0f; 

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            obj.GetComponent<Image>().color = Color.Lerp(new Color(1f, 1f, 1f, 1f), endColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

            obj.GetComponent<Image>().color = endColor;
    }
    public static IEnumerator UndoFadeToBlackTransparent(GameObject obj, float duration)
    {
        Color endColor = new Color(0f, 0f, 0f, 0f);
        float elapsedTime = 0f; 

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            obj.GetComponent<Image>().color = Color.Lerp(endColor, new Color(1f, 1f, 1f, 1f), t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

            obj.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
    }
    public static void FlipRectTransformXScale(GameObject character)
    {
        Vector3 currentScale = character.GetComponent<RectTransform>().localScale;
        currentScale.x *= -1f;
        character.GetComponent<RectTransform>().localScale = currentScale;
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
    public static IEnumerator ScaleCameraSize(Camera cam, float targetSize, float duration)
    {
        float elapsed = 0f;
        float startSize = cam.orthographicSize;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, t);

            yield return null;
        }

        // Ensure exact final value
        cam.orthographicSize = targetSize;
    }
    public static IEnumerator ChangeImageColor(Image image, Color targetColor, float duration)
    {
        float elapsed = 0f;
        Color startColor = image.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            image.color = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }

        // Ensure exact final color
        image.color = targetColor;
    }
    public static IEnumerator FlashSprite(SpriteRenderer sr, float interval, bool remain)
    {
        for (int i = 0; i < 3; i++)
        {
            sr.enabled = true;
            yield return new WaitForSeconds(interval);

            sr.enabled = false;
            yield return new WaitForSeconds(interval);
        }
        if (remain)
        {
            sr.enabled = true;
        }
        else
        {
            sr.enabled = false;
        }
    }
    public static IEnumerator EnterCharacter(SpriteRenderer sr, float duration)
    {
        sr.enabled = true;
        Color startColor = Color.black;
        Color endColor = Color.white;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            sr.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        sr.color = endColor;
        yield return new WaitForSeconds(1f);
    }
    public static void EnableCharacterHoverAndClick()
    {
        GameObject characters = GameObject.Find("Characters");
        foreach (Transform child in characters.transform)
        {
            child.GetComponent<PlayerController>().hoverable = true;
        }
    }
    public static void EnableEnemyHoverAndClick()
    {
        GameObject enemies = GameObject.Find("Enemies");
        foreach (Transform child in enemies.transform)
        {
            child.GetComponent<EnemyController>().hoverable = true;
        }
    }
    public static void DisableCharacterHoverAndClick()
    {
        GameObject characters = GameObject.Find("Characters");
        foreach (Transform child in characters.transform)
        {
            child.GetComponent<PlayerController>().hoverable = false;
        }
    }
    public static void DisableEnemyHoverAndClick()
    {
        GameObject enemies = GameObject.Find("Enemies");
        foreach (Transform child in enemies.transform)
        {
            child.GetComponent<EnemyController>().hoverable = false;
        }
    }
    public static IEnumerator FadeSpriteToBlack(GameObject character) {
        SpriteRenderer spriteRenderer = character.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        float duration = .15f;
        Color startColor = Color.white;
        Color endColor = Color.black;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            spriteRenderer.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        spriteRenderer.color = endColor;
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(1f);
    }
    public static IEnumerator UndoFadeSpriteToBlack(GameObject character) {
        SpriteRenderer spriteRenderer = character.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        float duration = .15f;
        Color startColor = Color.black;
        Color endColor = Color.white;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            spriteRenderer.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        spriteRenderer.color = endColor;
        spriteRenderer.enabled = true;
        yield return new WaitForSeconds(1f);
    }
    public static IEnumerator GrayAllLargePortraits()
    {
        GameObject allPortraits = GameObject.Find("LargePortraits");

        float duration = 0.2f;
        float elapsed = 0f;
        Color endColor = new Color(0.35f, 0.35f, 0.35f, 1f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            // Calculate the percentage of completion (0 to 1)
            float t = elapsed / duration;

            foreach (Transform childPortrait in allPortraits.transform)
            {
                childPortrait.gameObject.GetComponent<Image>().color = Color.Lerp(childPortrait.gameObject.GetComponent<Image>().color, endColor, t);
            }

            // Wait until the next frame
            yield return null;
        }

        foreach (Transform childPortrait in allPortraits.transform)
        {
            childPortrait.gameObject.GetComponent<Image>().color = endColor;
        }

    }
    public static IEnumerator HighlightLargePortrait(string characterName)
    {
        GameObject largePortraits = GameObject.Find("LargePortraits");
        GameObject largePortrait = GameObject.Find(characterName + "LargePortrait");
        if (largePortrait == null)
        {
            largePortrait = GameObject.Find("MainCharacterLargePortrait");
        }

        float duration = .2f;
        float elapsed = 0f;
        Color startColor = largePortrait.GetComponent<Image>().color;
        Color endColor = Color.white;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            // Calculate the percentage of completion (0 to 1)
            float t = elapsed / duration;
            
            // Apply the interpolated color
            largePortrait.GetComponent<Image>().color = Color.Lerp(startColor, endColor, t);

            // Wait until the next frame
            yield return null;
        }

        // Ensure we land exactly on the target color
        largePortrait.GetComponent<Image>().color = endColor;
        largePortrait.transform.SetAsLastSibling();
    }
    public static void DisableAllSmallPortraits()
    {
        GameObject allSmallPortraits = GameObject.Find("SmallPortraits");
        foreach (Transform childPortrait in allSmallPortraits.transform)
        {
            childPortrait.gameObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        }
    }
    public static IEnumerator DialogueBlinker(string size)
    {
        GameObject blinker = null;
        
        if (size == "small")
        {
            blinker = GameObject.Find("SmallDialogueBlinker");
        }
        else
        {
            blinker = GameObject.Find("LargeDialogueBlinker");
        }

        while (true)
        {
            blinker.GetComponent<Image>().color = new Color(1f ,1f ,1f ,1f);
            yield return new WaitForSeconds(.75f);
            blinker.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            yield return new WaitForSeconds(.75f);
        }
    }
    public static void DisableBlinker(string size)
    {
        GameObject blinker = null;
        
        if (size == "small")
        {
            blinker = GameObject.Find("SmallDialogueBlinker");
            blinker.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        }
        else
        {
            blinker = GameObject.Find("LargeDialogueBlinker");
            blinker.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        }

    }

}