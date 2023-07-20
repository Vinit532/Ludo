using UnityEngine;
using Photon.Pun;

public class TokenManager : MonoBehaviourPunCallbacks
{
    public GameObject[] tokenPrefabs; // Array of token prefabs (one for each color)

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            // Check if the player count is within the range of available tokenPrefabs
            if (playerCount > 0 && playerCount <= tokenPrefabs.Length)
            {
                // Instantiate the tokenPrefabs for each player
                for (int i = 0; i < playerCount; i++)
                {
                    GameObject tokenPrefab = tokenPrefabs[i];
                   // PhotonNetwork.Instantiate(tokenPrefab.name, Vector3.zero, Quaternion.identity, 0);
                }
            }
            else
            {
                Debug.LogWarning("Invalid player count or tokenPrefabs not assigned correctly.");
            }
        }
    }
}
