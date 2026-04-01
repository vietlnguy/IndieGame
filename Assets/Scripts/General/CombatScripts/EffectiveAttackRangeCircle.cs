using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(CircleCollider2D))]
public class EffectiveAttackRangeCircle : MonoBehaviour
{
    public bool active = false;
    public BattleController battleController;
    public int segments = 64; // Smoothness of the circle
    public string sortingLayerName = "MoveRangeUI";
    public int sortingOrder = 0;
    public Color fillColor = new Color(1f, 1f, 1f, 0.25f);
    private MeshFilter meshFilter;
    private CircleCollider2D circleCollider;
    public List<GameObject> alliesInRange;
    public List<GameObject> enemiesInRange;
    void Awake()
    {
        battleController = GameObject.Find("BattleController").GetComponent<BattleController>();
        alliesInRange = new List<GameObject>();
        enemiesInRange = new List<GameObject>();

        meshFilter = GetComponent<MeshFilter>();

        circleCollider = GetComponent<CircleCollider2D>();

        // Make sure the collider acts as a trigger
        circleCollider.isTrigger = true;

        // Assign material with sprite shader for transparency support
        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = fillColor;

        // Sorting for 2D layering

    }
    public void enableEffectiveAttackRange(GameObject character)
    {

        gameObject.transform.position = character.transform.position;
        float radius;
        circleCollider.enabled = true;

        if (character.GetComponent<EnemyController>() != null)
        {
            radius = character.GetComponent<EnemyController>().attackRange + 0.9f * character.GetComponent<EnemyController>().moveRange;
        }
        else
        {
            radius = character.GetComponent<PlayerController>().attackRange + 0.9f * character.GetComponent<PlayerController>().moveRange;

        }        
        DrawFilledCircle(radius);
        UpdateCollider(radius);
        active = true;
    }
    public void disableEffectiveAttackRange()
    {
        active = false;
        circleCollider.enabled = false;
        enemiesInRange.Clear();
        alliesInRange.Clear();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //Character is selected and others enter AttackRange
        if (battleController.characterSelected != null)
        {
            if (other.CompareTag("enemy"))
            {
                other.GetComponent<EnemyController>().highlightAttackable();
                if (!enemiesInRange.Contains(other.gameObject))
                {
                    enemiesInRange.Add(other.gameObject);
                }
            }
            if (other.CompareTag("character"))
            {
                if (other.gameObject != battleController.characterSelected)
                {
                    other.GetComponent<PlayerController>().highlightAssistable();
                    if (!alliesInRange.Contains(other.gameObject))
                    {
                        alliesInRange.Add(other.gameObject);
                    }
                }
            }
        }

        //Enemy is selected and others enter AttackRange
        if (battleController.enemySelected != null && battleController.characterSelected == null)
        {
            if (other.CompareTag("enemy") && other.gameObject != battleController.enemySelected)
            {
                if (!alliesInRange.Contains(other.gameObject))
                {
                    alliesInRange.Add(other.gameObject);
                }

            }
            if (other.CompareTag("character"))
            {
                if (other.gameObject != battleController.characterSelected)
                {
                    if (!enemiesInRange.Contains(other.gameObject))
                    {
                        enemiesInRange.Add(other.gameObject);
                    }
                }

            }
        
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {

        //Character is selected and others leave the AttackRange
        if (battleController.characterSelected != null)
        {
            if (other.CompareTag("enemy"))
            {
                enemiesInRange.RemoveAll(item => item == other.gameObject);
            }
            if (other.CompareTag("character"))
            {
                alliesInRange.RemoveAll(item => item == other.gameObject);
            }
        }

        //Enemy is selected and others leave the AttackRange
        if (battleController.characterSelected == null && battleController.enemySelected != null)
        {
            if (other.CompareTag("character"))
            {
                enemiesInRange.RemoveAll(item => item == other.gameObject);
            }
            else if (other.CompareTag("enemy"))
            {
                enemiesInRange.RemoveAll(item => item == other.gameObject);  
            }
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
