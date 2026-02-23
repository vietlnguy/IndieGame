using UnityEngine;
using UnityEngine.UI;

public class CombatAnimToggle : MonoBehaviour
{
    public Toggle combatAnimToggle;

    void Start()
    {
        combatAnimToggle.onValueChanged.AddListener(Toggle);
    }
    public void Toggle(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("combatAnim", 1);

        }
        else
        {
            PlayerPrefs.SetInt("combatAnim", 0);
        }
    }

}