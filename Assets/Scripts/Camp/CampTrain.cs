using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;

public class CampTrain : MonoBehaviour
{
    private Camera worldCamera;
    private RectTransform characterMenuParentCanvasRect;
    public GameObject characterMenu;
    public bool active = false;
    private bool statSelected = false;
    private int index = 0;
    private int leftRightIndex = 0;
    public AudioSource selectorAudio;
    public AudioSource deselectAudio;
    public GameObject selector;
    public Image blackScreen;
    public GameObject mainCharacterImage;
    private SaveManager scm;
    public GameObject sceneries;
    public GameObject portraits;
    public GameObject trainingWindow;
    public GameObject topArrow;
    public GameObject bottomArrow;
    private CampPlayerController characterScript;
    private int originalAtk;
    private int originalInt;
    private int originalDef;
    private int originalRes;
    private int originalSkl;
    private int originalSpd;
    private int originalHp;
    private int originalMana;
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

    void Awake()
    {
        scm = FindFirstObjectByType<SaveManager>();
    }
    void LateUpdate()
    {
        if (active)
        {
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(DisableTrainingMenu());
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
                    GetStatText().color = Color.white;

                } 
            }


        }
    }
    public IEnumerator EnableTrainingMenu(GameObject character)
    {
        yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1f));

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

        pointsAvailableText.text = characterScript.pointsAvailable.ToString();

        yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));
        active = true;

    }
    public IEnumerator DisableTrainingMenu()
    {
        yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1f));

        mainCharacterImage.SetActive(false);
        trainingWindow.SetActive(false);
        DisableAllScenery();
        DisableAllPortraits();
        ResetSelectorPosition();
        statSelected = false;

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
        
    }
    private void RemovePointFromStat()
    {
        
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
}
