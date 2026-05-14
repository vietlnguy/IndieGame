using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class SaveButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    private SaveManager saveManager;
    public GameObject saveMenu;
    public AudioSource selectAudio;
    void Awake()
    {
        image = GetComponent<Image>();
        saveManager = FindAnyObjectByType<SaveManager>();
    }

    void Start()
    {
    }

    void Update()
    {
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
        saveMenu.SetActive(true);
        selectAudio.Play();
    }

}
