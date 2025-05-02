using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Important: use TMPro, not UnityEngine.UI

public class DialogueSystem : MonoBehaviour
{
    
    public TextMeshProUGUI textComponent;
    private string[] lines;
    public float textSpeed;
    private int index;

    void Start()
    {
        lines = new string[] {"helllllloooooooo I love doing dialogue stuff. it really makes me wanna kill myself. What even is this how do I know that this is going to look good?"};
        textComponent.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            if (textComponent.text == lines[index])
            {
                NextLine(); 
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue() {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine() {
        foreach (char c in lines[index].ToCharArray()) {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine() {
        if (index < lines.Length - 1) {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else {
            gameObject.SetActive(false);
        }
    }

}
