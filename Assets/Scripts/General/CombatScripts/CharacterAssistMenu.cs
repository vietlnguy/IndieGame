using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CharacterAssistMenu : MonoBehaviour
{
    public Camera worldCamera;
    private RectTransform characterMenuParentCanvasRect;
    public GameObject characterMenu;
    public bool active = false;
    private int index = 0;
    public BattleController battleController;
    public InventoryMenu inventoryMenuScript;
    public AttackPreview attackPreviewScript;
    public GameObject selector;
    public AudioSource selectorAudio;
    public AudioSource deselectAudio;
    public TextMeshProUGUI talkText;
    public TextMeshProUGUI assistText;

    void Awake()
    {
        worldCamera = Camera.main;
        characterMenuParentCanvasRect = GetComponent<RectTransform>();
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
                    inventoryMenuScript.enableTradingMenu();
                    disableCharacterAssistMenu();
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
        assistText.color = new Color(.5f, .5f, .5f, .5f);
        foreach (AttackMoves attackMove in battleController.characterSelected.GetComponent<PlayerController>().knownAttacks)
        {
            if (attackMove is SupportMove)
            {
                assistText.color = Color.white;
                break;
            }
        }

        //Check if character can be talked to
        //TODO: what does this even mean

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
