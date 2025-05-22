using UnityEngine;
using System.Collections;

public class ArrowToggle : MonoBehaviour
{
    private float toggleInterval = 0.2f;
    private int blinkCount = 3;
    public AudioClip soundClip;       // Drag your audio file here in the Inspector
    private AudioSource audioSource;
    private SpriteRenderer sr;
    private Coroutine blinkRoutine;
    public GameObject controller;

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
        controller.GetComponent<Controller>().arrowFinished = true;
    }
    public void ToggleOff()
    {
            sr.enabled = false;
    }

    public void ToggleOn()
    {
            sr.enabled = true;
    }

    private void Play()
    {
        audioSource.PlayOneShot(soundClip);
    }
    
}