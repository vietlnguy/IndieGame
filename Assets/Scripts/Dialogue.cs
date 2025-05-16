using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Important: use TMPro, not UnityEngine.UI

public class DialogueSystem : MonoBehaviour
{
    
    public TextMeshProUGUI textComponent;
    private string[] lines;
    public float textSpeed;
    private int index = 0;
    public Image targetImage;
    public Sprite mainCharacterSprite;
    public Sprite penelopeSprite;
    private bool startedDialogue = false;
    public TextMeshProUGUI title;
    public GameObject canvas;
    private RectTransform rectTransform;
    private CanvasGroup group;
    private float time = .15f;
    public GameObject controller;
    private Controller controllerScript;
    void Start()
    {
        rectTransform = canvas.GetComponent<RectTransform>();
        group = canvas.GetComponent<CanvasGroup>();
        controllerScript = controller.GetComponent<Controller>();
        
        lines = new string[] {
            "Phew we made it... I thought we were done for.",
            "What a beautiful oasis! Nice to finally get out of that wretched castle..",
            "We should resupply while we have the chance.",
            "Wait a minute, something is going on over there..."
        };
        textComponent.text = string.Empty;
    }
    void Update()
    {
        if (startedDialogue) {
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
    }
    public void StartDialogue() {
        StartCoroutine(FadeBoxInAndStartDialogue(mainCharacterSprite, "Liam"));
    }
    void NextLine() {
        if (index < lines.Length - 1) {
            index++;
            textComponent.text = string.Empty;
            if (index == 1) {
                startedDialogue = false;
                StartCoroutine(FadeBoxInAndStartDialogue(penelopeSprite, "Penelope"));
            }
            else if (index == 2) {
                startedDialogue = false;
                StartCoroutine(FadeBoxInAndStartDialogue(mainCharacterSprite, "Liam"));
            }
            else {
                StartCoroutine(TypeLine());
            }

        }
        else {
            Debug.Log("exit dialogue");
            StartCoroutine(FadeOutAndMove());
            startedDialogue = false;
            controllerScript.dialogueFinished = true;
        }
        
    }
    IEnumerator TypeLine() {
        foreach (char c in lines[index].ToCharArray()) {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    IEnumerator FadeBoxInAndStartDialogue(Sprite sprite, string name) {
        if (index == 1 || index == 2) {
            yield return StartCoroutine(FadeOutAndMove());
        }
        targetImage.sprite = sprite;
        title.text = name;
        Vector2 targetPos = new Vector2(0f, 0f);
        Vector2 startPos = new Vector2(0f, -10f); // Off-screen bottom
        rectTransform.anchoredPosition = startPos;
        group.alpha = 0f;

        float elapsed = 0f;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / time);
            group.alpha = t;
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // Ensure final values are set
        group.alpha = 1f;
        rectTransform.anchoredPosition = targetPos;
        startedDialogue = true;
        StartCoroutine(TypeLine());

    }
    IEnumerator FadeOutAndMove() {
        Vector2 targetPos = new Vector2(0f, -10f);
        Vector2 startPos = new Vector2(0f, 0f);

        float elapsed = 0f;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / time);
            group.alpha = group.alpha = Mathf.Lerp(1, 0f, elapsed / time);
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // Ensure final values are set
        group.alpha = 0f;
        rectTransform.anchoredPosition = targetPos;
        yield return new WaitForSeconds(.25f);
    }


}
