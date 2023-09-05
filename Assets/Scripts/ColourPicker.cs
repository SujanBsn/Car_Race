using UnityEngine;
using UnityEngine.UI;

public class ColourPicker : MonoBehaviour
{
    public SelectedPart part;

    RectTransform Rect;
    Texture2D colourTexture;
    Material material;

    void Start()
    {
        Rect = GetComponent<RectTransform>();
        colourTexture = GetComponent<Image>().mainTexture as Texture2D;
    }

    void Update()
    {
        material = part.SelectedMaterial;

        if (RectTransformUtility.RectangleContainsScreenPoint(Rect, Input.mousePosition))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Rect, Input.mousePosition, null, out Vector2 delta);

            float width = Rect.rect.width;
            float height = Rect.rect.height;
            delta += new Vector2(width * .5f, height * .5f);

            float x = Mathf.Clamp(delta.x / width, 0, 1);
            float y = Mathf.Clamp(delta.y / height, 0, 1);

            int texX = Mathf.RoundToInt(x * colourTexture.width);
            int texY = Mathf.RoundToInt(y * colourTexture.height);

            Color color = colourTexture.GetPixel(texX, texY);
            if (Input.GetMouseButtonDown(0))
                material.color = color;
        }
    }
}
