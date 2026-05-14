using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UndoPointsButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    private CampTrain campTrainScript;

    void Awake()
    {
        image = GetComponent<Image>();
        campTrainScript = FindAnyObjectByType<CampTrain>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = new Color(.75f, .75f, .75f, 1f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {   
        image.color = new Color(1f, 1f, 1f, 1f);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        image.color = new Color(1f, 1f, 1f);
        campTrainScript.UndoPoints();
    }

}
