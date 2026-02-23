using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class EnterNameButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    public AudioSource selectAudio;
    public GameObject enterNameObject;

    void Awake()
    {
        image = GetComponent<Image>();
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
        selectAudio.Play();
        enterNameObject.SetActive(false);
    }

}
