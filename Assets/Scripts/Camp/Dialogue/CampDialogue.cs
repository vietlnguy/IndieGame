using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CampDialogue : MonoBehaviour
{
    //Script References
    private SaveManager scm;
    private CampAssistMenu campAssistMenuScript;
    private DialogueController dialogueControllerScript;
    
    public Image blackScreen;

    //Sceneries
    public GameObject everdellScenery;

    //Portraits
    public GameObject mainCharacterImage;
    public GameObject allLargePortraits;

    void Awake()
    {
        scm = FindAnyObjectByType<SaveManager>();
        campAssistMenuScript = FindAnyObjectByType<CampAssistMenu>();
        dialogueControllerScript = FindAnyObjectByType<DialogueController>();
    }
    void Update()
    {

    }
    public IEnumerator EnableDialogueWindow(GameObject character)
    {
        yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1f));

        CampPlayerController characterScript = character.GetComponent<CampPlayerController>();
        mainCharacterImage.SetActive(true);

        //Enable the right character image
        foreach(Transform child in allLargePortraits.transform)
        {
            if (child.gameObject.name.Contains(characterScript.title, System.StringComparison.OrdinalIgnoreCase))
            {
                child.gameObject.SetActive(true);
            }
        }

        //Enable the right background
        if (scm.loadedData.currentChapter == "Chapter 2" || scm.loadedData.currentChapter == "Chapter 3")
        {
            everdellScenery.SetActive(true);
        }
        else if (scm.loadedData.currentChapter == "Chapter 4" || scm.loadedData.currentChapter == "Chapter 5")
        {
            
        }

        //Choose correct dialogue
        dialogueControllerScript.SetCampDialogue(characterScript);
        
        yield return StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));

        //Play dialogue
        yield return Helpers.PlayDialogueAndWait(dialogueControllerScript, false);

    }

}