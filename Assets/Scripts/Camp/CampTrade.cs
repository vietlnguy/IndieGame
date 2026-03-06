using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine.UI;
public class CampTrade : MonoBehaviour
{
    private SaveManager scm;
    public GameObject inventoryGiverMenu;
    public GameObject inventoryRecipientMenu;
    public bool active = false;
    public int index = 0;
    private int supplyTopIndex = 0;
    private int supplyBotIndex = 8;
    private int leftRightIndex = 0;
    private int confirmBoxIndex = 0;
    private CampController campControllerScript;
    private CampAssistMenu campAssistMenuScript;
    public GameObject selector;
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
    private bool tradingWithSupply = false;
    private bool tradingWithOthers = false;
    public GameObject arrows;
    public GameObject confirmButton;
    public Item itemToGive;
    private int itemToGiveIndex;
    private List<Item> originalGiverInventory;
    private List<Item> originalRecipientInventory;
    private Coroutine flashCoroutine;
    private TextMeshProUGUI flashingText;
    private TextMeshProUGUI flashingText2;
    public AudioSource potionAudio;
    public GameObject supplyItemPrefab;
    public GameObject supplyContent;
    public GameObject supplyWindow;
    public ScrollRect supplyScrollRect;
    void Awake()
    {
        scm = FindFirstObjectByType<SaveManager>();
        campAssistMenuScript = FindFirstObjectByType<CampAssistMenu>();
        campControllerScript = FindFirstObjectByType<CampController>();
        originalHpManaBarSize = previewPlayerHpBar.GetComponent<RectTransform>().sizeDelta;
        itemToGive = null;
    }
    void Update()
    {
        if (active)
        { 
            if (tradingWithOthers) {
                //Close menu
                if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Q))
                {
                    active = false;
                    resetInventories();
                    disableRecipientMenu();
                    disableGiverMenu(false);
                    confirmButton.GetComponent<CampConfirmTrade>().disableButton();
                    confirmButton.SetActive(false);
                    arrows.SetActive(false);
                    itemToGive = null;
                    originalGiverInventory = null;
                    originalRecipientInventory = null;
                    campAssistMenuScript.active = true;
                    try
                    {
                        StopFlashing();
                    }
                    catch
                    {
                        //Intentional
                    }
                    resetSelectorPosition();
                    tradingWithOthers = false;
                }
                
                else
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
                                    //if (campControllerScript.mainCharacterObj.GetComponent<CampPlayerController>().inventory[index] == null) ;
                                    itemToGive = campControllerScript.mainCharacterObj.GetComponent<CampPlayerController>().inventory[index];
                                    itemToGiveIndex = index;

                                    //Flash item selection
                                    StartFlashing(items.transform.GetChild(itemToGiveIndex).gameObject.GetComponent<TextMeshProUGUI>(), items.transform.GetChild(itemToGiveIndex).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>(), 1.5f);

                                    leftRightIndex++;
                                    moveSelectorRight();
                                }
                                else if (leftRightIndex == 1)
                                {
                                    //if (campControllerScript.mainCharacterObj.GetComponent<CampPlayerController>().inventory[index] == null) ;
                                    itemToGive = campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().inventory[index];
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
                                //Intentional
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
                                    //if (campControllerScript.mainCharacterObj.GetComponent<CampPlayerController>().inventory[index] == null) ;
                                    Item itemToRecieve = campControllerScript.mainCharacterObj.GetComponent<CampPlayerController>().inventory[index];
                                    
                                    campControllerScript.mainCharacterObj.GetComponent<CampPlayerController>().inventory[index] = itemToGive;
                                    campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().inventory[itemToGiveIndex] = itemToRecieve;
                                }
                                else if (leftRightIndex == 1)
                                {
                                    //if (campControllerScript.mainCharacterObj.GetComponent<CampPlayerController>().inventory[index] == null) ;
                                    Item itemToRecieve = campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().inventory[index];
                                    campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().inventory[index] = itemToGive;
                                    campControllerScript.mainCharacterObj.GetComponent<CampPlayerController>().inventory[itemToGiveIndex] = itemToRecieve;
                                }
                            }

                            //Empty row was selected. Should give item to recipient
                            catch
                            {
                                if (leftRightIndex == 0)
                                {
                                    campControllerScript.mainCharacterObj.GetComponent<CampPlayerController>().inventory.Add(itemToGive);
                                    campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().inventory.RemoveAt(itemToGiveIndex);
                                }
                                else if (leftRightIndex == 1)
                                {
                                    campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().inventory.Add(itemToGive);
                                    campControllerScript.mainCharacterObj.GetComponent<CampPlayerController>().inventory.RemoveAt(itemToGiveIndex);
                                }
                            }

                            StopFlashing();
                            itemToGive = null;
                            updateItemList();
                            confirmButton.GetComponent<CampConfirmTrade>().enableButton();
                        }
                    }
                
                }
            }
        
            else if (tradingWithSupply)
            {
                //Move selector
                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (leftRightIndex == 0)
                    {
                        if (index != 0)
                        {
                            index--;
                            moveSelectorUp();
                        }
                    }
                    else if (leftRightIndex == 1)
                    {
                        if (index == supplyTopIndex && index != 0)
                        {
                            moveSupplyContentWindowUp();
                            index--;
                        }
                        
                        else if (index != 0)
                        {
                            moveSupplySelectorUp();
                            index--;
                        }
                        
                    }
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    if (leftRightIndex == 0)
                    {
                        if (index != 4)
                        {
                            index++;
                            moveSelectorDown();
                        }
                    }
                    else if (leftRightIndex == 1)
                    {
                        if (index == supplyBotIndex && index != scm.loadedData.supplyItems.Count)
                        {
                            moveSupplyContentWindowDown();
                            index++;
                        }
                        
                        else if (index != scm.loadedData.supplyItems.Count)
                        {
                            moveSupplySelectorDown();
                            index++;
                        }
                        
                    }
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    if (leftRightIndex != 0)
                    {
                        resetSelectorPosition();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    if (leftRightIndex != 1)
                    {
                        resetSupplySelector();
                    }

                }

                //Make item selection
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (leftRightIndex == 0)
                    {
                        try
                        {
                            scm.loadedData.supplyItems.Add(campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().inventory[index]);
                            campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().inventory.RemoveAt(index);
                            updateItemList();
                        }
                        catch
                        {
                           //Should do nothing. Because trying to trade an empty row 
                        }
                    }
                }
            
            }
        
        }

        
    }
    public IEnumerator enableInventoryGiverMenu(GameObject character)
    {
        yield return null;

        selector.SetActive(true);
        inventoryGiverMenu.SetActive(true);
        int tempIndex = 0;

        //Update titles
        characterTitle.text = character.GetComponent<CampPlayerController>().title;

        //Update health bars and values
        previewPlayerHp.text = character.GetComponent<CampPlayerController>().currentHp.ToString();
        previewPlayerMaxHp.text = character.GetComponent<CampPlayerController>().maxHp.ToString();
        previewPlayerMana.text = character.GetComponent<CampPlayerController>().currentMana.ToString();
        previewPlayerMaxMana.text = character.GetComponent<CampPlayerController>().maxMana.ToString();

        previewPlayerHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)character.GetComponent<CampPlayerController>().currentHp / character.GetComponent<CampPlayerController>().maxHp, 1f);
        previewPlayerManaBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)character.GetComponent<CampPlayerController>().currentMana / character.GetComponent<CampPlayerController>().maxMana, 1f);

        //Populate inventory
        foreach (Transform row in items.transform)
        {
            try
            {
                row.gameObject.GetComponent<TextMeshProUGUI>().text = character.GetComponent<CampPlayerController>().inventory[tempIndex].name;
                row.GetChild(0).GetComponent<TextMeshProUGUI>().text = character.GetComponent<CampPlayerController>().inventory[tempIndex].currentQuantity.ToString() + "/" + character.GetComponent<CampPlayerController>().inventory[tempIndex].maxQuantity.ToString();
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
        recipientCharacterTitle.text = character.GetComponent<CampPlayerController>().title;

        //Update health bars and values
        recipientPreviewPlayerHp.text = character.GetComponent<CampPlayerController>().currentHp.ToString();
        recipientPreviewPlayerMaxHp.text = character.GetComponent<CampPlayerController>().maxHp.ToString();
        recipientPreviewPlayerMana.text = character.GetComponent<CampPlayerController>().currentMana.ToString();
        recipientPreviewPlayerMaxMana.text = character.GetComponent<CampPlayerController>().maxMana.ToString();

        recipientPreviewPlayerHpBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)character.GetComponent<CampPlayerController>().currentHp / character.GetComponent<CampPlayerController>().maxHp, 1f);
        recipientPreviewPlayerManaBar.GetComponent<RectTransform>().sizeDelta *= new Vector2((float)character.GetComponent<CampPlayerController>().currentMana / character.GetComponent<CampPlayerController>().maxMana, 1f);

        //Populate inventory
        foreach (Transform row in recipientItems.transform)
        {
            try
            {
                row.gameObject.GetComponent<TextMeshProUGUI>().text = character.GetComponent<CampPlayerController>().inventory[tempIndex].name;
                row.GetChild(0).GetComponent<TextMeshProUGUI>().text = character.GetComponent<CampPlayerController>().inventory[tempIndex].currentQuantity.ToString() + "/" + character.GetComponent<CampPlayerController>().inventory[tempIndex].maxQuantity.ToString();
            }
            catch
            {
                //Intentional
            }
            tempIndex++;
        }

        updateRecipientDescription(character);

    }
    public void disableGiverMenu(bool characterMenuWasActive)
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
        if (!characterMenuWasActive)
        {

        }

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
    public void enableTradingMenu(GameObject character)
    {
        StartCoroutine(enableInventoryGiverMenu(campControllerScript.mainCharacterObj));
        enableInventoryRecipientMenu(character);
        storeOriginalInventories(character);
        arrows.SetActive(true);
        confirmButton.SetActive(true);
        active = true;
        tradingWithOthers = true;

    }
    private void resetInventories()
    {
        campControllerScript.mainCharacterObj.GetComponent<CampPlayerController>().inventory = originalGiverInventory.Select(item => new Item(item)).ToList();
        campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().inventory = originalRecipientInventory.Select(item => new Item(item)).ToList();
        originalGiverInventory = null;
        originalRecipientInventory = null;
    }
    private void resetSelectorPosition()
    {
        if (tradingWithOthers) {
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
        else if (tradingWithSupply)
        {
            selectorAudio.Play();
            selector.GetComponent<RectTransform>().anchoredPosition = new Vector2(-382f, 9f);
            index = 0;
            leftRightIndex = 0;
        }
    }
    private void resetSupplySelector()
    {
        selectorAudio.Play();
        selector.GetComponent<RectTransform>().anchoredPosition = new Vector2(421f, 95f);
        index = supplyTopIndex;
        leftRightIndex = 1;
    }
    private void storeOriginalInventories(GameObject character)
    {
        originalGiverInventory = campControllerScript.mainCharacterObj.GetComponent<CampPlayerController>().inventory
            .Select(item => new Item(item))
            .ToList();

        originalRecipientInventory = character.GetComponent<CampPlayerController>().inventory
            .Select(item => new Item(item))
            .ToList();
    }
    private void moveSupplySelectorDown()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y -= 51f;
        rt.anchoredPosition = anchoredPos;
        if (leftRightIndex == 0)
        {
            updateDescription(campControllerScript.mainCharacterObj);
        }
        else
        {
            updateRecipientDescription(campControllerScript.mainCharacterObj);
        }

    }
    private void moveSupplySelectorUp()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y += 51f;
        rt.anchoredPosition = anchoredPos;
        if (leftRightIndex == 0)
        {
            updateDescription(campControllerScript.mainCharacterObj);
        }
        else
        {
            updateRecipientDescription(campControllerScript.mainCharacterObj);
        }
    }
    private void moveSupplyContentWindowUp()
    {
        selectorAudio.Play();
        RectTransform temp = supplyContent.GetComponent<RectTransform>();
        temp.anchoredPosition += new Vector2(0f, -25f);
        supplyBotIndex--;
        supplyTopIndex--;
    }
    private void moveSupplyContentWindowDown()
    {
        selectorAudio.Play();
        RectTransform temp = supplyContent.GetComponent<RectTransform>();
        temp.anchoredPosition += new Vector2(0f, 25f);
        supplyBotIndex++;
        supplyTopIndex++;
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
            updateDescription(campControllerScript.mainCharacterObj);
        }
        else
        {
            updateRecipientDescription(campControllerScript.mainCharacterObj);
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
            updateDescription(campControllerScript.mainCharacterObj);
        }
        else
        {
            updateRecipientDescription(campControllerScript.mainCharacterObj);
        }
    }
    private void moveSelectorRight()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.x += 786f;
        rt.anchoredPosition = anchoredPos;
        if (leftRightIndex == 0)
        {
            updateDescription(campControllerScript.mainCharacterObj);
        }
        else
        {
            updateRecipientDescription(campAssistMenuScript.characterSelected);
        }
    }
    private void moveSelectorLeft()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.x -= 786f;
        rt.anchoredPosition = anchoredPos;
        if (leftRightIndex == 0)
        {
            updateDescription(campControllerScript.mainCharacterObj);
        }
        else
        {
            updateRecipientDescription(campAssistMenuScript.characterSelected);
        }
    }
    private void updateDescription(GameObject character)
    {
        try
        {
            itemDescriptionText.text = character.GetComponent<CampPlayerController>().inventory[index].description;
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
            recipientItemDescriptionText.text = character.GetComponent<CampPlayerController>().inventory[index].description;
        }
        catch
        {
            recipientItemDescriptionText.text = "-";
        }
    }
    private void updateItemList()
    {
        int tempIndex = 0;

        if (tradingWithOthers)
        {
            //Populate giver inventory
            foreach (Transform row in items.transform)
            {
                try
                {
                    row.gameObject.GetComponent<TextMeshProUGUI>().text = campControllerScript.mainCharacterObj.GetComponent<CampPlayerController>().inventory[tempIndex].name;
                    row.GetChild(0).GetComponent<TextMeshProUGUI>().text = campControllerScript.mainCharacterObj.GetComponent<CampPlayerController>().inventory[tempIndex].currentQuantity.ToString() + "/" + campControllerScript.mainCharacterObj.GetComponent<CampPlayerController>().inventory[tempIndex].maxQuantity.ToString();
                }
                catch
                {
                    row.gameObject.GetComponent<TextMeshProUGUI>().text = "-";
                    row.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-";
                }
                tempIndex++;
            }

            tempIndex = 0;
            //Populate recipient inventory
            foreach (Transform row in recipientItems.transform)
            {
                try
                {
                    row.gameObject.GetComponent<TextMeshProUGUI>().text = campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().inventory[tempIndex].name;
                    row.GetChild(0).GetComponent<TextMeshProUGUI>().text = campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().inventory[tempIndex].currentQuantity.ToString() + "/" + campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().inventory[tempIndex].maxQuantity.ToString();
                }
                catch
                {
                    row.gameObject.GetComponent<TextMeshProUGUI>().text = "-";
                    row.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-";
                }
                tempIndex++;
            }
            
        }

        else if (tradingWithSupply)
        {
            tempIndex = 0;
            //Populate recipient inventory
            foreach (Transform row in items.transform)
            {
                try
                {
                    row.gameObject.GetComponent<TextMeshProUGUI>().text = campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().inventory[tempIndex].name;
                    row.GetChild(0).GetComponent<TextMeshProUGUI>().text = campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().inventory[tempIndex].currentQuantity.ToString() + "/" + campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().inventory[tempIndex].maxQuantity.ToString();
                }
                catch
                {
                    row.gameObject.GetComponent<TextMeshProUGUI>().text = "-";
                    row.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-";
                }
                tempIndex++;
            }
            populateSupply();
        }
    }
    public void confirmTrade()
    {
        disableRecipientMenu();
        disableGiverMenu(false);
        resetSelectorPosition();
        confirmButton.GetComponent<CampConfirmTrade>().disableButton();
        confirmButton.SetActive(false);
        arrows.SetActive(false);
        itemToGive = null;
        active = false;
        campAssistMenuScript.active = true;
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
    public void enableSupplyMenu(GameObject character)
    {
        StartCoroutine(enableInventoryGiverMenu(campAssistMenuScript.characterSelected));
        enableSupplyWindow();
        //storeOriginalInventories(character);
        arrows.SetActive(true);
        confirmButton.SetActive(true);
        active = true;
        tradingWithSupply = true;

    }
    private void enableSupplyWindow()
    {   
        supplyWindow.SetActive(true);
        populateSupply();
    }
    private void populateSupply()
    {
        //Clear the content
        foreach (Transform child in supplyContent.transform)
        {
            Destroy(child.gameObject);
        }

        //Instantiate itemPrefab for each item in scm.loadedData.supplyItems
        foreach (Item item in scm.loadedData.supplyItems)
        {
            GameObject temp = Instantiate(supplyItemPrefab, supplyContent.transform, false);
            temp.GetComponent<SupplyItem>().item = item;
        }
    }

}
