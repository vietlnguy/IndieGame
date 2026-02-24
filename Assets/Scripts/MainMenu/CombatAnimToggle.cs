using UnityEngine;
using UnityEngine.UI;

public class CombatAnimToggle : MonoBehaviour
{
    public Toggle combatAnimToggle;

    void Start()
    {
        // Set toggle state
        combatAnimToggle.isOn = false;
        PlayerPrefs.SetInt("combatAnim", 0);

    }

    public void OnToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("combatAnim", isOn ? 1 : 0);
    }


}