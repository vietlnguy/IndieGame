using UnityEngine;

public class OpenSupply : MonoBehaviour
{
    SpriteRenderer sr;
    CampMoveCircle cmc;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        cmc = FindFirstObjectByType<CampMoveCircle>();
    }
    private void OnMouseDown()
    {
        if (cmc.alliesInRange.Contains(gameObject))
        {
            Debug.Log("open supply");
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