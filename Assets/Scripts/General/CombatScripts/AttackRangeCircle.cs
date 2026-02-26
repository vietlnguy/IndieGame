using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(CircleCollider2D))]
public class AttackRangeCircle : MonoBehaviour
{
    public bool active = false;
    public BattleController battleController;
    public int segments = 64; // Smoothness of the circle
    public string sortingLayerName = "MoveRangeUI";
    public int sortingOrder = 0;
    public Color fillColor = new Color(1f, 0f, 0f, 0.25f);
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private CircleCollider2D circleCollider;
    public List<GameObject> alliesInRange;
    public List<GameObject> enemiesInRange;
    public bool enemyIsRangedAndMoving = false;
    public TilemapPathfinder pathfinder;

    void Awake()
    {
        battleController = GameObject.Find("BattleController").GetComponent<BattleController>();
        alliesInRange = new List<GameObject>();
        enemiesInRange = new List<GameObject>();

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
    }
    void Update()
    {
        if (active)
        {
            try
            {
                gameObject.transform.position = battleController.characterSelected.transform.position;
            }
            catch
            {
                gameObject.transform.position = battleController.enemySelected.transform.position;
            }
        }
    }
    public void enableAttackRange(GameObject character)
    {
        float radius;
        meshRenderer.enabled = true;
        circleCollider.enabled = true;

        try
        {
            radius = character.GetComponent<PlayerController>().attackRange;
        }
        catch
        {
            radius = character.GetComponent<EnemyController>().attackRange;
        }

        DrawFilledCircle(radius);
        UpdateCollider(radius);
        active = true;
    }
    public void disableAttackRange()
    {
        active = false;
        meshRenderer.enabled = false;
        circleCollider.enabled = false;
        enemiesInRange.Clear();
        alliesInRange.Clear();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //Player's turn
        if (!battleController.isEnemyTurn)
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
                    other.GetComponent<EnemyController>().highlightAssistable();
                    if (!alliesInRange.Contains(other.gameObject))
                    {
                        alliesInRange.Add(other.gameObject);
                    }
                }
                if (other.CompareTag("character"))
                {
                    if (other.gameObject != battleController.characterSelected)
                    {
                        other.GetComponent<PlayerController>().highlightAttackable();
                        if (!enemiesInRange.Contains(other.gameObject))
                        {
                            enemiesInRange.Add(other.gameObject);
                        }
                    }
                }           
            }
            
        }

        //Enemy's turn
        else
        {
            if (other.CompareTag("character"))
            {
                other.GetComponent<PlayerController>().highlightAttackable();
                if (!enemiesInRange.Contains(other.gameObject))
                {
                    enemiesInRange.Add(other.gameObject);
                }
                
                if (enemyIsRangedAndMoving && other.gameObject == battleController.enemyTarget)
                {
                    pathfinder.StopEnemyFollow();
                }
                
            }
            else if (other.CompareTag("enemy"))
            {
                if (other.gameObject != battleController.enemySelected)
                {
                    other.GetComponent<EnemyController>().highlightAssistable();
                    if (!alliesInRange.Contains(other.gameObject))
                    {
                        alliesInRange.Add(other.gameObject);
                    }
                    if (enemyIsRangedAndMoving && other.gameObject == battleController.enemyTarget)
                    {
                        pathfinder.StopEnemyFollow();
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
                other.GetComponent<EnemyController>().unhighlight();
                enemiesInRange.RemoveAll(item => item == other.gameObject);
            }
            if (other.CompareTag("character"))
            {
                if (battleController.disabledCharacters.Contains(other.gameObject))
                {
                    other.GetComponent<PlayerController>().graySpriteAndFreeze();
                }
                else
                {
                    other.GetComponent<PlayerController>().unhighlight();
                }
                alliesInRange.RemoveAll(item => item == other.gameObject);
            }
        }

        //Enemy is selected and others leave the AttackRange
        if (battleController.characterSelected == null && battleController.enemySelected != null)
        {
            if (other.CompareTag("character"))
            {
                if (battleController.disabledCharacters.Contains(other.gameObject))
                {
                    other.GetComponent<PlayerController>().graySpriteAndFreeze();
                }
                else
                {
                    other.GetComponent<PlayerController>().unhighlight();
                }
                enemiesInRange.RemoveAll(item => item == other.gameObject);
            }
            else if (other.CompareTag("enemy"))
            {
                if (battleController.disabledEnemies.Contains(other.gameObject))
                {
                    other.GetComponent<EnemyController>().graySpriteAndFreeze();
                }
                else
                {
                    other.GetComponent<EnemyController>().unhighlight();
                }
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
