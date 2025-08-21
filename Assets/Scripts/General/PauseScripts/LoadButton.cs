using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class LoadButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    public Image blackScreen;
    public GameObject pauseMenu;
    public BattleController battleController;
    public SaveManager saveManager;
    public GameObject saveMenu;

    void Awake()
    {
        image = GetComponent<Image>();
        saveManager = FindFirstObjectByType<SaveManager>();
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

        //saveMenu.SetActive(true);
        //saveManager.SaveGame();
        //StartCoroutine(FadeScreen(blackScreen, 2f));
        //SceneManager.LoadScene("MainMenu");
    

    }

}
