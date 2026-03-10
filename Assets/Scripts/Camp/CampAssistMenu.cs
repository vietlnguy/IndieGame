using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CampAssistMenu : MonoBehaviour
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
        gameObject.GetComponent<Canvas>().worldCamera = worldCamera;
    }
    void LateUpdate()
    {
        if (active)
        {
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Q))
            {
                disableCharacterAssistMenu();

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
                if (index != 4)
                {
                    index++;
                    moveSelectorUp();
                }
            }

            //Make selection
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                //TODO: Handle Talk option
                if (index == 0)
                {
                    active = false;
                    StartCoroutine(campDialogueScript.EnableDialogueWindow(characterSelected));
                }

                //Open info screen
                else if (index == 1)
                {
                    active = false;
                    StartCoroutine(campInfoScript.enableCharacterInfo(characterSelected));
                }

                //Open trading menu
                else if (index == 2)
                {
                    campTradeScript.enableTradingMenu(characterSelected);
                    active = false;
                }

                //Open equipment menu
                else if (index == 3)
                {
                    active = false;
                    campTradeScript.enableEquipmentMenu(characterSelected);
                }

                //Open supply menu
                else if (index == 4)
                {
                    campTradeScript.enableSupplyMenu(characterSelected);
                    active = false;
                }
            }
        }
    }
    public void enableCharacterAssistMenu(GameObject character)
    {
        //Set position of UI panel
        characterMenu.SetActive(true);
        characterSelected = character;
        active = true;
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(worldCamera, character.transform.position);
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(characterMenuParentCanvasRect, screenPos, worldCamera, out localPos);
        characterMenu.GetComponent<RectTransform>().localPosition = localPos + new Vector2(-20f, 200f);
        campControllerScript.movementEnabled = false;

        //Check if character can be talked to
        //TODO: what does this even mean

    }
    public void disableCharacterAssistMenu()
    {
        deselectAudio.Play();
        characterMenu.SetActive(false);
        active = false;
        campControllerScript.movementEnabled = true;
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
