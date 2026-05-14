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
    public GameObject lucasImage;
    public GameObject celesteImage;
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
        else if (characterScript.title == "Elani")
        {
            
        }

        //Enable the right background
        if (scm.loadedData.currentChapter == "Chapter 2" || scm.loadedData.currentChapter == "Chapter 3")
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
                if (!characterScript.spokenToAlready)
                {
                    dialogues.Add(new CharacterDialogue(astridImage, characterScript.title, new string[] {"I'm glad Lucas and Celeste are staying with us, despite the circumstances."}));
                    dialogues.Add(new CharacterDialogue(mainCharacterImage, scm.loadedData.mainCharacterName, new string[] {"Can't stand to look at me anymore?"}));
                    dialogues.Add(new CharacterDialogue(astridImage, characterScript.title, new string[] {"I'm serious!"}));
                    dialogues.Add(new CharacterDialogue(mainCharacterImage, scm.loadedData.mainCharacterName, new string[] {"Sorry.."}));
                    dialogues.Add(new CharacterDialogue(astridImage, characterScript.title, new string[] {"Its okay.", "And yes now I can finally have another lady to talk to."}));
                    dialogues.Add(new CharacterDialogue(mainCharacterImage, scm.loadedData.mainCharacterName, new string[] {"Hey!"}));
                    dialogues.Add(new CharacterDialogue(astridImage, characterScript.title, new string[] {"Did you need anything else?"}));

                }
                else
                {
                    dialogues.Add(new CharacterDialogue(astridImage, characterScript.title, new string[] {"Hi, dear. Did you need something?"}));
                }  
            }
        }
        else if (characterScript.title == "Lucas")
        {
            if (scm.loadedData.currentChapter == "Chapter 3")
            {
                if (!characterScript.spokenToAlready)
                {
                    dialogues.Add(new CharacterDialogue(mainCharacterImage, scm.loadedData.mainCharacterName, new string[] {"Hey Lucas, are you busy?", "You fought with a lot of courage back there."}));
                    dialogues.Add(new CharacterDialogue(lucasImage, characterScript.title, new string[] {"Thanks.", "..."}));
                    dialogues.Add(new CharacterDialogue(mainCharacterImage, scm.loadedData.mainCharacterName, new string[] {"...", "Look, I know you don't know me, and we're going to be traveling together now.", "But I promise I will do everything I can to keep you and your sister safe."}));
                    dialogues.Add(new CharacterDialogue(lucasImage, characterScript.title, new string[] {"I appreciate it, but we won't be staying long.", "Once we talk to Lord Beesly and get this straightened out, we're going back home."}));
                    dialogues.Add(new CharacterDialogue(mainCharacterImage, scm.loadedData.mainCharacterName, new string[] {"I understand.", "I just want you to know that you don't have to fight alone.", "I know what it's like to care deeply for someone.", "... and to lose someone you love."}));
                    dialogues.Add(new CharacterDialogue(lucasImage, characterScript.title, new string[] {"...", "Did you need anything else?"}));


                }
                else
                {
                    dialogues.Add(new CharacterDialogue(astridImage, characterScript.title, new string[] {"Did you need something?"}));
                }
            
            }
            else if (scm.loadedData.currentChapter == "Chapter 4")
            {
                
            }
        }
        else if (characterScript.title == "Celeste")
        {
            if (scm.loadedData.currentChapter == "Chapter 3")
            {
                if (!characterScript.spokenToAlready)
                {
                    dialogues.Add(new CharacterDialogue(celesteImage, characterScript.title, new string[] {"We follow your word.., let your light guide us toward salvation..."}));
                    dialogues.Add(new CharacterDialogue(mainCharacterImage, scm.loadedData.mainCharacterName, new string[] {"Sorry to inturrept, priestess.", "I brought you some food. I thought you might be hungry after that unexpected skirmish."}));
                    dialogues.Add(new CharacterDialogue(celesteImage, characterScript.title, new string[] {"Ah- thank you, " + scm.loadedData.mainCharacterName + "...", "Ilvera's light shines through you. I can sense it..."}));
                    dialogues.Add(new CharacterDialogue(mainCharacterImage, scm.loadedData.mainCharacterName, new string[] {"Haha, I don't know about that.", "I've done alot of bad in my life, I guess this is my way of making amends."}));
                    dialogues.Add(new CharacterDialogue(celesteImage, characterScript.title, new string[] {"Through Ilvera, all can be redeemed..."}));

                }
                else
                {
                    dialogues.Add(new CharacterDialogue(astridImage, characterScript.title, new string[] {"May Ilvera guide you..."}));
                }
            
            }
            else if (scm.loadedData.currentChapter == "Chapter 4")
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

        if (sceneName == "Lucas1" && !scm.loadedData.campTrainingAllowed)
        {
            yield return StartCoroutine(CampTrainingSequence());
        }
    }
    private IEnumerator ShouldGainAttack()
    {
        active = false;
        CampPlayerController characterScript = characterSelected.GetComponent<CampPlayerController>();
        AttackMoves newAttack = null;
       
        if (sceneName.Contains("Astrid"))
        {
            if (sceneName == "Astrid1" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new Attack("Power Draw", "physical", 1.5f, 1.0f, 90, 0, 4, "Shoot a powerful shot at the enemy.");
                characterScript.knownAttacks.Add(newAttack);
            }

            else if (sceneName == "Astrid2" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new Attack("Ankle Snare", "physical", 1.1f, 1.0f, 75, 0, 6, new List<Debuff>(){new Debuff("Crippled", 100, 1)}, "Target the enemies footing. 100% chance to cripple.");
                characterScript.knownAttacks.Add(newAttack); 
            }

            else if (sceneName == "Astrid3" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new Attack("Headshot", "physical", 1.5f, 1.0f, 60, 100, 10, "Strike with extreme precision. Always crits.");
                characterScript.knownAttacks.Add(newAttack); 
            }
        }
        else if (sceneName.Contains("Lucas"))
        {
            if (sceneName == "Lucas1" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new Attack("Unseen Fist", "physical", 1.1f, 1.0f, 75, 0, 6, new List<Debuff>{new Debuff("Confused", 50, 2)}, "Confuse the enemy with a flurry of strikes. 50% chance to confuse for 2 turns.");                
                characterScript.knownAttacks.Add(newAttack);
            }

            else if (sceneName == "Lucas2" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new Attack("Underdog Spirit", "physical", 1.5f, 1.0f, 75, 0, 6, "Deals double damage if user is below 50% Max HP.");
                characterScript.knownAttacks.Add(newAttack);  
            }

            else if (sceneName == "Lucas3" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new Attack("Killer Instinct", "physical", 2f, 1.0f, 85, 0, 8, new List<Buff>(){new Buff("Flowing", 100, 2)}, "Enter a flow state.");
                characterScript.knownAttacks.Add(newAttack); 
            }
        }
        else if (sceneName.Contains("Celeste"))
        {
            if (sceneName == "Celeste1" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new SupportMove("Mana Restore", 4, "mana", 5, "Restores mana to target. Scales with INT.");
                characterScript.knownAttacks.Add(newAttack);
            }

            else if (sceneName == "Celeste2" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new SupportMove("Cure", 4, "hp", 10, new List<string>(){"all"}, "Restores mana to target. Scales with INT.");
                characterScript.knownAttacks.Add(newAttack);  
            }

            else if (sceneName == "Celeste3" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new SupportMove("Ilvera's Protection", 10, "both", 1000, new List<Buff>(){new Buff("Blessed", 100, 1)}, "Call on the goddess to protect an ally.");
                characterScript.knownAttacks.Add(newAttack); 
            }
        }
        else if (sceneName.Contains("Gerard"))
        {
           if (sceneName == "Gerard1" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new Attack("Triumphant Shout", "physical", 1.5f, 1.0f, 75, 0, 6, new List<Debuff>(){new Debuff("Taunted", 75, 1)}, "Taunts the enemy. Forced to attack closest.");
                characterScript.knownAttacks.Add(newAttack);
            }

            else if (sceneName == "Gerard2" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new Attack("Shield Bash", "physical", 1.3f, 1.0f, 90, 0, 4, "Does bonus damage based on user's DEF.");
                characterScript.knownAttacks.Add(newAttack);  
            }

            else if (sceneName == "Gerard3" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new Attack("Last Stand", "physical", 1.5f, 1.0f, 60, 100, 10, new List<Buff>(){new Buff("Undying", 100, 1)}, "Make a heroic last stand.");
                characterScript.knownAttacks.Add(newAttack); 
            } 
        }
        else if (sceneName.Contains("Penelope"))
        {
           if (sceneName == "Penelope1" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                //newAttack = new SupportMove("Split Chord", 2, "both", 4, "Strum your harp to restore hp and mana to an ally. Scales with INT.");
                newAttack = new SupportMove("Power Chord", 5, "neither", 0, new List<Buff>(){new Buff("Charged", 100, 1)}, "Strum a gnarly chord. Your ally's next attack will do double damage.");
                characterScript.knownAttacks.Add(newAttack);
            }

            else if (sceneName == "Penelope2" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new SupportMove("Rejuvenate", 4, "neither", 0, "Energize your ally with the sound of music. They may take another turn.");
                characterScript.knownAttacks.Add(newAttack);  
            }

            else if (sceneName == "Penelope3" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new SupportMove("Crecendo", 8, "neither", 0, new List<Buff>(){new Buff("Invigorated", 100, 2)}, "Inspire your ally with angelic music. Boosts all primary stats.");
                characterScript.knownAttacks.Add(newAttack); 
            } 
        }
        else if (sceneName.Contains("Katherine"))
        {
           if (sceneName == "Katherine1" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new Attack("Flank", "physical", 1.5f, 1.0f, 90, 0, 4, "Does bonus damage to enemies that can't attack back.");
                characterScript.knownAttacks.Add(newAttack);
            }

            else if (sceneName == "Katherine2" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new Attack("Flame Charge", "magical", 1.5f, 1.0f, 75, 0, 6, "Resisted by RES rather than DEF.");
                characterScript.knownAttacks.Add(newAttack);  
            }

            else if (sceneName == "Katherine3" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new Attack("Stampede", "physical", 1.5f, 1.0f, 85, 0, 6, "Does not end turn.");
                characterScript.knownAttacks.Add(newAttack); 
            } 
        }
        else if (sceneName.Contains("Ivy"))
        {
           if (sceneName == "Ivy1" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new Attack("Noxious Fumes", "magical", 1.0f, 1.2f, 90, 0, 4, new List<Debuff>(){new Debuff("Poisoned", 75, 5)}, "Summon toxic fumes to poison the enemy. 75% chance to poison.");
                characterScript.knownAttacks.Add(newAttack);
            }

            else if (sceneName == "Ivy2" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new Attack("Essence Drain", "magical", 1.0f, 1.5f, 75, 0, 5, "Restore mana equal to damage dealt.");
                characterScript.knownAttacks.Add(newAttack);  
            }

            else if (sceneName == "Ivy3" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new Attack("Spore Burst", "magical", 1.0f, 2.0f, 80, 0, 10, new List<Debuff>(), "Does bonus damage if target is poisoned. Removes poison.");
                characterScript.knownAttacks.Add(newAttack); 
            } 
        }
        else if (sceneName.Contains("Maeve"))
        {
           if (sceneName == "Maeve1" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new Attack("Life Drain", "magical", 1.0f, 1.2f, 90, 0, 4, "Heal for half the damage dealt.");
                characterScript.knownAttacks.Add(newAttack);
            }

            else if (sceneName == "Maeve2" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new SupportMove("Sacrifice", 4, "hp", 5,"Transfer 25% of your health to an ally.");
                characterScript.knownAttacks.Add(newAttack);  
            }

            else if (sceneName == "Maeve3" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new Attack("Blood Bomb", "magical", 1.0f, 2.0f, 80, 0, 10, "Sacrifice 50% current health to do huge damage.");
                characterScript.knownAttacks.Add(newAttack); 
            } 
        }   
        else if (sceneName.Contains("Elani"))
        {
           if (sceneName == "Elani1" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new Attack("Punishment", "physical", 1.5f, 1.0f, 90, 0, 4, "Bonus damage against targets that have a buff.");
                characterScript.knownAttacks.Add(newAttack);
            }

            else if (sceneName == "Elani2" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new Attack("Babydoll Eyes", "physical", 1.3f, 1.0f, 75, 0, 6, new List<Debuff>() {new Debuff("Vulnerable", 100, 2)}, "Enemy lowers their gaurd. Set Vulnerable for 2 turns.");
                characterScript.knownAttacks.Add(newAttack);  
            }

            else if (sceneName == "Elani3" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
            {
                newAttack = new Attack("Climax", "physical", 1.5f, 1.0f, 70, 0, 10, new List<Debuff>(), "The dramatic kind. If target is brought below 20% max HP, target dies.");
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
        
        characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained = true;
        active = true;
        yield return null;
    }
    private IEnumerator CampTrainingSequence()
    {
        active = false;
        newAttackBox.SetActive(true);
        newAttackBoxName.text = "Camp Feature Unlocked!";
        newAttackBoxText.text = "Training";
        gainedNewAttackAudio.Play();

        yield return new WaitForSeconds(4f);
        newAttackBox.SetActive(false);
        scm.loadedData.campTrainingAllowed = true;
        campAssistMenuScript.trainText.color = Color.white;
        active = true;

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