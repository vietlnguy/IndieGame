using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public bool enabled = false;
    public BattleController battleController;
    void Awake()
    {
    }

    void Start()
    {
    }

    void Update()
    {
        if (enabled)
        {
            gameObject.transform.position = battleController.characterSelected.transform.position;
        }
    }

    public void enableAttackRange(GameObject character)
    {
        gameObject.SetActive(true);
        float scale = character.GetComponent<PlayerController>().attackRange;
        gameObject.transform.localScale = new Vector3(scale, scale, scale);
        enabled = true;
    }

    public void disableAttackRange()
    {
        enabled = false;
        gameObject.SetActive(false);
    }
}
