using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CharacterAssistMenu : MonoBehaviour
{
    private SaveManager saveManager;
    public Camera worldCamera;
    private RectTransform characterMenuParentCanvasRect;
    public GameObject characterMenu;
    public bool active = false;
    private int index = 0;
    private BattleController battleController;
    private InventoryMenu inventoryMenuScript;
    private AttackPreview attackPreviewScript;
    private RecruitDialogue recruitDialogueScript;
    public GameObject selector;
    public AudioSource selectorAudio;
    public AudioSource deselectAudio;
    public TextMeshProUGUI talkText;
    public TextMeshProUGUI assistText;
    public TextMeshProUGUI tradeText;

    void Awake()
    {
        worldCamera = Camera.main;
        characterMenuParentCanvasRect = GetComponent<RectTransform>();
        battleController = GameObject.Find("BattleController").GetComponent<BattleController>();
        inventoryMenuScript = GameObject.Find("InventoryMenu").GetComponent<InventoryMenu>();
        attackPreviewScript = GameObject.Find("AttackPreview").GetComponent<AttackPreview>();
        recruitDialogueScript = FindAnyObjectByType<RecruitDialogue>();
        gameObject.GetComponent<Canvas>().worldCamera = worldCamera;
        saveManager = FindAnyObjectByType<SaveManager>();
    }
    void LateUpdate()
    {
        if (active)
        {
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Q))
            {
                disableCharacterAssistMenu();
                battleController.characterSelected.GetComponent<PlayerController>().movementEnabled = true;
                battleController.assistableCharacterSelected = null;
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
                if (index != 2)
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
                    if (!battleController.assistableCharacterSelected.GetComponent<PlayerController>().owned)
                    {
                        recruitDialogueScript.Recruit(battleController.assistableCharacterSelected);
                        disableCharacterAssistMenu();
                    }
                }

                //TODO: Handle assist preview
                else if (index == 1)
                {
                    if (assistText.color == Color.white)
                    {
                        StartCoroutine(attackPreviewScript.enablePreview(true));
                        disableCharacterAssistMenu();
                    }
                }

                //Open trading menu
                else if (index == 2)
                {
                    if (battleController.assistableCharacterSelected.GetComponent<PlayerController>().owned)
                    {
                        inventoryMenuScript.enableTradingMenu();
                        disableCharacterAssistMenu();
                    }
                }
            }
        }
    }
    public void enableCharacterAssistMenu(GameObject character)
    {
        //Set position of UI panel
        characterMenu.SetActive(true);
        active = true;
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(worldCamera, character.transform.position);
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(characterMenuParentCanvasRect, screenPos, worldCamera, out localPos);
        characterMenu.GetComponent<RectTransform>().localPosition = localPos + new Vector2(-20f, 235f);

        //Check if any of character attacks are support moves
        assistText.color = Color.black;
        foreach (AttackMoves attackMove in battleController.characterSelected.GetComponent<PlayerController>().knownAttacks)
        {
            if (attackMove is SupportMove)
            {
                assistText.color = Color.white;
                break;
            }
        }

        //Check if character can be talked to
        if (!character.GetComponent<PlayerController>().owned && battleController.characterSelected.GetComponent<PlayerController>().title == saveManager.loadedData.mainCharacterName)
        {
            talkText.color = Color.white;
            tradeText.color = Color.black;
        }
        else
        {   
            tradeText.color = Color.white;
            talkText.color = Color.black;
        }

    }
    public void disableCharacterAssistMenu()
    {
        deselectAudio.Play();
        characterMenu.SetActive(false);
        active = false;
    }
    private void moveSelectorDown()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y += 27f;
        rt.anchoredPosition = anchoredPos;
    }
    private void moveSelectorUp()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y -= 27f;
        rt.anchoredPosition = anchoredPos;
    }

}
