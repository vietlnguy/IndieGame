using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EnterName : MonoBehaviour
{
    public TMP_InputField inputBox;
    public SaveManager scm;

    void Start()
    {
        // Ensure it has focus at the start if you want
        inputBox.ActivateInputField();
    }

    void Update()
    {
        // Check if user pressed Enter/Return
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnSubmit();
        }
    }
    public void OnSubmit()
    {
        string text = inputBox.text;
        scm.NewSave(text);
        SceneManager.LoadScene("Chapter1");
    }
}