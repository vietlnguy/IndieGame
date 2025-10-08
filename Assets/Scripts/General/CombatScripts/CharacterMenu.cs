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

    void Awake()
    {
        worldCamera = Camera.main;
        characterMenuParentCanvasRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (active)
        {
            //Move the selector
            if (Input.GetKey(KeyCode.W))
            {
                if (index != 0)
                {
                    index--;
                }
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (index != 2)
                {
                    index++;
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
        characterMenu.SetActive(false);
        active = false;
    }

}
