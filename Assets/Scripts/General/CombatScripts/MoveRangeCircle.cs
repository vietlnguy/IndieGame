using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MoveRangeCircle : MonoBehaviour
{
    public int segments = 64;     // Smoothness of the circle
    public string sortingLayerName = "MoveRangeUI";
    public int sortingOrder = 0;
    public Color fillColor = new Color(0f, 0f, 1f, 0.25f); // semi-transparent blue
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private EdgeCollider2D edgeCollider;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Assign material with sprite shader for transparency support
        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = fillColor;
        meshRenderer.material = mat;

        // Sorting for 2D layering
        meshRenderer.sortingLayerName = sortingLayerName;
        meshRenderer.sortingOrder = sortingOrder;

        edgeCollider = GetComponent<EdgeCollider2D>();
    }
    private void DrawFilledCircle(float radius)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[segments + 1];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero; // center of circle

        float angleStep = 2 * Mathf.PI / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleStep;
            vertices[i + 1] = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
        }

        for (int i = 0; i < segments; i++)
        {
            int triStart = i * 3;
            triangles[triStart] = 0;
            triangles[triStart + 1] = i + 1;
            triangles[triStart + 2] = (i + 2 > segments) ? 1 : i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;

        UpdateCollider(radius);
        edgeCollider.enabled = true;
    }
    private void UpdateCollider(float radius)
    {
        Vector2[] points = new Vector2[segments + 1];
        float angleStep = 2 * Mathf.PI / segments;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep;
            points[i] = new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
        }

        edgeCollider.points = points;
    }
    public void enableMoveRange(GameObject character)
    {
        meshRenderer.enabled = true;
        transform.position = character.transform.position;
        try
        {
            DrawFilledCircle(character.GetComponent<PlayerController>().moveRange);
        }
        catch
        {
            DrawFilledCircle(character.GetComponent<EnemyController>().moveRange);
        }

    }
    public void disableMoveRange()
    {
        meshRenderer.enabled = false;
        edgeCollider.enabled = false;
    }
}
