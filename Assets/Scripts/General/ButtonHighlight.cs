using UnityEngine;

public class ButtonHighlight  : MonoBehaviour
{
    private SpriteRenderer sr;

    void Awake() 
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
    }

    void Update()
    {
    }

    void OnMouseEnter()
    {   
        sr.enabled = true;
    }

    void OnMouseExit()
    {
        sr.enabled = false;
    }

}
