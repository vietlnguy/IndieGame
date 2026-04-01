using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CampDialogue : MonoBehaviour
{
    private SaveManager scm;
    private CampAssistMenu campAssistMenuScript;
    public Image blackScreen;
    public GameObject mainCamera;
    public GameObject eventSystem;
    public GameObject nameBox;
    public GameObject textBox;
    public GameObject selector;
    public GameObject scrollView;
    public GameObject scrollViewContent;
    public GameObject campDialoguePrefab;
    public GameObject everdellScenery;
    public GameObject astridImage;
    public GameObject mainCharacterImage;
    public GameObject characterSelected;
    private bool active = false;
    public bool nextLine = false;
    private bool isTyping = false;
    private bool dialogueOptionsActive = false;
    public bool cutSceneActive = false; 
    private bool coroutineRunning = false;   
    public List<CharacterDialogue> dialogues;
    public Coroutine typingCoroutine;
    private string lineToBeTyped = "";
    public AudioSource typingAudio;
    public AudioSource selectorAudio;
    public AudioSource backgroundAudio;
    public int dialogueIndex = 0;
    public int dialogueTopIndex = 0;
    public int dialogueBotIndex = 2;
    private string sceneName = "";
    public GameObject newAttackBox;
    public TextMeshProUGUI newAttackBoxName;
    public TextMeshProUGUI newAttackBoxText;
    public AudioSource gainedNewAttackAudio;

    void Awake()
    {
        scm = FindFirstObjectByType<SaveManager>();
        campAssistMenuScript = FindFirstObjectByType<CampAssistMenu>();
        dialogues = new List<CharacterDialogue>();
    }
    void Update()
    {
        if (!cutSceneActive)
        {
            if (active)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    if (isTyping)
                    {
                        StopCoroutine(typingCoroutine);
                        typingCoroutine = null;
                        textBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = lineToBeTyped;
                        typingAudio.Stop();
                        isTyping = false;
                    }
                    else
                    {
                        nextLine = true;
                    }
                }
            }
            
            if (dialogueOptionsActive)
            {
                //Move selector
                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (dialogueIndex == dialogueTopIndex && dialogueIndex != 0)
                    {
                        moveContentWindowUp();
                        dialogueIndex--;
                    }
                    
                    else if (dialogueIndex != 0)
                    {
                        moveSelectorUp();
                        dialogueIndex--;
                    }
                        
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    if (dialogueIndex == dialogueBotIndex && dialogueIndex < characterSelected.GetComponent<CampPlayerController>().subquests.Count)
                    {
                        moveContentWindowDown();
                        dialogueIndex++;
                    }
                    
                    else if (dialogueIndex != characterSelected.GetComponent<CampPlayerController>().subquests.Count)
                    {
                        moveSelectorDown();
                        dialogueIndex++;
                    }
                        
                
                }
            
                //Make item selection
                else if (Input.GetKeyDown(KeyCode.Space) && !coroutineRunning)
                {
                    //Close dialogue
                    if (dialogueIndex == characterSelected.GetComponent<CampPlayerController>().subquests.Count)
                    {
                        coroutineRunning = true;
                        StartCoroutine(DisableDialogueWindow());
                    }
                    else
                    {
                        if (characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].completed)
                        {
                            coroutineRunning = true;
                            StartCoroutine(PlayCutscene());
                        }

                    }
                }
        
            }     
        }
    }
    public IEnumerator EnableDialogueWindow(GameObject character)
    {
        characterSelected = character;
        DetermineDialogue(character);
        yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1f));

        CampPlayerController characterScript = character.GetComponent<CampPlayerController>();
        mainCharacterImage.SetActive(true);

        //Enable the right character image
        if (characterScript.title == "Astrid")
        {
            astridImage.SetActive(true);
        }
        else if (characterScript.title == "Amara")
        {
            
        }

        //Enable the right background
        if (scm.loadedData.currentChapter == "Chapter 2")
        {
            everdellScenery.SetActive(true);
        }

        yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));

        //Go through each dialogue
        active = true;
        for (int index = 0; index < dialogues.Count; index++)
        {
            //Update name text
            nameBox.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = dialogues[index].name;

            //Update nameBox position
            nameBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(dialogues[index].characterImage.GetComponent<RectTransform>().anchoredPosition.x, nameBox.GetComponent<RectTransform>().anchoredPosition.y);

            //Fade in text box
            StartCoroutine(Helpers.MoveRectTransform(textBox, textBox.GetComponent<RectTransform>().anchoredPosition, textBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 10f), .25f));
            StartCoroutine(Helpers.FadeInCanvasGroup(textBox.GetComponent<CanvasGroup>(), 0.25f));

            yield return new WaitForSeconds(.25f);
            //Type each line
            for (int index2 = 0; index2 < dialogues[index].lines.Length; index2++)
            {
                nextLine = false;
                typingCoroutine = StartCoroutine(TypeLine(dialogues[index].lines[index2], dialogues[index].name, typingAudio, textBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>(), .05f));
                lineToBeTyped = dialogues[index].lines[index2];

                while (isTyping || !nextLine)
                {
                    yield return new WaitForSeconds(.25f);
                }
                textBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
            
            }

            //Fade out text box
            StartCoroutine(Helpers.MoveRectTransform(textBox, textBox.GetComponent<RectTransform>().anchoredPosition, textBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, -10f), .25f));
            StartCoroutine(Helpers.FadeOutCanvasGroup(textBox.GetComponent<CanvasGroup>(), 0.25f));

            yield return new WaitForSeconds(0.25f);

        }
 
        scrollView.SetActive(true);
        nameBox.SetActive(false);
        selector.SetActive(true);

        //Instantiate dialoguePrefab options
        int index3 = 1;
        foreach (Subquest subquest in character.GetComponent<CampPlayerController>().subquests)
        {
            GameObject temp = Instantiate(campDialoguePrefab, scrollViewContent.transform, false);
            string s = index3.ToString() + ". ";
            if (subquest.completed)
            {
                s = s + "[Subquest Completed] ";
                temp.GetComponent<TextMeshProUGUI>().color = Color.white;
            }
            else if (subquest.failed)
            {
                s = s + "[Subquest Failed] ";
                temp.GetComponent<TextMeshProUGUI>().color = Color.gray;
            }
            else
            {
                s = s + "[Subquest Incomplete] ";
                temp.GetComponent<TextMeshProUGUI>().color = Color.gray;
            }

            s = s + subquest.campDescription;
            temp.GetComponent<TextMeshProUGUI>().text = s;
            index3++;
        }
        GameObject temp2 = Instantiate(campDialoguePrefab, scrollViewContent.transform, false);
        temp2.GetComponent<TextMeshProUGUI>().text = index3.ToString() + ". " + "Goodbye.";

        //Fade in text box
        StartCoroutine(Helpers.MoveRectTransform(textBox, textBox.GetComponent<RectTransform>().anchoredPosition, textBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 10f), .25f));
        StartCoroutine(Helpers.FadeInCanvasGroup(textBox.GetComponent<CanvasGroup>(), 0.25f));

        dialogueOptionsActive = true;
    }
    public IEnumerator DisableDialogueWindow()
    {
        yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1f));

        textBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
        active = false;
        dialogueOptionsActive = false;
        scrollView.SetActive(false);
        selector.SetActive(false);
        nameBox.SetActive(true);
        resetSelectorPosition();
        disableCharacterImages();
        disableBackgrounds();
        characterSelected.GetComponent<CampPlayerController>().spokenToAlready = true;
        dialogueIndex = 0;
        dialogueBotIndex = 2;
        dialogueTopIndex = 0;

        foreach (Transform obj in scrollViewContent.transform)
        {
            Destroy(obj.gameObject);
        }

        //Fade out text box
        StartCoroutine(Helpers.MoveRectTransform(textBox, textBox.GetComponent<RectTransform>().anchoredPosition, textBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, -10f), .25f));
        StartCoroutine(Helpers.FadeOutCanvasGroup(textBox.GetComponent<CanvasGroup>(), 0.25f));

        yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));
        campAssistMenuScript.active = true;
        coroutineRunning = false;
        

    }
    private void disableCharacterImages()
    {
        astridImage.SetActive(false);
        mainCharacterImage.SetActive(false);
    }
    private void disableBackgrounds()
    {
        everdellScenery.SetActive(false);
    }
    private void DetermineDialogue(GameObject character)
    {
        dialogues.Clear();
        CampPlayerController characterScript = character.GetComponent<CampPlayerController>();
        

        if (characterScript.title == "Astrid")
        {
            if (scm.loadedData.currentChapter == "Chapter 2")
            {
                if (!characterScript.spokenToAlready)
                {
                    dialogues.Add(new CharacterDialogue(astridImage, characterScript.title, new string[] {"Even though we had to leave our home, it's nice to be out in the countryside again.", "I wonder how long we'll be gone.", "Ooo when we get to Maplemire do you think we can get turkey pies??", "It's been so long since I've had one.", "No, we shouldn't we should stay focused..."}));
                    dialogues.Add(new CharacterDialogue(mainCharacterImage, scm.loadedData.mainCharacterName, new string[] {"*laughs*"}));
                    dialogues.Add(new CharacterDialogue(astridImage, characterScript.title, new string[] {"What's so funny??"}));
                    dialogues.Add(new CharacterDialogue(mainCharacterImage, scm.loadedData.mainCharacterName, new string[] {"You, you're very cute.", "We can get turkey pies, dear. I think it would do us some good to take our mind off of everything that's happened.", "I miss this life sometimes.", "Travelling the king's road, sword by my side, no plan. Just living life.",}));
                    dialogues.Add(new CharacterDialogue(astridImage, characterScript.title, new string[] {"*coyly* You mean the life before you met me?"}));
                    dialogues.Add(new CharacterDialogue(mainCharacterImage, scm.loadedData.mainCharacterName, new string[] {"Of course not!", "I would much rather live in the forest in isolation shoveling dirt and breaking my back.", "...rather than spend my time at the tavern, drinking good ale, and knee deep in pus--"}));
                    dialogues.Add(new CharacterDialogue(astridImage, characterScript.title, new string[] {"*bonk*", "Oh hush, you."}));
                    dialogues.Add(new CharacterDialogue(mainCharacterImage, scm.loadedData.mainCharacterName, new string[] {"*chuckles*"}));
                    dialogues.Add(new CharacterDialogue(astridImage, characterScript.title, new string[] {"Is there anything else you need, dear?"}));

                }
                else
                {
                    dialogues.Add(new CharacterDialogue(astridImage, characterScript.title, new string[] {"Hi, dear. Did you need something?"}));
                }
            
            }
            else if (scm.loadedData.currentChapter == "Chapter 3")
            {
                
            }
        }
    }
    private void moveSelectorDown()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y -= 32f;
        rt.anchoredPosition = anchoredPos;

    }
    private void moveSelectorUp()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y += 32f;
        rt.anchoredPosition = anchoredPos;
    }
    private void moveContentWindowUp()
    {
        selectorAudio.Play();
        RectTransform temp = scrollViewContent.GetComponent<RectTransform>();
        temp.anchoredPosition += new Vector2(0f, -32.5f);
        dialogueBotIndex--;
        dialogueTopIndex--;
    }
    private void moveContentWindowDown()
    {
        selectorAudio.Play();
        RectTransform temp = scrollViewContent.GetComponent<RectTransform>();
        temp.anchoredPosition += new Vector2(0f, 32.5f);
        dialogueBotIndex++;
        dialogueTopIndex++;
    }
    private void resetSelectorPosition()
    {
        selector.GetComponent<RectTransform>().anchoredPosition = new Vector2(-469f, 37f);
    }
    private IEnumerator TypeLine(string line, string speaker, AudioSource audioSource, TextMeshProUGUI textBox, float textSpeed) {
        if (speaker == "Astrid")
        {
            audioSource.pitch = 1.2f;
            textBox.color = new Color(1f, .75f, .79f, 1f);
        }
        else
        {
            audioSource.pitch = 1.0f;
            textBox.color = Color.white;
        }
        isTyping = true;
        audioSource.Play();
        foreach (char c in line.ToCharArray()) {
            textBox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        audioSource.Stop();
        isTyping = false;
    }
    private IEnumerator PlayCutscene()
    {
        // Disable gameplay systems
        cutSceneActive = true;
        coroutineRunning = true;
        sceneName = characterSelected.GetComponent<CampPlayerController>().title + (dialogueIndex + 1).ToString();

        StartCoroutine(Helpers.FadeOutAudio(backgroundAudio, 0.75f));
        yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1f));

        mainCamera.SetActive(false);
        eventSystem.SetActive(false);

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        
        yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));
    }
    public void Resume()
    {
        StartCoroutine(ResumeHelper());
    }
    private IEnumerator ResumeHelper()
    {
        StartCoroutine(Helpers.FadeInAudio(backgroundAudio, 1.5f));
        yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1f));
        SceneManager.UnloadSceneAsync(sceneName);
        mainCamera.SetActive(true);
        eventSystem.SetActive(true);
        cutSceneActive = false;
        coroutineRunning = false;
        yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));
        yield return StartCoroutine(ShouldGainAttack());

    }
    private IEnumerator ShouldGainAttack()
    {
        CampPlayerController characterScript = characterSelected.GetComponent<CampPlayerController>();
        Attack newAttack = null;
       
        if (sceneName.Contains("Astrid"))
        {
            if (characterScript.knownAttacks.Count == 1) 
            {
                newAttack = new Attack("Power Draw", "physical", 1.5f, 1.0f, 90, 0, 4, "Shoot a powerful shot at the enemy.");
                characterScript.knownAttacks.Add(newAttack);
            }
            else if (characterScript.knownAttacks.Count == 2)
            {
                newAttack = new Attack("Ankle Snare", "physical", 1.1f, 1.0f, 75, 0, 6, "Target the enemies footing. 50% chance to cripple (Target cannot move).");
                characterScript.knownAttacks.Add(newAttack);                
            }
            else if (characterScript.knownAttacks.Count == 3)
            {
                newAttack = new Attack("Headshot", "physical", 1.5f, 1.0f, 60, 100, 10, "Strike with extreme precision. Always crits.");
                characterScript.knownAttacks.Add(newAttack); 
            }
        }

        if (newAttack != null)
        {
            newAttackBox.SetActive(true);
            newAttackBoxName.text = characterScript.title + " learned a new Attack!";
            newAttackBoxText.text = newAttack.name;
            gainedNewAttackAudio.Play();

            yield return new WaitForSeconds(4f);
            newAttackBox.SetActive(false);

        }
        yield return null;
    }

    public struct CharacterDialogue {
        public string[] lines;
        public string name;
        public GameObject characterImage;
        public CharacterDialogue(GameObject characterImage, string name,string[] lines)
        {
            this.lines = lines;
            this.name = name;
            this.characterImage = characterImage;
        }
    }

}