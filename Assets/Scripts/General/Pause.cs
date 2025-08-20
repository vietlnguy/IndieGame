using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class Pause : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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
        string tag = gameObject.tag;

        if (tag == "return")
        {
            pauseMenu.SetActive(false);
            battleController.isPaused = false;
        }
        else if (tag == "settings")
        {

        }
        else if (tag == "save and menu")
        {
            saveMenu.SetActive(true);
            //saveManager.SaveGame();
            //StartCoroutine(FadeScreen(blackScreen, 2f));
            //SceneManager.LoadScene("MainMenu");
        }
        else if (tag == "save and close")
        {
            saveMenu.SetActive(true);
            //saveManager.SaveGame();
            //Application.Quit();
        }

    }
    private IEnumerator FadeScreen(Image obj, float duration){
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        float startAlpha = 0f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

}
