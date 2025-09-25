using UnityEngine;
using TMPro;
using System.Collections;

public class DamageBlockText : MonoBehaviour
{
    TextMeshProUGUI tmpText;
    private bool flashing = false;
    void Awake()
    {
        tmpText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (!flashing)
        {
            StartCoroutine(FlashTextColor());
        }
    }


    private IEnumerator FlashTextColor()
    {
        flashing = true;
        float duration = 1.8f; // total time
        float half = duration / 2f;
        float elapsed = 0f;

        // White -> Red
        while (elapsed < half)
        {
            float t = elapsed / half;
            tmpText.color = Color.Lerp(Color.white, Color.red, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        tmpText.color = Color.red; // snap to red

        elapsed = 0f;
        // Red -> White
        while (elapsed < half)
        {
            float t = elapsed / half;
            tmpText.color = Color.Lerp(Color.red, Color.white, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        tmpText.color = Color.white; // snap back to white
        flashing = false;
    }

}
