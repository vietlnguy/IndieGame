using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private string saveFilePath;
    public string introBattleOutro;
    public GameSaveData loadedData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Path.Combine(Application.persistentDataPath, "game_save.json");
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
        dataToSave.introBattleOutro = this.introBattleOutro;

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

        // 3. Serialize the GameData object to a JSON string
        string jsonData = JsonUtility.ToJson(dataToSave, true);

        // 4. Write the JSON string to a file
        File.WriteAllText(saveFilePath, jsonData);
        Debug.Log("Game saved to: " + saveFilePath);
    }
    public GameSaveData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string jsonData = File.ReadAllText(saveFilePath);
            return JsonUtility.FromJson<GameSaveData>(jsonData);
        }
        return null;
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

}