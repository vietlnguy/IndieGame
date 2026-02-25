using UnityEngine;
using UnityEngine.UI;

public class CombatAnimToggle : MonoBehaviour
{
    public Toggle combatAnimToggle;

    void Start()
    {
        // Set toggle state
        if (PlayerPrefs.GetInt("combatAnim", -1) == -1)
        {
            combatAnimToggle.isOn = false;
            PlayerPrefs.SetInt("combatAnim", 0);
        }
        else
        {
            if (PlayerPrefs.GetInt("combatAnim") == 0)
            {
                combatAnimToggle.isOn = false;
            }
            else
            {
                combatAnimToggle.isOn = true;
            }
        }
    }

    public void OnToggleChanged()
    {   
        PlayerPrefs.SetInt("combatAnim", combatAnimToggle.isOn ? 1 : 0);
    }


}