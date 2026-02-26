using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public GameSaveData loadedData;
    public GameObject saveSelected;
    public GameObject saveEntryPrefab;
    private GameObject content;
    private string openedSaveFilePath = "";

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
        //Go through each character and create a serializable Character to save
        //GameObject characters = GameObject.Find("Characters");
        //foreach (Transform child in characters.transform)
        //{
        //    PlayerController pc = child.GetComponent<PlayerController>();
        //    if (pc.owned)
        //    {
        //        Character temp = new Character(pc.title, pc.baseMaxHp, pc.baseMaxMana, pc.baseAttack, pc.baseIntelligence, pc.baseDefense, pc.baseResistance, pc.baseSkill, pc.baseSpeed, pc.baseAttackRange, pc.baseMoveRange, pc.owned, pc.ranged);
        //        temp.knownAttacks = pc.knownAttacks;
        //        temp.inventory = pc.inventory;
        //        temp.weaponEquiped = pc.weaponEquiped;
        //        temp.armorEquiped = pc.armorEquiped;
        //        temp.accessoryEquiped = pc.accessoryEquiped;
        //        dataToSave.characters.Add(temp);
        //    }
        //}

        //Create save file name
        string time = DateTime.Now.ToString("F");
        time = time.Replace(":", "-");
        string jsonData = JsonUtility.ToJson(loadedData, true);
        string filename = loadedData.mainCharacterName + "_" + loadedData.currentChapter + "_" + loadedData.introBattleOutro + "_" + time + ".json";
        string fullFilePath = Path.Combine(Application.persistentDataPath, filename);
        openedSaveFilePath = fullFilePath;
        File.WriteAllText(fullFilePath, jsonData);
    }
    public void NewSave(string mainCharacterName)
    {
        GameSaveData dataToSave = new GameSaveData();

        //Get scene data
        dataToSave.currentChapter = "Prologue";
        dataToSave.introBattleOutro = "Intro";
        dataToSave.mainCharacterName = mainCharacterName;

        //Populate character data
        Character mainCharacter = new Character(mainCharacterName, 12, 8, 8, 4, 5, 5, 6, 6, 1, 5, true, false);
        mainCharacter.knownAttacks.Add(new Attack("Slash", "physical", 1.0f, 1.0f, 90, 0, 0, "Slash with your sword."));
        mainCharacter.inventory.Add(new Item("Potion", 5, "hp", 10, "Restores 10 HP.", false, false, false));
        mainCharacter.weaponEquiped = new Equipment("Basic", "weapon", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Completely ordinary.");
        mainCharacter.armorEquiped = new Equipment("Leather", "armor", 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "For unexpected adventures. +2 Max HP.");

        Character astrid = new Character("Astrid", 11, 8, 6, 5, 3, 6, 8, 7, 3, 5, true, true);
        astrid.knownAttacks.Add(new Attack("Bow Shot", "physical", 1.0f, 1.0f, 90, 5, 0, "Shoot an arrow at the enemy."));
        astrid.inventory.Add(new Item("Potion", 5, "hp", 10, "Restores 10 HP.", false, false, false));
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
        openedSaveFilePath = fullFilePath;
        string jsonData = JsonUtility.ToJson(dataToSave, true);
        File.WriteAllText(fullFilePath, jsonData);

        loadedData = dataToSave;

    }
    public IEnumerator LoadGame()
    {
        SaveEntry saveEntry = saveSelected.GetComponent<SaveEntry>();
        string s = Application.persistentDataPath + "/" + saveEntry.characterName + "_" + saveEntry.chapter + "_" + saveEntry.scene + "_" + saveEntry.timestamp;
        if (File.Exists(s))
        {
            string jsonData = File.ReadAllText(s);
            loadedData = JsonUtility.FromJson<GameSaveData>(jsonData);
            openedSaveFilePath = s;
        }
        yield return StartCoroutine(SceneTransition());
        SceneManager.LoadScene(loadedData.currentChapter);
    }
    public List<string> GetAllSaveFiles()
    {
        if (!Directory.Exists(Application.persistentDataPath)) 
            return new List<string>();

        DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath);
        
        // Grab all .json files, sort by the time they were last saved, 
        // and then pull just the names into a list.
        return info.GetFiles("*.json")
                .OrderByDescending(f => f.LastWriteTime)
                .Select(f => f.Name)
                .ToList();
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
        for (int i = 0; i < saveFiles.Count; i++)
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
    public void OverwriteSave()
    {
        File.Delete(openedSaveFilePath);
        SaveGame();
        PopulateSaveList();
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
        //Stuff to do whenever a new scene loads
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("save_menu_content"))
            {
                content = obj;
                break;
            }
        }
        try {
            PopulateSaveList();
        }
        catch
        {
            //Some scenes like chapter bridge and prologue do not have a save menu
        }
    }
    private void OnDestroy()
    {
        // Always unsubscribe to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public IEnumerator SceneTransition()
    {
        //Fade out all audios
        AudioSource[] sources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource source in sources)
        {
            StartCoroutine(FadeOut(source));
        }

        //Find SceneTransitionCanvas and fade in black screen
        GameObject sceneTransitionCanvas = GameObject.Find("SceneTransitionCanvas");
        float time = 0f;
        float duration = 2f;
        while (time < duration)
        {
            time += Time.deltaTime;
            sceneTransitionCanvas.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, time / duration);
            yield return null;
        }
        sceneTransitionCanvas.GetComponent<CanvasGroup>().alpha = 1f;

    }
    private IEnumerator FadeOut(AudioSource source)
    {
        float startVolume = source.volume;
        float duration = 1.5f;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0, time / duration);
            yield return null;
        }

        source.volume = 0;
        source.Stop();
    }
}