using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FixedAspectRatio : MonoBehaviour
{
    public float targetAspectRatio = 16f / 9f;  // Change to whatever you want

    void Start()
    {
        UpdateViewport();
    }

    void Update()
    {
        // Optional: dynamically respond to window resizing
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            UpdateViewport();
        }
    }

    private int lastScreenWidth, lastScreenHeight;

    void UpdateViewport()
    {
        Camera cam = GetComponent<Camera>();

        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspectRatio;

        if (scaleHeight < 1f)
        {
            // Letterbox: add black bars top/bottom
            Rect rect = cam.rect;

            rect.width = 1f;
            rect.height = scaleHeight;
            rect.x = 0f;
            rect.y = (1f - scaleHeight) / 2f;

            cam.rect = rect;
        }
        else
        {
            // Pillarbox: add black bars left/right
            float scaleWidth = 1f / scaleHeight;

            Rect rect = cam.rect;

            rect.width = scaleWidth;
            rect.height = 1f;
            rect.x = (1f - scaleWidth) / 2f;
            rect.y = 0f;

            cam.rect = rect;
        }

        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
    }
}
