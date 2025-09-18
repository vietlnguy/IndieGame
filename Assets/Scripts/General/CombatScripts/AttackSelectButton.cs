using UnityEngine;
using UnityEngine.EventSystems;

public class AttackSelectButton : MonoBehaviour, IPointerClickHandler
{
    public AttackPreview attackPreviewScript;
    public int index;
    void Awake()
    {
    }

    void Start()
    {
    }

    void Update()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        attackPreviewScript.chooseAttack(index);
    }

}
