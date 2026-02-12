using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class CharacterToolTip : MonoBehaviour
{
    public Camera worldCamera;
    private RectTransform characterToolTipCanvasRect;
    public GameObject characterToolTip;
    public TextMeshProUGUI characterToolTipHp;
    public TextMeshProUGUI characterToolTipMaxHp;
    public TextMeshProUGUI characterToolTipMana;
    public TextMeshProUGUI characterToolTipMaxMana;
    public TextMeshProUGUI characterToolTipName;

    void Awake()
    {
        worldCamera = Camera.main;
        characterToolTipCanvasRect = GetComponent<RectTransform>();
    }

    void Update()
    {

    }
    public void enableCharacterToolTip(GameObject character)
    {
        characterToolTip.SetActive(true);
        try
        {
            characterToolTipHp.text = character.GetComponent<PlayerController>().currentHp.ToString();
            characterToolTipMaxHp.text = character.GetComponent<PlayerController>().maxHp.ToString();
            characterToolTipMana.text = character.GetComponent<PlayerController>().currentMana.ToString();
            characterToolTipMaxMana.text = character.GetComponent<PlayerController>().maxMana.ToString();
            characterToolTipName.text = character.GetComponent<PlayerController>().title;
        }
        catch
        {
            characterToolTipHp.text = character.GetComponent<EnemyController>().currentHp.ToString();
            characterToolTipMaxHp.text = character.GetComponent<EnemyController>().maxHp.ToString();
            characterToolTipMana.text = character.GetComponent<EnemyController>().currentMana.ToString();
            characterToolTipMaxMana.text = character.GetComponent<EnemyController>().maxMana.ToString();
            characterToolTipName.text = character.GetComponent<EnemyController>().title;
        }
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(worldCamera, character.transform.position);

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(characterToolTipCanvasRect, screenPos, worldCamera, out localPos);

        characterToolTip.GetComponent<RectTransform>().localPosition = localPos + new Vector2(-90f, 200f);
    }

    public void disableCharacterToolTip()
    {
        characterToolTip.SetActive(false);
    }

}
