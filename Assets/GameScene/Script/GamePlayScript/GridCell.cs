using UnityEngine;
using UnityEngine.UI;

public class GridCell : MonoBehaviour
{
    private Image cellImage;

    private void Awake()
    {
        cellImage = GetComponent<Image>();
    }

    public void SetColor(Color color)
    {
        cellImage.color = color;
    }
}
