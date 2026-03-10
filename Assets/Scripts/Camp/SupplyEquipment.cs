using UnityEngine;
using TMPro;

public class SupplyEquipment : MonoBehaviour
{
    public Equipment equipment;
    public TextMeshProUGUI equipmentName;
    public GameObject blankImage;
    public GameObject weaponImage;
    public GameObject armorImage;
    public GameObject accessoryImage;

    public void populateData()
    {
        equipmentName.text = equipment.name;
        if (equipment.type == "weapon")
        {
            weaponImage.SetActive(true);
        }
        else if (equipment.type == "armor")
        {
            armorImage.SetActive(true);
        }
        else if (equipment.type == "accessory")
        {
            accessoryImage.SetActive(true);
        }
        
    }
    public void populateEmptyData()
    {
        equipmentName.text = "-";
        blankImage.SetActive(true);
    }
}