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
        dataToSave.currentChapter = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        dataToSave.introBattleOutro = introBattleOutro;
        dataToSave.mainCharacterName = loadedData.mainCharacterName;

        //Go through each character and create a serializable Character to save
        GameObject characters = GameObject.Find("Characters");
        foreach (Transform child in characters.transform)
        {
            PlayerController pc = child.GetComponent<PlayerController>();
            if (pc.owned)
            {
                Character temp = new Character(pc.title, pc.baseMaxHp, pc.baseMaxMana, pc.baseAttack, pc.baseIntelligence, pc.baseDefense, pc.baseResistance, pc.baseSkill, pc.baseSpeed, pc.baseAttackRange, pc.baseMoveRange, pc.owned);
                temp.knownAttacks = pc.knownAttacks;
                temp.inventory = pc.inventory;
                temp.weaponEquiped = pc.weaponEquiped;
                temp.armorEquiped = pc.armorEquiped;
                temp.accessoryEquiped = pc.accessoryEquiped;
                dataToSave.characters.Add(temp);
            }
        }

        //Create save file name
        string time = DateTime.Now.ToString("F");
        time = time.Replace(":", "-");
        string jsonData = JsonUtility.ToJson(dataToSave, true);
        string filename = loadedData.mainCharacterName + "_" + dataToSave.currentChapter + "_" + dataToSave.introBattleOutro + "_" + time + ".json";
        string fullFilePath = Path.Combine(Application.persistentDataPath, filename);
        File.WriteAllText(fullFilePath, jsonData);
    }
    public void NewSave(string mainCharacterName)
    {
        GameSaveData dataToSave = new GameSaveData();

        //Get scene data
        dataToSave.currentChapter = "Chapter1";
        dataToSave.introBattleOutro = "Intro";
        dataToSave.mainCharacterName = mainCharacterName;

        //Populate character data
        Character mainCharacter = new Character(mainCharacterName, 15, 8, 8, 4, 7, 5, 6, 6, 1, 5, true);
        mainCharacter.knownAttacks.Add(new Attack("Slash", "physical", 5, 90, 0, 0, false, "Slash the enemy with your sword."));
        mainCharacter.inventory.Add(new Item("Potion", 10, "hp", 10, "Restores 10 HP.", false, false, false));
        mainCharacter.weaponEquiped = new Equipment("Basic", "weapon", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Completely ordinary.");
        mainCharacter.armorEquiped = new Equipment("Leather", "armor", 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "For unexpected adventures. +2 Max HP.");

        Character astrid = new Character("Astrid", 11, 8, 6, 5, 5, 6, 8, 7, 3, 5, true);
        astrid.knownAttacks.Add(new Attack("Bow Shot", "physical", 4, 90, 0, 0, false, "Shoot an arrow at the enemy."));
        astrid.inventory.Add(new Item("Potion", 10, "hp", 10, "Restores 10 HP.", false, false, false));
        astrid.weaponEquiped = new Equipment("Basic", "weapon", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Completely ordinary.");
        astrid.armorEquiped = new Equipment("Cloth", "armor", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Completely ordinary."); 
        astrid.accessoryEquiped = new Equipment("Power Bracelet", "accessory", 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Tingles with power. +1 ATK.");

        dataToSave.characters.Add(mainCharacter);
        dataToSave.characters.Add(astrid);

        //Create save file name
        string time = DateTime.Now.ToString("F");
        time = time.Replace(":", "-");
        string filename = dataToSave.mainCharacterName + "_" + dataToSave.currentChapter + "_" + dataToSave.introBattleOutro + "_" + time + ".json";
        string fullFilePath = Path.Combine(Application.persistentDataPath, filename);

        string jsonData = JsonUtility.ToJson(dataToSave, true);
        File.WriteAllText(fullFilePath, jsonData);

        loadedData = dataToSave;
        SceneManager.LoadScene(loadedData.currentChapter);
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
        PopulateSaveList();
    }
    private void OnDestroy()
    {
        // Always unsubscribe to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}