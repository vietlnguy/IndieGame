using UnityEngine;
using System.Collections;

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



}