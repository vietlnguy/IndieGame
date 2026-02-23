using UnityEngine;
using System.Collections;
using TMPro;

public class PrologueController : MonoBehaviour
{

    public AudioSource backgroundAudio;
    public List<string> dialogues;

    void Start()
    {
        dialogues.Add("Before the age of man, there were only beings of nature.");
        dialogues.Add("Spirits, nymphs, and early mankind living in perfect harmony.");
        dialogues.Add("For a thousand years they grew, sharing knowledge and wisdom.");
        dialogues.Add("... and pleasure.");
        dialogues.Add("Eventually, these beings became indistinguishable from one another, and thus adopted a new name.. ");
        dialogues.Add("The Tah'Lo.");
        dialogues.Add("An extension of nature and technology, the Tah'Lo were masters of the weave.");
        dialogues.Add("Bestowing magic into artifacts and each other alike, the Tah'Lo enjoyed an age of prosperity.");
        dialogues.Add("But dark times loomed ahead...");
        dialogues.Add("For some, it was not enough.");
        dialogues.Add("The lure of power was all it took to topple the mighty civilization.");
        dialogues.Add("Millenia have since passed, and tales of the Tah'Lo have turned into legend.");
        dialogues.Add("Life ");
        dialogues.Add("But once again the power of the Tah'Lo threaten the stablity of the world.");
 





        StartCoroutine(Intro());
    }

    private IEnumerator Intro() {
        //Fade in Audio
        backgroundAudio.Play();
        StartCoroutine(FadeInAudio(backgroundAudio));
        yield return null;
    }

    private IEnumerator FadeInText()
    {
        yield return null;
    }
    private IEnumerator FadeInAudio(AudioSource source)
    {
        float duration = 0.25f;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(0f, 0.5f, time / duration);
            yield return null;
        }

        source.volume = 0.5f;

    }
    private IEnumerator FadeOutAudio(AudioSource source)
    {
        float startVolume = source.volume;
        float duration = 1.5f;
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