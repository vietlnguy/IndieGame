// Grid-Based A* Pathfinding integrated with Unity Tilemap using Collider Detection
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapPathfinder : MonoBehaviour
{
    public Tilemap tilemap;
    public LayerMask obstacleLayer; // Layer assigned to obstacle tiles

    public Vector3Int startCell;
    public Vector3Int endCell;

    private HashSet<Vector3Int> blockedCells = new HashSet<Vector3Int>();

    void Start()
    {
        CacheBlockedCells();
        List<Vector3Int> path = FindPath(startCell, endCell);

        foreach (var cell in path)
        {
            Vector3 worldPos = tilemap.GetCellCenterWorld(cell);
            Debug.DrawLine(worldPos, worldPos + Vector3.up * 0.25f, Color.green, 5f);
        }
    }

    void CacheBlockedCells()
    {
        blockedCells.Clear();
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;

            Vector3 worldPos = tilemap.GetCellCenterWorld(pos);
            if (Physics2D.OverlapPoint(worldPos, obstacleLayer))
            {
                blockedCells.Add(pos);
            }
        }
    }

    public List<Vector3> GetWorldPath(Vector3 worldStart, Vector3 worldEnd)
    {
        Vector3Int startCell = tilemap.WorldToCell(worldStart);
        Vector3Int endCell = tilemap.WorldToCell(worldEnd);

        CacheBlockedCells();
        List<Vector3Int> cellPath = FindPath(startCell, endCell);
        List<Vector3> worldPath = new List<Vector3>();

        foreach (var cell in cellPath)
        {
            worldPath.Add(tilemap.GetCellCenterWorld(cell));
        }

        return worldPath;
    }

    List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal)
    {
        var openSet = new List<Vector3Int> { start };
        var cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        var gScore = new Dictionary<Vector3Int, float> { [start] = 0 };
        var fScore = new Dictionary<Vector3Int, float> { [start] = Heuristic(start, goal) };

        while (openSet.Count > 0)
        {
            Vector3Int current = openSet[0];
            foreach (var node in openSet)
            {
                if (fScore.ContainsKey(node) && fScore[node] < fScore[current])
                    current = node;
            }

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            openSet.Remove(current);

            foreach (Vector3Int neighbor in GetNeighbors(current))
            {
                if (blockedCells.Contains(neighbor)) continue;

                float tentativeG = gScore[current] + 1;
                if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    fScore[neighbor] = tentativeG + Heuristic(neighbor, goal);
                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return new List<Vector3Int>();
    }

    List<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        var totalPath = new List<Vector3Int> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }
        return totalPath;
    }

    float Heuristic(Vector3Int a, Vector3Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    List<Vector3Int> GetNeighbors(Vector3Int pos)
    {
        var neighbors = new List<Vector3Int>
        {
            new Vector3Int(pos.x + 1, pos.y, 0),
            new Vector3Int(pos.x - 1, pos.y, 0),
            new Vector3Int(pos.x, pos.y + 1, 0),
            new Vector3Int(pos.x, pos.y - 1, 0)
        };

        return neighbors;
    }
}
