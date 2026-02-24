using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class CancelExitGameButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    public AudioSource selectAudio;
    public GameObject obj;

    void Awake()
    {
        image = GetComponent<Image>();
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
        selectAudio.Play();
        obj.SetActive(false);
    }

}
