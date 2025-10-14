using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterMenu : MonoBehaviour
{
    public Camera worldCamera;
    private RectTransform characterMenuParentCanvasRect;
    public GameObject characterMenu;
    public bool active = false;
    private int index = 0;
    public BattleController battleController;
    public InventoryMenu inventoryMenuScript;
    public GameObject selector;
    public AudioSource selectorAudio;
    public AudioSource deselectAudio;

    void Awake()
    {
        worldCamera = Camera.main;
        characterMenuParentCanvasRect = GetComponent<RectTransform>();
    }
    void LateUpdate()
    {
        if (active)
        {
            if (Input.GetMouseButtonDown(1))
            {
                disableCharacterMenu();
                battleController.characterSelected.GetComponent<PlayerController>().movementEnabled = true;
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

                }

                //Open ineventory UI
                else if (index == 1)
                {
                    inventoryMenuScript.enableInventoryGiverMenu();
                    disableCharacterMenu();
                }

                //End turn
                else if (index == 2)
                {
                    battleController.characterSelected.GetComponent<PlayerController>().endTurn();
                    selectorAudio.Play();
                }
            }
        }
    }
    public void enableCharacterMenu(GameObject character)
    {
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
