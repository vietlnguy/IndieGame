using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConfirmTradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    public InventoryMenu inventoryMenuScript;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        image = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();

    }

    void Start()
    {
    }

    void Update()
    {
    }

    public void enableButton()
    {
        image.color = new Color(1f, 1f, 1f, 1f);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    public void disableButton()
    {
        image.color = new Color(1f, 1f, 1f, .35f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;  
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = new Color(.65f, .65f, .65f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = new Color(1f, 1f, 1f);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        image.color = new Color(1f, 1f, 1f);
        inventoryMenuScript.confirmTrade();        
    }

}
