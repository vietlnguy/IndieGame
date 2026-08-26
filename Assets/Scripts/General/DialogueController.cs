using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using System.IO;
using UnityEngine.UI;
using Febucci.TextAnimatorForUnity;

public class DialogueController : MonoBehaviour
{
    public bool active = false;   
    private bool isTyping = false;
    private bool nextLine = false;
    private bool isTypingComplete;
    private int dialogueIndex = 0;
    private Coroutine typingCoroutine;
    private string lineToBeTyped = "";
    private SaveManager saveManager;
    public event Action OnDialogueFinished;
    
    //TextBox GameObjects
    public AudioSource typingAudio;
    public GameObject dialoguePanel;
    public TextMeshProUGUI textBoxText;
    [SerializeField] TypewriterComponent typewriter;
    public GameObject nameBox;
    public TextMeshProUGUI nameBoxText;

    //Character Portraits
    public GameObject allLargePortraits;
    public GameObject allSmallPortraits;
    public GameObject mainCharacterLargePortrait;
    public GameObject astridLargePortrait;
    public GameObject hegsethLargePortrait;
    public GameObject soldierLargePortrait;

    //Lines
    public DialogueWrapper allDialogues;

    public void Start()
    {
        saveManager = FindAnyObjectByType<SaveManager>();

        string chapter = saveManager.loadedData.currentChapter;
        string language = PlayerPrefs.GetString("language", "english").ToLower();
        string filePath = Path.Combine(Application.streamingAssetsPath, chapter, $"{language}.json");

        string jsonString = File.ReadAllText(filePath);

        allDialogues = JsonUtility.FromJson<DialogueWrapper>(jsonString);

    }
    public void Update()
    {
        
    }
    public void PlayNextDialogue(bool useLargePortraits)
    {
        active = true;
        if (useLargePortraits)
        {
            allLargePortraits.SetActive(true);
            allSmallPortraits.SetActive(false);
            SetDialogueBoxPosition(true);
        }
        else
        {
            allLargePortraits.SetActive(false);
            allSmallPortraits.SetActive(true);
            SetDialogueBoxPosition(false);
        }
        StartCoroutine(PlayDialogue(useLargePortraits));
    }
    public void HideLargePortraits()
    {
        allLargePortraits.SetActive(false);
    }
    public void ShowLargePortraits()
    {
        allLargePortraits.SetActive(true);
    }
    private IEnumerator PlayDialogue(bool useLargePortraits)
    {
        for (int index = dialogueIndex; index < allDialogues.dialogueEntries.Count; index++)
        {
            textBoxText.text = "";
            
            //Update name text
            if (allDialogues.dialogueEntries[index].name == "MainCharacter")
            {
                nameBoxText.text = saveManager.loadedData.mainCharacterName;
            }
            else
            {
                nameBoxText.text = allDialogues.dialogueEntries[index].name;
            }

            if (useLargePortraits)
            {
                //Grayout all large portraits
                StartCoroutine(Helpers.GrayAllLargePortraits());

                //Light talking portrait
                StartCoroutine(Helpers.HighlightLargePortrait(allDialogues.dialogueEntries[index].name));
            }
            else
            {
                DisableAllSmallPortraits();
                EnableSmallPortrait(allDialogues.dialogueEntries[index].name);
            }

            //Fade in text box
            StartCoroutine(Helpers.MoveRectTransform(dialoguePanel, dialoguePanel.GetComponent<RectTransform>().anchoredPosition, dialoguePanel.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 10f), .25f));
            StartCoroutine(Helpers.FadeInCanvasGroup(dialoguePanel.GetComponent<CanvasGroup>(), 0.25f));

            yield return new WaitForSeconds(.25f);
            
            for (int index2 = 0; index2 < allDialogues.dialogueEntries[index].lines.Count; index2++)
            {
                isTypingComplete = false;

                typewriter.ShowText(allDialogues.dialogueEntries[index].lines[index2].line);

                // 1. Wait until text finishes typing NATURALLY OR the user CLICKS
                yield return new WaitUntil(() => isTypingComplete || Input.GetMouseButtonDown(0));

                // 2. If the user clicked while still typing, instantly show full text
                if (!isTypingComplete)
                {
                    typewriter.SkipTypewriter();
                    yield return null; // Wait 1 frame so the skip click isn't registered for the next line
                }

                // 3. Now wait for the NEXT click to advance to the next line
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                yield return null; // Wait 1 frame before starting the next loop iteration
            }


            //Fade out text box
            StartCoroutine(Helpers.MoveRectTransform(dialoguePanel, dialoguePanel.GetComponent<RectTransform>().anchoredPosition, dialoguePanel.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, -10f), .25f));
            StartCoroutine(Helpers.FadeOutCanvasGroup(dialoguePanel.GetComponent<CanvasGroup>(), 0.25f));

            if (!useLargePortraits)
            {
                foreach (Transform child in allSmallPortraits.transform)
                {
                    if (child.gameObject.name == allDialogues.dialogueEntries[index].name)
                    {
                        //Fade out image
                        StartCoroutine(Helpers.MoveRectTransform(child.gameObject, child.gameObject.GetComponent<RectTransform>().anchoredPosition, child.gameObject.GetComponent<RectTransform>().anchoredPosition + new Vector2(-10f, 0f), .25f));
                        StartCoroutine(Helpers.FadeOutImageAlpha(child.gameObject.GetComponent<Image>(), 0.25f));
                        break;
                    }
                } 
            }

            yield return new WaitForSeconds(0.25f);

            if (allDialogues.dialogueEntries[index].pauseExecution)
            {
                dialogueIndex = index + 1;
                
                // Notify subscribers that dialogue is complete
                OnDialogueFinished?.Invoke();
                active = false;
                break;
            }
        }

        active = false;
    }
    private IEnumerator TypeLine(string line, string speaker) {
        float textSpeed = .05f;
        
        if (speaker == "Astrid")
        {
            typingAudio.pitch = 1.2f;
        }
        else
        {
            typingAudio.pitch = 1.0f;
        }
        isTyping = true;
        typingAudio.Play();
        foreach (char c in line.ToCharArray()) {
            textBoxText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        typingAudio.Stop();
        isTyping = false;
    }
    private void SetDialogueBoxPosition(bool useLargePortraits)
    {
        if (useLargePortraits)
        {
            dialoguePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-72f, 0f);
        }
        else
        {
            dialoguePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        }
    }
    private void DisableAllSmallPortraits()
    {
        foreach (Transform child in allSmallPortraits.transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    private void EnableSmallPortrait(string name)
    {
        foreach (Transform child in allSmallPortraits.transform)
        {
            if (child.gameObject.name == name)
            {
                child.gameObject.SetActive(true);
                child.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

                //Fade in image
                StartCoroutine(Helpers.MoveRectTransform(child.gameObject, child.gameObject.GetComponent<RectTransform>().anchoredPosition, child.gameObject.GetComponent<RectTransform>().anchoredPosition + new Vector2(10f, 0f), .25f));
                StartCoroutine(Helpers.FadeInImageAlpha(child.gameObject.GetComponent<Image>(), 0.25f));

                break;
            }
        }
    }
    private void OnEnable()
    {
        typewriter.onTextShowed.AddListener(OnTypingFinished);
    }
    private void OnDisable()
    {
        typewriter.onTextShowed.RemoveListener(OnTypingFinished);
    }
    private void OnTypingFinished()
    {
        isTypingComplete = true;
    }
    public void SelectCampDialogue(string character) {

    }


}