using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnterName : MonoBehaviour
{
    public TMP_InputField inputBox;
    public SaveManager scm;
    public AudioSource confirmAudio;
    Coroutine runningCoroutine;

    void Start()
    {
        // Ensure it has focus at the start if you want
        inputBox.ActivateInputField();
    }

    void Update()
    {
        // Check if user pressed Enter/Return
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && runningCoroutine == null)
        {
            runningCoroutine = StartCoroutine(SubmitHelper());
        }
    }
    public void OnSubmit()
    {   
        Debug.Log("submitted");
        runningCoroutine = StartCoroutine(SubmitHelper());
    }

    private IEnumerator SubmitHelper()
    {
        Debug.Log("submit helper called");
        string text = inputBox.text;
        scm.NewSave(text);
        confirmAudio.Play();
        yield return new WaitForSeconds(2f);
        //Debug.Log("shouldve waited");
        yield return StartCoroutine(scm.SceneTransition());
        SceneManager.LoadScene("Chapter1");
    }
}