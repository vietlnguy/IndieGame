using UnityEngine;
using TMPro;

public class SupplyItem : MonoBehaviour
{
    public Item item;
    private TextMeshProUGUI itemName;
    private TextMeshProUGUI itemQty;
    void Awake()
    {
        itemName = GetComponent<TextMeshProUGUI>();
        itemQty = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        itemName.text = item.name;
        itemQty.text = item.currentQuantity.ToString() + "/" + item.maxQuantity.ToString();
    }
}