using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using System;
public class CampTrade : MonoBehaviour
{
    private SaveManager scm;
    public GameObject inventoryGiverMenu;
    public GameObject inventoryRecipientMenu;
    public bool active = false;
    public int index = 0;
    public int supplyTopIndex = 0;
    public int supplyBotIndex = 8;
    private int leftRightIndex = 0;
    private int confirmBoxIndex = 0;
    private int supplySize = 40;
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
    private bool tradingWithEquipment = false;
    public GameObject arrows;
    public GameObject confirmButton;
    public Item itemToGive;
    private int itemToGiveIndex;
    private List<Item> originalGiverInventory;
    private List<Item> originalRecipientInventory;
    private List<Item> originalSupplyInventory;
    private Equipment originalWeapon;
    private Equipment originalArmor;
    private Equipment originalAccessory;
    private List<Equipment> originalSupplyEquipment;
    private Coroutine flashCoroutine;
    private TextMeshProUGUI flashingText;
    private TextMeshProUGUI flashingText2;
    public AudioSource errorAudio;
    public GameObject supplyItemPrefab;
    public GameObject supplyContent;
    public GameObject supplyWindow;
    public ScrollRect supplyScrollRect;
    public TextMeshProUGUI weaponSlot;
    public TextMeshProUGUI armorSlot;
    public TextMeshProUGUI accessorySlot;
    public TextMeshProUGUI baseAtkText;
    public TextMeshProUGUI baseIntText;
    public TextMeshProUGUI baseDefText;
    public TextMeshProUGUI baseResText;
    public TextMeshProUGUI baseSklText;
    public TextMeshProUGUI baseSpdText;
    public TextMeshProUGUI atkModText;
    public TextMeshProUGUI intModText;
    public TextMeshProUGUI defModText;
    public TextMeshProUGUI resModText;
    public TextMeshProUGUI sklModText;
    public TextMeshProUGUI spdModText;
    public TextMeshProUGUI atkMultText;
    public TextMeshProUGUI intMultText;
    public TextMeshProUGUI defMultText;
    public TextMeshProUGUI resMultText;
    public TextMeshProUGUI sklMultText;
    public TextMeshProUGUI spdMultText;
    public TextMeshProUGUI equipmentCurrentHP;
    public TextMeshProUGUI equipmentMaxHP;
    public TextMeshProUGUI equipmentCurrentMana;
    public TextMeshProUGUI equipmentMaxMana;
    public TextMeshProUGUI equipmentNamePlate;
    public TextMeshProUGUI equipmentDescription;
    public TextMeshProUGUI supplyEquipmentDescription;
    public GameObject equipmentTrade;
    public GameObject supplyEquipmentPrefab;
    public GameObject equipmentSupplyContent;
    public GameObject equipmentSelector;
    public int equipmentIndex = 0;
    public int equipmentLeftRightIndex = 0;
    public int equipmentTopIndex = 0;
    public int equipmentBotIndex = 7;
    public bool soloEquipmentOrSupply = false;

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
                //Close menu
                if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Q))
                {
                    active = false;
                    resetInventoriesSupply();
                    disableGiverMenu(false);
                    disableSupplyWindow();
                    confirmButton.GetComponent<CampConfirmTrade>().disableButton();
                    confirmButton.SetActive(false);
                    arrows.SetActive(false);
                    originalGiverInventory = null;
                    originalSupplyInventory = null;
                    campAssistMenuScript.active = true;
                    resetSelectorPosition();
                    tradingWithSupply = false;
                    supplyContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
                    index = 0;
                    supplyBotIndex = 8;
                    supplyTopIndex = 0;
                    if (soloEquipmentOrSupply)
                    {
                        campAssistMenuScript.active = false;
                        soloEquipmentOrSupply = false;
                        campControllerScript.movementEnabled = true;
                    }
                }

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
                        if (index == supplyBotIndex && index < supplySize - 1)
                        {
                            moveSupplyContentWindowDown();
                            index++;
                        }
                        
                        else if (index != supplySize - 1)
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
                            if (scm.loadedData.supplyItems.Count < supplySize)
                            {
                                scm.loadedData.supplyItems.Add(campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().inventory[index]);
                                selectorAudio.Play();
                            }
                            else
                            {
                                errorAudio.Play();
                            }
                            campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().inventory.RemoveAt(index);
                            updateItemList();
                            confirmButton.GetComponent<CampConfirmTrade>().enableButton();
                        }
                        catch
                        {
                           //Should do nothing. Because trying to trade an empty row 
                        }
                    }
                    else if (leftRightIndex == 1)
                    {
                        if (campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().inventory.Count < 5)
                        {
                            try 
                            {
                                selectorAudio.Play();
                                campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().inventory.Add(scm.loadedData.supplyItems[index]);
                                scm.loadedData.supplyItems.RemoveAt(index);
                                updateItemList();
                                confirmButton.GetComponent<CampConfirmTrade>().enableButton();
                            }
                            catch
                            {
                                //Trying to move an empty row
                            }

                        }
                        else
                        {
                           errorAudio.Play(); 
                        }
                    }
                }
            
            }
        
            else if (tradingWithEquipment)
            {
                //Close menu
                if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Q))
                {
                    active = false;
                    confirmButton.GetComponent<CampConfirmTrade>().disableButton();
                    confirmButton.SetActive(false);
                    resetOriginalEquipment();
                    equipmentTrade.SetActive(false);
                    campAssistMenuScript.active = true;
                    tradingWithEquipment = false;
                    if (soloEquipmentOrSupply)
                    {
                        campAssistMenuScript.active = false;
                        soloEquipmentOrSupply = false;
                        campControllerScript.movementEnabled = true;
                    }
                }
                

                //Move selector
                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (equipmentLeftRightIndex == 0)
                    {
                        if (equipmentIndex != 0)
                        {
                            equipmentIndex--;
                            moveEquipmentSelectorUp();
                        }
                    }
                    else if (equipmentLeftRightIndex == 1)
                    {
                        if (equipmentIndex == equipmentTopIndex && equipmentIndex != 0)
                        {
                            moveEquipmentSupplyContentWindowUp();
                            equipmentIndex--;
                        }
                        
                        else if (equipmentIndex != 0)
                        {
                            moveEquipmentSupplySelectorUp();
                            equipmentIndex--;
                        }
                        
                    }

                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    if (equipmentLeftRightIndex == 0)
                    {
                        if (equipmentIndex != 2)
                        {
                            equipmentIndex++;
                            moveEquipmentSelectorDown();
                        }
                    }
                    else if (equipmentLeftRightIndex == 1)
                    {
                        if (equipmentIndex == equipmentBotIndex && equipmentIndex < supplySize - 1)
                        {
                            moveEquipmentSupplyContentWindowDown();
                            equipmentIndex++;
                        }
                        
                        else if (equipmentIndex != supplySize - 1)
                        {
                            moveEquipmentSupplySelectorDown();
                            equipmentIndex++;
                        }
                        updateEquipmentDescription();
                        
                    }

                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    moveEquipmentSelectorLeft();
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    moveEquipmentSelectorRight();
                }

                //Make item selection
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (equipmentLeftRightIndex == 0)
                    {
                        try
                        {
                            if (scm.loadedData.supplyEquipment.Count < supplySize)
                            {
                                selectorAudio.Play();
                                if (equipmentIndex == 0)
                                {   
                                    if (campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().weaponEquiped != null)
                                    {
                                        scm.loadedData.supplyEquipment.Add(campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().weaponEquiped);
                                        campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().weaponEquiped = null;
                                    }
                                }
                                else if (equipmentIndex == 1)
                                {
                                    if (campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().armorEquiped != null)
                                    {
                                        scm.loadedData.supplyEquipment.Add(campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().armorEquiped);
                                        campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().armorEquiped = null;
                                    }
                                }
                                else if (equipmentIndex == 2)
                                {
                                    if (campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().accessoryEquiped != null)
                                    {
                                        scm.loadedData.supplyEquipment.Add(campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().accessoryEquiped);
                                        campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().accessoryEquiped = null;
                                    }
                                }
                                campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().calculateStats();
                                populateCharacterInfo(campAssistMenuScript.characterSelected);
                                populateEquipmentSupply();
                                confirmButton.GetComponent<CampConfirmTrade>().enableButton();
                            }

                            //Supply is full
                            else
                            {
                                errorAudio.Play();
                            }

                        }
                        catch
                        {
                           //Should do nothing. Because trying to trade an empty row 
                        }
                    }
                    else if (equipmentLeftRightIndex == 1)
                    {
                        try 
                        {
                            selectorAudio.Play();
                            if (scm.loadedData.supplyEquipment[equipmentIndex].type == "weapon")
                            {
                                if (campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().weaponEquiped == null)
                                {
                                    campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().weaponEquiped = scm.loadedData.supplyEquipment[equipmentIndex];                   
                                    scm.loadedData.supplyEquipment.RemoveAt(equipmentIndex);
                                }
                                else
                                {
                                    Equipment temp = campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().weaponEquiped;
                                    campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().weaponEquiped = scm.loadedData.supplyEquipment[equipmentIndex];                   
                                    scm.loadedData.supplyEquipment[equipmentIndex] = temp;
                                }
                            }
                            else if (scm.loadedData.supplyEquipment[equipmentIndex].type == "armor")
                            {
                                if (campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().armorEquiped == null)
                                {
                                    campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().armorEquiped = scm.loadedData.supplyEquipment[equipmentIndex];
                                    scm.loadedData.supplyEquipment.RemoveAt(equipmentIndex);
                                }
                                else
                                {
                                    Equipment temp = campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().armorEquiped;
                                    campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().armorEquiped = scm.loadedData.supplyEquipment[equipmentIndex];                   
                                    scm.loadedData.supplyEquipment[equipmentIndex] = temp;
                                }
                            }
                            else if (scm.loadedData.supplyEquipment[equipmentIndex].type == "accessory")
                            {
                                if (campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().accessoryEquiped == null)
                                {
                                    campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().accessoryEquiped = scm.loadedData.supplyEquipment[equipmentIndex];
                                    scm.loadedData.supplyEquipment.RemoveAt(equipmentIndex);
                                }
                                else
                                {
                                    Equipment temp = campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().accessoryEquiped;
                                    campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().accessoryEquiped = scm.loadedData.supplyEquipment[equipmentIndex];                   
                                    scm.loadedData.supplyEquipment[equipmentIndex] = temp;
                                }
                            }

                            campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().calculateStats();
                            populateCharacterInfo(campAssistMenuScript.characterSelected);
                            populateEquipmentSupply();
                            confirmButton.GetComponent<CampConfirmTrade>().enableButton();
                        }
                        catch
                        {
                            //Trying to move an empty row
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
    private void resetInventoriesSupply()
    {
        campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().inventory = originalGiverInventory.Select(item => new Item(item)).ToList();
        scm.loadedData.supplyItems = originalSupplyInventory.Select(item => new Item(item)).ToList();
        originalGiverInventory = null;
        originalSupplyInventory = null;
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
    private void storeOriginalInventoriesSupply(GameObject character)
    {
        originalGiverInventory = character.GetComponent<CampPlayerController>().inventory
            .Select(item => new Item(item))
            .ToList();

        originalSupplyInventory = scm.loadedData.supplyItems
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
            //Populate leftside inventory
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
        if (tradingWithOthers)
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
            tradingWithOthers = false;
        }
        else if (tradingWithSupply)
        {
            disableSupplyWindow();
            disableGiverMenu(false);
            resetSelectorPosition();
            confirmButton.GetComponent<CampConfirmTrade>().disableButton();
            confirmButton.SetActive(false);
            arrows.SetActive(false);
            active = false;
            campAssistMenuScript.active = true;
            originalGiverInventory = null;
            originalSupplyInventory = null;
            tradingWithSupply = false;
        }
        else if (tradingWithEquipment)
        {
            active = false;
            confirmButton.GetComponent<CampConfirmTrade>().disableButton();
            confirmButton.SetActive(false);
            originalAccessory = null;
            originalArmor = null;
            originalWeapon = null;
            originalSupplyEquipment = null;
            equipmentTrade.SetActive(false);
            campAssistMenuScript.active = true;
            tradingWithEquipment = false;
        }
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
        storeOriginalInventoriesSupply(character);
        enableSupplyWindow();
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
    private void disableSupplyWindow()
    {   
        //Clear the content
        foreach (Transform child in supplyContent.transform)
        {
            Destroy(child.gameObject);
        }
        supplyWindow.SetActive(false);
    }
    private void populateSupply()
    {
        //Clear the content
        foreach (Transform child in supplyContent.transform)
        {
            Destroy(child.gameObject);
        }

        //Instantiate items
        for (int i = 0; i < supplySize; i++)
        {
            try
            {   
                Item item = scm.loadedData.supplyItems[i];
                GameObject temp = Instantiate(supplyItemPrefab, supplyContent.transform, false);
                temp.GetComponent<SupplyItem>().item = item;
            }
            catch
            {
                GameObject temp = Instantiate(supplyItemPrefab, supplyContent.transform, false);
            }
        }
    }
    public void enableEquipmentMenu(GameObject character)
    {
        equipmentTrade.SetActive(true);
        storeOriginalEquipment(character);
        confirmButton.SetActive(true);
        active = true;
        tradingWithEquipment = true;
        equipmentIndex = 0;
        equipmentLeftRightIndex = 0;
        equipmentTopIndex = 0;
        equipmentBotIndex = 7;
        equipmentSupplyContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        resetEquipmentSelectorPosition();
        updateEquipmentDescription();

        character.GetComponent<CampPlayerController>().calculateStats();
        populateCharacterInfo(character);
        populateEquipmentSupply();
    }
    private void storeOriginalEquipment(GameObject character)
    {
        originalWeapon = character.GetComponent<CampPlayerController>().weaponEquiped;
        originalArmor = character.GetComponent<CampPlayerController>().armorEquiped;    
        originalAccessory = character.GetComponent<CampPlayerController>().accessoryEquiped;

        originalSupplyEquipment = scm.loadedData.supplyEquipment
            .Select(item => new Equipment(item))
            .ToList();
    }
    private void resetOriginalEquipment()
    {
        CampPlayerController characterScript = campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>();
        characterScript.weaponEquiped = originalWeapon;
        characterScript.armorEquiped = originalArmor;
        characterScript.accessoryEquiped = originalAccessory;
        scm.loadedData.supplyEquipment = originalSupplyEquipment;

        originalAccessory = null;
        originalArmor = null;
        originalWeapon = null;
        originalSupplyEquipment = null;

    }
    private void populateCharacterInfo(GameObject character)
    {
        CampPlayerController characterScript = character.GetComponent<CampPlayerController>();

        equipmentNamePlate.text = characterScript.title;

        //Populate equipment names
        if (characterScript.weaponEquiped != null)
        {
            weaponSlot.text = characterScript.weaponEquiped.name;
        }
        else
        {
            weaponSlot.text = "-";
        }
        if (characterScript.armorEquiped != null)
        {
            armorSlot.text = characterScript.armorEquiped.name;
        }
        else
        {
            armorSlot.text = "-";
        }
        if (characterScript.accessoryEquiped != null)
        {
            accessorySlot.text = characterScript.accessoryEquiped.name;
        }
        else
        {
            accessorySlot.text = "-";
        }
    
        //Populate stats
        baseAtkText.text = characterScript.baseAttack.ToString();
        baseIntText.text = characterScript.baseIntelligence.ToString();
        baseDefText.text = characterScript.baseDefense.ToString();
        baseResText.text = characterScript.baseResistance.ToString();
        baseSklText.text = characterScript.baseSkill.ToString();
        baseSpdText.text = characterScript.baseSpeed.ToString();

        //Populate mods
        formatStatMods(characterScript.totalAttackMod,atkModText );
        formatStatMods(characterScript.totalIntelligenceMod,intModText );
        formatStatMods(characterScript.totalDefenseMod,defModText );
        formatStatMods(characterScript.totalResistanceMod,resModText );
        formatStatMods(characterScript.totalSkillMod,sklModText );
        formatStatMods(characterScript.totalSpeedMod,spdModText );
           
        //Populate mults
        formatStatMult(characterScript.totalAttackMult,atkMultText );
        formatStatMult(characterScript.totalIntelligenceMult, intMultText);
        formatStatMult(characterScript.totalDefenseMult,defMultText );
        formatStatMult(characterScript.totalResistanceMult,resMultText );
        formatStatMult(characterScript.totalSkillMult,sklMultText );
        formatStatMult(characterScript.totalSpeedMult, spdMultText);

        //Populate hp and mana
        equipmentCurrentHP.text = characterScript.maxHp.ToString();
        equipmentMaxHP.text = characterScript.maxHp.ToString();
        equipmentCurrentMana.text = characterScript.currentMana.ToString();
        equipmentMaxMana.text = characterScript.maxMana.ToString();

        updateEquipmentDescription();
    }
    private void populateEquipmentSupply()
    {
        //Clear the content
        foreach (Transform child in equipmentSupplyContent.transform)
        {
            Destroy(child.gameObject);
        }

        //Instantiate items
        for (int i = 0; i < supplySize; i++)
        {
            try
            {   
                Equipment equipment = scm.loadedData.supplyEquipment[i];
                GameObject temp = Instantiate(supplyEquipmentPrefab, equipmentSupplyContent.transform, false);
                temp.GetComponent<SupplyEquipment>().equipment = equipment;
                temp.GetComponent<SupplyEquipment>().populateData();
            }
            catch
            {
                GameObject temp = Instantiate(supplyEquipmentPrefab, equipmentSupplyContent.transform, false);
                temp.GetComponent<SupplyEquipment>().populateEmptyData();
            }
        }
    }
    private void resetEquipmentSelectorPosition()
    {
        equipmentSelector.GetComponent<RectTransform>().anchoredPosition = new Vector2(94f, 57f);
        equipmentSelector.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(383f, 38f);
    }
    private void updateEquipmentDescription()
    {
        if (equipmentLeftRightIndex == 0)
        {

            if (equipmentIndex == 0)
            {
                try { equipmentDescription.text = campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().weaponEquiped.description; }
                catch { equipmentDescription.text = "-"; }
            }
            else if (equipmentIndex == 1)
            {
                try { equipmentDescription.text = campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().armorEquiped.description; }
                catch { equipmentDescription.text = "-"; }
            }
            else if (equipmentIndex == 2)
            {
                try { equipmentDescription.text = campAssistMenuScript.characterSelected.GetComponent<CampPlayerController>().accessoryEquiped.description; }
                catch { equipmentDescription.text = "-"; }
            }
            

        }
        else if (equipmentLeftRightIndex == 1)
        {
            try
            {
                supplyEquipmentDescription.text = scm.loadedData.supplyEquipment[equipmentIndex].description;
            }
            catch
            {
                supplyEquipmentDescription.text = "-";
            }
        }
    }
    private void moveEquipmentSupplySelectorDown()
    {
        selectorAudio.Play();
        RectTransform rt = equipmentSelector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y -= 50f;
        rt.anchoredPosition = anchoredPos;
        updateDescription(campControllerScript.mainCharacterObj);
    }
    private void moveEquipmentSupplySelectorUp()
    {
        selectorAudio.Play();
        RectTransform rt = equipmentSelector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y += 50f;
        rt.anchoredPosition = anchoredPos;
        updateDescription(campControllerScript.mainCharacterObj);
    }
    private void moveEquipmentSupplyContentWindowUp()
    {
        selectorAudio.Play();
        RectTransform temp = equipmentSupplyContent.GetComponent<RectTransform>();
        temp.anchoredPosition += new Vector2(0f, -25f);
        equipmentBotIndex--;
        equipmentTopIndex--;
    }
    private void moveEquipmentSupplyContentWindowDown()
    {
        selectorAudio.Play();
        RectTransform temp = equipmentSupplyContent.GetComponent<RectTransform>();
        temp.anchoredPosition += new Vector2(0f, 25f);
        equipmentBotIndex++;
        equipmentTopIndex++;
    }
    private void moveEquipmentSelectorDown()
    {
        selectorAudio.Play();
        RectTransform rt = equipmentSelector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y -= 106f;
        rt.anchoredPosition = anchoredPos;
        updateEquipmentDescription();

    }
    private void moveEquipmentSelectorUp()
    {
        selectorAudio.Play();
        RectTransform rt = equipmentSelector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y += 106f;
        rt.anchoredPosition = anchoredPos;
        updateEquipmentDescription();

    }
    private void moveEquipmentSelectorRight()
    {
        equipmentLeftRightIndex = 1;
        equipmentIndex = 0;
        selectorAudio.Play();
        equipmentSelector.GetComponent<RectTransform>().anchoredPosition = new Vector2(858f, 52f);
        equipmentSelector.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(513f, 38f);

        updateEquipmentDescription();

    }
    private void moveEquipmentSelectorLeft()
    {
        selectorAudio.Play();
        resetEquipmentSelectorPosition();
        equipmentLeftRightIndex = 0;
        equipmentIndex = 0;
        updateEquipmentDescription();
    }
    private void formatStatMods(int stat, TextMeshProUGUI text)
    {
        if (stat == 0)
        {
            text.text = "";
            text.color = Color.white;
        }
        else if (stat < 0)
        {
            text.text = stat.ToString();
            text.color = Color.red;
            StartCoroutine(PulseLoop(text, "red"));
        }
        
        else
        {
            text.text = "+" + stat.ToString();
            text.color = Color.blue;
            StartCoroutine(PulseLoop(text, "blue"));
        }
    }
    private void formatStatMult(float stat, TextMeshProUGUI text)
    {
        if (stat == 1f)
        {
            text.text = "";
        } 
        else
        {
            text.text = "x" + stat.ToString();
        }

        if (stat < 1f)
        {
            text.color = Color.red;
            StartCoroutine(PulseLoop(text, "red"));
        }
        else if (stat > 1f)
        {
            text.color = Color.blue;
            StartCoroutine(PulseLoop(text, "blue"));
        }
        else
        {
            text.color = Color.white;
        }

    }
    private System.Collections.IEnumerator PulseLoop(TextMeshProUGUI text, string s)
    {
        Color originalColor = text.color;
        Color tempColor;

        if (s == "blue" ) { tempColor = new Color(0f, 0.5f, 1f, 1f); }
        else { tempColor = Color.red; }

        float halfDuration = 0.5f;

        while (true)
        {
            float t = 0f;

            // Original → Blue
            while (t < halfDuration)
            {
                t += Time.deltaTime;
                text.color = Color.Lerp(originalColor, tempColor, t / halfDuration);
                yield return null;
            }

            t = 0f;

            // Blue → Original
            while (t < halfDuration)
            {
                t += Time.deltaTime;
                text.color = Color.Lerp(tempColor, originalColor, t / halfDuration);
                yield return null;
            }
        }
    }
    public void Flash(TextMeshProUGUI text, string colorToFlash)
    {
        StartCoroutine(PulseLoop(text, colorToFlash));
    }
}
