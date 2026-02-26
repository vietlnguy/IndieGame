using UnityEngine;

[DefaultExecutionOrder(-1)]
public class DataLoader : MonoBehaviour
{
    private SaveManager saveManager;
    public GameObject mainCharacterPrefab;
    public GameObject astridPrefab;
    public GameObject characters;
    void Awake()
    {
        saveManager = FindFirstObjectByType<SaveManager>();
        foreach (Character character in saveManager.loadedData.characters)
        {
            if (character.characterName == saveManager.loadedData.mainCharacterName)
            {
                Instantiate(mainCharacterPrefab, new Vector3(-20f, -9.35f), Quaternion.identity, characters.transform);
            }
            else if (character.characterName == "Astrid")
            {
                Instantiate(astridPrefab, new Vector3(-4.45f, -11.65f), Quaternion.identity, characters.transform);
            }
        }
    }

}

