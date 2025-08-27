using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class EndTurnButtons : MonoBehaviour
{
    public BattleController battleController;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
    }

    void Update()
    {

    }

    public void OnMouseEnter()
    {
        sr.enabled = true;
    }

    public void OnMouseExit()
    {
        sr.enabled = false;
    }
    public void OnMouseDown()
    {
        if (gameObject.tag == "end_turn_yes")
        {
            battleController.endCharacterTurn();
        }
        else if (gameObject.tag == "end_turn_no")
        {
            battleController.disableEndTurnUI();
        }
    }

}
