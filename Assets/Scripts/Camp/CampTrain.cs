using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CampTrain : MonoBehaviour
{
    private Camera worldCamera;
    private RectTransform characterMenuParentCanvasRect;
    public GameObject characterMenu;
    public bool active = false;
    private int index = 0;
    private CampController campControllerScript;
    private InventoryMenu inventoryMenuScript;
    private CampInfoScreen campInfoScript;
    private CampTrade campTradeScript;
    private CampDialogue campDialogueScript;
    private CampTrain campTrainScript;
    public GameObject selector;
    public AudioSource selectorAudio;
    public AudioSource deselectAudio;
    public GameObject characterSelected;


    void Awake()
    {
        worldCamera = Camera.main;
        characterMenuParentCanvasRect = GetComponent<RectTransform>();
        //inventoryMenuScript = GameObject.Find("InventoryMenu").GetComponent<InventoryMenu>();
        campInfoScript = FindFirstObjectByType<CampInfoScreen>();
        campTradeScript = FindFirstObjectByType<CampTrade>();
        campDialogueScript = FindFirstObjectByType<CampDialogue>();
        campControllerScript = FindFirstObjectByType<CampController>();
        campTrainScript = FindFirstObjectByType<CampTrain>();
        gameObject.GetComponent<Canvas>().worldCamera = worldCamera;
    }
    void LateUpdate()
    {
        if (active)
        {
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Q))
            {
                disableTrainingMenu();

            }
            //Move the selector
            else if (Input.GetKeyDown(KeyCode.W))
            {
                if (index != 0)
                {
                    index--;
                    moveSelectorDown();
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (index != 5)
                {
                    index++;
                    moveSelectorUp();
                }
            }

            //Make selection
            else if (Input.GetKeyDown(KeyCode.Space))
            {
            }
        }
    }
    public void enableTrainingMenu(GameObject character)
    {

    }
    public void disableTrainingMenu()
    {

    }
    private void moveSelectorDown()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y += 25f;
        rt.anchoredPosition = anchoredPos;
    }
    private void moveSelectorUp()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y -= 25f;
        rt.anchoredPosition = anchoredPos;
    }

}
