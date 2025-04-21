using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Define the camera movement boundaries
    public Vector2 minPosition; // Bottom-left corner
    public Vector2 maxPosition; // Top-right corner
    public float moveSpeed = 5f;
    public int edgeSize = 10; // Pixels from edge to trigger scrolling
    void Update()
    {
        Vector3 moveDirection = Vector3.zero;
        Vector3 mousePos = Input.mousePosition;

        if (mousePos.x < edgeSize)
            moveDirection.x -= 1;

        if (mousePos.x > Screen.width - edgeSize)
            moveDirection.x += 1;

        if (mousePos.y < edgeSize)
            moveDirection.y -= 1;

        if (mousePos.y > Screen.height - edgeSize)
            moveDirection.y += 1;

        // Move and clamp the camera
        Vector3 newPosition = transform.position + moveDirection.normalized * moveSpeed * Time.deltaTime;

        // Clamp camera position
        newPosition.x = Mathf.Clamp(newPosition.x, minPosition.x, maxPosition.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minPosition.y, maxPosition.y);

        transform.position = newPosition;
    }
}