using UnityEngine;

public class GroupSpriteYSort : MonoBehaviour
{
    
    public int offset = 0; // Optional manual offset if needed
    private SpriteRenderer[] renderers;

    void Start()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        float yAverage = 0;

        foreach (var r in renderers)
        {
            yAverage = yAverage + r.transform.position.y;
        }

        yAverage = yAverage / renderers.Length;
        int i = 0;
        foreach (var r in renderers)
        {
            r.sortingOrder = -(int)(yAverage * 100) + offset + i;
            i++;
        }

    }

}
