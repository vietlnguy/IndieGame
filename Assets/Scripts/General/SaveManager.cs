using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public string introBattleOutro;
    public string mainCharacterName = "MainCharacterName";
    public string chapter;
    public GameSaveData loadedData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
    //public GameSaveData LoadGame()
    //{
    //    if (File.Exists(saveFilePath))
    //    {
    //        string jsonData = File.ReadAllText(saveFilePath);
    //        return JsonUtility.FromJson<GameSaveData>(jsonData);
    //    }
    //    return null;
    //}
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

}