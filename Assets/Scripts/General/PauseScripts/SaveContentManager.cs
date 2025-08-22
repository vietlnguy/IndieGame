using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        //Clear the existing list
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        //Get all of the save files and create saveEntrys for each
        List<string> saveFiles = saveManager.GetAllSaveFiles();
        for (int i = saveFiles.Count - 1; i >= 0; i--)
        {
            string filename = saveFiles[i];
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
    public void DeleteSelectedSave()
    {
        if (saveSelected != null)
        {
            SaveEntry se = saveSelected.GetComponent<SaveEntry>();
            string filename = se.characterName + "_" + se.chapter + "_" + se.scene + "_" + se.timestamp;
            string fullFilePath = Path.Combine(Application.persistentDataPath, filename);
            saveSelected = null;
            File.Delete(fullFilePath);
            PopulateSaveList();
        }
    }
}
