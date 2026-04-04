// Grid-Based A* Pathfinding for Unity 2D using Physics2D and obstacleLayer
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapPathfinder : MonoBehaviour
{
    public Tilemap tilemap;
    public LayerMask obstacleLayer;
    public int padding = 0; // New variable for padding
    public AudioSource walkingAudio;
    public float moveSpeed = 1f;
    public List<Vector3Int> occupiedTiles;
    public bool cancelFollow;
    public List<Vector3> GetWorldPath(Vector3 worldStart, Vector3 worldEnd)
    {
        Vector3Int startCell = tilemap.WorldToCell(worldStart);
        Vector3Int endCell = tilemap.WorldToCell(worldEnd);

        // Optional: Ensure start/end are not *directly* blocked (without padding for goal)
        // If the start cell itself is an obstacle, we can't start there.
        if (IsCellDirectlyBlocked(startCell))
        {
            Debug.LogWarning("Start cell is directly blocked. Path might not be found.");
            return new List<Vector3>();
        }
        // If the end cell itself is an obstacle, we can't end there.
        if (IsCellDirectlyBlocked(endCell))
        {
            Debug.LogWarning("End cell is directly blocked. Path might not be found.");
            return new List<Vector3>();
        }


        // Pass the goal cell to FindPath
        List<Vector3Int> cellPath = FindPath(startCell, endCell);
        List<Vector3> worldPath = new List<Vector3>();

        foreach (var cell in cellPath)
        {
            worldPath.Add(tilemap.GetCellCenterWorld(cell));
        }

        return worldPath;
    }
    public List<Vector3Int> GetUnoccupiedAdjacent(Vector3 targetPos)
    {
        // 1. Determine the character's current cell position
        Vector3Int currentCell = tilemap.WorldToCell(targetPos);

        // CHANGE: Return type is List<Vector3Int>
        List<Vector3Int> adjacentCells = new List<Vector3Int>();

        // 2. Get the 4 cardinal neighbors
        List<Vector3Int> neighbors = GetNeighbors(currentCell);

        // 3. Iterate through neighbors and check for blocking/unreachability
        foreach (Vector3Int neighborCell in neighbors)
        {
            // Check if the neighbor is blocked, considering the unit's padding.
            bool isBlocked = IsBlockedWithPadding(neighborCell, neighborCell);

            if (!isBlocked && !occupiedTiles.Contains(neighborCell))
            {
                adjacentCells.Add(neighborCell);
            }
        }

        return adjacentCells;
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
                // Pass the goal cell to IsBlockedWithPadding
                if (IsBlockedWithPadding(neighbor, goal)) continue;

                float tentativeG = gScore[current] + 1; // Assuming cardinal movement cost of 1
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

        Debug.Log("No path found!");
        return new List<Vector3Int>(); // No path found
    }
    bool IsBlockedWithPadding(Vector3Int cell, Vector3Int goalCell)
    {
        // If the current cell being checked is the GOAL CELL,
        // we only care if it's directly blocked. We don't apply padding.
        if (cell == goalCell)
        {
            return IsCellDirectlyBlocked(cell);
        }

        // Otherwise (it's not the goal cell), apply full padding logic
        if (IsCellDirectlyBlocked(cell))
        {
            return true;
        }

        // Then, check surrounding cells based on padding
        for (int xOffset = -padding; xOffset <= padding; xOffset++)
        {
            for (int yOffset = -padding; yOffset <= padding; yOffset++)
            {
                // Skip the center cell as it's already checked
                if (xOffset == 0 && yOffset == 0) continue;

                Vector3Int paddedCell = new Vector3Int(cell.x + xOffset, cell.y + yOffset, cell.z);

                // CRITICAL: Ensure we don't accidentally block the goal if it's within the padding radius
                // of a non-goal cell being checked. This ensures the path can always reach the goal.
                if (paddedCell == goalCell) continue;

                if (IsCellDirectlyBlocked(paddedCell))
                {
                    return true; // A cell within the padding area (and not the goal) is blocked
                }
            }
        }
        return false; // No cells within padding (or the cell itself) are blocked for non-goal cells
    }
    bool IsCellDirectlyBlocked(Vector3Int cell)
    {
        // Physics2D.OverlapPoint checks if *any* collider on the obstacleLayer overlaps the center of the cell.
        // This implicitly assumes obstacles are aligned with cell centers or are small enough.
        Vector3 worldPos = tilemap.GetCellCenterWorld(cell);
        return Physics2D.OverlapPoint(worldPos, obstacleLayer);
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
        return Mathf.Max(
            Mathf.Abs(a.x - b.x),
            Mathf.Abs(a.y - b.y)
        );
    }
    List<Vector3Int> GetNeighbors(Vector3Int pos)
    {
        var neighbors = new List<Vector3Int>
        {
            new Vector3Int(pos.x + 1, pos.y, 0), // Right
            new Vector3Int(pos.x - 1, pos.y, 0), // Left
            new Vector3Int(pos.x, pos.y + 1, 0), // Up
            new Vector3Int(pos.x, pos.y - 1, 0),  // Down
            // Add diagonal neighbors if you want 8-directional movement:
             new Vector3Int(pos.x + 1, pos.y + 1, 0),
             new Vector3Int(pos.x + 1, pos.y - 1, 0),
             new Vector3Int(pos.x - 1, pos.y + 1, 0),
             new Vector3Int(pos.x - 1, pos.y - 1, 0)
        };

        return neighbors;
    }
    public System.Collections.IEnumerator EnemyFollowPath(GameObject enemy, Vector3 targetPos)
    {
        cancelFollow = false;
        float moveLimit;
        try
        {
            moveLimit = enemy.GetComponent<EnemyController>().moveRange;
        }
        catch
        {
            moveLimit = enemy.GetComponent<PlayerController>().moveRange;
 
        }
        float distanceMoved = 0f;
        Vector3 previousPos = enemy.transform.position;

        //check if already adjacent to target
        if (!alreadyAdjacent(enemy, targetPos))
        {
            List<Vector3Int> unoccupiedTiles = GetUnoccupiedAdjacent(targetPos);
            List<Vector3> path = GetWorldPath(enemy.transform.position, unoccupiedTiles[0]);

            walkingAudio.Play();

            //Move enemy
            foreach (Vector3 waypoint in path)
            {
                // Keep moving until you reach the waypoint
                while (!cancelFollow && Vector3.Distance(enemy.transform.position, waypoint) > 0.1f)
                {
                    // Step toward waypoint
                    Vector3 oldPos = enemy.transform.position;
                    enemy.transform.position = Vector3.MoveTowards(
                        enemy.transform.position,
                        waypoint,
                        moveSpeed * Time.deltaTime
                    );

                    // Add to total distance moved
                    distanceMoved += Vector3.Distance(oldPos, enemy.transform.position);

                    // Check if move limit reached
                    if (distanceMoved >= moveLimit)
                    {
                        yield break; // stop moving any further
                    }
                    yield return null;
                }
            }
        }
    }
    
    
    public System.Collections.IEnumerator FollowPath(GameObject enemy, Vector3 targetPos)
    {
        List<Vector3> path = GetWorldPath(enemy.transform.position, targetPos);

        walkingAudio.Play();

        foreach (Vector3 waypoint in path)
        {
            // Keep moving until you reach the waypoint
            while (Vector3.Distance(enemy.transform.position, waypoint) > 0.1f)
            {
                // Step toward waypoint
                enemy.transform.position = Vector3.MoveTowards(
                    enemy.transform.position,
                    waypoint,
                    moveSpeed * Time.deltaTime
                );

                yield return null;
            }
        }

        walkingAudio.Stop();
    }
    public void calculateOccupiedTiles(GameObject characters, GameObject enemies)
    {
        occupiedTiles.Clear();
        foreach (Transform child in characters.transform)
        {
            Vector3 worldStart = child.gameObject.transform.position;
            Vector3Int currentCell = tilemap.WorldToCell(worldStart);
            occupiedTiles.Add(currentCell);
        }
        foreach (Transform child in enemies.transform)
        {
            Vector3 worldStart = child.gameObject.transform.position;
            Vector3Int currentCell = tilemap.WorldToCell(worldStart);
            occupiedTiles.Add(currentCell);
        }
    }
    private bool alreadyAdjacent(GameObject enemy, Vector3 targetPos)
    {
        Vector3Int enemyCell = tilemap.WorldToCell(enemy.transform.position);
        Vector3Int targetCell = tilemap.WorldToCell(targetPos);

        int dx = Mathf.Abs(enemyCell.x - targetCell.x);
        int dy = Mathf.Abs(enemyCell.y - targetCell.y);

        // Adjacent if one step away in cardinal direction. i.e. returns true if adjacent
        return (dx + dy) == 1;

    }
    public void StopEnemyFollow()
    {
        cancelFollow = true;
    }
}