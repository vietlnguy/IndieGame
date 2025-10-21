using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class InventoryMenu : MonoBehaviour
{

    public GameObject inventoryGiverMenu;
    public GameObject inventoryRecipientMenu;
    public bool active = false;
    public int index = 0;
    private int leftRightIndex = 0;
    private int confirmBoxIndex = 0;
    public BattleController battleController;
    public CharacterMenu characterMenuScript;
    public GameObject selector;
    public GameObject confirmBoxSelector;
    public AudioSource selectorAudio;
    public AudioSource deselectAudio;
    public GameObject items;
    public GameObject recipientItems;
    public TextMeshProUGUI previewPlayerHp;
    public TextMeshProUGUI previewPlayerMaxHp;
    public TextMeshProUGUI previewPlayerMana;
    public TextMeshProUGUI previewPlayerMaxMana;
    public GameObject previewPlayerHpBar;
    public GameObject previewPlayerManaBar;
    public TextMeshProUGUI recipientPreviewPlayerHp;
    public TextMeshProUGUI recipientPreviewPlayerMaxHp;
    public TextMeshProUGUI recipientPreviewPlayerMana;
    public TextMeshProUGUI recipientPreviewPlayerMaxMana;
    public GameObject recipientPreviewPlayerHpBar;
    public GameObject recipientPreviewPlayerManaBar;
    private Vector2 originalHpManaBarSize;
    public TextMeshProUGUI characterTitle;
    public TextMeshProUGUI recipientCharacterTitle;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI recipientItemDescriptionText;
    public GameObject confirmBox;
    private bool confirmBoxActive = false;
    private bool coroutineRunning = false;
    public GameObject arrows;
    public GameObject confirmButton;
    private bool trading = false;
    private Item itemToGive;
    private int itemToGiveIndex;
    private List<Item> originalGiverInventory;
    private List<Item> originalRecipientInventory;
    private Coroutine flashCoroutine;
    private TextMeshProUGUI flashingText;
    private TextMeshProUGUI flashingText2;
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
                else if (trading)
                {
                    resetInventories();
                    disableRecipientMenu();
                    disableGiverMenu();
                    confirmButton.GetComponent<ConfirmTradeButton>().disableButton();
                    confirmButton.SetActive(false);
                    arrows.SetActive(false);
                    trading = false;
                    itemToGive = null;
                    originalGiverInventory = null;
                    originalRecipientInventory = null;
                    try
                    {
                        StopFlashing();
                    }
                    catch
                    {
                    }

                }
                else
                {
                    disableGiverMenu();
                }
                
                resetSelectorPosition();
            }

            else if (!trading)
            {
                //Move selector
                if (Input.GetKeyDown(KeyCode.W) && !confirmBoxActive)
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
                else if (Input.GetKeyDown(KeyCode.W) && confirmBoxActive && !coroutineRunning)
                {
                    if (confirmBoxIndex != 0)
                    {
                        moveConfirmBoxSelectorUp();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.S) && confirmBoxActive && !coroutineRunning)
                {
                    if (confirmBoxIndex != 1)
                    {
                        moveConfirmBoxSelectorDown();
                    }
                }

                //Make item selection
                else if (Input.GetKeyDown(KeyCode.Space) && !confirmBoxActive && !battleController.disabledCharacters.Contains(battleController.characterSelected))
                {
                    try
                    {
                        if (battleController.characterSelected.GetComponent<PlayerController>().inventory[index] == null) ;
                        confirmBox.SetActive(true);
                        confirmBoxActive = true;
                    }
                    catch
                    {
                    }
                }

                //Confirm item use
                else if (Input.GetKeyDown(KeyCode.Space) && confirmBoxActive)
                {
                    if (confirmBoxIndex == 0)
                    {
                        StartCoroutine(useItem());
                    }
                    else if (confirmBoxIndex == 1)
                    {
                        resetConfirmBoxSelector();
                        confirmBox.SetActive(false);
                        confirmBoxActive = false;
                    }
                }
            }
            else if (trading)
            {
                //Move selector
                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (index != 0)
                    {
                        index--;
                        moveSelectorUp();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    if (index != 4)
                    {
                        index++;
                        moveSelectorDown();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.A) && itemToGive == null)
                {
                    if (leftRightIndex != 0)
                    {
                        leftRightIndex--;
                        moveSelectorLeft();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.D) && itemToGive == null)
                {
                    if (leftRightIndex != 1)
                    {
                        leftRightIndex++;
                        moveSelectorRight();
                    }

                }

                //Make item selection
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    //Item to give not chosen yet
                    if (itemToGive == null)
                    {
                        try
                        {
                            if (leftRightIndex == 0)
                            {
                                if (battleController.characterSelected.GetComponent<PlayerController>().inventory[index] == null) ;
                                itemToGive = battleController.characterSelected.GetComponent<PlayerController>().inventory[index];
                                itemToGiveIndex = index;

                                //Flash item selection
                                StartFlashing(items.transform.GetChild(itemToGiveIndex).gameObject.GetComponent<TextMeshProUGUI>(), items.transform.GetChild(itemToGiveIndex).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>(), 1.5f);


                                leftRightIndex++;
                                moveSelectorRight();
                            }
                            else if (leftRightIndex == 1)
                            {
                                if (battleController.assistableCharacterSelected.GetComponent<PlayerController>().inventory[index] == null) ;
                                itemToGive = battleController.assistableCharacterSelected.GetComponent<PlayerController>().inventory[index];
                                itemToGiveIndex = index;

                                //Flash item selection
                                StartFlashing(recipientItems.transform.GetChild(itemToGiveIndex).gameObject.GetComponent<TextMeshProUGUI>(), recipientItems.transform.GetChild(itemToGiveIndex).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>(), 1.5f);

                                leftRightIndex--;
                                moveSelectorLeft();
                            }
                        }

                        //Empty row was selected
                        catch
                        {
                        }
                    }

                    //Item to give already chosen. Should move selector to opposite side and wait selection
                    else if (itemToGive != null)
                    {
                        try
                        {
                            //Make the trade
                            if (leftRightIndex == 0)
                            {
                                if (battleController.characterSelected.GetComponent<PlayerController>().inventory[index] == null) ;
                                Item itemToRecieve = battleController.characterSelected.GetComponent<PlayerController>().inventory[index];
                                battleController.characterSelected.GetComponent<PlayerController>().inventory[index] = itemToGive;
                                battleController.assistableCharacterSelected.GetComponent<PlayerController>().inventory[itemToGiveIndex] = itemToRecieve;
                            }
                            else if (leftRightIndex == 1)
                            {
                                if (battleController.assistableCharacterSelected.GetComponent<PlayerController>().inventory[index] == null) ;
                                Item itemToRecieve = battleController.assistableCharacterSelected.GetComponent<PlayerController>().inventory[index];
                                battleController.assistableCharacterSelected.GetComponent<PlayerController>().inventory[index] = itemToGive;
                                battleController.characterSelected.GetComponent<PlayerController>().inventory[itemToGiveIndex] = itemToRecieve;
                            }
                        }

                        //Empty row was selected. Should give item to recipient
                        catch
                        {
                            if (leftRightIndex == 0)
                            {
                                battleController.characterSelected.GetComponent<PlayerController>().inventory.Add(itemToGive);
                                battleController.assistableCharacterSelected.GetComponent<PlayerController>().inventory.RemoveAt(itemToGiveIndex);
                            }
                            else if (leftRightIndex == 1)
                            {
                                battleController.assistableCharacterSelected.GetComponent<PlayerController>().inventory.Add(itemToGive);
                                battleController.characterSelected.GetComponent<PlayerController>().inventory.RemoveAt(itemToGiveIndex);
                            }
                        }

                        StopFlashing();
                        itemToGive = null;
                        updateItemList();
                        confirmButton.GetComponent<ConfirmTradeButton>().enableButton();
                    }
                }
            }
        }
    }
    public void enableInventoryGiverMenu(GameObject character)
    {
        selector.SetActive(true);
        inventoryGiverMenu.SetActive(true);
        active = true;
        int tempIndex = 0;
        characterMenuScript.disableCharacterMenu();

        //Update titles
        characterTitle.text = character.GetComponent<PlayerController>().title;

        //Update health bars and values
        previewPlayerHp.text = character.GetComponent<PlayerController>().hp.ToString();
        previewPlayerMaxHp.text = character.GetComponent<PlayerController>().maxHp.ToString();
        previewPlayerMana.text = character.GetComponent<PlayerController>().mana.ToString();
        previewPlayerMaxMana.text = character.GetComponent<PlayerController>().maxMana.ToString();

        previewPlayerHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)character.GetComponent<PlayerController>().hp / character.GetComponent<PlayerController>().maxHp, 1f);
        previewPlayerManaBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)character.GetComponent<PlayerController>().mana / character.GetComponent<PlayerController>().maxMana, 1f);

        //Populate inventory
        foreach (Transform row in items.transform)
        {
            try
            {
                row.gameObject.GetComponent<TextMeshProUGUI>().text = character.GetComponent<PlayerController>().inventory[tempIndex].name;
                row.GetChild(0).GetComponent<TextMeshProUGUI>().text = character.GetComponent<PlayerController>().inventory[tempIndex].currentQuantity.ToString() + "/" + character.GetComponent<PlayerController>().inventory[tempIndex].maxQuantity.ToString();
            }
            catch
            {
                //Nothing
            }
            tempIndex++;
        }

        updateDescription(character);
    }
    public void enableInventoryRecipientMenu(GameObject character)
    {
        inventoryRecipientMenu.SetActive(true);
        int tempIndex = 0;

        //Update titles
        recipientCharacterTitle.text = character.GetComponent<PlayerController>().title;

        //Update health bars and values
        recipientPreviewPlayerHp.text = character.GetComponent<PlayerController>().hp.ToString();
        recipientPreviewPlayerMaxHp.text = character.GetComponent<PlayerController>().maxHp.ToString();
        recipientPreviewPlayerMana.text = character.GetComponent<PlayerController>().mana.ToString();
        recipientPreviewPlayerMaxMana.text = character.GetComponent<PlayerController>().maxMana.ToString();

        recipientPreviewPlayerHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)character.GetComponent<PlayerController>().hp / character.GetComponent<PlayerController>().maxHp, 1f);
        recipientPreviewPlayerManaBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)character.GetComponent<PlayerController>().mana / character.GetComponent<PlayerController>().maxMana, 1f);

        //Populate inventory
        foreach (Transform row in recipientItems.transform)
        {
            try
            {
                row.gameObject.GetComponent<TextMeshProUGUI>().text = character.GetComponent<PlayerController>().inventory[tempIndex].name;
                row.GetChild(0).GetComponent<TextMeshProUGUI>().text = character.GetComponent<PlayerController>().inventory[tempIndex].currentQuantity.ToString() + "/" + character.GetComponent<PlayerController>().inventory[tempIndex].maxQuantity.ToString();
            }
            catch
            {
            }
            tempIndex++;
        }

        updateRecipientDescription(character);

    }
    public void disableGiverMenu()
    {
        selector.SetActive(false);
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
    public void disableRecipientMenu() {
        foreach (Transform row in recipientItems.transform)
        {
            row.gameObject.GetComponent<TextMeshProUGUI>().text = "-";
            row.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-";
        }
        recipientItemDescriptionText.text = "-";

        //Reset health and mana bars
        recipientPreviewPlayerHpBar.GetComponent<RectTransform>().sizeDelta = originalHpManaBarSize;
        recipientPreviewPlayerManaBar.GetComponent<RectTransform>().sizeDelta = originalHpManaBarSize;

        inventoryRecipientMenu.SetActive(false);

    }
    public void disableConfirmBox()
    {
        confirmBox.SetActive(false);
        confirmBoxActive = false;
        resetConfirmBoxSelector();
    }
    public void enableTradingMenu()
    {
        enableInventoryGiverMenu(battleController.characterSelected);
        enableInventoryRecipientMenu(battleController.assistableCharacterSelected);
        storeOriginalInventories();
        arrows.SetActive(true);
        confirmButton.SetActive(true);
        trading = true;
    }
    private void resetInventories()
    {
        battleController.characterSelected.GetComponent<PlayerController>().inventory = originalGiverInventory.Select(item => new Item(item)).ToList();
        battleController.assistableCharacterSelected.GetComponent<PlayerController>().inventory = originalRecipientInventory.Select(item => new Item(item)).ToList();
        originalGiverInventory = null;
        originalRecipientInventory = null;
    }
    private void resetSelectorPosition()
    {
        if (leftRightIndex != 0)
        {
            moveSelectorLeft();
            leftRightIndex--;
        }
        while (index != 0)
        {
            moveSelectorUp();
            index--;
        }
    }
    private void storeOriginalInventories()
    {
        originalGiverInventory = battleController.characterSelected.GetComponent<PlayerController>().inventory
            .Select(item => new Item(item))
            .ToList();
        originalRecipientInventory = battleController.assistableCharacterSelected.GetComponent<PlayerController>().inventory
            .Select(item => new Item(item))
            .ToList();
    }
    private void moveSelectorDown()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y -= 51f;
        rt.anchoredPosition = anchoredPos;
        if (leftRightIndex == 0)
        {
            updateDescription(battleController.characterSelected);
        }
        else
        {
            updateRecipientDescription(battleController.assistableCharacterSelected);
        }

    }
    private void moveSelectorUp()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y += 51f;
        rt.anchoredPosition = anchoredPos;
        if (leftRightIndex == 0)
        {
            updateDescription(battleController.characterSelected);
        }
        else
        {
            updateRecipientDescription(battleController.assistableCharacterSelected);
        }
    }
    private void moveSelectorRight()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.x += 736f;
        rt.anchoredPosition = anchoredPos;
        if (leftRightIndex == 0)
        {
            updateDescription(battleController.characterSelected);
        }
        else
        {
            updateRecipientDescription(battleController.assistableCharacterSelected);
        }
    }
    private void moveSelectorLeft()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.x -= 736f;
        rt.anchoredPosition = anchoredPos;
        if (leftRightIndex == 0)
        {
            updateDescription(battleController.characterSelected);
        }
        else
        {
            updateRecipientDescription(battleController.assistableCharacterSelected);
        }
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
    private void updateDescription(GameObject character)
    {
        try
        {
            itemDescriptionText.text = character.GetComponent<PlayerController>().inventory[index].description;
        }
        catch
        {
            itemDescriptionText.text = "-";
        }
    }
    private void updateRecipientDescription(GameObject character)
    {
        try
        {
            recipientItemDescriptionText.text = character.GetComponent<PlayerController>().inventory[index].description;
        }
        catch
        {
            recipientItemDescriptionText.text = "-";
        }
    }
    private void resetConfirmBoxSelector()
    {
        RectTransform rect = confirmBoxSelector.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(.89f, 15f);
        confirmBoxIndex = 0;
    }
    private IEnumerator useItem()
    {
        Item item = battleController.characterSelected.GetComponent<PlayerController>().inventory[index];

        //Restore HP or Mana
        if (item.hpOrMana == "hp")
        {
            int tempStartNumber = battleController.characterSelected.GetComponent<PlayerController>().hp;
            int tempEndNumber;
            if (battleController.characterSelected.GetComponent<PlayerController>().hp + item.restorationAmount > battleController.characterSelected.GetComponent<PlayerController>().maxHp)
            {
                battleController.characterSelected.GetComponent<PlayerController>().hp = battleController.characterSelected.GetComponent<PlayerController>().maxHp;
                tempEndNumber = battleController.characterSelected.GetComponent<PlayerController>().maxHp;
            }
            else
            {
                battleController.characterSelected.GetComponent<PlayerController>().hp += item.restorationAmount;
                tempEndNumber = battleController.characterSelected.GetComponent<PlayerController>().hp;
            }
            yield return StartCoroutine(animateHealthOrManaBars("hp", tempStartNumber, tempEndNumber));
        }
        else if (item.hpOrMana == "mana")
        {
            int tempStartNumber = battleController.characterSelected.GetComponent<PlayerController>().mana;
            int tempEndNumber;
            if (battleController.characterSelected.GetComponent<PlayerController>().mana + item.restorationAmount > battleController.characterSelected.GetComponent<PlayerController>().maxMana)
            {
                battleController.characterSelected.GetComponent<PlayerController>().mana = battleController.characterSelected.GetComponent<PlayerController>().maxMana;
                tempEndNumber = battleController.characterSelected.GetComponent<PlayerController>().maxMana;
            }
            else
            {
                battleController.characterSelected.GetComponent<PlayerController>().mana += item.restorationAmount;
                tempEndNumber = battleController.characterSelected.GetComponent<PlayerController>().mana;
            }
            yield return StartCoroutine(animateHealthOrManaBars("mana", tempStartNumber, tempEndNumber));
        }

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
        battleController.characterSelected.GetComponent<PlayerController>().inventory[index].currentQuantity--;
        if (battleController.characterSelected.GetComponent<PlayerController>().inventory[index].currentQuantity == 0)
        {
            battleController.characterSelected.GetComponent<PlayerController>().inventory.RemoveAt(index);
        }

        //End turn
        disableConfirmBox();
        disableGiverMenu();
        battleController.characterSelected.GetComponent<PlayerController>().endTurn();

    }
    private IEnumerator animateHealthOrManaBars(string type, int startNumber, int endNumber)
    {
        coroutineRunning = true;
        if (type == "hp")
        {
            // Initial size and text
            RectTransform rect = previewPlayerHpBar.GetComponent<RectTransform>();
            Vector2 initialSize = rect.sizeDelta;

            // Calculate target size
            PlayerController player = battleController.characterSelected.GetComponent<PlayerController>();
            Vector2 targetSize = originalHpManaBarSize * new Vector2((float)player.hp / player.maxHp, 1f);

            // Animation duration
            float duration = 1.5f;
            float elapsed = 0f;

            // Smoothly interpolate between the initial and target sizes
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                // Optionally smooth interpolation
                float smoothT = Mathf.SmoothStep(0f, 1f, t);

                // Update bar size
                rect.sizeDelta = Vector2.Lerp(initialSize, targetSize, smoothT);

                // Update text
                int currentHp = Mathf.RoundToInt(Mathf.Lerp(startNumber, endNumber, smoothT));
                previewPlayerHp.text = currentHp.ToString();

                yield return null;
            }

            // Ensure final size is exact
            rect.sizeDelta = targetSize;
            previewPlayerHp.text = endNumber.ToString();
        }
        else if (type == "mana")
        {
            // Initial size
            RectTransform rect = previewPlayerManaBar.GetComponent<RectTransform>();
            Vector2 initialSize = rect.sizeDelta;

            // Calculate target size
            PlayerController player = battleController.characterSelected.GetComponent<PlayerController>();
            Vector2 targetSize = originalHpManaBarSize * new Vector2((float)player.mana / player.maxMana, 1f);

            // Animation duration
            float duration = 1.5f;
            float elapsed = 0f;

            // Smoothly interpolate between the initial and target sizes
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                // Optionally smooth interpolation
                float smoothT = Mathf.SmoothStep(0f, 1f, t);

                // Update bar size
                rect.sizeDelta = Vector2.Lerp(initialSize, targetSize, smoothT);

                // Update text
                int currentMana = Mathf.RoundToInt(Mathf.Lerp(startNumber, endNumber, smoothT));
                previewPlayerMana.text = currentMana.ToString();

                yield return null;
            }

            // Ensure final size is exact
            rect.sizeDelta = targetSize;
            previewPlayerMana.text = endNumber.ToString();
        }

        yield return new WaitForSeconds(1f);
        coroutineRunning = false;
    }
    private void updateItemList()
    {
        int tempIndex = 0;

        //Populate giver inventory
        foreach (Transform row in items.transform)
        {
            try
            {
                row.gameObject.GetComponent<TextMeshProUGUI>().text = battleController.characterSelected.GetComponent<PlayerController>().inventory[tempIndex].name;
                row.GetChild(0).GetComponent<TextMeshProUGUI>().text = battleController.characterSelected.GetComponent<PlayerController>().inventory[tempIndex].currentQuantity.ToString() + "/" + battleController.characterSelected.GetComponent<PlayerController>().inventory[tempIndex].maxQuantity.ToString();
            }
            catch
            {
                row.gameObject.GetComponent<TextMeshProUGUI>().text = "-";
                row.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-";
            }
            tempIndex++;
        }

        tempIndex = 0;
        //Populate inventory
        foreach (Transform row in recipientItems.transform)
        {
            try
            {
                row.gameObject.GetComponent<TextMeshProUGUI>().text = battleController.assistableCharacterSelected.GetComponent<PlayerController>().inventory[tempIndex].name;
                row.GetChild(0).GetComponent<TextMeshProUGUI>().text = battleController.assistableCharacterSelected.GetComponent<PlayerController>().inventory[tempIndex].currentQuantity.ToString() + "/" + battleController.assistableCharacterSelected.GetComponent<PlayerController>().inventory[tempIndex].maxQuantity.ToString();
            }
            catch
            {
                row.gameObject.GetComponent<TextMeshProUGUI>().text = "-";
                row.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-";
            }
            tempIndex++;
        }


    }
    public void confirmTrade()
    {
        disableRecipientMenu();
        disableGiverMenu();
        resetSelectorPosition();
        confirmButton.GetComponent<ConfirmTradeButton>().disableButton();
        confirmButton.SetActive(false);
        arrows.SetActive(false);
        trading = false;
        itemToGive = null;
        originalGiverInventory = null;
        originalRecipientInventory = null;
    }
    public void StartFlashing(TextMeshProUGUI textComponent, TextMeshProUGUI textComponent2, float duration = 1.5f)
    {
        flashCoroutine = StartCoroutine(FlashCoroutine(textComponent, textComponent2, duration));
        flashingText = textComponent;
        flashingText2 = textComponent2;
    }
    public void StopFlashing()
    {
        StopCoroutine(flashCoroutine);

        flashingText.color = Color.white;
        flashingText2.color = Color.white;
    }
    private IEnumerator FlashCoroutine(TextMeshProUGUI textComponent, TextMeshProUGUI textComponent2, float duration)
    {
        float halfDuration = duration / 2f;

        while (true) // keeps flashing
        {
            float elapsed = 0f;

            // White → Red
            while (elapsed < halfDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / halfDuration);
                textComponent.color = Color.Lerp(Color.white, Color.red, t);
                textComponent2.color = Color.Lerp(Color.white, Color.red, t);
                yield return null;
            }

            elapsed = 0f;

            // Red → White
            while (elapsed < halfDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / halfDuration);
                textComponent.color = Color.Lerp(Color.red, Color.white, t);
                textComponent2.color = Color.Lerp(Color.red, Color.white, t);
                yield return null;
            }
        }
    }


}
