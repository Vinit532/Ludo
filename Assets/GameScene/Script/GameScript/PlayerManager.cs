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
                    photonView.RPC("SetInstanceParent", RpcTarget.AllBuffered, instance.GetPhotonView().ViewID, i);
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
            int[,] blockArray = gridManager.GetBlockArray(assignedBlockName);

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

    [PunRPC]
    void SetInstanceParent(int instanceViewID, int index)
    {
        GameObject instance = PhotonView.Find(instanceViewID).gameObject;
        if (instance != null)
        {
            // Get the GridManager instance
            GridManager gridManager = GridManager.Instance;

            // Check if the assignedBlockName matches one of the block names in the GridManager
            if (gridManager && !string.IsNullOrEmpty(assignedBlockName))
            {
                // Get the corresponding 2D array based on the assignedBlockName
                int[,] blockArray = gridManager.GetBlockArray(assignedBlockName);

                // Check if the index is valid for the blockArray
                if (blockArray != null && index >= 0 && index < blockArray.GetLength(0))
                {
                    int row = blockArray[index, 0];
                    int column = blockArray[index, 1];
                    GridCell gridCell = gridManager.GetGridCell(row, column);

                    // Set the instance as a child of the grid cell
                    if (gridCell)
                    {
                        instance.transform.SetParent(gridCell.transform);
                    }
                }
            }
        }
    }
}
