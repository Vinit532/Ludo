using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DieController : MonoBehaviourPunCallbacks
{
    public GameObject dieNumberPrefab;
    private List<GameObject> generatedInstances = new List<GameObject>();
    private bool canClick = true;

  
    void Update()
    {
        if (canClick && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Die Clicked!");
            GenerateDieNumbers();
            photonView.RPC("NextTurn", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }

    [PunRPC]
    public void SetCanClick(bool value)
    {
        canClick = value;
    }

    private void GenerateDieNumbers()
    {
        // Clear previously generated instances
        ClearGeneratedInstances();

        // Generate a random number between 1 and 6
        int randomNumber = Random.Range(1, 7);
        Debug.Log("Generated Number: " + randomNumber);

        // Create instances of the dieNumberPrefab based on the generated number
        for (int i = 0; i < randomNumber; i++)
        {
            // Use the Die object as the parent transform and set the local position
            Vector3 spawnPosition = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            GameObject instance = PhotonNetwork.Instantiate(dieNumberPrefab.name, transform.position + spawnPosition, Quaternion.identity);
            instance.transform.SetParent(transform); // Set the Die object as the parent
            generatedInstances.Add(instance);
        }

        Debug.Log("Number of Instances Created: " + randomNumber);
    }

    private void ClearGeneratedInstances()
    {
        foreach (var instance in generatedInstances)
        {
            PhotonNetwork.Destroy(instance);
        }
        generatedInstances.Clear();
    }

    [PunRPC]
    public void NextTurn(int playerActorNumber)
    {
        // Check if it's the local player's turn based on the player's actor number
        bool isLocalPlayerTurn = photonView.Owner.ActorNumber == playerActorNumber;

        // Notify the player manager to set the player's turn
        PlayerManager playerManager = GetComponentInParent<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.SetPlayerTurn(isLocalPlayerTurn);
        }
    }
}
