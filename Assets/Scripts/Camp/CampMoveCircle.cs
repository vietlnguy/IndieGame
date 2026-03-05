using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(CircleCollider2D))]
public class CampMoveCircle : MonoBehaviour
{

    public int segments = 64; // Smoothness of the circle
    public string sortingLayerName = "MoveRangeUI";
    public int sortingOrder = 0;
    public Color fillColor = new Color(0f, 0f, 1f, 0.25f);
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private CircleCollider2D circleCollider;
    public List<GameObject> alliesInRange;

    void Awake()
    {
        alliesInRange = new List<GameObject>();

        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();

        // Make sure the collider acts as a trigger
        circleCollider.isTrigger = true;

        // Assign material with sprite shader for transparency support
        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = fillColor;
        meshRenderer.material = mat;

        // Sorting for 2D layering
        meshRenderer.sortingLayerName = sortingLayerName;
        meshRenderer.sortingOrder = sortingOrder;

        DrawFilledCircle(.75f);
        UpdateCollider(.75f);
    }
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.tag == "mainCharacter")
        {
            //Do nothing
        }
        else if (other.tag == "character") {
            other.gameObject.GetComponent<CampPlayerController>().highlightAssistable();
            alliesInRange.Add(other.gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "mainCharacter")
        {
            //Do nothing
        }
        else if (other.tag == "character") {
            other.gameObject.GetComponent<CampPlayerController>().unhighlight();
            alliesInRange.RemoveAll(item => item == other.gameObject);
        }

    }
    public void DrawFilledCircle(float radius)
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
    }
    public void UpdateCollider(float radius)
    {
        if (circleCollider != null)
        {
            circleCollider.radius = radius;
        }
    }
}
