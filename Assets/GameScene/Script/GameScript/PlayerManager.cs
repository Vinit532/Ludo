using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    private static Dictionary<int, bool> playerInstantiated = new Dictionary<int, bool>();

    public GameObject tokenPrefab; // Token prefab assigned in the Inspector
    public string assignedBlockName; // Assign the block name (e.g., "RedBlock", "BlueBlock", "GreenBlock", "YellowBlock") in the Inspector

    void Start()
    {
        if (photonView.IsMine)
        {
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            int playerIndex = photonView.Owner.ActorNumber - 1;

            // Check if the player has already been instantiated
            if (!playerInstantiated.ContainsKey(playerIndex))
            {
                playerInstantiated.Add(playerIndex, true);

                // Instantiate the tokenPrefab for the player
                for (int i = 0; i < 4; i++)
                {
                    Vector3 spawnPosition = GetSpawnPosition(i);
                    GameObject instance = PhotonNetwork.Instantiate(tokenPrefab.name, spawnPosition, Quaternion.identity, 0);
                    SetInstanceParent(instance);
                }
            }
        }
    }

    Vector3 GetSpawnPosition(int index)
    {
        Vector3 spawnPosition = Vector3.zero;

        // Get the GridManager instance
        GridManager gridManager = GridManager.Instance;

        // Check if the assignedBlockName matches one of the block names in the GridManager
        if (gridManager && !string.IsNullOrEmpty(assignedBlockName))
        {
            // Get the corresponding 2D array based on the assignedBlockName
            int[,] blockArray = null;
            switch (assignedBlockName)
            {
                case "RedBlock":
                    blockArray = gridManager.redRows;
                    break;
                case "BlueBlock":
                    blockArray = gridManager.blueRows;
                    break;
                case "GreenBlock":
                    blockArray = gridManager.greenRows;
                    break;
                case "YellowBlock":
                    blockArray = gridManager.yellowRows;
                    break;
                default:
                    Debug.LogError("Invalid assignedBlockName: " + assignedBlockName);
                    break;
            }

            // Check if the index is valid for the blockArray
            if (blockArray != null && index >= 0 && index < blockArray.GetLength(0))
            {
                int row = blockArray[index, 0];
                int column = blockArray[index, 1];
                spawnPosition = gridManager.GetCellPosition(row, column);
            }
        }

        return spawnPosition;
    }

    void SetInstanceParent(GameObject instance)
    {
        // Get the GridManager instance
        GridManager gridManager = GridManager.Instance;

        // Check if the assignedBlockName matches one of the block names in the GridManager
        if (gridManager && !string.IsNullOrEmpty(assignedBlockName))
        {
            // Find the corresponding grid cell based on the assignedBlockName
            GridCell gridCell = null;
            switch (assignedBlockName)
            {
                case "RedBlock":
                    gridCell = gridManager.GetRandomGridCellInBlock(gridManager.redBlock);
                    break;
                case "BlueBlock":
                    gridCell = gridManager.GetRandomGridCellInBlock(gridManager.blueBlock);
                    break;
                case "GreenBlock":
                    gridCell = gridManager.GetRandomGridCellInBlock(gridManager.greenBlock);
                    break;
                case "YellowBlock":
                    gridCell = gridManager.GetRandomGridCellInBlock(gridManager.yellowRows);
                    break;
                default:
                    Debug.LogError("Invalid assignedBlockName: " + assignedBlockName);
                    break;
            }

            // Set the instance as a child of the grid cell
            if (gridCell)
            {
                instance.transform.SetParent(gridCell.transform);
            }
        }
    }
}
