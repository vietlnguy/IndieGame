using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageGradient : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public bool leftToRight = true;
    public bool center = false;

    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        Texture2D gradientTexture = null;

        if (center)
        {
            gradientTexture = CreateCenterGradientTexture(width, height);
     
        }
        else
        {
            gradientTexture = CreateGradientTexture(width, height);
        }

        Sprite gradientSprite = Sprite.Create(
            gradientTexture,
            new Rect(0, 0, width, height),
            new Vector2(0.5f, 0.5f)
        );

        image.sprite = gradientSprite;
    }

    Texture2D CreateGradientTexture(int width, int height)
    {
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);

        tex.wrapMode = TextureWrapMode.Clamp;

        for (int x = 0; x < width; x++)
        {
            float alpha = 0;
            if (leftToRight) { alpha = 1f - ((float)x / (width - 1)); }
            else { alpha = (float)x / (width - 1); }

            for (int y = 0; y < height; y++)
            {
                tex.SetPixel(x, y, new Color(0f, 0f, 0f, alpha));
            }
        }

        tex.Apply();

        return tex;
    }
    Texture2D CreateCenterGradientTexture(int width, int height)
    {
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;

        float center = (width - 1) / 2f;

        for (int x = 0; x < width; x++)
        {
            // Distance from center (0 at center, 1 at edges)
            float distanceFromCenter = Mathf.Abs(x - center) / center;

            // Invert so center = 1, edges = 0
            float alpha = 1f - distanceFromCenter;

            for (int y = 0; y < height; y++)
            {
                tex.SetPixel(x, y, new Color(0f, 0f, 0f, alpha));
            }
        }

        tex.Apply();
        return tex;
    }
}