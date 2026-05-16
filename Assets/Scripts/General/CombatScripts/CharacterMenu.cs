using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CharacterMenu : MonoBehaviour
{
    public Camera worldCamera;
    private RectTransform characterMenuParentCanvasRect;
    public GameObject characterMenu;
    public bool active = false;
    private int index = 0;
    private BattleController battleController;
    private InventoryMenu inventoryMenuScript;
    private CharacterInfoScreen characterInfoScript;
    public GameObject selector;
    public AudioSource selectorAudio;
    public AudioSource deselectAudio;
    public TextMeshProUGUI endTurnText;
    public TextMeshProUGUI inventoryText;

    void Awake()
    {
        worldCamera = Camera.main;
        characterMenuParentCanvasRect = GetComponent<RectTransform>();
        battleController = GameObject.Find("BattleController").GetComponent<BattleController>();
        inventoryMenuScript = GameObject.Find("InventoryMenu").GetComponent<InventoryMenu>();
        characterInfoScript = GameObject.Find("CharacterInfoScreen").GetComponent<CharacterInfoScreen>();
        gameObject.GetComponent<Canvas>().worldCamera = worldCamera;
    }
    void LateUpdate()
    {

        if (active)
        {
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Q))
            {
                disableCharacterMenu();
                if (!battleController.disabledCharacters.Contains(battleController.characterSelected))
                {
                    battleController.characterSelected.GetComponent<PlayerController>().movementEnabled = true;
                }
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
                //Open info UI
                if (index == 0)
                {
                    active = false;
                    StartCoroutine(characterInfoScript.enableCharacterInfo(battleController.characterSelected));
                }

                //Open ineventory UI
                else if (index == 1)
                {
                    if (battleController.characterSelected.GetComponent<PlayerController>().owned)
                    {
                        StartCoroutine(inventoryMenuScript.enableInventoryGiverMenu(battleController.characterSelected));
                        active = false;
                    }
                }

                //End turn
                else if (index == 2)
                {
                    if (!battleController.disabledCharacters.Contains(battleController.characterSelected) && battleController.characterSelected.GetComponent<PlayerController>().owned)
                    {
                        StartCoroutine(battleController.characterSelected.GetComponent<PlayerController>().endTurn());
                        selectorAudio.Play();
                    }
                }
            }
        }

    }
    public IEnumerator enableCharacterMenu(GameObject character)
    {   
        yield return null;
        //Gray out EndTurn if character already ended turn
        if (battleController.disabledCharacters.Contains(battleController.characterSelected) || !battleController.characterSelected.GetComponent<PlayerController>().owned)
        {
            endTurnText.color = Color.black;
        }
        else
        {
            endTurnText.color = new Color(1f, 1f, 1f, 1f); 
        }

        //Gray out inventory if character not owned
        if (!battleController.characterSelected.GetComponent<PlayerController>().owned)
        {
            inventoryText.color = Color.black;
        }
        else
        {
            inventoryText.color = Color.white;
        }
        
        characterMenu.SetActive(true);
        active = true;
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(worldCamera, character.transform.position);
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(characterMenuParentCanvasRect, screenPos, worldCamera, out localPos);

        characterMenu.GetComponent<RectTransform>().localPosition = localPos + new Vector2(-20f, 235f);
    }
    public void disableCharacterMenu()
    {
        deselectAudio.Play();
        active = false;
        characterMenu.SetActive(false);
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
