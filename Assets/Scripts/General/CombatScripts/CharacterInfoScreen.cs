using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CharacterInfoScreen : MonoBehaviour
{
    public PlayerController characterScript;
    public bool active = true;
    private int index = 0;
    private int upDownIndex = 0;
    private int leftRightIndex = 0;
    public GameObject selector;
    public AudioSource selectorAudio;
    public AudioSource deselectAudio;
    public GameObject blackScreen;
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
        atkStat.text = characterScript.attack.ToString();
        intStat.text = characterScript.intelligence.ToString();
        sklStat.text = characterScript.skill.ToString();
        defStat.text = characterScript.defense.ToString();
        resStat.text = characterScript.resistance.ToString();
        spdStat.text = characterScript.speed.ToString();

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
        
    }
    public void updateItemDescription()
    {
        
    }
}

