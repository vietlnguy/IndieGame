using UnityEngine;

public class OpenEquipment : MonoBehaviour
{
    SpriteRenderer sr;
    CampMoveCircle cmc;
    CampAssistMenu cam;
    CampTrade ct;
    CampController campControllerScript;
    public GameObject mainCharacter;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        cmc = FindFirstObjectByType<CampMoveCircle>();
        cam = FindFirstObjectByType<CampAssistMenu>();
        ct = FindFirstObjectByType<CampTrade>();
        campControllerScript = FindFirstObjectByType<CampController>();
    }
    private void OnMouseDown()
    {
        if (cmc.alliesInRange.Contains(gameObject))
        {
            cam.characterSelected = mainCharacter;
            ct.enableEquipmentMenu(mainCharacter);
            campControllerScript.movementEnabled = false;
            ct.soloEquipmentOrSupply = true;
        }
    }
    public void highlight()
    {
        sr.color = new Color(0.7f, 0.7f, 1f, 1f);
    }
    public void unhighlight()
    {
        sr.color = Color.white;
    }
}