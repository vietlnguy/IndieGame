using UnityEngine;
using UnityEngine.UI;

public class CombatAnimToggle : MonoBehaviour
{
    public Toggle combatAnimToggle;

    void Start()
    {
        // Load saved value (default = 1 if not found)
        int savedValue = PlayerPrefs.GetInt("combatAnim", 0);

        // Set toggle state
        combatAnimToggle.isOn = (savedValue == 1);

        // Add listener AFTER setting value to avoid unnecessary save call
        combatAnimToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    public void OnToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("combatAnim", isOn ? 1 : 0);
    }


}