using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;

public class CampTrain : MonoBehaviour
{
    private SaveManager scm;
    private CampPlayerController characterScript;
    private CampAssistMenu campAssistMenuScript;
    public ConfirmTrainButton confirmTrainButtonScript;
    public bool active = false;
    private bool statSelected = false;
    private int index = 0;
    private int leftRightIndex = 0;
    public AudioSource selectorAudio;
    public AudioSource deselectAudio;
    public Image blackScreen;
    public GameObject selector;
    public GameObject mainCharacterImage;
    public GameObject sceneries;
    public GameObject portraits;
    public GameObject trainingWindow;
    public GameObject topArrow;
    public GameObject bottomArrow;
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueBoxText;
    private int originalAtk;
    private int originalInt;
    private int originalDef;
    private int originalRes;
    private int originalSkl;
    private int originalSpd;
    private int originalHp;
    private int originalMana;
    private int originalPoints;
    public TextMeshProUGUI atkStat;
    public TextMeshProUGUI intStat;
    public TextMeshProUGUI defStat;
    public TextMeshProUGUI resStat;
    public TextMeshProUGUI sklStat;
    public TextMeshProUGUI spdStat;
    public TextMeshProUGUI hpStat;
    public TextMeshProUGUI manaStat;
    public TextMeshProUGUI pointsAvailableText;
    private Coroutine flashCoroutine;
    private List<string> astridDialogueOptions;
    private List<string> penelopeDialogueOptions;
    private List<string> lucasDialogueOptions;
    private List<string> celesteDialogueOptions;
    private List<string> gerardDialogueOptions;
    private List<string> katherineDialogueOptions;
    private List<string> ivyDialogueOptions;
    private List<string> maeveDialogueOptions;
    private List<string> elaniDialogueOptions;
    private List<string> mainCharacterDialogueOptions;

