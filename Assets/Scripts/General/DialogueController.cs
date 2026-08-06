using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using System.IO;

public class DialogueController : MonoBehaviour
{
    public bool active = false;   
    private bool isTyping = false;
    private bool nextLine = false;
    private int dialogueIndex = 0;
    private Coroutine typingCoroutine;
    private string lineToBeTyped = "";
    private SaveManager saveManager;
    public event Action OnDialogueFinished;
    
    //TextBox GameObjects
    public AudioSource typingAudio;
    public GameObject textBox;
    public TextMeshProUGUI textBoxText;
    public GameObject nameBox;
    public TextMeshProUGUI nameBoxText;

    //Character Portraits
    public GameObject allLargePortraits;
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
        if (active)
        {
            //Intro typing control
            if (Input.GetMouseButtonDown(0))
            {
                if (isTyping)
                {
                    StopCoroutine(typingCoroutine);
                    typingCoroutine = null;
                    textBoxText.text = lineToBeTyped;
                    typingAudio.Stop();
                    isTyping = false;
                }
                else
                {
                    nextLine = true;
                }
            }
        }
        
    }
    public void PlayNextDialogue()
    {
        active = true;
        StartCoroutine(PlayDialogue());
    }
    public void HideLargePortraits()
    {
        allLargePortraits.SetActive(false);
    }
    public void ShowLargePortraits()
    {
        allLargePortraits.SetActive(true);
    }
    private IEnumerator PlayDialogue()
    {
        for (int index = dialogueIndex; index < allDialogues.dialogueEntries.Count; index++)
        {
            //Update name text
            if (allDialogues.dialogueEntries[index].name == "MainCharacter")
            {
                nameBoxText.text = saveManager.loadedData.mainCharacterName;
            }
            else
            {
                nameBoxText.text = allDialogues.dialogueEntries[index].name;
            }

            //Grayout all large portraits
            StartCoroutine(Helpers.GrayAllLargePortraits());

            //Light talking portrait
            StartCoroutine(Helpers.HighlightLargePortrait(allDialogues.dialogueEntries[index].name));

            //Fade in text box
            StartCoroutine(Helpers.MoveRectTransform(textBox, textBox.GetComponent<RectTransform>().anchoredPosition, textBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 10f), .25f));
            StartCoroutine(Helpers.FadeInCanvasGroup(textBox.GetComponent<CanvasGroup>(), 0.25f));

            yield return new WaitForSeconds(.25f);
            
            //Type each line
            for (int index2 = 0; index2 < allDialogues.dialogueEntries[index].lines.Count; index2++)
            {
                nextLine = false;
                typingCoroutine = StartCoroutine(TypeLine(allDialogues.dialogueEntries[index].lines[index2], allDialogues.dialogueEntries[index].name));
                lineToBeTyped = allDialogues.dialogueEntries[index].lines[index2];

                Coroutine blinking = null;
                while (isTyping || !nextLine)
                {
                    yield return new WaitForSeconds(.25f);
                    if (!isTyping && !nextLine && blinking == null)
                    {
                        blinking = StartCoroutine(Helpers.DialogueBlinker("large"));
                    }

                }
                try
                {
                    StopCoroutine(blinking);
                }
                catch
                {
                    
                }
                blinking = null;
                Helpers.DisableBlinker("large");
                textBoxText.text = "";
            
            }

            //Fade out text box
            StartCoroutine(Helpers.MoveRectTransform(textBox, textBox.GetComponent<RectTransform>().anchoredPosition, textBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, -10f), .25f));
            StartCoroutine(Helpers.FadeOutCanvasGroup(textBox.GetComponent<CanvasGroup>(), 0.25f));

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
            textBoxText.color = new Color(1f, .75f, .79f, 1f);
        }
        else
        {
            typingAudio.pitch = 1.0f;
            textBoxText.color = Color.white;
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






}