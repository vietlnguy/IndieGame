using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackRange : MonoBehaviour
{
    public bool active = false;
    public BattleController battleController;
    void Awake()
    {
    }

    void Start()
    {
    }

    void Update()
    {
        if (active)
        {
            gameObject.transform.position = battleController.characterSelected.transform.position;
        }
    }

    public void enableAttackRange(GameObject character)
    {
        gameObject.SetActive(true);
        float scale = character.GetComponent<PlayerController>().attackRange;
        gameObject.transform.localScale = new Vector3(scale, scale, scale);
        active = true;
    }

    public void disableAttackRange()
    {
        active = false;
        gameObject.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy")
        {
            other.gameObject.GetComponent<EnemyController>().highlightAttackable();
            battleController.enemiesInRange.Add(other.gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy")
        {
            other.gameObject.GetComponent<EnemyController>().unhighlightAttackable();
            battleController.enemiesInRange.RemoveAll(item => item == other.gameObject);
        }
    }

}
