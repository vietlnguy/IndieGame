using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private string saveFilePath;
    public string introBattleOutro;

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
        MainPlayerController mpc = FindFirstObjectByType<MainPlayerController>();

        for (int i = 0; i < mpc.transform.childCount; i++)
        {
            Transform childTransform = mpc.transform.GetChild(i);
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
    
    // You would also have a public LoadGame() method
    public GameSaveData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string jsonData = File.ReadAllText(saveFilePath);
            return JsonUtility.FromJson<GameSaveData>(jsonData);
        }
        return null;
    }
}