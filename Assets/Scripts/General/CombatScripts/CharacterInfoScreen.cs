using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CharacterInfoScreen : MonoBehaviour
{
    public PlayerController characterScript;
    public bool active = false;
    private int index = 0;
    private int upDownIndex = 0;
    private int leftRightIndex = 0;
    public GameObject selector;
    public AudioSource selectorAudio;
    public AudioSource deselectAudio;
    public GameObject blackScreen;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI hpStat;
    public TextMeshProUGUI manaStat;
    public TextMeshProUGUI atkStat;
    public TextMeshProUGUI intStat;
    public TextMeshProUGUI sklStat;
    public TextMeshProUGUI defStat;
    public TextMeshProUGUI resStat;
    public TextMeshProUGUI spdStat;
    public TextMeshProUGUI statsDescription;
    public TextMeshProUGUI attack1;
    public TextMeshProUGUI attack2;
    public TextMeshProUGUI attack3;
    public TextMeshProUGUI attack4;
    public TextMeshProUGUI attackDescription;
    public TextMeshProUGUI attackManaCost;
    public TextMeshProUGUI inventory1;
    public TextMeshProUGUI inventory2;
    public TextMeshProUGUI inventory3;
    public TextMeshProUGUI inventory4;
    public TextMeshProUGUI inventory5;
    public TextMeshProUGUI inventoryQty1;
    public TextMeshProUGUI inventoryQty2;
    public TextMeshProUGUI inventoryQty3;
    public TextMeshProUGUI inventoryQty4;
    public TextMeshProUGUI inventoryQty5;
    public TextMeshProUGUI inventoryDescription;
    public TextMeshProUGUI weaponEquipped;
    public TextMeshProUGUI armorEquipped;
    public TextMeshProUGUI accessoryEquipped;
    public TextMeshProUGUI equipmentDescription;
    public ModifierTextColor mtcScript;
    public TextMeshProUGUI atkModText;
    public GameObject childObject;
    public CharacterMenu characterMenuScript;

    void LateUpdate()
    {
        if (active)
        {
            //Move the selector
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (index == 0 && upDownIndex == 1)
                {
                    if (leftRightIndex == 0)
                    {
                        bottomOfTopLeft();
                    }
                    else if (leftRightIndex == 1)
                    {
                        bottomOfTopRight();
                    }
                }
                else if (index > 0)
                {
                    moveSelectorUp();
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (upDownIndex == 0)
                {
                    if (leftRightIndex == 0 && index == 5)
                    {
                        bottomLeft();
                    }
                    else if (leftRightIndex == 1 && index == 3)
                    {
                        bottomRight();
                    }
                    else
                    {
                        moveSelectorDown();
                    }
                }
                else {
                    moveSelectorDown();
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (leftRightIndex == 1)
                {
                    if (upDownIndex == 0)
                    {
                        topLeft();
                    }
                    else if (upDownIndex == 1)
                    {
                        bottomLeft();
                    }

                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (leftRightIndex == 0)
                {
                    if (upDownIndex == 0)
                    {
                        topRight();
                    }
                    else if (upDownIndex == 1)
                    {
                        bottomRight();
                    } 
                }
            }
            else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(UndoTransition(blackScreen, .25f));
                active = false;
            }
        }
    }
    private void moveSelectorDown()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;

        //Stats Block
        if (leftRightIndex == 0 && upDownIndex == 0)
        {
            if (index < 5)
            {
                anchoredPos.y -= 50f;
                index++;
                updateStatsDescription();
            }
        }
        
        //Equipment Block
        else if (leftRightIndex == 0 && upDownIndex == 1)
        {
            if (index < 2) {
                anchoredPos.y -= 110f;
                index++;
                updateEquipmentDescription(); 
            }   
        }
        
        //Attack Block
        else if (leftRightIndex == 1 && upDownIndex == 0)
        {
            if (index < 3) {
                index++;
                anchoredPos.y -= 65f;
                updateAttackDescription();
            }
        }
        
        //Inventory Block
        else if (leftRightIndex == 1 && upDownIndex == 1)
        {
            if (index < 4) {
                anchoredPos.y -= 45f;
                index++;
                updateItemDescription();   
            }
        }

        rt.anchoredPosition = anchoredPos;
    }
    private void moveSelectorUp()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;

        //Stats Block
        if (leftRightIndex == 0 && upDownIndex == 0)
        {
            anchoredPos.y += 50f;
            index--;
            updateStatsDescription();
        }
        
        //Equipment Block
        else if (leftRightIndex == 0 && upDownIndex == 1)
        {
            if (index > 0) {
                anchoredPos.y += 110f;
                index--;
                updateEquipmentDescription();  
            } 
        }

        //Attacks Block
        else if (leftRightIndex == 1 && upDownIndex == 0)
        {
            if (index > 0) {
                index--;
                anchoredPos.y += 65f;
                updateAttackDescription();
            }
        }

        //Inventory Block
        else if (leftRightIndex == 1 && upDownIndex == 1)
        {
            if (index > 0) {
                anchoredPos.y += 45f;
                index--;
                updateItemDescription();  
            } 
        }
        rt.anchoredPosition = anchoredPos;
    }
    private void moveSelectorLeft()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y += 27f;
        rt.anchoredPosition = anchoredPos;
    }
    private void moveSelectorRight()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y -= 27f;
        rt.anchoredPosition = anchoredPos;
    }
    private void topLeft()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y = 393f;
        anchoredPos.x = 62f;
        rt.anchoredPosition = anchoredPos;
        leftRightIndex = 0;
        upDownIndex = 0;
        index = 0;
        updateStatsDescription();
    }
    private void topRight()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y = 368f;
        anchoredPos.x = 605f;
        rt.anchoredPosition = anchoredPos;
        leftRightIndex = 1;
        upDownIndex = 0;
        index = 0;
        updateAttackDescription();
    }
    private void bottomLeft()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y = -125f;
        anchoredPos.x = 60f;
        rt.anchoredPosition = anchoredPos;
        index = 0;
        upDownIndex = 1;
        leftRightIndex = 0;
        updateEquipmentDescription();
    }
    private void bottomRight()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y = -153f;
        anchoredPos.x = 605f;
        rt.anchoredPosition = anchoredPos;
        leftRightIndex = 1;
        upDownIndex = 1;
        index = 0;
        updateInventoryDescription();
    }
    private void bottomOfTopLeft()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y = 143f;
        anchoredPos.x = 62f;
        rt.anchoredPosition = anchoredPos;
        leftRightIndex = 0;
        upDownIndex = 0;
        index = 5;
        updateStatsDescription();
    }
    private void bottomOfTopRight()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y = 173f;
        anchoredPos.x = 605f;
        rt.anchoredPosition = anchoredPos;
        leftRightIndex = 1;
        upDownIndex = 0;
        index = 3;
        updateAttackDescription();
    }
    public void populateInitialData()
    {
        //Populate Stats
        characterNameText.text = characterScript.title;
        hpStat.text = characterScript.currentHp.ToString() + "/" + characterScript.maxHp;
        manaStat.text = characterScript.currentMana.ToString() + "/" + characterScript.maxMana;
        atkStat.text = characterScript.baseAttack.ToString();
        intStat.text = characterScript.baseIntelligence.ToString();
        sklStat.text = characterScript.baseSkill.ToString();
        defStat.text = characterScript.baseDefense.ToString();
        resStat.text = characterScript.baseResistance.ToString();
        spdStat.text = characterScript.baseSpeed.ToString();


        if (characterScript.totalAttackMod != 0)
        {
            if (characterScript.totalAttackMod > 0) { atkModText.text = "+" + characterScript.totalAttackMod.ToString(); mtcScript.Flash(atkModText, "blue");}
            else { atkModText.text = "-" + characterScript.totalAttackMod.ToString(); mtcScript.Flash(atkModText, "red"); }
        }


        //Update Descriptions
        updateStatsDescription();

        //Populate Attacks
        try {
            attack1.text = characterScript.knownAttacks[0].name;
            attack2.text = characterScript.knownAttacks[1].name;
            attack3.text = characterScript.knownAttacks[2].name;
            attack4.text = characterScript.knownAttacks[3].name;
        }
        catch{}

        updateAttackDescription();

        //Populate Inventory
        try {
            inventory1.text = characterScript.inventory[0].name;
            inventoryQty1.text = characterScript.inventory[0].currentQuantity + "/" + characterScript.inventory[0].maxQuantity;
            inventory2.text = characterScript.inventory[1].name;
            inventoryQty1.text = characterScript.inventory[1].currentQuantity + "/" + characterScript.inventory[1].maxQuantity;
            inventory3.text = characterScript.inventory[2].name;
            inventoryQty1.text = characterScript.inventory[2].currentQuantity + "/" + characterScript.inventory[2].maxQuantity;
            inventory4.text = characterScript.inventory[3].name;
            inventoryQty1.text = characterScript.inventory[3].currentQuantity + "/" + characterScript.inventory[3].maxQuantity;
            inventory5.text = characterScript.inventory[4].name;
            inventoryQty1.text = characterScript.inventory[4].currentQuantity + "/" + characterScript.inventory[4].maxQuantity;
        }
        catch{}

        updateInventoryDescription();

        //Populate Equipment
        try
        {
            weaponEquipped.text = characterScript.weaponEquiped.name;
            armorEquipped.text = characterScript.armorEquiped.name;
            accessoryEquipped.text = characterScript.accessoryEquiped.name; 
        }
        catch{}

        updateEquipmentDescription();


    
    
    }
    public void updateStatsDescription()
    {
        if (index == 0)
        {
            statsDescription.text = "Physical strength. Affects damage with swords, bows, etc.";
        }
        else if (index == 1)
        {
            statsDescription.text = "Magical intelligence. Affects damage with tomes, staffs, etc.";            
        }
        else if (index == 2)
        {
            statsDescription.text = " Skill. Increases accuracy and critical strike chance.";            
        }
        else if (index == 3)
        {
            statsDescription.text = "Physical defense. Reduces damage taken from swords, etc.";            
        }
        else if (index == 4)
        {
            statsDescription.text = "Magical resistance. Reduces damage taken from tomes, etc.";            
        }
        else if (index == 5)
        {
            statsDescription.text = "Speed. Increases dodge and allows for multiple attacks.";           
        }
    }
    public void updateAttackDescription()
    {
        try {
            if (index == 0)
            {
                attackDescription.text = characterScript.knownAttacks[0].description;
                attackManaCost.text = characterScript.knownAttacks[0].manaCost.ToString();
            }
            else if (index == 1)
            {
                attackDescription.text = characterScript.knownAttacks[1].description;
                attackManaCost.text = characterScript.knownAttacks[1].manaCost.ToString(); 
            }
            else if (index == 2)
            {
                attackDescription.text = characterScript.knownAttacks[2].description; 
                attackManaCost.text = characterScript.knownAttacks[2].manaCost.ToString();
            }
            else if (index == 3)
            {
                attackDescription.text = characterScript.knownAttacks[3].description; 
                attackManaCost.text = characterScript.knownAttacks[3].manaCost.ToString();
            }
        }
        catch
        {
            attackDescription.text = "-";
            attackManaCost.text = "-";
        }
    }
    public void updateInventoryDescription()
    {
        try {
            if (index == 0)
            {
                inventoryDescription.text = characterScript.inventory[0].description;
            }
            else if (index == 1)
            {
                inventoryDescription.text = characterScript.inventory[1].description;
            }
            else if (index == 2)
            {
                inventoryDescription.text = characterScript.inventory[2].description; 
            }
            else if (index == 3)
            {
                inventoryDescription.text = characterScript.inventory[3].description; 

            }
            else if (index == 4)
            {
                inventoryDescription.text = characterScript.inventory[4].description; 
            }
        }
        catch
        {
            inventoryDescription.text = "-";
        }  
    }
    public void updateEquipmentDescription()
    {
        try {
            if (index == 0)
            {
                equipmentDescription.text = characterScript.weaponEquiped.description;
            }
            else if (index == 1)
            {
                equipmentDescription.text = characterScript.armorEquiped.description;
            }
            else if (index == 2)
            {
                equipmentDescription.text = characterScript.accessoryEquiped.description; 
            }
        }
        catch
        {
            equipmentDescription.text = "-";
        } 
    }
    public void updateItemDescription()
    {
        
    }
    public IEnumerator enableCharacterInfo(GameObject characterSelected)
    {
        yield return null;
        characterScript = characterSelected.GetComponent<PlayerController>();
        active = true;
        populateInitialData();
        StartCoroutine(TransitionToCharacterInfo(blackScreen, .25f));
    }
    private IEnumerator TransitionToCharacterInfo(GameObject obj, float duration){
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        float startAlpha = 0f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, time / duration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
        
        childObject.SetActive(true);

        startAlpha = 1f;
        time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;


    }
    public IEnumerator UndoTransition(GameObject obj, float duration){
        active = false;
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        float startAlpha = 0f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, time / duration);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        childObject.SetActive(false);

        startAlpha = 1f;
        time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;

        characterMenuScript.active = true;

    }
}