    void Start()
    {
        scm = FindAnyObjectByType<SaveManager>();
        campAssistMenuScript = FindAnyObjectByType<CampAssistMenu>();

        astridDialogueOptions = new List<string>() {"To protect everyone..", "I hope we get through this soon.", "Strength through precision.", "Take aim, breath, and release.", "I have to do my part."};
        penelopeDialogueOptions = new List<string>() {"Let's not work too hard..", "I can't only rely on Gerard and Katherine.", "And I just took a bath..", "Yipee!", "Princesses can be strong too!"};
        lucasDialogueOptions = new List<string>() {"This is gonna be easy!", "Gotta make sis proud.", "I feel stronger already!", "Piece of cake!", "For mom and dad..."};
        celesteDialogueOptions = new List<string>() {"Ilvera guide me...", "In the name of the goddess..", "May the light flow through me.", "I feel blessed.", "Do no harm."};
        gerardDialogueOptions = new List<string>() {"Hmph.", "On my honor.", "As duty commands..", "Consider it done.", "For the princess.."};
        katherineDialogueOptions = new List<string>() {};
        ivyDialogueOptions = new List<string>() {};
        maeveDialogueOptions = new List<string>() {};
        elaniDialogueOptions = new List<string>() {};
        mainCharacterDialogueOptions = new List<string>() {"Can never be too prepared.", "Gotta stay sharp.", "Let's do this!", "Hm, what should I train next?", "Can't fall behind."};
    }
    void LateUpdate()
    {
        if (active)
        {
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(DisableTrainingMenu(true));
            }
            
            //Move the selector
            if (!statSelected)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (index != 0)
                    {
                        index--;
                        MoveSelectorDown();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    if (index != 3)
                    {
                        index++;
                        MoveSelectorUp();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    if (leftRightIndex != 0)
                    {
                        leftRightIndex--;
                        MoveSelectorLeft();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    if (leftRightIndex != 1)
                    {
                        leftRightIndex++;
                        MoveSelectorRight();
                    }
                }  
            
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    statSelected = true;
                    bottomArrow.SetActive(true);
                    topArrow.SetActive(true);
                    flashCoroutine = StartCoroutine(Helpers.FlashTextColor(GetStatText(), new Color32(48, 92, 222, 255), 1.5f));
                }
            }
            
            //Put points in stats
            else
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (characterScript.pointsAvailable > 0)
                    {
                        PutPointIntoStat();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    RemovePointFromStat();
                } 
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    statSelected = false;
                    bottomArrow.SetActive(false);
                    topArrow.SetActive(false);
                    StopCoroutine(flashCoroutine);

                    SetTextColor();

                } 
            }


        }
    }
    public IEnumerator EnableTrainingMenu(GameObject character)
    {
        yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1f));

        try { StopCoroutine(flashCoroutine); }
        catch {}

        hpStat.color = Color.white;
        manaStat.color = Color.white;
        atkStat.color = Color.white;
        intStat.color = Color.white;
        resStat.color = Color.white;
        defStat.color = Color.white;
        spdStat.color = Color.white;
        sklStat.color = Color.white;

        characterScript = character.GetComponent<CampPlayerController>();

        //Enable the right character image
        foreach(Transform child in portraits.transform)
        {
            mainCharacterImage.SetActive(true);

            if (child.gameObject.name.Contains(characterScript.title, StringComparison.OrdinalIgnoreCase))
            {
                mainCharacterImage.SetActive(false);
                child.gameObject.SetActive(true);
                break;
            }
        }
        
        //Enable the right background
        if (scm.loadedData.currentChapter == "Chapter 2" || scm.loadedData.currentChapter == "Chapter 3")
        {
            foreach (Transform child in sceneries.transform)
            {
                if (child.gameObject.name.Contains("everdell"))
                {
                    child.gameObject.SetActive(true);
                }
            }
        }

        //Enable trainingWindow
        trainingWindow.SetActive(true);

        //Populate stats
        PopulateStats();

        //Store original stats
        StoreOriginalStats();

        yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));
        active = true;

        yield return new WaitForSeconds(0.5f);

        //Text box
        DetermineText();
        StartCoroutine(Helpers.MoveRectTransform(dialogueBox, dialogueBox.GetComponent<RectTransform>().anchoredPosition, dialogueBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 10f), .25f));
        StartCoroutine(Helpers.FadeInCanvasGroup(dialogueBox.GetComponent<CanvasGroup>(), 0.25f));

        yield return new WaitForSeconds(2f);

        StartCoroutine(Helpers.MoveRectTransform(dialogueBox, dialogueBox.GetComponent<RectTransform>().anchoredPosition, dialogueBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, -10f), .25f));
        StartCoroutine(Helpers.FadeOutCanvasGroup(dialogueBox.GetComponent<CanvasGroup>(), 0.25f));
    }
    public IEnumerator DisableTrainingMenu(bool reset)
    {
        yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1f));

        mainCharacterImage.SetActive(false);
        trainingWindow.SetActive(false);
        DisableAllScenery();
        DisableAllPortraits();
        ResetSelectorPosition();
        ResetIndexes();
        statSelected = false;
        active = false;
        bottomArrow.SetActive(false);
        topArrow.SetActive(false);

        if (reset)
        {
            ResetStatsAndPoints();
        }

        dialogueBox.GetComponent<CanvasGroup>().alpha = 0;

        yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));
        campAssistMenuScript.active = true;

    }
    private void MoveSelectorDown()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y += 70f;
        rt.anchoredPosition = anchoredPos;
    }
    private void MoveSelectorUp()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y -= 70f;
        rt.anchoredPosition = anchoredPos;
    }
    private void MoveSelectorLeft()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.x -= 254f;
        rt.anchoredPosition = anchoredPos;
    }
    private void MoveSelectorRight()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.x += 254f;
        rt.anchoredPosition = anchoredPos;  
    }
    private void ResetSelectorPosition()
    {
        selector.GetComponent<RectTransform>().anchoredPosition = new Vector2(-268f, 100f);
    }
    private void ResetIndexes()
    {
        index = 0;
        leftRightIndex = 0;
    }
    private void DisableAllScenery()
    {
        foreach (Transform child in sceneries.transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    private void DisableAllPortraits()
    {
        foreach (Transform child in portraits.transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    private void PutPointIntoStat()
    {
        if (leftRightIndex == 0)
        {
            if (index == 0)
            {
                characterScript.baseMaxHp += 2;
                hpStat.text = characterScript.baseMaxHp.ToString();
            }
            else if (index == 1)
            {
                characterScript.baseAttack++;
                atkStat.text = characterScript.baseAttack.ToString();
            }
            else if (index == 2)
            {
                characterScript.baseIntelligence++;
                intStat.text = characterScript.baseIntelligence.ToString();
            }
            else if (index == 3)
            {
                characterScript.baseDefense++;
                defStat.text = characterScript.baseDefense.ToString();
            }

        }
        else if (leftRightIndex == 1)
        {
            if (index == 0)
            {
                characterScript.baseMaxMana += 2;
                manaStat.text = characterScript.baseMaxMana.ToString();
            }
            else if (index == 1)
            {
                characterScript.baseSpeed++;
                spdStat.text = characterScript.baseSpeed.ToString();
            }
            else if (index == 2)
            {
                characterScript.baseSkill++;
                sklStat.text = characterScript.baseSkill.ToString();
            }
            else if (index == 3)
            {
                characterScript.baseResistance++;
                resStat.text = characterScript.baseResistance.ToString();
            }

        } 
        
        characterScript.pointsAvailable--;
        pointsAvailableText.text = characterScript.pointsAvailable.ToString();
        confirmTrainButtonScript.Activate();
    }
    private void RemovePointFromStat()
    {
        if (leftRightIndex == 0)
        {
            if (index == 0)
            {
                if (characterScript.baseMaxHp > originalHp)
                {
                    characterScript.baseMaxHp -= 2;
                    hpStat.text = characterScript.baseMaxHp.ToString();
                    characterScript.pointsAvailable++;
                }

            }
            else if (index == 1)
            {
                if (characterScript.baseAttack > originalAtk)
                {
                    characterScript.baseAttack--;
                    atkStat.text = characterScript.baseAttack.ToString();
                    characterScript.pointsAvailable++;        
                }

            }
            else if (index == 2)
            {
                if (characterScript.baseIntelligence > originalInt)
                {
                    characterScript.baseIntelligence--;
                    intStat.text = characterScript.baseIntelligence.ToString();
                    characterScript.pointsAvailable++;
                }
            }
            else if (index == 3)
            {
                if (characterScript.baseDefense > originalDef)
                {
                    characterScript.baseDefense--;
                    defStat.text = characterScript.baseDefense.ToString();
                    characterScript.pointsAvailable++;
                }
            }

        }
        else if (leftRightIndex == 1)
        {
            if (index == 0)
            {
                if (characterScript.baseMaxMana > originalMana)
                {
                    characterScript.baseMaxMana -= 2;
                    manaStat.text = characterScript.baseMaxMana.ToString();
                    characterScript.pointsAvailable++;
                }
            }
            else if (index == 1)
            {
                if (characterScript.baseSpeed > originalSpd)
                {
                    characterScript.baseSpeed--;
                    spdStat.text = characterScript.baseSpeed.ToString();
                    characterScript.pointsAvailable++;
                }
            }
            else if (index == 2)
            {
                if (characterScript.baseSkill > originalSkl)
                {
                    characterScript.baseSkill--;
                    sklStat.text = characterScript.baseSkill.ToString();
                    characterScript.pointsAvailable++;
                }
            }
            else if (index == 3)
            {
                if (characterScript.baseResistance > originalRes)
                {
                    characterScript.baseResistance--;
                    resStat.text = characterScript.baseResistance.ToString();
                    characterScript.pointsAvailable++;
                }
            }

        } 
        
        pointsAvailableText.text = characterScript.pointsAvailable.ToString();
    }
    private void PopulateStats()
    {
        atkStat.text = characterScript.baseAttack.ToString();
        intStat.text = characterScript.baseIntelligence.ToString();
        defStat.text = characterScript.baseDefense.ToString();
        resStat.text = characterScript.baseResistance.ToString();
        sklStat.text = characterScript.baseSkill.ToString();
        spdStat.text = characterScript.baseSpeed.ToString();
        hpStat.text = characterScript.baseMaxHp.ToString();
        manaStat.text = characterScript.baseMaxMana.ToString();
        pointsAvailableText.text = characterScript.pointsAvailable.ToString();
    }
    private void StoreOriginalStats()
    {
        originalAtk =  characterScript.baseAttack;
        originalInt =  characterScript.baseIntelligence;
        originalDef =  characterScript.baseDefense;
        originalRes =  characterScript.baseResistance;
        originalSkl =  characterScript.baseSkill;
        originalSpd =  characterScript.baseSpeed;
        originalHp = characterScript.baseMaxHp;
        originalMana = characterScript.baseMaxMana;
        originalPoints = characterScript.pointsAvailable;
    }
    private TextMeshProUGUI GetStatText()
    {
        if (leftRightIndex == 0)
        {
            if (index == 0)
            {
                return hpStat;
            }
            else if (index == 1)
            {
                return atkStat;
            }
            else if (index == 2)
            {
                return intStat;
            }
            else if (index == 3)
            {
                return defStat;
            }

        }
        else if (leftRightIndex == 1)
        {
            if (index == 0)
            {
                return manaStat;
            }
            else if (index == 1)
            {
                return spdStat;

            }
            else if (index == 2)
            {
                return sklStat;
            }
            else if (index == 3)
            {
                return resStat;
            }

        } 

        return null;
    }
    private void SetTextColor()
    {
        if (leftRightIndex == 0)
        {
            if (index == 0)
            {
                if (characterScript.baseMaxHp > originalHp)
                {
                    GetStatText().color = Color.blue;
                }
                else
                {
                    GetStatText().color = Color.white;
                }

            }
            else if (index == 1)
            {
                if (characterScript.baseAttack > originalAtk)
                {
                    GetStatText().color = Color.blue;
                }
                else
                {
                    GetStatText().color = Color.white;
                }
            }
            else if (index == 2)
            {
                if (characterScript.baseIntelligence > originalInt)
                {
                    GetStatText().color = Color.blue;
                }
                else
                {
                    GetStatText().color = Color.white;
                }                
            }
            else if (index == 3)
            {
                if (characterScript.baseDefense > originalDef)
                {
                    GetStatText().color = Color.blue;
                }
                else
                {
                    GetStatText().color = Color.white;
                }   
            }

        }
        else if (leftRightIndex == 1)
        {
            if (index == 0)
            {
                if (characterScript.baseMaxMana > originalMana)
                {
                    GetStatText().color = Color.blue;
                }
                else
                {
                    GetStatText().color = Color.white;
                }
            }
            else if (index == 1)
            {
                if (characterScript.baseSpeed > originalSpd)
                {
                    GetStatText().color = Color.blue;
                }
                else
                {
                    GetStatText().color = Color.white;
                }
            }
            else if (index == 2)
            {
                if (characterScript.baseSkill > originalSkl)
                {
                    GetStatText().color = Color.blue;
                }
                else
                {
                    GetStatText().color = Color.white;
                }
            }
            else if (index == 3)
            {
                if (characterScript.baseResistance > originalRes)
                {
                    GetStatText().color = Color.blue;
                }
                else
                {
                    GetStatText().color = Color.white;
                }
            }

        } 
    }
    private void DetermineText()
    {
        if (characterScript.title == "Astrid")
        {
            dialogueBoxText.text = astridDialogueOptions[UnityEngine.Random.Range(0, 5)];
        }
        else if (characterScript.title == "Penelope")
        {
            dialogueBoxText.text = penelopeDialogueOptions[UnityEngine.Random.Range(0, 5)]; 
        }
        else if (characterScript.title == "Gerard")
        {
            dialogueBoxText.text = gerardDialogueOptions[UnityEngine.Random.Range(0, 5)]; 
        }
        else if (characterScript.title == "Katherine")
        {
            dialogueBoxText.text = katherineDialogueOptions[UnityEngine.Random.Range(0, 5)];
        }
        else if (characterScript.title == "Lucas")
        {
            dialogueBoxText.text = lucasDialogueOptions[UnityEngine.Random.Range(0, 5)];       
        }
        else if (characterScript.title == "Celeste")
        {
            dialogueBoxText.text = celesteDialogueOptions[UnityEngine.Random.Range(0, 5)];  
        }
        else if (characterScript.title == "Ivy")
        {
            dialogueBoxText.text = ivyDialogueOptions[UnityEngine.Random.Range(0, 5)];   
        }
        else if (characterScript.title == "Maeve")
        {
            dialogueBoxText.text = ivyDialogueOptions[UnityEngine.Random.Range(0, 5)];
        }
        else if (characterScript.title == "Elani")
        {
            dialogueBoxText.text = elaniDialogueOptions[UnityEngine.Random.Range(0, 5)];
        }
        else
        {
            dialogueBoxText.text = mainCharacterDialogueOptions[UnityEngine.Random.Range(0, 5)];
        }



    }
    private void ResetStatsAndPoints()
    {
        characterScript.baseAttack = originalAtk;
        characterScript.baseIntelligence = originalInt;
        characterScript.baseDefense = originalDef;
        characterScript.baseResistance = originalRes;
        characterScript.baseSkill = originalSkl;
        characterScript.baseSpeed = originalSpd;
        characterScript.pointsAvailable = originalPoints;
        characterScript.baseMaxHp = originalHp;
        characterScript.baseMaxMana = originalMana;
    }
    public void UndoPoints()
    {
        characterScript.baseAttack = originalAtk;
        characterScript.baseIntelligence = originalInt;
        characterScript.baseDefense = originalDef;
        characterScript.baseResistance = originalRes;
        characterScript.baseSkill = originalSkl;
        characterScript.baseSpeed = originalSpd;
        characterScript.pointsAvailable = originalPoints;
        characterScript.baseMaxHp = originalHp;
        characterScript.baseMaxMana = originalMana;
        characterScript.pointsAvailable = originalPoints;
        
        hpStat.color = Color.white;
        manaStat.color = Color.white;
        atkStat.color = Color.white;
        intStat.color = Color.white;
        resStat.color = Color.white;
        defStat.color = Color.white;
        spdStat.color = Color.white;
        sklStat.color = Color.white;


        PopulateStats();
    }
    public void ConfirmTrain()
    {
        StartCoroutine(DisableTrainingMenu(false));
    }
}
