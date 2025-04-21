using UnityEngine;
using System.Collections;

public class ArrowToggle : MonoBehaviour
{
    public float toggleInterval = 0.5f;
    public int blinkCount = 4;
    public AudioClip soundClip;       // Drag your audio file here in the Inspector
    private AudioSource audioSource;

    private SpriteRenderer sr;
    private Coroutine blinkRoutine;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void StartBlinking()
    {
        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        blinkRoutine = StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(toggleInterval);
            sr.enabled = true;
            Play();
            yield return new WaitForSeconds(toggleInterval);
        }
    }
        private void ToggleOff()
    {
            sr.enabled = false;
    }

            private void ToggleOn()
    {
            sr.enabled = true;
    }

    private void Play()
    {
        audioSource.PlayOneShot(soundClip);
    }
    
}