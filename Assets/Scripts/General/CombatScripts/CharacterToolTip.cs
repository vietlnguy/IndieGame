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
    public GameObject rangedImage;
    public GameObject meleeImage;

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
            PlayerController characterScript = character.GetComponent<PlayerController>();
            characterToolTipHp.text = characterScript.currentHp.ToString();
            characterToolTipMaxHp.text = characterScript.maxHp.ToString();
            characterToolTipMana.text = characterScript.currentMana.ToString();
            characterToolTipMaxMana.text = characterScript.maxMana.ToString();
            characterToolTipName.text = characterScript.title;
            if (characterScript.ranged) { rangedImage.SetActive(true); meleeImage.SetActive(false); }
            else { rangedImage.SetActive(false); meleeImage.SetActive(true);  }
        }
        catch
        {
            EnemyController characterScript = character.GetComponent<EnemyController>();
            characterToolTipHp.text = characterScript.currentHp.ToString();
            characterToolTipMaxHp.text = characterScript.maxHp.ToString();
            characterToolTipMana.text = characterScript.currentMana.ToString();
            characterToolTipMaxMana.text = characterScript.maxMana.ToString();
            characterToolTipName.text = characterScript.title;
            if (characterScript.ranged) { rangedImage.SetActive(true); meleeImage.SetActive(false); }
            else { rangedImage.SetActive(false); meleeImage.SetActive(true);  }
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
