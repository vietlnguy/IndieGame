using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class AttackRangeCircle : MonoBehaviour
{
    public bool active = false;
    public BattleController battleController;
    public string sortingLayerName = "MoveRangeUI";
    public int sortingOrder = 0;
    public Color fillColor = new Color(1f, 0f, 0f, 0.25f);
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    public List<GameObject> alliesInRange;
    public List<GameObject> enemiesInRange;
    public bool enemyIsRangedAndMoving = false;
    private TilemapPathfinder pathfinder;
    private LayerMask obstacleLayer;
    private LayerMask enemyLayer;
    private float viewDistance;
    private int rayCount = 180; // Higher = smoother mesh, lower = better performance
    private LayerMask combinedLayerMask; // Combines obstacles and enemies
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    void Awake()
    {
        pathfinder = FindAnyObjectByType<TilemapPathfinder>();
        battleController = GameObject.Find("BattleController").GetComponent<BattleController>();
        alliesInRange = new List<GameObject>();
        enemiesInRange = new List<GameObject>();

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        
        // Assign material with sprite shader for transparency support
        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = fillColor;
        meshRenderer.material = mat;

        // Sorting for 2D layering
        meshRenderer.sortingLayerName = sortingLayerName;
        meshRenderer.sortingOrder = sortingOrder;

        obstacleLayer = LayerMask.GetMask("Obstacle");
        enemyLayer = LayerMask.GetMask("Characters");
        combinedLayerMask = obstacleLayer | enemyLayer;
    }
    void LateUpdate()
    {
        if (active)
        {
            GenerateFOVMesh();
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
        meshRenderer.enabled = true;

        if (character.GetComponent<PlayerController>() != null) {
            viewDistance = character.GetComponent<PlayerController>().attackRange;
        }
        else 
        {
            viewDistance = character.GetComponent<EnemyController>().attackRange;
        }
        GenerateFOVMesh();


        active = true;
    }
    public void disableAttackRange()
    {
        active = false;
        meshRenderer.enabled = false;
        enemiesInRange.Clear();
        alliesInRange.Clear();
    }
    void GenerateFOVMesh()
    {
        float angleStep = 360f / rayCount;
        vertices = new Vector3[rayCount + 1]; 
        triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero;

        int vertexIndex = 1;
        int triangleIndex = 0;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 rayDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, rayDirection, viewDistance, combinedLayerMask);
            
            // FORCE SORT: Guarantees hits are processed from closest to furthest
            Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

            Vector3 vertex = Vector3.zero + rayDirection * viewDistance;

            foreach (RaycastHit2D hit in hits)
            {
                // Ignore the player's own root collider if the ray starts inside it
                //if (hit.collider.transform == transform.parent || hit.collider.transform == transform)
                //    continue;

                // 1. Check for Obstacle Layer first
                if ((obstacleLayer.value & (1 << hit.collider.gameObject.layer)) > 0)
                {
                    vertex = transform.InverseTransformPoint(hit.point);
                    break; // Stop looking past this wall
                }
                
                else if (hit.collider.GetComponent<EnemyController>() != null && (battleController.isPlayerTurn || battleController.isNeutralTurn) && !battleController.enemySelected)
                {
                    hit.collider.GetComponent<EnemyController>().InAttackRange();
                    vertex = transform.InverseTransformPoint(hit.point);
                    break; // Stop looking past this enemy
                }
                else if (hit.collider.GetComponent<PlayerController>() != null && battleController.isEnemyTurn)
                {
                    hit.collider.GetComponent<PlayerController>().InAttackRange();
                    vertex = transform.InverseTransformPoint(hit.point);
                    break; // Stop looking past this enemy 
                }
            }

            vertices[vertexIndex] = vertex;

            triangles[triangleIndex + 0] = 0; 
            triangles[triangleIndex + 1] = vertexIndex; 
            
            if (i == rayCount - 1)
            {
                triangles[triangleIndex + 2] = 1; 
            }
            else
            {
                triangles[triangleIndex + 2] = vertexIndex + 1; 
            }

            triangleIndex += 3;
            vertexIndex++;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
    }

}
