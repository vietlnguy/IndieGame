using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class ReturnButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    public GameObject pauseMenu;
    public BattleController battleController;
    public AudioSource selectBeep;

    void Awake()
    {
        image = GetComponent<Image>();
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
        selectBeep.Play();
        if (battleController.introFinished)
        {
            battleController.isPaused = false;
        }

        pauseMenu.SetActive(false);;
    }
}
