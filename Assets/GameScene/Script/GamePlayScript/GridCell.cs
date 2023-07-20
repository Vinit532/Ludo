using UnityEngine;
using UnityEngine.UI;

public class GridCell : MonoBehaviour
{
    private Image cellImage;

    public static float cellWidth;
    public static float cellHeight;
    private void Awake()
    {
        cellImage = GetComponent<Image>();
    }

    public void SetColor(Color color)
    {
        cellImage.color = color;
    }
}
