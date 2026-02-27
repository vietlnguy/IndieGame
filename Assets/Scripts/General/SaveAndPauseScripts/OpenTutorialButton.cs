using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class OpenTutorialButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    private Tutorial tutorialScript;
    public GameObject pauseMenu;
    private BattleController battleController;

    void Awake()
    {
        image = GetComponent<Image>();
    }
    void Start()
    {
        tutorialScript = GameObject.Find("Tutorial").GetComponent<Tutorial>();
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
        pauseMenu.SetActive(false);
        try
        {
            battleController = GameObject.Find("BattleController").GetComponent<BattleController>();
            battleController.isPaused = false;
        }
        catch
        {
        }
        tutorialScript.EnableTutorial();

    }

}
