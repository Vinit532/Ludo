using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TurnManager : MonoBehaviourPunCallbacks
{
    public static TurnManager Instance;

    private int currentPlayerIndex = 0;
    private List<Player> players = new List<Player>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Add all players in the room to the players list
        foreach (var player in PhotonNetwork.PlayerList)
        {
            players.Add(player);
        }
    }

    public bool IsPlayerTurn()
    {
        if (players.Count == 0)
        {
            return false;
        }

        // Get the current player
        Player currentPlayer = players[currentPlayerIndex];

        if (PhotonNetwork.IsConnected)
        {
            if (currentPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                return true;
            }
        }

        return false;
    }

    [PunRPC]
    public void NextTurn()
    {
        // Increment the current player index to move to the next player
        currentPlayerIndex++;
        if (currentPlayerIndex >= players.Count)
        {
            currentPlayerIndex = 0;
        }

        // Notify all players about the current player's turn
        Player currentPlayer = players[currentPlayerIndex];
        photonView.RPC("SetCurrentPlayerTurn", RpcTarget.All, currentPlayer.NickName, currentPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber);
    }

    [PunRPC]
    private void SetCurrentPlayerTurn(string playerName, bool isTurn)
    {
        // Update the player turns for all players in the scene
        PlayerManager[] playerManagers = FindObjectsOfType<PlayerManager>();
        foreach (PlayerManager playerManager in playerManagers)
        {
            playerManager.SetPlayerTurn(isTurn);
        }
    }
}
