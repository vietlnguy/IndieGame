using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueOptions : MonoBehaviour
{
    private SaveManager scm;
    private CampAssistMenu campAssistMenuScript;
    public Image blackScreen;
    public GameObject mainCamera;
    public GameObject eventSystem;
    public GameObject textBox;
    public GameObject selector;
    public GameObject scrollView;
    public GameObject scrollViewContent;

    public GameObject characterSelected;
    private bool active = false;
    private bool dialogueOptionsActive = false;
    public bool cutSceneActive = false; 
    private bool coroutineRunning = false;   
    public AudioSource selectorAudio;
    public AudioSource backgroundAudio;
    public int dialogueIndex = 0;
    public int dialogueTopIndex = 0;
    public int dialogueBotIndex = 2;
    private string sceneName = "";

    void Awake()
    {
        scm = FindAnyObjectByType<SaveManager>();
    }
    void Update()
    {
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
                    //StartCoroutine(DisableDialogueWindow());
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

    public void EnableWindow(GameObject character)
    {
        StartCoroutine(Sequence());
    }
    private IEnumerator Sequence()
    {
        yield return null;
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
        //yield return StartCoroutine(ShouldGainAttack());

        if (sceneName == "Lucas1" && !scm.loadedData.campTrainingAllowed)
        {
           // yield return StartCoroutine(CampTrainingSequence());
        }
    }
    /*
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
        foreach(Transform child in allLargePortraits.transform)
        {
            child.gameObject.SetActive(false);
        }

    }
    private void disableBackgrounds()
    {
        everdellScenery.SetActive(false);
    }

    
    */
    
}