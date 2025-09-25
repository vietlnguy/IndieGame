using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class ConfirmAttackButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    public SaveManager scm;
    public AttackPreview attackPreview;
    public bool validAttack = false;

    void Awake()
    {
        image = GetComponent<Image>();
        scm = FindFirstObjectByType<SaveManager>();
    }

    void Start()
    {
    }

    void Update()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = new Color(.65f, .65f, .65f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = new Color(1f, 1f, 1f);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (validAttack)
        {
            StartCoroutine(attackPreview.startAttackSequence());
        }
            
    }

}
