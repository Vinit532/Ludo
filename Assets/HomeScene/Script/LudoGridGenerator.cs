using UnityEngine;
using UnityEngine.UI;

public class LudoGridGenerator : MonoBehaviour
{
    public GameObject squarePrefab; // Reference to the square prefab

    public int rows = 5; // Number of rows in the Ludo game board grid
    public int columns = 5; // Number of columns in the Ludo game board grid

    public Color startingAreaColor; // Color for the starting area
    public Color homeAreaColor; // Color for the home area
    public Color safeZoneColor; // Color for the safe zone
    public Color regularSquareColor; // Color for the regular square

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        float squareSize = squarePrefab.GetComponent<RectTransform>().sizeDelta.x;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject square = Instantiate(squarePrefab, transform);

                // Position the square based on row and column
                float posX = col * squareSize;
                float posY = row * squareSize;
                square.transform.localPosition = new Vector3(posX, posY, 0f);

                // Customize the color based on the section type
                Image squareImage = square.GetComponent<Image>();
                if (IsStartingArea(row, col))
                {
                    squareImage.color = startingAreaColor;
                }
                else if (IsHomeArea(row, col))
                {
                    squareImage.color = homeAreaColor;
                }
                else if (IsSafeZone(row, col))
                {
                    squareImage.color = safeZoneColor;
                }
                else
                {
                    squareImage.color = regularSquareColor;
                }
            }
        }
    }

    private bool IsStartingArea(int row, int col)
    {
        // Logic to determine if the given row and column represent a starting area square
        // Implement your specific condition here
        return false;
    }

    private bool IsHomeArea(int row, int col)
    {
        // Logic to determine if the given row and column represent a home area square
        // Implement your specific condition here
        return false;
    }

    private bool IsSafeZone(int row, int col)
    {
        // Logic to determine if the given row and column represent a safe zone square
        // Implement your specific condition here
        return false;
    }
}
