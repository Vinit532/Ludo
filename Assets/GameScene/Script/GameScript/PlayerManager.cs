using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    private static Dictionary<int, bool> playerInstantiated = new Dictionary<int, bool>();

    public GameObject tokenPrefab; // Token prefab assigned in the Inspector
    public string assignedBlockName; // Assign the block name (e.g., "RedBlock", "BlueBlock", "GreenBlock", "YellowBlock") in the Inspector

    public int currentPlayerIndex = 0;
    private List<GameObject> tokens = new List<GameObject>();
    private bool canClick = false;

    private bool isLocalPlayerTurn = false;

    public int ActorNumber => photonView.Owner.ActorNumber;
    public string NickName => photonView.Owner.NickName;


    private void Start()
    {
        if (photonView.IsMine)
        {
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            int playerIndex = photonView.Owner.ActorNumber - 1;

            if (!playerInstantiated.ContainsKey(playerIndex))
            {
                playerInstantiated.Add(playerIndex, true);

                for (int i = 0; i < 4; i++)
                {
                    Vector3 spawnPosition = GetSpawnPosition(i);
                    GameObject instance = PhotonNetwork.Instantiate(tokenPrefab.name, spawnPosition, Quaternion.identity, 0);
                    photonView.RPC("SetInstanceParent", RpcTarget.AllBuffered, instance.GetPhotonView().ViewID, i);
                }

                if (playerIndex == 0)
                {
                    SetNextPlayerTurn();
                }
            }
        }
    }

    void SetNextPlayerTurn()
    {
        currentPlayerIndex++;
        if (currentPlayerIndex >= PhotonNetwork.PlayerList.Length)
        {
            currentPlayerIndex = 0;
        }

        // Get the next player in the turn rotation
        Player nextPlayer = PhotonNetwork.PlayerList[currentPlayerIndex];
        // Set the turn for the next player
        SetCurrentPlayerTurn(nextPlayer.NickName, nextPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber);
    }

    public void SetPlayerTurn(bool isTurn)
    {
        canClick = isTurn;

        if (canClick)
        {
            Debug.Log("It's your turn now! Player: " + photonView.Owner.NickName);
        }
        else
        {
            Debug.Log("It's not your turn. Player: " + photonView.Owner.NickName);
        }
    }


    [PunRPC]
    public void SetCurrentPlayerTurn(string playerName, bool isTurn)
    {
        SetPlayerTurn(isTurn);
    }
    [PunRPC]
    public void SetLocalPlayerCanClick(bool canClick)
    {
        DieController dieController = GetComponentInChildren<DieController>();

        if (dieController != null)
        {
            dieController.photonView.RPC("SetCanClick", RpcTarget.AllBuffered, canClick);
        }
    }

    [PunRPC]
    public void NextTurn(int playerIndex)
    {
        SetNextPlayerTurn();
    }

    Vector3 GetSpawnPosition(int index)
    {
        Vector3 spawnPosition = Vector3.zero;

        GridManager gridManager = GridManager.Instance;

        if (gridManager && !string.IsNullOrEmpty(assignedBlockName))
        {
            int[,] blockArray = gridManager.GetBlockArray(assignedBlockName);

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
            GridManager gridManager = GridManager.Instance;
            if (gridManager && !string.IsNullOrEmpty(assignedBlockName))
            {
                int[,] blockArray = gridManager.GetBlockArray(assignedBlockName);

                if (blockArray != null && index >= 0 && index < blockArray.GetLength(0))
                {
                    int row = blockArray[index, 0];
                    int column = blockArray[index, 1];
                    GridCell gridCell = gridManager.GetGridCell(row, column);

                    if (gridCell)
                    {
                        instance.transform.SetParent(gridCell.transform);
                    }
                }
            }
        }
    }

    public void RollDice()
    {
        if (canClick && photonView.IsMine)
        {
            // Do the dice rolling logic here
            // ...

            // Notify the DieController to start the next turn (call RPC on all clients)
            photonView.RPC("NextTurn", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }
}
