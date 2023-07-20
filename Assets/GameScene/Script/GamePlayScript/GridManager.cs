using System.Collections.Generic;
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
    public static GridManager Instance { get; private set; }

    // Store the grid cells for each block


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public int[,] redRows = new int[17, 2]
    {
        { 0, 0 }, { 0, 1 }, { 0, 2 }, { 0, 3 }, { 1, 0 }, { 1, 3 }, { 2, 0 }, { 2, 3 }, { 3, 0 },
        { 3, 1 }, { 3, 2 }, { 3, 3 }, { 4, 1 }, { 5, 1 }, { 5, 2 }, { 5, 3 }, { 5, 4 }
    };

    public int[,] greenRows = new int[17, 2]
    {
        { 0, 7 }, { 0, 8 }, { 0, 9 }, { 0, 10 }, { 1, 7 }, { 1, 10 }, { 2, 7 }, { 2, 10 }, { 3, 7 },
        { 3, 8 }, { 3, 9 }, { 3, 10 }, { 1, 6 }, { 1, 5 }, { 2, 5 }, { 3, 5 }, { 4, 5 }
    };

    public int[,] blueRows = new int[17, 2]
    {
        { 7, 7 }, { 7, 8 }, { 7, 9 }, { 7, 10 }, { 8, 7 }, { 8, 10 }, { 9, 7 }, { 9, 10 }, { 10, 7 },
        { 10, 8 }, { 10, 9 }, { 10, 10 }, { 6, 9 }, { 5, 9 }, { 5, 8 }, { 5, 7 }, { 5, 6 }
    };

    public int[,] yellowRows = new int[17, 2]
    {
        { 7, 0 }, { 7, 1 }, { 7, 2 }, { 7, 3 }, { 8, 0 }, { 8, 3 }, { 9, 0 }, { 9, 3 }, { 10, 0 },
        { 10, 1 }, { 10, 2 }, { 10, 3 }, { 9, 4 }, { 9, 5 }, { 8, 5 }, { 7, 5 }, { 6, 5 }
    };

    public int[,] blueBlock = new int[4, 2]
    {
        { 8, 8 }, { 8, 9 }, { 9, 8 }, { 9, 9 }
    };

    public int[,] redBlock = new int[4, 2]
    {
        { 1, 1 }, { 1, 2 }, { 2, 1 }, { 2, 2 }
    };

    public int[,] greenBlock = new int[4, 2]
    {
        { 1, 8 }, { 1, 9 }, { 2, 8 }, { 2, 9 }
    };

    public int[,] yellowBlock = new int[4, 2]
    {
        { 8, 1 }, { 8, 2 }, { 9, 1 }, { 9, 2 }
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

    public Vector3 GetCellPosition(int row, int column)
    {
        float cellWidth = boardWidth / numColumns;
        float cellHeight = boardHeight / numRows;
        float xPos = column * cellWidth;
        float yPos = row * cellHeight;
        return new Vector3(xPos, yPos, 0);
    }

    public GridCell GetRandomGridCellInBlock(int[,] blockArray)
    {
        int index = Random.Range(0, blockArray.GetLength(0));
        int row = blockArray[index, 0];
        int column = blockArray[index, 1];

        GameObject cellObject = gridContainer.Find("Cell[" + row + "," + column + "]").gameObject;
        if (cellObject)
        {
            return cellObject.GetComponent<GridCell>();
        }

        return null;
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

    public int[,] GetBlockArray(string blockName)
    {
        switch (blockName)
        {
            case "BlueBlock":
                return blueBlock;
            case "RedBlock":
                return redBlock;
            case "GreenBlock":
                return greenBlock;
            case "YellowBlock":
                return yellowBlock;
            default:
                Debug.LogWarning("Invalid block name: " + blockName);
                return null;
        }
    }

    public GridCell GetGridCell(int row, int column)
    {
        string cellName = "Cell[" + row + "," + column + "]";
        Transform cellTransform = gridContainer.Find(cellName);

        if (cellTransform != null)
        {
            return cellTransform.GetComponent<GridCell>();
        }

        return null;
    }
}
