using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConfirmTrainButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    private CampTrain campTrainScript;
    private bool active = false;

    void Awake()
    {
        image = GetComponent<Image>();
        campTrainScript = FindAnyObjectByType<CampTrain>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (active)
        {
            image.color = new Color(.75f, .75f, .75f, 1f);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {   
        if (active)
        {
            image.color = new Color(1f, 1f, 1f, 1f);
        }

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (active)
        {
            image.color = new Color(1f, 1f, 1f);
            campTrainScript.ConfirmTrain();
        }
    }
    public void Activate()
    {
        active = true;
        image.color = new Color(1f, 1f, 1f, 1f);
    }

}
