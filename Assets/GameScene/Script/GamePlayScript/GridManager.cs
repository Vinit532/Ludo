using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public int numRows = 11;
    public int numColumns = 11;
    public float boardWidth = 10f;
    public float boardHeight = 10f;
    public GameObject gridCellPrefab;
    public Transform gridContainer;
    public Color redColor;
    public Color greenColor;
    public Color blueColor;
    public Color yellowColor;

    private int[,] redRows = new int[17, 2]
    {
        { 0, 0 }, { 0, 1 }, { 0, 2 }, { 0, 3 }, { 1, 0 }, { 1, 3 }, { 2, 0 }, { 2, 3 }, { 3, 0 },
        { 3, 1 }, { 3, 2 }, { 3, 3 }, { 4, 1 }, { 5, 1 }, { 5, 2 }, { 5, 3 }, { 5, 4 }
    };

    private int[,] greenRows = new int[17, 2]
    {
        { 0, 7 }, { 0, 8 }, { 0, 9 }, { 0, 10 }, { 1, 7 }, { 1, 10 }, { 2, 7 }, { 2, 10 }, { 3, 7 },
        { 3, 8 }, { 3, 9 }, { 3, 10 }, { 1, 6 }, { 1, 5 }, { 2, 5 }, { 3, 5 }, { 4, 5 }
    };

    private int[,] blueRows = new int[17, 2]
    {
        { 7, 7 }, { 7, 8 }, { 7, 9 }, { 7, 10 }, { 8, 7 }, { 8, 10 }, { 9, 7 }, { 9, 10 }, { 10, 7 },
        { 10, 8 }, { 10, 9 }, { 10, 10 }, { 6, 9 }, { 5, 9 }, { 5, 8 }, { 5, 7 }, { 5, 6 }
    };

    private int[,] yellowRows = new int[17, 2]
    {
        { 7, 0 }, { 7, 1 }, { 7, 2 }, { 7, 3 }, { 8, 0 }, { 8, 3 }, { 9, 0 }, { 9, 3 }, { 10, 0 },
        { 10, 1 }, { 10, 2 }, { 10, 3 }, { 9, 4 }, { 9, 5 }, { 8, 5 }, { 7, 5 }, { 6, 5 }
    };

    private void Start()
    {
        float cellWidth = boardWidth / numColumns;
        float cellHeight = boardHeight / numRows;

        for (int row = 0; row < numRows; row++)
        {
            for (int column = 0; column < numColumns; column++)
            {
                float xPos = column * cellWidth;
                float yPos = row * cellHeight;

                GameObject cell = Instantiate(gridCellPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                cell.name = "Cell[" + row + "," + column + "]";
                cell.transform.SetParent(gridContainer);

                GridCell gridCell = cell.AddComponent<GridCell>();

                if (IsInArray(redRows, row, column))
                    gridCell.SetColor(redColor);
                else if (IsInArray(greenRows, row, column))
                    gridCell.SetColor(greenColor);
                else if (IsInArray(blueRows, row, column))
                    gridCell.SetColor(blueColor);
                else if (IsInArray(yellowRows, row, column))
                    gridCell.SetColor(yellowColor);
            }
        }
    }

    private bool IsInArray(int[,] array, int row, int column)
    {
        int length = array.GetLength(0);
        for (int i = 0; i < length; i++)
        {
            if (array[i, 0] == row && array[i, 1] == column)
                return true;
        }
        return false;
    }
}
