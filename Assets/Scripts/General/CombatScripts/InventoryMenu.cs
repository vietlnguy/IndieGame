using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class InventoryMenu : MonoBehaviour
{

    public GameObject inventoryGiverMenu;
    public bool active = false;
    private int index = 0;
    public BattleController battleController;
    public CharacterMenu characterMenuScript;
    public GameObject selector;
    public GameObject confirmBoxSelector;
    public AudioSource selectorAudio;
    public AudioSource deselectAudio;
    public GameObject items;
    public TextMeshProUGUI previewPlayerHp;
    public TextMeshProUGUI previewPlayerMaxHp;
    public TextMeshProUGUI previewPlayerMana;
    public TextMeshProUGUI previewPlayerMaxMana;
    public GameObject previewPlayerHpBar;
    public GameObject previewPlayerManaBar;
    private Vector2 originalHpManaBarSize;
    public TextMeshProUGUI characterTitle;
    public TextMeshProUGUI itemDescriptionText;
    private int itemIndex = 0;
    public int confirmBoxIndex = 0;
    public GameObject confirmBox;
    private bool confirmBoxActive = false;

    void Awake()
    {
        characterMenuScript = GameObject.Find("CharacterMenu").GetComponent<CharacterMenu>();
        originalHpManaBarSize = previewPlayerHpBar.GetComponent<RectTransform>().sizeDelta;
    }
    void LateUpdate()
    {
        if (active)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (confirmBoxActive)
                {
                    disableConfirmBox();
                }
                else
                {
                    disableInventoryMenu();
                }
            }

            //Move the selector
            else if (Input.GetKeyDown(KeyCode.W) && !confirmBoxActive)
            {
                if (index != 0)
                {
                    index--;
                    moveSelectorUp();
                }
            }
            else if (Input.GetKeyDown(KeyCode.S) && !confirmBoxActive)
            {
                if (index != 4)
                {
                    index++;
                    moveSelectorDown();
                }
            }

            //Move confirm box selector
            else if (Input.GetKeyDown(KeyCode.W) && confirmBoxActive)
            {
                if (confirmBoxIndex != 0)
                {
                    moveConfirmBoxSelectorUp();
                }
            }
            else if (Input.GetKeyDown(KeyCode.S) && confirmBoxActive)
            {
                if (confirmBoxIndex != 1)
                {
                    moveConfirmBoxSelectorDown();
                }
            }

            //Make item selection
            else if (Input.GetKeyDown(KeyCode.Space) && !confirmBoxActive)
            {
                try
                {
                    if (battleController.characterSelected.GetComponent<PlayerController>().inventory[itemIndex] == null) ;
                    confirmBox.SetActive(true);
                    confirmBoxActive = true;
                }
                catch
                {
                    //Not a valid item selection
                }
            }

            //Confirm item use
            else if (Input.GetKeyDown(KeyCode.Space) && confirmBoxActive)
            {
                if (confirmBoxIndex == 0)
                {
                    useItem();
                }
                else if (confirmBoxIndex == 1)
                {
                    resetConfirmBoxSelector();
                    confirmBox.SetActive(false);
                    confirmBoxActive = false;
                }
            }
        }
    }
    public void enableInventoryGiverMenu()
    {
        inventoryGiverMenu.SetActive(true);
        active = true;
        int index = 0;
        characterMenuScript.disableCharacterMenu();

        //Update titles
        characterTitle.text = battleController.characterSelected.GetComponent<PlayerController>().title;

        //Update health bars and values
        previewPlayerHp.text = battleController.characterSelected.GetComponent<PlayerController>().hp.ToString();
        previewPlayerMaxHp.text = battleController.characterSelected.GetComponent<PlayerController>().maxHp.ToString();
        previewPlayerMana.text = battleController.characterSelected.GetComponent<PlayerController>().mana.ToString();
        previewPlayerMaxMana.text = battleController.characterSelected.GetComponent<PlayerController>().maxMana.ToString();

        previewPlayerHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.characterSelected.GetComponent<PlayerController>().hp / battleController.characterSelected.GetComponent<PlayerController>().maxHp, 1f);
        previewPlayerManaBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.characterSelected.GetComponent<PlayerController>().mana / battleController.characterSelected.GetComponent<PlayerController>().maxMana, 1f);

        //Populate inventory
        foreach (Transform row in items.transform)
        {
            try
            {
                row.gameObject.GetComponent<TextMeshProUGUI>().text = battleController.characterSelected.GetComponent<PlayerController>().inventory[index].name;
                row.GetChild(0).GetComponent<TextMeshProUGUI>().text = battleController.characterSelected.GetComponent<PlayerController>().inventory[index].currentQuantity.ToString() + "/" + battleController.characterSelected.GetComponent<PlayerController>().inventory[index].maxQuantity.ToString();
            }
            catch
            {
                //Nothing
            }
            index++;
        }

        updateDescription();

    }
    public void disableInventoryMenu()
    {
        deselectAudio.Play();
        foreach (Transform row in items.transform)
        {
            row.gameObject.GetComponent<TextMeshProUGUI>().text = "-";
            row.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-";
        }
        itemDescriptionText.text = "-";

        //Reset health and mana bars
        previewPlayerHpBar.GetComponent<RectTransform>().sizeDelta = originalHpManaBarSize;
        previewPlayerManaBar.GetComponent<RectTransform>().sizeDelta = originalHpManaBarSize;

        inventoryGiverMenu.SetActive(false);
        active = false;
        battleController.characterSelected.GetComponent<PlayerController>().movementEnabled = true;
    }
    public void disableConfirmBox()
    {
        confirmBox.SetActive(false);
        confirmBoxActive = false;
        resetConfirmBoxSelector();
    }
    public void enableTradingMenu()
    {

    }
    private void moveSelectorDown()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y -= 25.5f;
        rt.anchoredPosition = anchoredPos;
        itemIndex++;
        updateDescription();
    }
    private void moveSelectorUp()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y += 25.5f;
        rt.anchoredPosition = anchoredPos;
        itemIndex--;
        updateDescription();
    }
    private void moveConfirmBoxSelectorDown()
    {
        selectorAudio.Play();
        RectTransform rt = confirmBoxSelector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y -= 31f;
        rt.anchoredPosition = anchoredPos;
        confirmBoxIndex++;
    }
    private void moveConfirmBoxSelectorUp()
    {
        selectorAudio.Play();
        RectTransform rt = confirmBoxSelector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y += 31f;
        rt.anchoredPosition = anchoredPos;
        confirmBoxIndex--;
    }
    private void updateDescription()
    {
        try
        {
            itemDescriptionText.text = battleController.characterSelected.GetComponent<PlayerController>().inventory[itemIndex].description;
        }
        catch
        {
            itemDescriptionText.text = "-";
        }
    }
    private void resetConfirmBoxSelector()
    {
        RectTransform rect = confirmBoxSelector.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(.89f, 15f);
        confirmBoxIndex = 0;
    }
    private void useItem()
    {
        Item item = battleController.characterSelected.GetComponent<PlayerController>().inventory[itemIndex];

        //Restore HP or Mana
        if (item.hpOrMana == "hp")
        {
            if (battleController.characterSelected.GetComponent<PlayerController>().hp + item.restorationAmount > battleController.characterSelected.GetComponent<PlayerController>().maxHp)
            {
                battleController.characterSelected.GetComponent<PlayerController>().hp = battleController.characterSelected.GetComponent<PlayerController>().maxHp;
            }
            else
            {
                battleController.characterSelected.GetComponent<PlayerController>().hp += item.restorationAmount;
            }
        }
        else if (item.hpOrMana == "mana")
        {
            if (battleController.characterSelected.GetComponent<PlayerController>().mana + item.restorationAmount > battleController.characterSelected.GetComponent<PlayerController>().maxMana)
            {
                battleController.characterSelected.GetComponent<PlayerController>().mana = battleController.characterSelected.GetComponent<PlayerController>().maxMana;
            }
            else
            {
                battleController.characterSelected.GetComponent<PlayerController>().mana += item.restorationAmount;
            }
        }

        //Update health bars and values
        previewPlayerHp.text = battleController.characterSelected.GetComponent<PlayerController>().hp.ToString();
        previewPlayerMaxHp.text = battleController.characterSelected.GetComponent<PlayerController>().maxHp.ToString();
        previewPlayerMana.text = battleController.characterSelected.GetComponent<PlayerController>().mana.ToString();
        previewPlayerMaxMana.text = battleController.characterSelected.GetComponent<PlayerController>().maxMana.ToString();
        previewPlayerHpBar.GetComponent<RectTransform>().sizeDelta = originalHpManaBarSize;
        previewPlayerManaBar.GetComponent<RectTransform>().sizeDelta = originalHpManaBarSize;
        previewPlayerHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.characterSelected.GetComponent<PlayerController>().hp / battleController.characterSelected.GetComponent<PlayerController>().maxHp, 1f);
        previewPlayerManaBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)battleController.characterSelected.GetComponent<PlayerController>().mana / battleController.characterSelected.GetComponent<PlayerController>().maxMana, 1f);

        //Cure status ailments
        if (item.curesBlind)
        {
            battleController.characterSelected.GetComponent<PlayerController>().statuses.Remove("Blind");
        }
        if (item.curesBleed)
        {
            battleController.characterSelected.GetComponent<PlayerController>().statuses.Remove("Bleed");
        }
        if (item.curesRooted)
        {
            battleController.characterSelected.GetComponent<PlayerController>().statuses.Remove("Rooted");
        }

        //TODO: Update UI to remove status ailment icons

        //Decrement item quantity
        battleController.characterSelected.GetComponent<PlayerController>().inventory[itemIndex].currentQuantity--;
        if (battleController.characterSelected.GetComponent<PlayerController>().inventory[itemIndex].currentQuantity == 0)
        {
            battleController.characterSelected.GetComponent<PlayerController>().inventory.RemoveAt(itemIndex);
        }

        //End turn
        disableConfirmBox();
        disableInventoryMenu();
        battleController.characterSelected.GetComponent<PlayerController>().endTurn();

    }
}
