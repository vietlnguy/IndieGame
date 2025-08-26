using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public string introBattleOutro;
    public string mainCharacterName = "MainCharacterName";
    public string chapter;
    public GameSaveData loadedData;
    public GameObject saveSelected;
    public GameObject saveEntryPrefab;
    public GameObject content;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    void Start()
    {
        PopulateSaveList();
    }
    public void SaveGame()
    {
        GameSaveData dataToSave = new GameSaveData();

        //Get scene data
        dataToSave.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        dataToSave.introBattleOutro = introBattleOutro;

        //Get character data
        GameObject characters = GameObject.FindWithTag("charactersParentObject");
        for (int i = 0; i < characters.transform.childCount; i++)
        {
            Transform childTransform = characters.transform.GetChild(i);
            PlayerController child = childTransform.gameObject.GetComponent<PlayerController>();

            if (child.owned)
            {
                CharacterData characterData = new CharacterData
                {
                    characterName = child.name,
                    maxHp = child.maxHp,
                    maxMana = child.maxMana,
                    attack = child.attack,
                    defense = child.defense,
                    skill = child.skill,
                    speed = child.speed,
                    attackRange = child.attackRange,
                    moveRange = child.moveRange,
                    relationship = child.relationship,
                };
                dataToSave.characters.Add(characterData);
            }
        }

        //Create save file name
        string time = DateTime.Now.ToString("F");
        time = time.Replace(":", "-");
        string jsonData = JsonUtility.ToJson(dataToSave, true);
        string filename = mainCharacterName + "_" + chapter + "_" + introBattleOutro + "_" + time + ".json";
        string fullFilePath = Path.Combine(Application.persistentDataPath, filename);
        File.WriteAllText(fullFilePath, jsonData);
    }
    public void LoadGame()
    {
        SaveEntry saveEntry = saveSelected.GetComponent<SaveEntry>();
        string saveFilePath = Application.persistentDataPath + "/" + saveEntry.characterName + "_" + saveEntry.chapter + "_" + saveEntry.scene + "_" + saveEntry.timestamp;
        if (File.Exists(saveFilePath))
        {
            string jsonData = File.ReadAllText(saveFilePath);
            loadedData = JsonUtility.FromJson<GameSaveData>(jsonData);
        }

        SceneManager.LoadScene(loadedData.currentChapter);
    }
    public List<string> GetAllSaveFiles()
    {
        List<string> fileNames = new List<string>();
        if (Directory.Exists(Application.persistentDataPath))
        {
            string[] files = Directory.GetFiles(Application.persistentDataPath, "*.json");
            foreach (string filePath in files)
            {

                fileNames.Add(Path.GetFileName(filePath));
            }
        }
        return fileNames;
    }
    public void PopulateSaveList()
    {
        //Clear the existing list
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        //Get all of the save files and create saveEntrys for each
        List<string> saveFiles = GetAllSaveFiles();
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
    public void UnselectOtherEntry()
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
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("save_menu_content"))
            {
                content = obj;
                break;
            }
        }
    }
    private void OnDestroy()
    {
        // Always unsubscribe to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}