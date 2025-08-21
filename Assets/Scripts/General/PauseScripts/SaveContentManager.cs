using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class SaveContentManager : MonoBehaviour
{
    public BattleController battleController;
    public GameObject saveSelected;
    public SaveManager saveManager;
    public GameObject saveEntryPrefab;
    public GameObject content;

    void Start()
    {
        saveManager = FindFirstObjectByType<SaveManager>();
        PopulateSaveList();
    }

    public void PopulateSaveList()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        List<string> saveFiles = saveManager.GetAllSaveFiles();
        foreach (string filename in saveFiles)
        {
            GameObject newSaveEntry = Instantiate(saveEntryPrefab, content.transform, false);
            SaveEntry script = newSaveEntry.GetComponent<SaveEntry>();
            string[] split = filename.Split('_');
            script.characterName = split[0];
            script.chapter = split[1];
            script.scene = split[2];
            script.timestamp = split[3];
        }
    }

    public void unselectOtherEntry()
    {
        if (saveSelected)
        {
            saveSelected.GetComponent<Image>().color = new Color(.2f, .24f, .52f, 0f);
        }
    }

}
