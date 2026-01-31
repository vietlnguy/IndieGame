using UnityEngine;
using TMPro;

public class ModifierTextColor : MonoBehaviour
{
    public bool atkModPositive = false;
    public bool intModPositive = false;
    public bool defModPositive = false;
    public bool resModPositive = false;
    public bool sklModPositive = false;
    public bool spdModPositive = false;
    public bool atkMultPositive = false;
    public bool intMultPositive = false;
    public  bool defMultPositive = false;
    public bool resMultPositive = false;
    public bool sklMultPositive = false;
    public bool spdMultPositive = false;

    private Coroutine pulseRoutine;
    private System.Collections.IEnumerator PulseLoop(TextMeshProUGUI text, string s)
    {
        Color originalColor = text.color;
        Color tempColor;

        if (s == "blue" ) { tempColor = new Color(0f, 0.5f, 1f, 1f); }
        else { tempColor = Color.red; }

        float halfDuration = 0.5f;

        while (true)
        {
            float t = 0f;

            // Original → Blue
            while (t < halfDuration)
            {
                t += Time.deltaTime;
                text.color = Color.Lerp(originalColor, tempColor, t / halfDuration);
                yield return null;
            }

            t = 0f;

            // Blue → Original
            while (t < halfDuration)
            {
                t += Time.deltaTime;
                text.color = Color.Lerp(tempColor, originalColor, t / halfDuration);
                yield return null;
            }
        }
    }
    public void Flash(TextMeshProUGUI text, string colorToFlash)
    {
        StartCoroutine(PulseLoop(text, colorToFlash));
    }
}