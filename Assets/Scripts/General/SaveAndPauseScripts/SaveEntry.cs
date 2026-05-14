using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class SaveEntry : MonoBehaviour, IPointerClickHandler

{
    private Image image;
    private SaveManager scm;
    public string characterName;
    public string chapter;
    public string scene;
    public string timestamp;
    public GameObject characterText;
    public GameObject chapterText;
    public GameObject sceneText;
    public GameObject timestampText;
    public GameObject autosaveObject;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        scm = FindAnyObjectByType<SaveManager>();
        characterText.GetComponent<TextMeshProUGUI>().text = characterName;
        chapterText.GetComponent<TextMeshProUGUI>().text = chapter;
        sceneText.GetComponent<TextMeshProUGUI>().text = scene;
        timestampText.GetComponent<TextMeshProUGUI>().text = timestamp.Substring(0, timestamp.IndexOf(".json"));
    }

    void Update()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        image.color = new Color(.2f, .24f, .52f, 1f);
        if (scm.saveSelected != gameObject)
        {
            scm.UnselectOtherEntry();
            scm.saveSelected = gameObject;
        }
    }
    public void ShowAutosaveText()
    {
        autosaveObject.SetActive(true);
    }

}
